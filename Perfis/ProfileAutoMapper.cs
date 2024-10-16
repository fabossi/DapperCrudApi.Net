using AutoMapper;
using DapperCrudApi.Dto;
using DapperCrudApi.Models;

namespace DapperCrudApi.Perfis
{
    public class ProfileAutoMapper : Profile
    {
        public ProfileAutoMapper()
        {
            CreateMap<UserModel, UserListDto>();
        }
    }
}
