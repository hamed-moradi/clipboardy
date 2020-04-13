using Core.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Test.Common.Utility {
    [TestClass]
    public class ExpressionTest {
        #region ctor

        public ExpressionTest() {
        }
        #endregion

        public static string GetPropertyName<T>(Expression<Func<T, object>> property) {
            LambdaExpression lambda = property;
            MemberExpression memberExpression;

            if(lambda.Body is UnaryExpression) {
                UnaryExpression unaryExpression = (UnaryExpression)(lambda.Body);
                memberExpression = (MemberExpression)(unaryExpression.Operand);
            }
            else {
                memberExpression = (MemberExpression)(lambda.Body);
            }

            return ((PropertyInfo)memberExpression.Member).Name;
        }

        [TestMethod, TestCategory("Expression"), TestCategory("ToModel")]
        public void ToToModel() {
            //var name = GetPropertyName<Clipboard>(p => p.Id == 0);

            Expression<Func<Clipboard, bool>> expression = p => p.Id == 0 && p.Content.Contains("") && p.CreatedAt <= DateTime.Now;
            var body = expression.Body;

            //Expression<Func<Clipboard>> expression2 = e=>new Clipboard();
            //var body2 = (MemberExpression)expression2.Body;
        }
    }
}
