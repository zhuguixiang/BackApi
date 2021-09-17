using System;
using System.Collections.Generic;
using System.Text;

namespace AstuteTec.Models.Dto
{
    public class DictionaryWithItemOutDto : DictionaryOutDto
    {
        public virtual ICollection<DictionaryItemOutDto> DictionaryItem { get; set; }
    }
}
