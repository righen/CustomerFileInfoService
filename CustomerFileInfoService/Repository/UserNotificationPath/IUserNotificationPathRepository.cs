using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerFileInfoService.Repository
{
    public interface IUserNotificationPathRepository
    {
        string GetUserEmail(string username);

        string GetUserNotificationPath(string username);
    }
}
