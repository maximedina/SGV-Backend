using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MOM.Core.Models.Email
{
    public class AuthMessageSender : IEmailSend
    {
        public EmailSettings _emailSettings { get; }

        public AuthMessageSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public Task SendEmailAsync(EmailModel data)
        {
            try
            {
                Execute(data).Wait();
                return Task.FromResult(0);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public async Task Execute(string recipient, string sender, string subject,string html, string message)
        public async Task Execute(EmailModel data)
        {
            try
            {
                //string toEmail = string.IsNullOrEmpty(recipient) ? _emailSettings.ToEmail : recipient;

                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(_emailSettings.UsernameEmail)//data.Sender)
                };

                mail.To.Add(data.Recipient);

                //mail.CC.Add(new MailAddress(_emailSettings.CcEmail));
                mail.Subject = data.Subject;
                mail.Body = data.Html;//data.Message + "</br>" + data.Html;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;


                //mail.Attachments.Add(new Attachment(arquivo));
                //
                SmtpClient smtp = new SmtpClient();
                smtp.EnableSsl = _emailSettings.EnableSsl;
                smtp.Host = _emailSettings.PrimaryDomain;
                smtp.Port = _emailSettings.PrimaryPort;
                switch (_emailSettings.ServerType)
                {
                    case 1:
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword);
                        break;
                    default:
                        break;
                }
                  
                    await smtp.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
