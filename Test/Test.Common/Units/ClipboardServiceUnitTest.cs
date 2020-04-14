using Assets.Utility;
using AutoMapper;
using Core.Application;
using Core.Domain.StoredProcedure.Schema;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var query = new ClipboardGetPagingSchema { };
            var result = _clipboardService.PagingAsync(query);
            Assert.IsTrue(result.Result.Any());
            Assert.IsTrue(query.TotalCount > 0);
            Assert.IsTrue(query.TotalPages > 0);
        }

        [TestMethod, TestCategory("Clipboard"), TestCategory("GetPagingByPredicate")]
        public void GetPagingByPredicate() {
            var query = new ClipboardGetPagingSchema { };
            var result = _clipboardService.PagingAsync(query);
            Assert.IsTrue(result.Result.Any());
            Assert.IsTrue(query.TotalCount > 0);
            Assert.IsTrue(query.TotalPages > 0);
        }

        [TestMethod, TestCategory("Clipboard"), TestCategory("GetPagingModelByEntity")]
        public void GetPagingModelByEntity() {
            var query = new ClipboardGetPagingSchema { };
            var result = _clipboardService.PagingAsync(query);
            Assert.IsTrue(result.Result.Any());
            Assert.IsTrue(query.TotalCount > 0);
            Assert.IsTrue(query.TotalPages > 0);
        }

        [TestMethod, TestCategory("Clipboard"), TestCategory("GetPagingModelByPredicate")]
        public void GetPagingModelByPredicate() {
            var query = new ClipboardGetPagingSchema { };
            var result = _clipboardService.PagingAsync(query);
            Assert.IsTrue(result.Result.Any());
            Assert.IsTrue(query.TotalCount > 0);
            Assert.IsTrue(query.TotalPages > 0);
        }
    }
}
