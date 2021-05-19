using AutoMapper;
using Linkdeed.DTO;
using Linkdeed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Linkdeed.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<RegisterModel, User>();
            CreateMap<RegisterAdminModel, User>();
            CreateMap<UpdateModel, User>();
        }
    }
}
