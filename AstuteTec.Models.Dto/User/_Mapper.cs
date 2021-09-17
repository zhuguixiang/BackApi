using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AstuteTec.Models.Dto
{
    public class UserMapper : IDtoMapper
    {
        public void CreateMappings(IMapperConfigurationExpression x)
        {
            x.CreateMap<User, UserOutDto>();

            x.CreateMap<UserInDto, User>();
        }
    }
}
