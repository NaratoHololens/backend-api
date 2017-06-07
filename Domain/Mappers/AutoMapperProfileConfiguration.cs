using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Model.Dto;
using Model.Models;

namespace Domain.Mappers
{

    public class AutoMapperProfileConfiguration : Profile
    {

        public AutoMapperProfileConfiguration()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<Log, LogDto>();
            CreateMap<LogDto, Log>();
        }
    }
}
