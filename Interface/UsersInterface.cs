using GppApp.DbContext;
using GppApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GppApp.Interface
{
    public interface UsersInterface : IDisposable
    {
        List<UserDetails> GetUsers();
        List<UserViewModel> GetAllUsers();
        void UpdateUsers(UserDetails aUserDetails);
        void AddUsers(UserDetails aUserDetails);
    }
}
