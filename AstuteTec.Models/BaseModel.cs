using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AstuteTec.Models
{
    public class BaseModel
    {
        /// <summary>
        /// GUID
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        public BaseModel()
        {
            Id = Guid.NewGuid();
        }
    }
}
