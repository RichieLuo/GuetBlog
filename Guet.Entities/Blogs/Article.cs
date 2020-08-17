using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Guet.Entities.ApplicationOrganization;

namespace Guet.Entities.Blogs
{
    /// <summary>
    /// 博客文章
    /// </summary>
    public class Article : EntityBase, IEntityBase
    {
        [Required]
        public Guid CategoryId { get; set; }
        /// <summary>
        /// 文章标题
        /// </summary>
        [Required]
        public string Title { get; set; }
        /// <summary>
        /// 文章封面
        /// </summary>
        public string Cover { get; set; }
        /// <summary>
        /// 文章摘要
        /// </summary>
        public string Abstract { get; set; }
        /// <summary>
        /// 文章内容
        /// </summary>
        [Required]
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public int VisitCount { get; set; }
        public int LikeCount { get; set; }
        /// <summary>
        /// 是否原创
        /// </summary>
        public bool IsOwn { get; set; }
        /// <summary>
        /// 非原创描述
        /// </summary>
        public string IsNotOwnDesctiption { get; set; }

        public bool IsRecommend { get; set; }
    }
}
