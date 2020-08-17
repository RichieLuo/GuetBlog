 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guet.Entities.Attachments
{
    public class BusinessVideo:EntityBase
    {
     
        [StringLength(150)]
        public string Name { get; set; }                              
        [StringLength(1000)]
        public string Description { get; set; }                       
 
        public DateTime AttachmentTimeUploaded { get; set; }          
        [StringLength(500)]
        public string OriginalFileName { get; set; }                  
        [StringLength(500)]
        public string UploadPath { get; set; }                        
        public bool IsInDB { get; set; }                              
        [StringLength(10)]
        public string UploadFileSuffix { get; set; }                  
        public byte[] BinaryContent { get; set; }                     
        public long FileSize { get; set; }                            
        [StringLength(120)]
        public string IconString { get; set; }                        

        public Guid RelevanceObjectId { get; set; }
        public Guid UploaderId { get; set; }                           // 关联上传人Id

        public BusinessVideo() 
        {
            this.Id = Guid.NewGuid(); 
            this.AttachmentTimeUploaded = DateTime.Now;
        }
    }
}
