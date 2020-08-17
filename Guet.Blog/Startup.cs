using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guet.DataAccess;
using Guet.DataAccess.Seeds;
using Guet.DataAccess.SqlServer;
using Guet.Entities.ApplicationOrganization;
using Guet.Entities.Attachments;
using Guet.Entities.Blogs;
using Guet.Entities.Site;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Guet.Blog
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<EntityDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<EntityDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = false;
                options.User.RequireUniqueEmail = false;

            });
            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(150);
                options.LoginPath = "/Account/Login"; //Î´µÇÂ¼Ìø×ª
                options.LogoutPath = "/Home/Index"; //×¢ÏúÌø×ª

            });

            services.AddTransient<IEntityRepository<BusinessFile>, EntityRepository<BusinessFile>>();
            services.AddTransient<IEntityRepository<BusinessImage>, EntityRepository<BusinessImage>>();

            services.AddTransient<IEntityRepository<Article>, EntityRepository<Article>>();
            services.AddTransient<IEntityRepository<Comment>, EntityRepository<Comment>>();
            services.AddTransient<IEntityRepository<CommentItem>, EntityRepository<CommentItem>>();
            services.AddTransient<IEntityRepository<Category>, EntityRepository<Category>>();
            services.AddTransient<IEntityRepository<Label>, EntityRepository<Label>>();
            services.AddTransient<IEntityRepository<ArticleAndLabel>, EntityRepository<ArticleAndLabel>>();
            services.AddTransient<IEntityRepository<FriendLink>, EntityRepository<FriendLink>>();
            services.AddTransient<IEntityRepository<About>, EntityRepository<About>>();
            services.AddTransient<IEntityRepository<Message>, EntityRepository<Message>>();
            services.AddTransient<IEntityRepository<SiteSetting>, EntityRepository<SiteSetting>>();
            services.AddTransient<IEntityRepository<Banner>, EntityRepository<Banner>>();

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, EntityDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            DbInitializer.Initialize(context);
        }
    }
}
