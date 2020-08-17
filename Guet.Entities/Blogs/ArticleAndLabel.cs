using System;
using System.Collections.Generic;
using System.Text;

namespace Guet.Entities.Blogs
{
    /// <summary>
    /// 文章和标签中间表
    /// </summary>
    public class ArticleAndLabel : EntityBase
    {
        public Guid ArticleId { get; set; }
        public Guid LabelId { get; set; }

    }
}
