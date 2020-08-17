using System;
using System.Collections.Generic;
using System.Text;
using Guet.Entities.Site;

namespace Guet.ViewModels.Site
{
    /// <summary>
    /// 网站留言
    /// </summary>
    public class MessageVM : EntityBaseVM
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

        public MessageVM() { }
        public MessageVM(Message m)
        {
            Id = m.Id;
            UserId = m.UserId;
            NickName = m.NickName;
            Avatar = m.Avatar;
            Email = m.Email;
            Url = m.Url;
            Content = m.Content;
            IPAddress = m.IPAddress;
            Address = m.Address;
            Brower = m.Brower;
            CreateTime = m.CreateTime;
            UpdateTime = m.UpdateTime;
        }

    }
}
