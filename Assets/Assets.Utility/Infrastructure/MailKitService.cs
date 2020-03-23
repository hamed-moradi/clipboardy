using System.Threading.Tasks;
using Assets.Model.Common;
using MailKit.Net.Smtp;
using MimeKit;

namespace Assets.Utility.Infrastructure {
    public class MailKitService: IEmailService {
        #region ctor
        private readonly AppSetting _appSetting;

        public MailKitService(
            AppSetting appSetting) {

            _appSetting = appSetting;
        }
        #endregion

        public async Task SendAsync(EmailModel email) {
            var message = new MimeMessage();

            var from = new MailboxAddress(_appSetting.SmtpConfig.MailboxName, _appSetting.SmtpConfig.MailboxAddress);
            message.From.Add(from);

            var to = new MailboxAddress(email.Name, email.Address);
            message.To.Add(to);


            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = email.HtmlBody;
            bodyBuilder.TextBody = email.TextBody;
            bodyBuilder.Attachments.Add(email.AttachmentPath);

            message.Subject = email.Subject;
            message.Body = bodyBuilder.ToMessageBody();

            var client = new SmtpClient();
            client.Connect(_appSetting.SmtpConfig.Address, _appSetting.SmtpConfig.Port, true);
            client.Authenticate(_appSetting.SmtpConfig.Username, _appSetting.SmtpConfig.Password);

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
            client.Dispose();
        }
    }
}
