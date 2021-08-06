using GppApp.DbContext;
using GppApp.Interface;
using GppApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GppApp.Repository
{
    public class UsersRepository : UsersInterface
    {
        private readonly GppAppEntities _context;
        int userId = 0;

        public UsersRepository(GppAppEntities context)
        {
            _context = context;
        }

        public List<UserDetails> GetUsers()
        {
            userId = Convert.ToInt32(HttpContext.Current.Session["userId"]);
            return _context.UserDetails.Where(c => c.UserId == userId).ToList();
        }

        public List<UserViewModel> GetAllUsers()
        {
            var result = (from c in _context.UserDetails
                          join d in _context.Gender on c.Gender equals d.Id
                          join f in _context.UserTypes on c.UserType equals f.Id
                          select new UserViewModel
                          {
                              UserId = c.UserId,
                              UserName = c.UserName,
                              Email = c.Email,
                              UserTypes = f.Types,
                              CreatedOn = c.CreatedOn
                          }).ToList();

            return result;
        }

        public void AddUsers(UserDetails aUserDetails)
        {
            try
            {
                UserDetails aUserDetail = new UserDetails();
                aUserDetail.UserName = aUserDetails.UserName;
                aUserDetail.Email = aUserDetails.Email;
                aUserDetail.Password = aUserDetails.Password;
                aUserDetail.UserType = aUserDetails.UserType;
                aUserDetail.Gender = aUserDetails.Gender;
                aUserDetail.DateOfBirth = Convert.ToDateTime("1990-09-10");
                aUserDetail.CreatedOn = DateTime.Now;
                aUserDetail.Status = 1;

                _context.UserDetails.Add(aUserDetail);
                _context.SaveChanges();
            }

            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public void UpdateUsers(UserDetails aUserDetails)
        {
            userId = Convert.ToInt32(HttpContext.Current.Session["userId"]);
            var result = _context.UserDetails.SingleOrDefault(c => c.UserId == userId);

            if (result != null)
            {
                result.UserName = aUserDetails.UserName;
                result.Gender = aUserDetails.Gender;
                result.DateOfBirth = aUserDetails.DateOfBirth;
            
                _context.SaveChanges();
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