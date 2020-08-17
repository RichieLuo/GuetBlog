using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Guet.Entities
{
    /// <summary>
    /// 实体基类
    /// </summary>
    public class EntityBase:IEntityBase
    {
        /// <summary>
        /// 记录的唯一标识
        /// </summary>
         [Key]
        public virtual Guid Id { get; set; }

        /// <summary>
        /// 创建的时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新的时间
        /// </summary>
        public virtual DateTime UpdateTime { get; set; }

        /// <summary>
        /// 基类构造函数
        /// </summary>
        public EntityBase()
        {
            this.Id =Guid.NewGuid();
            this.CreateTime = DateTime.Now;
        }
    }
}
