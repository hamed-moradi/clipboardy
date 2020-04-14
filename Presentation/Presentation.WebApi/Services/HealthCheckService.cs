using Assets.Model.Common;
using Core.Domain;
using Serilog;
using System;

namespace Presentation.WebApi.Services {
    public interface IHealthCkeckService {
        bool Analyze();
    }

    public class HealthCkeckService: IHealthCkeckService {
        #region ctor
        private readonly AppSetting _appSetting;

        public HealthCkeckService(AppSetting appSetting) {
            _appSetting = appSetting;
        }
        #endregion

        #region private
        private bool CheckConfig() {
            if(string.IsNullOrWhiteSpace(_appSetting.ConnectionStrings?.MsSql)) {
                Log.Error("There is no connection string to MsSqldb.");
                return false;
            }

            if(string.IsNullOrWhiteSpace(_appSetting.Encryption?.PrivateKey)) {
                Log.Error("PrivateKey is not defined.");
                return false;
            }

            if(string.IsNullOrWhiteSpace(_appSetting.Encryption?.PublicKey)) {
                Log.Error("PublicKey is not defined.");
                return false;
            }

            return true;
        }
        #endregion

        public bool Analyze() {
            return CheckConfig();
        }
    }
}
