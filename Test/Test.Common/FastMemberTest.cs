using FastMember;
using Core.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;
using System;
using Assets.Model.Base;

namespace Test.Common {
    [TestClass]
    public class FastMemberTest {
        #region ctor

        public FastMemberTest() {
        }
        #endregion

        [TestMethod, TestCategory("FastMember"), TestCategory("GetMethod")]
        public void GetMethod() {
            var cb = new Clipboard();

            var propName = "Id";

            var accessor = TypeAccessor.Create(cb.GetType());
            var getVal = accessor[cb, propName];
            accessor[cb, propName] = (int)getVal + 1;

            var wrapped = ObjectAccessor.Create(cb);
            var val = wrapped[propName];
            wrapped[propName] = (int)val + 1;

            Assert.IsTrue(cb.Id == 2);
        }

        [TestMethod, TestCategory("FastMember"), TestCategory("ReflectExpression")]
        public void ReflectExpression() {
            //var entity =
            Expression<Func<BaseEntity, bool>> predicate = p => p.Id == 0;

            var accessor = TypeAccessor.Create(predicate.Body.Type);
            //var getVal = accessor[predicate, nameof(BaseEntity.TotalCount)];

            var wrapped = ObjectAccessor.Create(predicate.Body.Type);
            //var val = wrapped[nameof(BaseEntity.TotalCount)];

            Assert.AreEqual(wrapped, typeof(BaseEntity));
        }
    }
}
