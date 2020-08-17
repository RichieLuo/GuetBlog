using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Guet.Entities.Attachments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Guet.Entities.ApplicationOrganization
{ 
    public class ApplicationUser : IdentityUser
    { 
        public string NickName { get; set; } 
    
        public string MobileNumber { get; set; }  
        public string Avatar { get; set; }

        public string Remark { get; set; }

        public ApplicationUser() : base()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        public ApplicationUser(string userName) : base(userName)
        {
            this.Id = Guid.NewGuid().ToString();
            this.UserName = userName;
        }
    }
}
