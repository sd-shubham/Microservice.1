using AutoMapper;
using Sample.Catalog.Service.Dtos;
using Sample.Catalog.Service.Entity;

namespace Sample.Catalog.Service.Mapper
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<CreateItemDto, Item>();
            CreateMap<Item, GetItemDto>();
        }
    }
}