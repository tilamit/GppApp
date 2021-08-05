using GppApp.DbContext;
using GppApp.Interface;
using GppApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GppApp.Controllers
{
    public class UserController : Controller
    {
        private UsersInterface _usersRepository;

        public UserController()
        {
            _usersRepository = new UsersRepository(new DbContext.GppAppEntities());
        }

        public UserController(UsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetUsers()
        {
            List<UserDetails> aLst = null;
            try
            {
                aLst = _usersRepository.GetUsers();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return Json(aLst, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> UpdateUsers(UserDetails aUserDetails)
        {
            try
            {
                _usersRepository.UpdateUsers(aUserDetails);
            }

            catch (Exception ex)
            {
                ex.ToString();
            }

            return await Task.Run(() => Json("User details updated successfully!", JsonRequestBehavior.AllowGet));
        }
    }
}