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
    public class MenusController : Controller
    {
        private MenusInterface _menusRepository;

        public MenusController()
        {
            _menusRepository = new MenusRepository(new DbContext.GppAppEntities());
        }

        public MenusController(MenusRepository menusRepository)
        {
            _menusRepository = menusRepository;
        }

        // GET: Menus
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAllMenus()
        {
            List<MenusViewModel> aLst = null;
            try
            {
                aLst = _menusRepository.GetAllMenus();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return Json(aLst, JsonRequestBehavior.AllowGet);
        }
    }
}