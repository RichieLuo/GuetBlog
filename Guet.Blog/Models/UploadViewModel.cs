using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guet.ViewModels.Common;

namespace Guet.Web.Models
{
    public class UploadViewModel : BaseResult
    {
        public Guid Id { get; set; }
        public Guid ObjectId { get; set; }
        public string Url { get; set; }   
        public string FullUrl { get; set; }
        public UploadViewModel() { }
        public UploadViewModel(Guid id, Guid objectId, string url,string fullUrl)
        {
            Id = id;
            ObjectId = objectId;
            Url = url;
            FullUrl = fullUrl;
        }

    }
}
