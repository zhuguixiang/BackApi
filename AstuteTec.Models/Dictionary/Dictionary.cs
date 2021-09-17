using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AstuteTec.Models
{
    /// <summary>
    /// 字典主表
    /// </summary>
    //[Table("Dict")]
    public class Dictionary: BaseModel
    {
        public Dictionary()
        {
           
        }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// 键
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Key { get; set; }

        public virtual ICollection<DictionaryItem> DictionaryItem { get; set; }
    }
}
