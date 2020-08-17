using System;
using System.Collections.Generic;
using System.Text;
using Guet.Entities.Blogs;

namespace Guet.ViewModels.Blogs
{
    public class LabelVM : EntityBaseVM
    {
        public string Name { get; set; }

        public LabelVM() { }
        public LabelVM(Label m)
        {
            Id = m.Id;
            Name = m.Name;
            CreateTime = m.CreateTime;
            UpdateTime = m.UpdateTime; 
        }
    }
}
