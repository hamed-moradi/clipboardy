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
        protected readonly CompressionHandler _compressionHandler;
        public bool FillValues { get; set; } = false;
        public bool ThrowException { get; set; } = false;

        public ArgumentBinderFilter() {
            _compressionHandler = ServiceLocator.Current.GetInstance<CompressionHandler>();
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
                                        var headerToken = _compressionHandler.Decompress(token.Value[0]);
                                        item.SetValue(param.Value, headerToken);
                                        if(FillValues) {
                                            var client = new { Id = 1 };
                                            if(client != null) {
                                                param.Value.GetType().GetProperties()
                                                    .FirstOrDefault(f => f.Name.ToLower().Equals("clientconnectionid"))
                                                    .SetValue(param.Value, client.Id);
                                            }
                                            else {
                                                if(ThrowException) {
                                                    throw new ArgumentException("Invalid token",
                                                        new Exception { Source = GlobalVariables.SystemGeneratedMessage });
                                                }
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