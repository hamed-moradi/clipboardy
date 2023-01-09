using Assets.Model.Base;
using Assets.Model.Common;
using ParsGreen;
using RestSharp;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Utility.Infrastructure {
    public class ParsGreenSMSService: ISMSService {
        #region ctor
        private readonly AppSetting _appSetting;

        public ParsGreenSMSService(
            AppSetting appSetting) {

            _appSetting = appSetting;
        }
        #endregion

        public async Task<IServiceResult> SendAsync(SMSModel sms) {
            Log.Debug($"ParsGreenSMSService.SendAsync.Begin => {sms}");

            try {
                //var response = await new SendSMSSoapClient(new SendSMSSoapClient.EndpointConfiguration() { })
                //    .SendAsync(_appSetting.SMSConfig.Signature, sms.PhoneNo, sms.TextBody, string.Empty);

                #region get
                var client = new RestClient("http://login.parsgreen.com/");
                var request = new RestRequest("UrlService/sendSMS.ashx", Method.Post);

                request.AddParameter("from", _appSetting.SMSConfig.Number);
                request.AddParameter("to", sms.PhoneNo);
                request.AddParameter("text", sms.TextBody);
                request.AddParameter("signature", "");

                var response = await client.ExecuteAsync<string>(request);
                Log.Debug($"ParsGreenSMSService.SendAsync.End => {response}");

                if(response.StatusCode == HttpStatusCode.OK) {
                    if(response.Data == string.Empty) {
                        return DataTransferer.Ok();
                    }
                }
                #endregion

                #region post
                //var client = new RestClient("http://login.parsgreen.com/");
                //var request = new RestRequest("UrlService/sendSMS.ashx", Method.POST);

                //var bodyparams = new Dictionary<string, string> {
                //    ["from"] = "",
                //    ["to"] = sms.PhoneNo,
                //    ["text"] = sms.TextBody,
                //    ["signature"] = "",
                //};
                //request.AddBody(bodyparams);

                //var response = client.Execute<string>(request);
                #endregion
            }
            catch(Exception ex) {
                Log.Error(ex, ex.Source);
            }

            return DataTransferer.InternalServerError();
        }
    }
}
