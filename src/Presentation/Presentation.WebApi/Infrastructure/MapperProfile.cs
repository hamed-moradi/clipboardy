using Assets.Model.Base;
using Assets.Model.Binding;
using Assets.Model.View;
using Assets.Utility.Extension;
using AutoMapper;
using Core.Domain.Entities;
using System;
using System.Text;

namespace Presentation.WebApi.Infrastructure {
  public class MapperProfile: Profile {
    public MapperProfile() {

      //CreateMap<MimeTypeModel, ContentType>()
      //  .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.KindOfDocument));

      //CreateMap<ClipboardGetBindingModel, ClipboardGetPagingSchema>();

      //CreateMap<ClipboardResult, ClipboardViewModel>();

      CreateMap<ClipboardAddBindingModel, Clipboard>()
        .ForMember(dst => dst.type_id, opt => opt.MapFrom(src => ContentTypeEnum.txt.ToInt()))
        .ForMember(dst => dst.content, opt => opt.MapFrom(src => Convert.ToBase64String(Encoding.UTF8.GetBytes(src.Content))));

      CreateMap<Clipboard, ClipboardViewModel>()
        .ForMember(dst => dst.Icon, opt => opt.MapFrom(src => "file-text"))
        .ForMember(dst => dst.TypeName, opt => opt.MapFrom(src => "text"))
        .ForMember(dst => dst.TypeId, opt => opt.MapFrom(src => src.type_id))
        .ForMember(dst => dst.InsertedAt, opt => opt.MapFrom(src => src.inserted_at))
        .ForMember(dst => dst.Content, opt => opt.MapFrom(src => Encoding.UTF8.GetString(Convert.FromBase64String(src.content))));

      //CreateMap<Account, AccountAuthenticateResult>();
      //
    }
  }
}
