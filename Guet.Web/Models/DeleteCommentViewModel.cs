using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guet.Web.Models
{
    public class DeleteCommentViewModel
    {
        public Guid Id { get; set; }

        public bool IsParent { get; set; }
    }
}
