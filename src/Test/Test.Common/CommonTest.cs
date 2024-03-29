﻿using Core.Application._App;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Test.Common {
  [TestClass]
  public class CommonTest {
    #region ctors
    private readonly BaseService _baseService;

    public CommonTest() {
      _baseService = new BaseService();
    }
    #endregion

    [TestMethod, TestCategory("Common"), TestCategory("DbConnection")]
    public void DbConnection() {
      Assert.IsTrue(_baseService.PostgresContext.Database.CanConnect());
    }

    [TestMethod, TestCategory("Common"), TestCategory("GetCorrectId")]
    public void GetCorrectId() {
    }

    [TestMethod, TestCategory("Common"), TestCategory("StringToJson")]
    public void StringToJson() {
      var strJson = "[{\"ID\":1,\"DCCode\":\"83\",\"DCName\":\"کرمانشاه\",\"DCCategory\":1,\"ManagerName\":\"\",\"Operator1\":\"\",\"Address\":\"\",\"Operator2\":\"\",\"Phone\":\"\",\"PostCode\":\"\",\"Status\":1,\"FaxNo\":\"\",\"DLCode\":\"1500001\",\"AreaRef\":null,\"FifthCode\":\"\",\"SixthCode\":\"8300\",\"ModifiedDateBeforeSend\":null,\"UserRefBeforeSend\":0,\"SafeDLCode\":\"\",\"DCGUID\":\"25ABC9F8-A6E4-4929-97BB-DDBB2A2CEC40\"},{\"ID\":0,\"DCCode\":\"0\",\"DCName\":\"ستاد مرکز\",\"DCCategory\":0,\"ManagerName\":null,\"Operator1\":null,\"Address\":null,\"Operator2\":null,\"Phone\":null,\"PostCode\":null,\"Status\":1,\"FaxNo\":null,\"DLCode\":null,\"AreaRef\":null,\"FifthCode\":null,\"SixthCode\":null,\"ModifiedDateBeforeSend\":null,\"UserRefBeforeSend\":null,\"SafeDLCode\":null,\"DCGUID\":\"DA4C6154-87BF-4ED3-A14A-096A98DD83B5\"}]";
      var siteInfos = JsonConvert.DeserializeObject(strJson);
      Assert.IsTrue(siteInfos != null);
    }

    [TestMethod, TestCategory("Common"), TestCategory("PropertyMapper")]
    public void PropertyMapper() {

    }
  }
}
