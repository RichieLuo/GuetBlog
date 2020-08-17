using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Guet.Entities.ApplicationOrganization;

namespace Guet.Entities.Blogs
{
    /// <summary>
    /// 评论
    /// </summary> 
    public class Comment : EntityBase
    { 
        public string Content { get; set; } 
         
        public Guid ArticleId { get; set; } 
         
        public string UserId { get; set; }
        public string NickName { get; set; }
        public string SiteUrl { get; set; }

        public string EMail { get; set; }
        public string Avatar { get; set; }

    }
}
