using Assets.Model.Base;
using Assets.Model.Binding;
using Assets.Model.Header;
using Assets.Model.Test;
using Assets.Model.View;
using Assets.Utility.Extension;
using AutoMapper;
using Core.Domain.StoredProcedure.Result;
using Core.Domain.StoredProcedure.Schema;
using System;
using System.Text;

namespace Presentation.WebApi.Infrastructure {
    public class MapperProfile: Profile {
        public MapperProfile() {

            //CreateMap<MimeTypeModel, ContentType>()
            //    .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.KindOfDocument));

            CreateMap<ClipboardGetBindingModel, ClipboardGetPagingSchema>();

            CreateMap<ClipboardResult, ClipboardViewModel>();

            CreateMap<ClipboardAddBindingModel, ClipboardAddSchema>()
                .ForMember(dst => dst.TypeId, opt => opt.MapFrom(src => ContentType.txt.ToInt()))
                .ForMember(dst => dst.StatusId, opt => opt.MapFrom(src => Status.Active))
                .ForMember(dst => dst.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dst => dst.Content, opt => opt.MapFrom(src => Convert.ToBase64String(Encoding.UTF8.GetBytes(src.Content))));

            CreateMap<ClipboardResult, ClipboardViewModel>()
                .ForMember(dst => dst.Icon, opt => opt.MapFrom(src => "file-text"))
                .ForMember(dst => dst.TypeName, opt => opt.MapFrom(src => "text"))
                .ForMember(dst => dst.Content, opt => opt.MapFrom(src => Encoding.UTF8.GetString(Convert.FromBase64String(src.Content))));

            //CreateMap<Account, AccountAuthenticateResult>();
            //
        }
    }
}
