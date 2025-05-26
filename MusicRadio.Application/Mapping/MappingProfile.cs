using AutoMapper;
using MusicRadio.Application.DTOs;
using MusicRadio.Core.Entities;
using MusicRadio.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRadio.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<AlbumSet, AlbumSetDto>().ReverseMap();
            CreateMap<SongSet, SongSetDto>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();

            CreateMap<ClientDto, Client>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => PasswordHasher.HashPassword(src.Password)));

            CreateMap<Client, ClientDto>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());

            CreateMap<PurchaseDetail, PurchaseDetailDto>().ReverseMap()
           .ForMember(dest => dest.Client, opt => opt.Ignore())
           .ForMember(dest => dest.AlbumSet, opt => opt.Ignore());

        }
    }
}
