using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Guet.Entities.ApplicationOrganization;
using Guet.Entities.Attachments;
using Guet.Entities.BusinessManagement.Audit;
using System;
using System.Collections.Generic;
using System.Text;
using Guet.Entities;
using Guet.Entities.Blogs;
using Guet.Entities.Site;

namespace Guet.DataAccess.SqlServer
{
    public class EntityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public EntityDbContext(DbContextOptions<EntityDbContext> options) : base(options)
        {
        }  
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; } 
        public DbSet<AuditRecord> AuditRecords { get; set; }
        public DbSet<BusinessFile> BusinessFiles { get; set; }
        public DbSet<BusinessImage> BusinessImages { get; set; }
        public DbSet<BusinessVideo> BusinessVideos { get; set; }  
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentItem> CommentItems { get; set; }
        
        public DbSet<Category> Categories { get; set; }
        public DbSet<Label> Labels { get; set; }
        public DbSet<ArticleAndLabel> ArticleAndLabels { get; set; }
        public DbSet<FriendLink> FriendLinks { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<SiteSetting> SiteSettings { get; set; }

        public DbSet<Banner> Banners { get; set; }


        /// <summary>
        /// 如果不需要 DbSet<T> 所定义的属性名称作为数据库表的名称，可以在下面的位置自己重新定义
        /// </summary>f
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
