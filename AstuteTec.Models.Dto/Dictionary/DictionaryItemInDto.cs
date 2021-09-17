using System;
using System.Collections.Generic;
using System.Text;

namespace AstuteTec.Models.Dto
{
    public class DictionaryItemInDto : BaseInModelDto
    {

        public Guid DictionaryId { get; set; }

        public string Text { get; set; }

        public string Key { get; set; }

        public int NumericalOrder { get; set; }

      
    }
}
