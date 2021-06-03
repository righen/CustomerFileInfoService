using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerFileInfoService.Services.EmailService
{
    public interface IEmailService
    {
        void Send(string toEmail, string subject, string body);
    }
}
