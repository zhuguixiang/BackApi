using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace AstuteTec.Models.Dto
{
    public interface IDtoMapper
    {
        void CreateMappings(IMapperConfigurationExpression configuration);
    }
}
