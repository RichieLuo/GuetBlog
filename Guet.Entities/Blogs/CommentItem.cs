using System;
using System.Collections.Generic;
using System.Text;

namespace Guet.Entities.Blogs
{
    public class CommentItem : EntityBase
    {
        public string Content { get; set; }
        public Guid ParentId { get; set; } 

        public string UserId { get; set; }
        public string NickName { get; set; }
        public string SiteUrl { get; set; }

        public string EMail { get; set; }
        public string Avatar { get; set; }
    }
}
