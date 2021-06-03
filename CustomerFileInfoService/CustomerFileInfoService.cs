using CustomerFileInfoService.Repository;
using CustomerFileInfoService.Services.EmailService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerFileInfoService
{
    public class CustomerFileInfoService : BackgroundService
    {
        private readonly string username = Environment.UserName;
        private const int retryCount = 3;
        private readonly ILogger<CustomerFileInfoService> _logger;
        private IChangeToken _fileChangeToken;
        private PhysicalFileProvider _fileProvider;
        private readonly IConfiguration _configuration;
        private readonly IUserNotificationPathRepository _userNotificationPathRepository;
        private readonly IEmailService _emailService;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        public CustomerFileInfoService(ILogger<CustomerFileInfoService> logger, IConfiguration configuration, IUserNotificationPathRepository userNotificationPathRepository, IEmailService emailService, IHostApplicationLifetime appLifetime)
        {
            _logger = logger;
            _configuration = configuration;
            _userNotificationPathRepository = userNotificationPathRepository;
            _emailService = emailService;
            _hostApplicationLifetime = appLifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int currentRetry = 1;

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Customer File Info Service running at: {time}", DateTimeOffset.Now);

                int delay = Convert.ToInt32(_configuration["CustomerFileInfoServiceConfig:Delay"]);

                try
                {
                    string documentPath = Convert.ToBoolean(_configuration["CustomerFileInfoServiceConfig:isProd"]) ? 
                        _userNotificationPathRepository.GetUserNotificationPath(username) : @$"{_configuration["CustomerFileInfoServiceConfig:LocalDirectoryPath"]}{username}";

                    _fileProvider = new PhysicalFileProvider(@$"{documentPath}");

                    WatchForFileChanges();
                }
                catch (Exception ex)
                {
                    delay = delay * 2;

                    _logger.LogInformation($"Retrying...{currentRetry}");

                    currentRetry++;
                    
                    if (retryCount < currentRetry)
                    {
                        _logger.LogInformation(ex.ToString());
                        _hostApplicationLifetime.StopApplication();
                    }
                }

                await Task.Delay(delay, stoppingToken);
            }
        }

        private void WatchForFileChanges()
        {
            _fileChangeToken = _fileProvider.Watch("*.pdf*");
            _fileChangeToken.RegisterChangeCallback(Notify, default);
        }


        private void Notify(object state)
        {
            _logger.LogInformation("File change detected at: {time}", DateTimeOffset.Now);

            sendMail();

            WatchForFileChanges();
        }

        private void sendMail()
        {
            _logger.LogInformation("Sending email to user: {time}", DateTimeOffset.Now);

            var email = Convert.ToBoolean(_configuration["CustomerFileInfoServiceConfig:isProd"]) ? _userNotificationPathRepository.GetUserEmail(username) : _configuration["CustomerFileInfoServiceConfig:testingMail"];
            var subject = _configuration["Smtp:Subject"];
            var body = _configuration["Smtp:Body"];

            _emailService.Send(email, subject, body);
        }
    }
}
