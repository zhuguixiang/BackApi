using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AstuteTec.Models.Dto;
using Sheng.Kernal;
using Sheng.Web.Infrastructure;

namespace AstuteTec.Api
{
    public class AutoMapperConfig
    {
        private static bool _Initialized = false;

        public static void Initialize()
        {
            if (_Initialized)
                return;

            Mapper.Initialize(Configuration);

            _Initialized = true;

        }

        public static void Configuration(IMapperConfigurationExpression x)
        {
            List<Type> dtoMapperTypeList = ReflectionHelper.GetTypeListBaseOn<IDtoMapper>();

            foreach (var item in dtoMapperTypeList)
            {
                IDtoMapper dtoMapper = (IDtoMapper)Activator.CreateInstance(item);
                dtoMapper.CreateMappings(x);
            }
        }
    }
}
