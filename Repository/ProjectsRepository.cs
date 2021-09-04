using GppApp.DbContext;
using GppApp.Interface;
using GppApp.Models;
using GppApp.Utility;
using OnlineRevision.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GppApp.Repository
{
    public class ProjectsRepository : ProjectsInterface
    {
        private readonly GppAppEntities _context;

        public ProjectsRepository(GppAppEntities context)
        {
            _context = context;
        }

        public List<Projects> GetProjects()
        {
            return _context.Projects.ToList();
        }

        public List<ProjectHistory> ShowProjectsHistory(string projectId)
        {
            var result = (from c in _context.Projects
                          join d in _context.ProjectsHistory on c.ProjectId equals d.ProjectId
                          join f in _context.UserDetails on d.PreCreatedBy equals f.UserId
                          where d.ProjectId == projectId
                          select new ProjectHistory
                          {
                              ProjectId = c.ProjectId,
                              PreProjectName = c.ProjectName,
                              PreProjectDetails = d.PreProjectDetails,
                              PersonCreated = f.UserName,
                              PreCreatedOn = d.PreCreatedOn,
                              PreProjectRef = d.PreviousRef,
                              UpdatedOn = d.UpdatedOn
                          }).ToList();

            return result.ToList();
        }

        public List<ProjectItemHistory> ShowProjectItemsHistory(int id)
        {
            var result = (from c in _context.ProjectItems
                          join d in _context.ProjectItemsHistory on c.Id equals d.PreId
                          join f in _context.UserDetails on d.PreCreatedBy equals f.UserId
                          where d.PreId == id
                          select new ProjectItemHistory
                          {
                              ProjectId = c.ProjectId,
                              PreItemDescription = d.PreItemDescription,
                              PersonCreated = f.UserName,
                              PreCreatedOn = d.PreCreatedOn,
                              PreProjectNotes = d.PreProjectNotes,
                              UpdatedOn = d.UpdatedOn
                          }).ToList();

            return result.ToList();
        }

        public List<ProjectItems> GetItems(int id)
        {
            return _context.ProjectItems.Where(c => c.Id == id).ToList();
        }

        public List<ProjectItems> GetAllProjectItems()
        {
            return _context.ProjectItems.ToList();
        }

        public List<ProjectsViewModel> GetProjectItems(string projectId)
        {
            var result = (from c in _context.Projects
                          join d in _context.ProjectItems on c.ProjectId equals d.ProjectId into g
                          from d in g.DefaultIfEmpty()
                          where c.ProjectId == projectId && d.Status == 1
                          select new ProjectsViewModel
                          {
                              Id = d.Id,
                              ProjectId = c.ProjectId,
                              ProjectName = c.ProjectName,
                              ProCreatedOn = c.CreatedOn,
                              ItemDescription = d.ItemDescription,
                              ProjectNotes = d.ProjectNotes,
                              Checked = d.Checked,
                              Image = _context.ItemImages.Where(c => c.ProjectItemId == d.Id).Select(d => d.Image).FirstOrDefault(),
                              CreatedOn = d.CreatedOn.ToString(),
                              Status = _context.ItemsConfirmed.Where(c => c.ProjectItemId == d.Id).Count() > 0 ? 1 : 0, //Confirmed by client
                              ProStatus = c.Status,
                              SubmittedBy = _context.ApprovalForSubmission.
                                            Join(_context.UserDetails, c => c.UserId, d => d.UserId,
                                            (c, d) => new { c, d }).
                                            Join(_context.UserTypes, g => g.d.UserType, h => h.Id, (g, h) => new { g, h }).
                                            Where(m => m.g.c.ProjectItemId == d.Id)
                                            .Select(m => m.h.Types).FirstOrDefault() ?? "",
                              SubmittedDt = _context.ApprovalForSubmission.
                                            Join(_context.UserDetails, c => c.UserId, d => d.UserId,
                                            (c, d) => new { c, d }).
                                            Join(_context.UserTypes, g => g.d.UserType, h => h.Id, (g, h) => new { g, h }).
                                            Where(m => m.g.c.ProjectItemId == d.Id)
                                            .Select(m => m.g.c.SubmissionDate).FirstOrDefault(),

                              TotalItems = _context.ProjectItems.Where(c => c.ProjectId == d.ProjectId && d.Status == 1).Count()
                          }).ToList();

            return result;
        }

        public List<ProjectsViewModel> GetProjectItemsInPdf(string projectId)
        {
            var result = (from c in _context.Projects
                          join d in _context.ProjectItems on c.ProjectId equals d.ProjectId into g
                          from d in g.DefaultIfEmpty()
                          where c.ProjectId == projectId && d.Status == 1
                          select new ProjectsViewModel
                          {
                              Id = d.Id,
                              ProjectId = c.ProjectId,
                              ProjectName = c.ProjectName,
                              ProCreatedOn = c.CreatedOn,
                              ItemDescription = d.ItemDescription,
                              ProjectNotes = d.ProjectNotes,
                              Checked = d.Checked,
                              Image = _context.ItemImages.Where(c => c.ProjectItemId == d.Id).Select(d => d.Image).FirstOrDefault(),
                              CreatedOn = d.CreatedOn.ToString(),
                              Status = _context.ItemsConfirmed.Where(c => c.ProjectItemId == d.Id).Count() > 0 ? 1 : 0, //Confirmed by client
                              ProStatus = c.Status,
                              SubmittedBy = _context.ApprovalForSubmission.
                                            Join(_context.UserDetails, c => c.UserId, d => d.UserId,
                                            (c, d) => new { c, d }).
                                            Join(_context.UserTypes, g => g.d.UserType, h => h.Id, (g, h) => new { g, h }).
                                            Where(m => m.g.c.ProjectItemId == d.Id)
                                            .Select(m => m.h.Types).FirstOrDefault() ?? "",
                              SubmittedDt = _context.ApprovalForSubmission.
                                            Join(_context.UserDetails, c => c.UserId, d => d.UserId,
                                            (c, d) => new { c, d }).
                                            Join(_context.UserTypes, g => g.d.UserType, h => h.Id, (g, h) => new { g, h }).
                                            Where(m => m.g.c.ProjectItemId == d.Id)
                                            .Select(m => m.g.c.SubmissionDate).FirstOrDefault(),

                              TotalItems = _context.ProjectItems.Where(c => c.ProjectId == d.ProjectId && d.Status == 1).Count(),

                              DateConfirmed = _context.ItemsConfirmed.Where(c => c.ProjectItemId == d.Id).Select(d => d.ConfirmDate).FirstOrDefault()
                          }).ToList();

            return result;
        }

        public void AddProjects(Projects aProject)
        {
            try
            {
                Projects aProjects = new Projects();
                aProjects.ProjectId = GetAutoId();
                aProjects.ProjectName = aProject.ProjectName;
                aProjects.ProjectRef = aProject.ProjectRef;
                aProjects.ProjectDetails = aProject.ProjectDetails;
                aProjects.CreatedOn = aProject.CreatedOn;
                aProjects.CreatedBy = Convert.ToInt32(HttpContext.Current.Session["userId"]);
                aProjects.Status = 1;

                _context.Projects.Add(aProjects);
                _context.SaveChanges();
            }
           
            catch(Exception ex)
            {
                ex.ToString();
            }
        }

        public string GetAutoId()
        {
            var result = _context.Projects.OrderByDescending(c => c.Id);

            string id = result.FirstOrDefault().ProjectId;
            string newId = id.Substring(id.LastIndexOf('/') + 1);

            string num = (Convert.ToInt32(newId) + 1).ToString("0###");

            return ("#Project-" + DateTimeAustralia.GetDateTime().Year.ToString() + "/" + DateTimeAustralia.GetDateTime().Month.ToString("0#") + "/" + num).ToString();
        }

        public void AddImages(int id, string imgName)
        {
            var result = _context.ProjectItems.SingleOrDefault(c => c.Id == id);

            if (result != null)
            {
                result.Image = imgName;

                _context.SaveChanges();

                ItemImages ItemImages = new ItemImages();
                ItemImages.ProjectItemId = id;
                ItemImages.Image = imgName;
                ItemImages.CreatedOn = DateTimeAustralia.GetDateTime();
                ItemImages.CreatedBy = Convert.ToInt32(HttpContext.Current.Session["userId"]);
                ItemImages.Status = 1;

                _context.ItemImages.Add(ItemImages);
                _context.SaveChanges();
            }
        }

        public void UpdateProjects(ProjectHistory aProjectHistory)
        {
            var result = _context.Projects.SingleOrDefault(c => c.ProjectId == aProjectHistory.ProjectId);

            if (result != null)
            {
                result.ProjectName = aProjectHistory.ProjectName;
                result.ProjectRef = aProjectHistory.ProjectRef;
                result.ProjectDetails = aProjectHistory.ProjectDetails;
                result.Status = aProjectHistory.Status;

                ProjectsHistory aProjectsHistory = new ProjectsHistory();
                aProjectsHistory.ProjectId = aProjectHistory.ProjectId;
                aProjectsHistory.PreviousName = aProjectHistory.PreProjectName;
                aProjectsHistory.PreviousRef = aProjectHistory.PreProjectRef;
                aProjectsHistory.PreProjectDetails = aProjectHistory.PreProjectDetails;
                aProjectsHistory.PreCreatedBy = aProjectHistory.PreCreatedBy;
                aProjectsHistory.PreCreatedOn = aProjectHistory.PreCreatedOn;
                aProjectsHistory.UpdatedOn = DateTimeAustralia.GetDateTime();

                _context.ProjectsHistory.Add(aProjectsHistory);
                _context.SaveChanges();
            }
        }

        public void UpdateItems(ProjectItemHistory aProjectItemHistory)
        {
            var result = _context.ProjectItems.SingleOrDefault(c => c.Id == aProjectItemHistory.Id);

            if (result != null)
            {
                result.ItemDescription = aProjectItemHistory.ItemDescription;
                result.ProjectNotes = aProjectItemHistory.ProjectNotes;
                result.Checked = aProjectItemHistory.PreChecked;

                ProjectItemsHistory aProjectItemsHistory = new ProjectItemsHistory();
                aProjectItemsHistory.PreId = aProjectItemHistory.Id;
                aProjectItemsHistory.ProjectId = aProjectItemHistory.ProjectId;
                aProjectItemsHistory.PreItemDescription = aProjectItemHistory.PreItemDescription;
                aProjectItemsHistory.PreProjectNotes = aProjectItemHistory.PreProjectNotes;
                aProjectItemsHistory.PreImage = aProjectItemHistory.PreImage;
                aProjectItemsHistory.PreChecked = aProjectItemHistory.PreChecked;
                aProjectItemsHistory.PreCreatedOn = aProjectItemHistory.PreCreatedOn;
                aProjectItemsHistory.PreCreatedBy = aProjectItemHistory.PreCreatedBy;
                aProjectItemsHistory.UpdatedOn = DateTimeAustralia.GetDateTime();
                //aProjectItemsHistory.Status = 1;

                _context.ProjectItemsHistory.Add(aProjectItemsHistory);
                _context.SaveChanges();
            }
        }

        public string AddProjectItems(ProjectItems aProjectItem)
        {
            string itemId = "";
            try
            {
                var query = (from c in _context.Projects
                             where c.ProjectId == aProjectItem.ProjectId && c.Status == 1
                             select c).ToList();

                if (query.Count() > 0)
                {
                    ProjectItems aProjectItems = new ProjectItems();
                    aProjectItems.ProjectId = aProjectItem.ProjectId;
                    aProjectItems.ItemDescription = aProjectItem.ItemDescription;
                    aProjectItems.ProjectNotes = aProjectItem.ProjectNotes;
                    aProjectItems.CreatedOn = aProjectItem.CreatedOn;
                    aProjectItems.CreatedBy = Convert.ToInt32(HttpContext.Current.Session["userId"]);
                    aProjectItems.Status = 1;
                    aProjectItems.Checked = false;

                    _context.ProjectItems.Add(aProjectItems);
                    _context.SaveChanges();

                    itemId = aProjectItems.Id.ToString();
                }
            }

            catch(Exception ex)
            {
                ex.ToString();
            }

            return itemId;
        }

        public void DeleteProjectItems(string itemId)
        {
            string[] id = itemId.Split(',');

            foreach (var item in id)
            {
                int projectItemId = Convert.ToInt32(item);
                var query = (from c in _context.ProjectItems
                             where c.Id == projectItemId && c.Status == 1 && c.Checked == false
                             select c).ToList();

                if (query.Count() > 0)
                {
                    int itemsId = Convert.ToInt32(item);
                    var result = _context.ProjectItems.Find(itemsId);

                    result.Status = 0;

                    _context.Entry(result).State = EntityState.Modified;
                    _context.SaveChanges();
                }
            }
        }

        public bool CheckImages(int id, string imgName)
        {
            bool status = false;
            List<ProjectItems> result = _context.ProjectItems.Where(c => c.Id == id && c.Image == imgName).ToList();

            if (result.Count > 0)
            {
                status = true;
            }

            return status;
        }

        public List<ImgHistoryViewModel> GetImages(int id)
        {
            var result = (from c in _context.ItemImages
                          join d in _context.UserDetails on c.CreatedBy equals d.UserId
                          where c.ProjectItemId == id
                          select new ImgHistoryViewModel
                          {
                              UserName = d.UserName,
                              Image = c.Image,
                              CreatedOn = c.CreatedOn
                          }).ToList();

            return result;
        }

        public void AddConfirmedItems(string projectId, string itemId)
        {
            string[] id = itemId.Split(',');

            foreach (var item in id)
            {
                int projectItemId = Convert.ToInt32(item);
                var result = _context.ItemsConfirmed.Where(c => c.ProjectItemId == projectItemId).ToList();
               
                if (result.Count() == 0)
                {
                    ItemsConfirmed aItemsConfirmed = new ItemsConfirmed();
                    aItemsConfirmed.ProjectId = projectId;
                    aItemsConfirmed.ProjectItemId = Convert.ToInt32(item);
                    aItemsConfirmed.UserId = Convert.ToInt32(HttpContext.Current.Session["userId"]);
                    aItemsConfirmed.ConfirmDate = DateTimeAustralia.GetDateTime();
                    aItemsConfirmed.Details = _context.Projects.Where(c => c.ProjectId == projectId).Select(d => d.ProjectName).FirstOrDefault();
                    aItemsConfirmed.Status = 1;
                    aItemsConfirmed.SystemDate = DateTimeAustralia.GetDateTime();

                    var query = _context.ProjectItems.Find(Convert.ToInt32(item));

                    query.Checked = true;

                    _context.ItemsConfirmed.Add(aItemsConfirmed);
                    _context.SaveChanges();
                }
            }
        }

        public void SubmitForApproval(string projectId, string itemId)
        {
            string[] id = itemId.Split(',');

            foreach (var item in id)
            {
                int projectItemId = Convert.ToInt32(item);
                var result1 = _context.ApprovalForSubmission.Where(c => c.ProjectItemId == projectItemId).ToList();
                var result2 = _context.ItemsConfirmed.Where(c => c.ProjectItemId == projectItemId).ToList();

                if (result1.Count() == 0 && result2.Count() == 0)
                {
                    ApprovalForSubmission aApprovalForSubmission = new ApprovalForSubmission();
                    aApprovalForSubmission.ProjectId = projectId;
                    aApprovalForSubmission.ProjectItemId = Convert.ToInt32(item);
                    aApprovalForSubmission.UserId = Convert.ToInt32(HttpContext.Current.Session["userId"]);
                    aApprovalForSubmission.SubmissionDate = DateTimeAustralia.GetDateTime();
                    aApprovalForSubmission.Details = _context.Projects.Where(c => c.ProjectId == projectId).Select(d => d.ProjectName).FirstOrDefault();
                    aApprovalForSubmission.Status = 1;
                    aApprovalForSubmission.SystemDate = DateTimeAustralia.GetDateTime();

                    _context.ApprovalForSubmission.Add(aApprovalForSubmission);
                    _context.SaveChanges();
                }
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
    }
}