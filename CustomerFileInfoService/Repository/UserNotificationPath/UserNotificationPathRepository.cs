using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerFileInfoService.Repository
{
    public class UserNotificationPathRepository : IUserNotificationPathRepository
    {
        private readonly CustomerFileInfoDbContext _customerFileInfoDbContext;

        public UserNotificationPathRepository(IServiceProvider serviceProvider)
        {
            _customerFileInfoDbContext = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<CustomerFileInfoDbContext>();
        }
        public string GetUserEmail(string username)
        {
            var sanitizedUsername = username.Replace("$", "");

            var email = _customerFileInfoDbContext.UserNotificationPaths.Where(w => w.SystemName == sanitizedUsername).Select(w => w.Email).FirstOrDefault();

            return email;
        }

        public string GetUserNotificationPath(string username)
        {
            var sanitizedUsername = username.Replace("$", "");

            var email = _customerFileInfoDbContext.UserNotificationPaths.Where(w => w.SystemName == sanitizedUsername).Select(w => w.Path).FirstOrDefault();
            return email;
        }
    }
}
