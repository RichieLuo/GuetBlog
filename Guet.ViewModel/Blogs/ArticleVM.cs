using System;
using System.Collections.Generic;
using System.Text;
using Guet.Entities.Blogs;
using Guet.ViewModels.ApplicationOrganization;

namespace Guet.ViewModels.Blogs
{

    public class ArticleVM : EntityBaseVM
    {
        /// <summary>
        /// 是否原创
        /// </summary>
        public bool IsOwn { get; set; }
        /// <summary>
        /// 非原创描述
        /// </summary>
        public string IsNotOwnDesctiption { get; set; }

        public string Title { get; set; }

        public string Cover { get; set; }

        public string Abstract { get; set; }

        public string Content { get; set; }

        public int VisitCount { get; set; }
        public int LikeCount { get; set; }
        public bool IsRecommend { get; set; }
        public string CTime { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUserVM User { get; set; }
        public Guid CategoryId { get; set; }
        public CategoryVM Category { get; set; }
        public List<LabelVM> Labels { get; set; }
        public ArticleVM() { }

        public ArticleVM(Article a)
        {
            Id = a.Id;
            Title = a.Title;
            Cover = a.Cover;
            Abstract = a.Abstract;
            Content = a.Content;
            VisitCount = a.VisitCount;
            LikeCount = a.LikeCount;
            CategoryId = a.CategoryId;
            CreateTime = a.CreateTime;
            UpdateTime = a.UpdateTime;
            IsOwn = a.IsOwn;
            IsNotOwnDesctiption = a.IsNotOwnDesctiption;
            IsRecommend = a.IsRecommend;
            UserId = a.UserId;
            User = new ApplicationUserVM();
            Category = new CategoryVM();
            Labels = new List<LabelVM>();
        }
    }
}
