using System;
using System.Collections.Generic;
using System.Text;

namespace Guet.ViewModels.Blogs
{
  
    public class ArticleAndLabelVM : EntityBaseVM
    {
        public Guid ArticleId { get; set; }
        public Guid LabelId { get; set; }

    }
}
