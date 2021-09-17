using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AstuteTec.Models
{
    /// <summary>
    /// 用户
    /// </summary>
    //[Table("UserInfo")]
    public class User : BaseModel
    {
        public User()
        {

        }

        /// <summary>
        /// 账号
        /// </summary>
        [Required]
        [MaxLength(30)]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Password { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [MaxLength(200)]
        public string Email { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [MaxLength(11)]
        public string Cellphone { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人Id
        /// </summary>
        [Required]
        public Guid CreateUserId { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [Required]
        [DefaultValue(false)]
        public bool Removed { get; set; }
    }
}
