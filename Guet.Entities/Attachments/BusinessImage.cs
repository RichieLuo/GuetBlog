 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guet.Entities.Attachments
{
    public class BusinessImage : EntityBase
    {  
        [StringLength(1000)]
        public string Description { get; set; }      // 图片说明 
        [StringLength(100)]
        public string DisplayName { get; set; }      // 图片显示名称
        [StringLength(256)]
        public string OriginalFileName { get; set; } // 图片原始文件
        public DateTime UploadedTime { get; set; }   // 图片上传时间
        [StringLength(256)]
        public string UploadPath { get; set; }       // 图片上传保存路径
        [StringLength(256)]
        public string UploadFileSuffix { get; set; } // 上传文件的后缀名
        public long FileSize { get; set; }
        [StringLength(120)]
        public string IconString { get; set; }       // 文件物理格式图标
 
        public Guid RelevanceObjectId { get; set; }  // 使用该图片的业务对象的 id
        public Guid UploaderId { get; set; }         // 关联上传人Id
        /// <summary>
        /// 是否作为封面图片
        /// </summary>
        public bool IsCover { get; set; }

        public BusinessImage()
        {
            this.Id = Guid.NewGuid(); 
            this.UploadedTime = DateTime.Now; 
        }
    }
}
