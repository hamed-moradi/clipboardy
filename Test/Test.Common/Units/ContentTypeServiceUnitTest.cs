using Assets.Model.Test;
using Assets.Utility;
using AutoMapper;
using Core.Application;
using Core.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Test.Common.Units {
    [TestClass]
    public class ContentTypeServiceUnitTest {
        #region ctor
        private readonly IMapper _mapper;
        //private readonly IContentTypeService _contentTypeService;

        public ContentTypeServiceUnitTest() {
            _mapper = ServiceLocator.Current.GetInstance<IMapper>();
            //_contentTypeService = ServiceLocator.Current.GetInstance<IContentTypeService>();
        }
        #endregion

        [TestMethod, TestCategory("ContentType"), TestCategory("FillTable")]
        public void FillTable() {
            var filepath = $"{Environment.CurrentDirectory}\\mimeTypesJson.txt";
            var json = File.ReadAllText(filepath);
            var mimetypes = JsonConvert.DeserializeObject<List<MimeTypeModel>>(json);

            //_contentTypeService.BulkInsertAsync(_mapper.Map<List<ContentType>>(mimetypes))
            //    .GetAwaiter().GetResult();
        }
    }
}
