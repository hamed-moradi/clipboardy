using Assets.Utility;
using Assets.Utility.Infrastructure;
using Core.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace Test.Common.Utility {
    [TestClass]
    public class ExpressionTest {
        #region ctor

        public ExpressionTest() {
        }
        #endregion

        [TestMethod, TestCategory("Expression"), TestCategory("ToSQLQuery")]
        public void ToSQLQuery() {
            Expression<Func<Clipboard, bool>> predicate = p => p.Id == 0;

            //string sql = ((ObjectQuery)predicate).ToTraceString();
            //var tsql=predicate.toTraceString()
        }
    }
}
