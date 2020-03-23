using Assets.Model.Base;
using Assets.Model.Binding;
using Assets.Model.Test;
using Assets.Model.View;
using AutoMapper;
using Core.Domain.Entities;
using System;
using System.Text;

namespace Presentation.WebApi.Infrastructure {
    public class MapperProfile: Profile {
        public MapperProfile() {

            CreateMap<MimeTypeModel, ContentType>()
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.KindOfDocument));

            CreateMap<ContentType, PrimaryKeyViewModel>();

            CreateMap<ClipboardGetBindingModel, Clipboard>()
                .ForMember(dst => dst.DeviceId, opt => opt.Ignore());

            CreateMap<ClipboardAddBindingModel, Clipboard>()
                .ForMember(dst => dst.DeviceId, opt => opt.Ignore())
                .ForMember(dst => dst.TypeId, opt => opt.MapFrom(src => 34))
                .ForMember(dst => dst.StatusId, opt => opt.MapFrom(src => Status.Active))
                .ForMember(dst => dst.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dst => dst.Content, opt => opt.MapFrom(src => Convert.ToBase64String(Encoding.UTF8.GetBytes(src.Content))));

            CreateMap<Clipboard, ClipboardViewModel>()
                .ForMember(dst => dst.Content, opt => opt.MapFrom(src => Encoding.UTF8.GetString(Convert.FromBase64String(src.Content))));

        }
    }
}
