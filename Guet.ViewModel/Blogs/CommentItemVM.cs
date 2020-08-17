using System;
using System.Collections.Generic;
using System.Text;
using Guet.Entities.Blogs;
using Guet.ViewModels.ApplicationOrganization;

namespace Guet.ViewModels.Blogs
{
    public class CommentItemVM : EntityBaseVM
    {
        public string Content { get; set; }
        public Guid ParentId { get; set; }

        public string UserId { get; set; }
        public string NickName { get; set; }
        public string SiteUrl { get; set; }
        public string CTime { get; set; }
        public string EMail { get; set; }
        public bool IsAdmin { get; set; } = false;
        public string Avatar { get; set; }
        public ApplicationUserVM User { get; set; }
        public bool CanDelete { get; set; }
        public CommentItemVM() { }
        public CommentItemVM(CommentItem c)
        {
            Id = c.Id;
            Content = c.Content;
            ParentId = c.ParentId;
            UserId = c.UserId;
            NickName = c.NickName;
            SiteUrl = c.SiteUrl;
            EMail = c.EMail;
            User = new ApplicationUserVM();
            CreateTime = c.CreateTime;
            Avatar = c.Avatar;
        }

    }
}
