using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guet.Web.Models
{
    public class DisEnableFriendLinkInput
    {
        public Guid Id { get; set; }

        public bool Enable { get; set; }
    }
}
