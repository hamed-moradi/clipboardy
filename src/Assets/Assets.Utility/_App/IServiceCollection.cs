using Assets.Model.Base;
using Assets.Model.Common;
using System.Threading.Tasks;

namespace Assets.Utility {
    public interface ISMSService {
        Task<IServiceResult> SendAsync(SMSModel email);
    }

    public interface IEmailService {
        Task<IServiceResult> SendAsync(EmailModel email);
    }
}
