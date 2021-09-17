using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace AstuteTec.Models.Dto
{
    public class DictionaryDtoMapper : IDtoMapper
    {
        public void CreateMappings(IMapperConfigurationExpression x)
        {
            x.CreateMap<DictionaryInDto, Dictionary>();

            x.CreateMap<Dictionary, DictionaryOutDto>();

            x.CreateMap<Dictionary, DictionaryWithItemOutDto>();

            x.CreateMap<DictionaryItemInDto, DictionaryItem>();

            x.CreateMap<DictionaryItem, DictionaryItemOutDto>();
        }
    }
}
