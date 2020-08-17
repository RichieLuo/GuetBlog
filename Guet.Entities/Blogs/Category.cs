using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Guet.Entities.Blogs
{
    /// <summary>
    /// 文章类别
    /// </summary>
    public class Category : EntityBase
    {
        /// <summary>
        /// 文章类别名称
        /// </summary>
        [Required]
        public virtual string Name { get; set; }   
    }
}
