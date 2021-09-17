using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AstuteTec.Models
{
    /// <summary>
    /// 字典明细表
    /// </summary>
    //[Table("DictItem")]
    public partial class DictionaryItem: BaseModel
    {
        /// <summary>
        /// 字典主表Id
        /// </summary>
        [Required]
        public Guid DictionaryId { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Text { get; set; }

        /// <summary>
        /// 键
        /// </summary>
        [MaxLength(100)]
        public string Key { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Required]
        public int NumericalOrder { get; set; }

        /// <summary>
        /// 字典主表
        /// </summary>
        [ForeignKey("DictionaryId")]
        public virtual Dictionary Dictionary { get; set; }
    }
}
