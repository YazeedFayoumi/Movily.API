using AutoMapper;
using test1.Dto;
using test1.Models;

namespace test1.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerDto>();
            CreateMap<Movie, MovieDto>();
            CreateMap<Genre, GenreDto>();
            CreateMap<CustomerRole, CustomerRoleDto>();
        }
    }
}
