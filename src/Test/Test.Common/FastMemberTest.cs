using Assets.Model.Base;
using Assets.Model.Header;
using FastMember;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.Expressions;

namespace Test.Common {
  [TestClass]
  public class FastMemberTest {
    #region ctor

    public FastMemberTest() {
    }
    #endregion

    [TestMethod, TestCategory("FastMember"), TestCategory("GetMethod")]
    public void GetMethod() {
      var cb = new Device();

      var propName = "Id";

      var accessor = TypeAccessor.Create(cb.GetType());
      var getVal = accessor[cb, propName];
      accessor[cb, propName] = (int)getVal + 1;

      var wrapped = ObjectAccessor.Create(cb);
      var val = wrapped[propName];
      wrapped[propName] = (int)val + 1;
      var ifexist = wrapped["Id"];

      Assert.IsTrue(cb.DeviceId == "2");
    }

    [TestMethod, TestCategory("FastMember"), TestCategory("IfExist")]
    public void IfExist() {
      var cb = new Device();
      var propName = "Id2";

      var accessor = TypeAccessor.Create(cb.GetType());
      var ifexist1 = accessor[cb, propName];

      var wrapped = ObjectAccessor.Create(cb);
      var ifexist2 = wrapped[propName];
    }

    [TestMethod, TestCategory("FastMember"), TestCategory("ReflectExpression")]
    public void ReflectExpression() {
      Expression<Func<BaseSchema, bool>> predicate = p => p.StatusCode == 0;

      var accessor = TypeAccessor.Create(predicate.Body.Type);
      //var getVal = accessor[predicate, nameof(BaseEntity.TotalCount)];

      var wrapped = ObjectAccessor.Create(predicate.Body.Type);

      //Assert.AreEqual(wrapped, typeof(BaseSchema));
    }
  }
}
