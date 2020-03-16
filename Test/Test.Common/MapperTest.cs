using Assets.Model.Base;
using Assets.Model.Binding;
using Assets.Utility;
using AutoMapper;
using Core.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Common {
    [TestClass]
    public class MapperTest {
        #region ctor
        private readonly IMapper _mapper;

        public MapperTest() {
            _mapper = ServiceLocator.Current.GetInstance<IMapper>();
        }
        #endregion

        [TestMethod, TestCategory("Mapper"), TestCategory("BindingModelToEntity")]
        public void ModelToEntity() {
            //var entity = _mapper.Map<UpdateState>(new UpdateStateBindingModel {
            //    Description = "",
            //    StatusId = UpdateStatus.Requested
            //});

            //Assert.IsTrue(entity.StatusId.Equals(UpdateStatus.Requested));
        }

        [TestMethod, TestCategory("Mapper"), TestCategory("EntityToModelView")]
        public void EntityToModel() {
            //var model = _mapper.Map<UpdateStateViewModel>(new UpdateState {
            //    Description = "",
            //    StatusId = UpdateStatus.Succeed
            //});

            //Assert.IsTrue(model.StatusId.Equals(UpdateStatus.Succeed));
        }
    }
}
