using AutoMapper;
using SampleWebApiAspNetCore.Dtos;
using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.MappingProfiles
{
    public class GenMappings : Profile
    {
        public GenMappings()
        {
            CreateMap<GenshinEntity, GenDto>().ReverseMap();
            CreateMap<GenshinEntity, GenUpdateDto>().ReverseMap();
            CreateMap<GenshinEntity, GenCreateDto>().ReverseMap();
        }
    }
}
