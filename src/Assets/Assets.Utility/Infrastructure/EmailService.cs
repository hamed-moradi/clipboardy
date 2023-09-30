using Assets.Model.Base;
using Assets.Model.Common;
using Serilog;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Assets.Utility.Infrastructure
{
    public class EmailService : IEmailService
    {
        #region ctor
        private readonly AppSetting _appSetting;

        public EmailService(
            AppSetting appSetting)
        {

            _appSetting = appSetting;
        }
        #endregion

        public async Task<IServiceResult> SendAsync(EmailModel email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email.Address) || string.IsNullOrWhiteSpace(email.Subject) ||
                    string.IsNullOrWhiteSpace(email.Body))
                {
                    return DataTransferer.DefectiveEntry();
                }

                var mail = new MailMessage()
                {
                    From = new MailAddress(_appSetting.SmtpConfig.Address),
                    Subject = email.Subject,
                    Body = email.Body,
                    IsBodyHtml = email.IsBodyHtml,
                    Priority = MailPriority.High
                };
                mail.To.Add(new MailAddress(email.Address));
                if (!string.IsNullOrWhiteSpace(email.AttachmentPath) && File.Exists(email.AttachmentPath))
                    mail.Attachments.Add(new Attachment(email.AttachmentPath));

                using (var smtp = new SmtpClient(_appSetting.SmtpConfig.Host, _appSetting.SmtpConfig.Port))
                {
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = new NetworkCredential(_appSetting.SmtpConfig.Username, _appSetting.SmtpConfig.Password);
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    await smtp.SendMailAsync(mail);
                }

                return DataTransferer.Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Source);
                return DataTransferer.InternalServerError();
            }
        }
    }
}
