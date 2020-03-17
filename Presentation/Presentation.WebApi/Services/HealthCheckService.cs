using Assets.Model.Settings;
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
        private readonly MsSqlDbContext _dbContext;

        public HealthCkeckService(AppSetting appSetting, MsSqlDbContext dbContext) {
            _appSetting = appSetting;
            _dbContext = dbContext;
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

        private bool CheckMsSqlDb() {
            try {
                return _dbContext.Database.CanConnect();
            }
            catch(Exception ex) {
                Log.Error(ex, "Error on openning connection to MsSqldb.");
                return false;
            }
        }
        #endregion

        public bool Analyze() {
            return CheckConfig() && CheckMsSqlDb();
        }
    }
}
