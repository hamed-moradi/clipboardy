using Assets.Model.Base;
using Assets.Model.Common;
using System.Threading.Tasks;

namespace Assets.Utility {
    public interface ISMSService {
        Task<SendSMSStatus> SendAsync(SMSModel email);
    }

    public interface IEmailService {
        Task SendAsync(EmailModel email);
    }
}
