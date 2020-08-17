using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guet.Web.Models
{
    public class UploadInput
    {
        public Guid ObjectId { get; set; }

        public bool IsRich { get; set; } = true;
    }


    public class RichImageCallback
    {
        public int Errno { get; set; }

        public List<string> Data { get; set; }

        public RichImageCallback()
        {
            Data = new List<string>();
        }

    }
}
