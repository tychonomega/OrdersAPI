using App.Data.Models;
using App.Models.DTO;
using AutoMapper;

namespace App.Mapping
{
    public class ObjectProfile : Profile
    {
        public ObjectProfile()
        {
            CreateMap<Order, PostOrderDto>();
            CreateMap<PostOrderDto, Order>();

            CreateMap<Order, GetOrderDto>();
            CreateMap<GetOrderDto, Order>();
        }
    }
}
