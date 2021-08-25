using GppApp.DbContext;
using GppApp.Interface;
using OnlineRevision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GppApp.Repository
{
    public class MenusRepository : MenusInterface
    {
        private readonly GppAppEntities _context;

        public MenusRepository(GppAppEntities context)
        {
            _context = context;
        }

        public List<MenusViewModel> GetAllMenus()
        {
            int userId = Convert.ToInt32(HttpContext.Current.Session["userId"]);

            var result = (from c in _context.UserDetails
                          join d in _context.AssignedMenus on c.AssignedMenu equals d.AssignedMenu
                          join f in _context.Menus on d.MenuId equals f.Id
                          where c.UserId == userId
                          select new MenusViewModel
                          {
                              AssignedMenu = d.AssignedMenu,
                              MenuId = f.MenuId,
                              MenuName = f.MenuName,
                              MenuIcon = f.MenuIcon,
                              UserType = c.UserType
                          }).ToList();

            return result.ToList();
        }
    }
}