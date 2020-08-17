using System;
using System.Collections.Generic;
using System.Text;

namespace Guet.Entities.Site
{
    /// <summary>
    /// 网站留言
    /// </summary>
    public class Message : EntityBase
    {
        /// <summary>
        /// 留言者（非游客）
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 留言者昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 留言者头像
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 留言者邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 留言者站点
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 留言内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 留言者IP地址
        /// </summary>
        public string IPAddress { get; set; }
        /// <summary>
        /// 留言者IP所在地
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 留言者浏览器
        /// </summary>
        public string Brower { get; set; }
    }
}
