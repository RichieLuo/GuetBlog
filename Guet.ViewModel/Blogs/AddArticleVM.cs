using System;
using System.Collections.Generic;
using System.Text;

namespace Guet.ViewModels.Blogs
{
    /// <summary>
    /// 新建或更新文章
    /// </summary>
    public class AddArticleVM : EntityBaseVM
    {
        public bool IsNew { get; set; } = true;
        public ArticleVM Article { get; set; }
        public List<Guid> Labels { get; set; }
        public Guid CategoryId { get; set; }
        public List<LabelVM> LabelsVM { get; set; }
        public CategoryVM CategoryVM { get; set; } 
    }
}
