using System;
using System.Collections.Generic;
using System.Data;
using Assets.Model.Common;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace Core.Domain {
    public class ConnectionPool {
        #region ctor
        private readonly AppSetting _appSetting;

        public ConnectionPool(AppSetting appSetting) {
            _appSetting = appSetting;
        }
        #endregion

        public IDbConnection MsSqlConnection => new SqlConnection(_appSetting.ConnectionStrings.MsSql);
        public IDbConnection MySqlConnection => new MySqlConnection(_appSetting.ConnectionStrings.MsSql);
    }
}
