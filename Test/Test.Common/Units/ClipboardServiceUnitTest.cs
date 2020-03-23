using Assets.Model.Base;
using Assets.Model.Test;
using Assets.Model.View;
using Assets.Utility;
using AutoMapper;
using Core.Application;
using Core.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Test.Common.Units {
    [TestClass]
    public class ClipboardServiceUnitTest {
        #region ctor
        private readonly IMapper _mapper;
        private readonly IClipboardService _clipboardService;

        public ClipboardServiceUnitTest() {
            _mapper = ServiceLocator.Current.GetInstance<IMapper>();
            _clipboardService = ServiceLocator.Current.GetInstance<IClipboardService>();
        }
        #endregion

        [TestMethod, TestCategory("Clipboard"), TestCategory("GetPagingByEntity")]
        public void GetPagingByEntity() {
            var result = _clipboardService.GetPaging(new Clipboard(), new QuerySetting());
            Assert.IsTrue(result.Result.Any());
            Assert.IsTrue(result.TotalCount > 0);
            Assert.IsTrue(result.TotalPage > 0);
        }

        [TestMethod, TestCategory("Clipboard"), TestCategory("GetPagingByPredicate")]
        public void GetPagingByPredicate() {
            var result = _clipboardService.GetPaging(p => p.Id == 1, new QuerySetting());
            Assert.IsTrue(result.Result.Any());
            Assert.IsTrue(result.TotalCount > 0);
            Assert.IsTrue(result.TotalPage > 0);
        }

        [TestMethod, TestCategory("Clipboard"), TestCategory("GetPagingModelByEntity")]
        public void GetPagingModelByEntity() {
            var result = _clipboardService.GetPaging<ClipboardViewModel>(new Clipboard(), new QuerySetting());
            Assert.IsTrue(result.Result.Any());
            Assert.IsTrue(result.TotalCount > 0);
            Assert.IsTrue(result.TotalPage > 0);
        }

        [TestMethod, TestCategory("Clipboard"), TestCategory("GetPagingModelByPredicate")]
        public void GetPagingModelByPredicate() {
            var result = _clipboardService.GetPaging<ClipboardViewModel>(p => p.Id == 1, new QuerySetting());
            Assert.IsTrue(result.Result.Any());
            Assert.IsTrue(result.TotalCount > 0);
            Assert.IsTrue(result.TotalPage > 0);
        }
    }
}
