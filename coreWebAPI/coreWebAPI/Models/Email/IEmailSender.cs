using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MOM.Core.Models.Email
{
    public interface IEmailSend
    {
        //Task SendEmailAsync(string recipient, string sender, string subject, string html, string message);
        Task SendEmailAsync(EmailModel data);
    }
}
