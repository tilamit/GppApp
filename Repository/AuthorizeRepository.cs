using GppApp.DbContext;
using GppApp.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineRevision.Repository
{
    public class AuthorizeRepository : AuthorizeInterface
    {
        private readonly GppAppEntities _context;

        public AuthorizeRepository(GppAppEntities context)
        {
            _context = context;
        }

        public string Login(string email, string password)
        {
            string str = "";
            var result = (from c in _context.UserDetails
                          where c.Email == email && c.Password == password
                          select c).ToList();

            if(result.Count == 0)
            {
                str = "UserName or Password invalid!";
            }
            else
            {
                str = result.SingleOrDefault().UserName + '~' + result.SingleOrDefault().UserId;
            }
           
            return str;
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