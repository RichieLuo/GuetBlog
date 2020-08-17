using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guet.Common.ViewModelComponents
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple=true )]
    public class SelfReferentialItemSpecification:Attribute
    {
        public string RelevanceId { get; set; }
        public SelfReferentialItemSpecification(string id) 
        {
            RelevanceId = id;
        }
    }
}
