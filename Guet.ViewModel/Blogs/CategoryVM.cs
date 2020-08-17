using System;
using System.Collections.Generic;
using System.Text;
using Guet.Entities.Blogs;

namespace Guet.ViewModels.Blogs
{
    public class CategoryVM : EntityBaseVM
    {
        public string Name { get; set; }

        public List<ArticleVM> Articles { get; set; }

        public CategoryVM()
        {
            Articles = new List<ArticleVM>();
        }
        public CategoryVM(Category category)
        {
            Articles = new List<ArticleVM>();
            Id = category.Id;
            Name = category.Name;
            CreateTime = category.CreateTime;
            UpdateTime = category.UpdateTime;
        }

    }
}
