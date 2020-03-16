using Assets.Model.Binding;
using Assets.Model.View;
using AutoMapper;
using Core.Domain.Entities;

namespace Presentation.WebApi.Infrastructure {
    public class MapperProfile: Profile {
        #region ctor
        public MapperProfile() {
            CreateMap<ContentType, PrimaryKeyViewModel>();
        }
        #endregion
    }
}
