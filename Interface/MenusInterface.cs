using OnlineRevision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GppApp.Interface
{
    public interface MenusInterface
    {
        List<MenusViewModel> GetAllMenus();
    }
}
