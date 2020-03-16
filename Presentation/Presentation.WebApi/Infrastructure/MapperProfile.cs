using Assets.Model.Binding;
using Assets.Model.Test;
using Assets.Model.View;
using AutoMapper;
using Core.Domain.Entities;

namespace Presentation.WebApi.Infrastructure {
    public class MapperProfile: Profile {
        #region ctor
        public MapperProfile() {

            CreateMap<MimeTypeModel, ContentType>()
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.KindOfDocument));

            CreateMap<ContentType, PrimaryKeyViewModel>();
        }
        #endregion
    }
}
