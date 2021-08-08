using GppApp.DbContext;
using GppApp.Interface;
using GppApp.Models;
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
                          where c.ProjectId == projectId
                          select new ProjectsViewModel
                          {
                              Id = d.Id,
                              ProjectId = c.ProjectId,
                              ProCreatedOn = c.CreatedOn,
                              ItemDescription = d.ItemDescription,
                              ProjectNotes = d.ProjectNotes,
                              Checked = d.Checked,
                              Image = d.Image,
                              CreatedOn = d.CreatedOn.ToString()
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
            var result = _context.Projects.ToList().OrderByDescending(c => c.ProjectId);

            string id = result.SingleOrDefault().ProjectId;
            string newId = id.Substring(id.LastIndexOf('/') + 1);

            string num = (Convert.ToInt32(newId) + 1).ToString("0###");

            return ("#Project-" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString("0#") + "/" + num).ToString();
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
                ItemImages.CreatedOn = DateTime.Now;
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

                ProjectsHistory aProjectsHistory = new ProjectsHistory();
                aProjectsHistory.ProjectId = aProjectHistory.ProjectId;
                aProjectsHistory.PreviousName = aProjectHistory.PreProjectName;
                aProjectsHistory.PreviousRef = aProjectHistory.PreProjectRef;
                aProjectsHistory.PreProjectDetails = aProjectHistory.PreProjectDetails;
                aProjectsHistory.PreCreatedBy = aProjectHistory.PreCreatedBy;
                aProjectsHistory.PreCreatedOn = aProjectHistory.PreCreatedOn;
                aProjectsHistory.PreCreatedOn = aProjectHistory.PreCreatedOn;
                aProjectsHistory.UpdatedOn = DateTime.Now;

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

                ProjectItemsHistory aProjectItemsHistory = new ProjectItemsHistory();
                aProjectItemsHistory.ProjectId = aProjectItemHistory.ProjectId;
                aProjectItemsHistory.PreItemDescription = aProjectItemHistory.PreItemDescription;
                aProjectItemsHistory.PreProjectNotes = aProjectItemHistory.PreProjectNotes;
                aProjectItemsHistory.PreImage = aProjectItemHistory.PreImage;
                aProjectItemsHistory.PreChecked = aProjectItemHistory.PreChecked;
                aProjectItemsHistory.PreCreatedOn = aProjectItemHistory.PreCreatedOn;
                aProjectItemsHistory.PreCreatedBy = aProjectItemHistory.PreCreatedBy;
                aProjectItemsHistory.UpdatedOn = DateTime.Now;

                _context.ProjectItemsHistory.Add(aProjectItemsHistory);
                _context.SaveChanges();
            }
        }

        public void AddProjectItems(ProjectItems aProjectItem)
        {
            try
            {
                ProjectItems aProjectItems = new ProjectItems();
                aProjectItems.ProjectId = aProjectItem.ProjectId;
                aProjectItems.ItemDescription = aProjectItem.ItemDescription;
                aProjectItems.ProjectNotes = aProjectItem.ProjectNotes;
                aProjectItems.CreatedOn = aProjectItem.CreatedOn;
                aProjectItems.CreatedBy = Convert.ToInt32(HttpContext.Current.Session["userId"]);
                aProjectItems.Status = 1;
                aProjectItems.Checked = true;

                _context.ProjectItems.Add(aProjectItem);
                _context.SaveChanges();
            }

            catch(Exception ex)
            {
                ex.ToString();
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

        public List<ItemImages> GetImages(int id)
        {
            var result = (from c in _context.ItemImages
                          where c.ProjectItemId == id
                          select c).ToList();

            return result;
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