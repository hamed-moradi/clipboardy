using Assets.Model.Base;
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
using System.Text;

namespace Test.Common.Units {
    [TestClass]
    public class GenericServiceUnitTest {
        #region ctor
        private readonly IGenericService<Clipboard> _genericService;

        public GenericServiceUnitTest() {
            _genericService = ServiceLocator.Current.GetInstance<IGenericService<Clipboard>>();
        }
        #endregion

        [TestMethod, TestCategory("ContentType"), TestCategory("Add")]
        public void Add() {
            var result = _genericService.Add(new Clipboard {
                DeviceId = 2,
                TypeId = 34,
                Content = Convert.ToBase64String(Encoding.UTF8.GetBytes("save and close test")),
                CreatedAt = DateTime.Now,
                StatusId = Status.Active
            });
            Assert.IsNotNull(result);
        }

        [TestMethod, TestCategory("ContentType"), TestCategory("Update")]
        public void Update() {
            var result = _genericService.Update(new Clipboard {
                Id = 1,
                DeviceId = 2,
                TypeId = 34,
                Content = Convert.ToBase64String(Encoding.UTF8.GetBytes("save and close test")),
                CreatedAt = DateTime.Now,
                StatusId = Status.Active
            });
            Assert.IsNotNull(result);
        }
    }
}
