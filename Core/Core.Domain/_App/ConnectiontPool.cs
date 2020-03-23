using System;
using System.Collections.Generic;
using System.Data;
using Assets.Model.Base;
using Assets.Model.Common;
using Microsoft.Data.SqlClient;

namespace Core.Domain {
    public class ConnectionPool {
        #region ctor
        private readonly AppSetting _appSetting;

        public ConnectionPool(AppSetting appSetting) {
            _appSetting = appSetting;
        }
        #endregion

        public IDbConnection DbConnection => new SqlConnection(_appSetting.ConnectionStrings.MsSql);
    }
}
