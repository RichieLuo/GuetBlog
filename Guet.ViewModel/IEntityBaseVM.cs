using Guet.Common.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guet.ViewModels
{
    /// <summary>
    /// 业务实体视图模型统一的接口规范，用于规约所有的视图模型的基础属性
    /// </summary>
    public interface IEntityBaseVM
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        Guid Id { get; set; } 
        /// <summary>
        /// 列表时候需要的序号
        /// </summary>
        string OrderNumber { get; set; } 
  
    }
}
