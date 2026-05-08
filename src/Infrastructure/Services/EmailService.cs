using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger) => _logger = logger;

        public Task SendAsync(string to, string subject, string body, CancellationToken ct = default)
        {
            _logger.LogInformation(
                "📧 [EMAIL] To: {To} | Subject: {Subject} | Body: {Body}",
                to, subject, body);

            return Task.CompletedTask;
        }
    }
}
