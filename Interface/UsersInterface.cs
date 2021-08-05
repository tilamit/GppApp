using GppApp.DbContext;
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
        void UpdateUsers(UserDetails aUserDetails);
    }
}
