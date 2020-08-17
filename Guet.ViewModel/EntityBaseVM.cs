using System;
using System.Collections.Generic;
using System.Text;

namespace Guet.ViewModels
{
    /// <summary>
    /// 视图模型基类
    /// </summary>
    public class EntityBaseVM : IEntityBaseVM
    {
        /// <summary>
        /// 记录的唯一标识
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 创建的时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新的时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 列表时候需要的序号
        /// </summary>
        public string OrderNumber { get; set; }
 
        public bool IsNew { get; set; }
    }
}
