using Assets.Model;
using Assets.Model.Base;
using Assets.Utility;
using Assets.Utility.Infrastructure;
using Core.Application;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.WebApi.FilterAttributes {
    public class ArgumentBinderFilter: ActionFilterAttribute {
        #region ctor
        protected readonly IAccountDeviceService _accountDeviceService;
        public bool ThrowException { get; set; } = false;
        public bool FillValues { get; set; } = false;

        public ArgumentBinderFilter() {
            _accountDeviceService = ServiceLocator.Current.GetInstance<IAccountDeviceService>();
        }
        #endregion

        public override void OnActionExecuting(ActionExecutingContext context) {
            foreach(var param in context.ActionArguments) {
                if(param.Value is IBaseModel) {
                    var properties = param.Value.GetType().GetProperties();
                    foreach(var item in properties) {
                        if(!string.IsNullOrWhiteSpace(item.Name)) {
                            switch(item.Name.ToLower()) {
                                case "token":
                                    var token = context.HttpContext.Request.Headers.FirstOrDefault(f => f.Key.ToLower().Equals("token"));
                                    if(token.Value.Any()) {
                                        var headerToken = token.Value[0];
                                        item.SetValue(param.Value, headerToken);

                                        if(ThrowException) {
                                            var device = _accountDeviceService.First(f => f.Token.Equals(headerToken));
                                            if(device == null) {
                                                throw new ArgumentException("Invalid token",
                                                    new Exception { Source = GlobalVariables.SystemGeneratedMessage });
                                            }
                                            if(device.StatusId != Status.Active) {
                                                throw new MemberAccessException("You're not an active user, Please confirm your Email or Phone number.",
                                                    new Exception { Source = GlobalVariables.SystemGeneratedMessage });
                                            }

                                            if(FillValues) {
                                                param.Value.GetType().GetProperties()
                                                    .FirstOrDefault(f => f.Name.ToLower().Equals("accountid"))
                                                    .SetValue(param.Value, device.AccountId);
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }
}