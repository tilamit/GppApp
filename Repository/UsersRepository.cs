using GppApp.DbContext;
using GppApp.Interface;
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