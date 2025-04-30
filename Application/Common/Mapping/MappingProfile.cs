using Application.Commands.UserCommands.RegisterUser;
using Application.DTOs;
using Application.Queries.UserQueries.AuthenticateUser;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LoginUserDto, AuthenticateUserQuery>();
            CreateMap<RegisterUserDto, RegisterUserCommand>();
        }
    }
}
