﻿using Assets.Model.Common;
using Serilog;

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
      if(string.IsNullOrWhiteSpace(_appSetting.ConnectionStrings?.Postgres)) {
        Log.Error("There is no connection string to MsSqldb.");
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
