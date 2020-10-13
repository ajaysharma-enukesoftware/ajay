using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IVPD.Models;


namespace IVPD.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AuthenticateModel, User>();
            CreateMap<SignupModel,User>();
           
            CreateMap<RegisterUserModel, User>();
        }
    }
}
