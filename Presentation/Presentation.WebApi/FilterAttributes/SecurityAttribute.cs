using Assets.Model;
using Assets.Model.Base;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Linq;

namespace Presentation.WebApi.FilterAttributes {
    public class SecurityAttribute: ActionFilterAttribute {

        #region Private
        private void KeywordChecker(ActionExecutingContext filterContext, string text) {
            if(GlobalVariables.ClientUnsafeKeywords.Contains(text.ToLower()) ||
                GlobalVariables.SqlUnsafeKeywords.Contains(text.ToLower())) {
                Log.Information(JsonConvert.SerializeObject(new {
                    Account = filterContext.HttpContext.User.Identity.Name,
                    IP = filterContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                    Method = filterContext.Controller.ToString(),
                    Keyword = text,
                    Message = InternalMessage.RestrictedKeywordDetection
                }));
                throw new Exception(GlobalVariables.ExceptionSource);
            }
        }
        #endregion

        public override void OnActionExecuting(ActionExecutingContext context) {
            foreach(var param in context.ActionArguments) {
                if(param.Value is string && param.Value != null) {
                    //filterContext.ActionParameters[param.Key] = new KeyValuePair<string, object>(param.Key, param.Value.ToString().CharacterNormalizer());
                    KeywordChecker(context, param.Value.ToString());
                }
                if(param.Value is IBaseBindingModel) {
                    var properties = param.Value.GetType().GetProperties();
                    foreach(var item in properties) {
                        if(item.PropertyType == typeof(string) && item != null) {
                            //filterContext.ActionParameters[param.Key] = new KeyValuePair<string, object>(param.Key, param.Value.ToString().CharacterNormalizer());
                            KeywordChecker(context, item.ToString());
                        }
                    }
                }
            }
        }
    }
}
