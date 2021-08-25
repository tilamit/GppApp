using GppApp.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GppApp.Interface
{
    public interface AuthorizeInterface : IDisposable
    {
        string Login(string email, string password);
        void TrackAllUsers(TrackUsers aTrackUsers);
    }
}
