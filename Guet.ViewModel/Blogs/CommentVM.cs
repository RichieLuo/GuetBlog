using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Guet.Entities.ApplicationOrganization;
using Guet.Entities.Blogs;
using Guet.ViewModels;
using Guet.ViewModels.ApplicationOrganization;
using Guet.ViewModels.Blogs;

namespace Guet.ViewModels.Blogs
{
    /// <summary>
    /// 评论
    /// </summary> 
    public class CommentVM : EntityBaseVM
    {
        public string Content { get; set; }

        public Guid ArticleId { get; set; }

        public string UserId { get; set; }
        public string NickName { get; set; }
        public string SiteUrl { get; set; }
        public bool IsAdmin { get; set; } = false;
        public string EMail { get; set; }
        public string CTime { get; set; }
        public string Avatar { get; set; }
        public bool CanDelete { get; set; }  
        public List<CommentItemVM> Children { get; set; }
        public ApplicationUserVM User { get; set; }

        public CommentVM() { }
        public CommentVM(Comment c)
        {
            Id = c.Id;
            Content = c.Content;
            ArticleId = c.ArticleId;
            UserId = c.UserId;
            NickName = c.NickName;
            SiteUrl = c.SiteUrl; 
            EMail = c.EMail;
            Children = new List<CommentItemVM>();
            User = new ApplicationUserVM();
            CreateTime = c.CreateTime;
            Avatar = c.Avatar;
        }

    }
}
