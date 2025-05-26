using AutoMapper;
using MusicRadio.Application.DTOs;
using MusicRadio.Web.ViewModels;

namespace MusicRadio.Web.Mapping
{
    public class WebMappingProfile : Profile
        {
            public WebMappingProfile()
            {
            // Configuración DTO → ViewModel
            CreateMap<LoginViewModel, LoginDto>().ReverseMap();
            CreateMap<RegisterViewModel, ClientDto>().ReverseMap();
            CreateMap<AlbumViewModel, AlbumSetDto>().ReverseMap();
            CreateMap<SongsViewModel, SongSetDto>().ReverseMap();
            CreateMap<PurchaseDetailViewModel, PurchaseDetailDto>().ReverseMap();
        }
        }
    }

