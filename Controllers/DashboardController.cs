using GppApp.DbContext;
using GppApp.Interface;
using GppApp.Models;
using GppApp.Repository;
using GppApp.Utility;
using OnlineRevision.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GppApp.Controllers
{
    public class DashboardController : Controller
    {
        private ProjectsInterface _projectsRepository;

        public DashboardController()
        {
            _projectsRepository = new ProjectsRepository(new DbContext.GppAppEntities());
        }

        public DashboardController(ProjectsRepository projectsRepository)
        {
            _projectsRepository = projectsRepository;
        }

        [GppAuthorize]
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        [GppAuthorize]
        //Load partial views
        public ActionResult LoadPartialView(string passVal)
        {
            switch (passVal)
            {
                case "dashboard":
                    return PartialView("Entry");
                case "projectDetails":
                    var result = _projectsRepository.GetProjects();

                    return PartialView("projectsPartialView", result);
                case "userDetails":
                    return PartialView("usersPartialView");

                default:
                    return PartialView("frontPartialView");
            }
        }

        [GppAuthorize]
        //Load partial views with id
        public ActionResult LoadPartialViewWithId(string passVal, string id)
        {
            switch (passVal)
            {
                case "editProjects":
                    ViewBag.projectId = id;

                    return PartialView("editProjectsPartialView");

                default:
                    return PartialView("frontPartialView");
            }
        }

        [HttpPost]
        public JsonResult GetProjects(string id)
        {
            List<Projects> aLst = null;
            try
            {
                if (id == "" || id == null)
                {
                    aLst = _projectsRepository.GetProjects();
                }
                else
                {
                    aLst = _projectsRepository.GetProjects().Where(c => c.ProjectId == id).ToList();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return Json(aLst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAllProjects()
        {
            int projectsTotal = 0;
            List<Projects> aLst = null;
            try
            {
                aLst = _projectsRepository.GetProjects().Where(c => c.Status == 1).ToList();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return Json(projectsTotal = aLst.Count(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAllProjectItems()
        {
            List<ProjectItems> aLst = null;
            try
            {
                aLst = _projectsRepository.GetAllProjectItems().Where(c => c.Status == 1).ToList();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return Json(aLst.Count(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetImages(int id)
        {
            List<ItemImages> aLst = null;
            try
            {
                aLst = _projectsRepository.GetImages(id);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return Json(aLst, JsonRequestBehavior.AllowGet);
        }

        //Get project items
        [HttpPost]
        public JsonResult GetItems(int id)
        {
            List<ProjectItems> aLst = null;
            try
            {
                aLst = _projectsRepository.GetItems(id);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return Json(aLst, JsonRequestBehavior.AllowGet);
        }

        //Add project details here
        public async Task<JsonResult> AddProjects(Projects aProject)
        {
            try
            {
                _projectsRepository.AddProjects(aProject);
            }

            catch (Exception ex)
            {
                ex.ToString();
            }

            return await Task.Run(() => Json("Project added successfully!", JsonRequestBehavior.AllowGet));
        }

        //Update project details here
        public async Task<JsonResult> UpdateProjects(ProjectHistory aProjectsHistory)
        {
            try
            {
                _projectsRepository.UpdateProjects(aProjectsHistory);
            }

            catch (Exception ex)
            {
                ex.ToString();
            }

            return await Task.Run(() => Json("Project updated successfully!", JsonRequestBehavior.AllowGet));
        }

        //Show project update history
        public async Task<JsonResult> ShowProjectsHistory(string projectId)
        {
            List<ProjectHistory> aLst = null;
            try
            {
                aLst = _projectsRepository.ShowProjectsHistory(projectId);
            }

            catch (Exception ex)
            {
                ex.ToString();
            }

            return await Task.Run(() => Json(aLst, JsonRequestBehavior.AllowGet));
        }

        //Show project items update history
        public async Task<JsonResult> ShowProjectItemsHistory(int id)
        {
            List<ProjectItemHistory> aLst = null;
            try
            {
                aLst = _projectsRepository.ShowProjectItemsHistory(id);
            }

            catch (Exception ex)
            {
                ex.ToString();
            }

            return await Task.Run(() => Json(aLst, JsonRequestBehavior.AllowGet));
        }

        //Update project items here
        public async Task<JsonResult> UpdateItems(ProjectItemHistory aProjectItemHistory)
        {
            try
            {
                _projectsRepository.UpdateItems(aProjectItemHistory);
            }

            catch (Exception ex)
            {
                ex.ToString();
            }

            return await Task.Run(() => Json("Project items updated successfully!", JsonRequestBehavior.AllowGet));
        }

        //Add project items here
        public async Task<JsonResult> AddProjectItems(ProjectItems aProjectItems)
        {
            try
            {
                _projectsRepository.AddProjectItems(aProjectItems);
            }

            catch (Exception ex)
            {
                ex.ToString();
            }

            return await Task.Run(() => Json(aProjectItems.Id, JsonRequestBehavior.AllowGet));
        }

        //Image upload
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadImg(string id, int uniqueId, string imgName)
        {
            string _imgname = string.Empty;
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var pic = System.Web.HttpContext.Current.Request.Files["MyImages"];
                if (pic.ContentLength > 0)
                {
                    if (_projectsRepository.CheckImages(uniqueId, imgName) == false)
                    {
                        var fileName = Path.GetFileName(pic.FileName);
                        var _ext = Path.GetExtension(pic.FileName);

                        _imgname = Guid.NewGuid().ToString();
                        var _comPath = Server.MapPath("/img/") + _imgname + _ext;
                        _imgname = "" + _imgname + _ext;

                        ViewBag.Msg = _comPath;
                        var path = _comPath;

                        pic.SaveAs(path);

                        MemoryStream ms = new MemoryStream();
                        System.Web.Helpers.WebImage img = new System.Web.Helpers.WebImage(_comPath);

                        img.Save(_comPath);

                        _projectsRepository.AddImages(uniqueId, _imgname);
                    }
                }
            }

            return Json(Convert.ToString(_imgname), JsonRequestBehavior.AllowGet);
        }
    }
}