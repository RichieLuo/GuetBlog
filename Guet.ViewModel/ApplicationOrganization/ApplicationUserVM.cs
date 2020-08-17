using Guet.Common.JsonModels;
using Guet.Common.ViewModelComponents;
using Guet.Entities.ApplicationOrganization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Guet.ViewModels.ApplicationOrganization
{
    public class ApplicationUserVM : EntityBaseVM
    {
        public string NickName { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
        public string UserName { get; set; }

        public string MobileNumber { get; set; }

        public string EMail { get; set; }
        public bool LockoutEnabled { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public Guid RoleId { get; set; }
        public List<ApplicationRoleVM> Roles { get; set; }
        public string Avatar { get; set; }
        public virtual string Remark { get; set; }
        public bool IsAdmin { get; set; }
        public ApplicationUserVM() { }

        public ApplicationUserVM(ApplicationUser bo)
        {
            Id = Guid.Parse(bo.Id);
            UserName = bo.UserName;
            MobileNumber = bo.MobileNumber;
            EMail = bo.Email;
            NickName = bo.NickName;
            LockoutEnabled = bo.LockoutEnabled;
            Avatar = bo.Avatar;
            Remark = bo.Remark;
            Roles = new List<ApplicationRoleVM>();
        }

    }
}
