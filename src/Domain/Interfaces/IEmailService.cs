using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string body, CancellationToken ct = default);
    }
}
