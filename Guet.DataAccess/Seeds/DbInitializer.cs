using Guet.DataAccess.SqlServer;
using Guet.Entities.ApplicationOrganization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guet.Entities.Blogs;
using Guet.Entities.Site;
using System.Text;

namespace Guet.DataAccess.Seeds
{
    /// <summary>
    /// 构建一个初始化原始数据的组件，用于程序启动的时候执行一些数据初始化的操作
    /// </summary>
    public static class DbInitializer
    {
        static EntityDbContext _Context;

        public static void Initialize(EntityDbContext context)
        {
            _Context = context;
            context.Database.EnsureCreated();

            _AddApplicationRole();
            _AddDefaultData();
        }
        private static void _AddApplicationRole()
        {
            if (_Context.Roles.Any()) return;
            var roles = new List<ApplicationRole>()
            {
               new ApplicationRole(){Name="Admin",DisplayName="系统管理", Description="适用于系统管理人员",ApplicationRoleType=ApplicationRoleTypeEnum.适用于系统管理人员},
               new ApplicationRole(){Name="User",DisplayName="普通注册用户", Description="适用于普通注册用户",ApplicationRoleType=ApplicationRoleTypeEnum.适用于普通注册用户}
            };
            _Context.ApplicationRoles.AddRange(roles);
            _Context.SaveChanges();
        }

        private static void _AddDefaultData()
        {
            //站点信息
            if (!_Context.SiteSettings.Any())
            {
                var siteSetting = new SiteSetting()
                {
                    Title = "我的博客",
                    Description = "这是我的个人博客",
                    Keywords = "个人,技术,博客",
                    Logo = "/images/logo.png",
                    Statistics = "<span id='cnzz_stat_icon_1278811683'></span><script src='https://v1.cnzz.com/z_stat.php?id=1278811683&online=1&show=line' type='text/javascript'></script>",
                    UseImgLogo = false,
                    Copyright = "这里填写网站版权信息",
                    ICP = "这是填写博客站点的备案号",
                    IsOpen = true,
                    QrCode= "/images/qrCode/qrcode.png"
                };
                _Context.SiteSettings.Add(siteSetting);
            }
            // 关于我
            if (!_Context.Abouts.Any())
            {
                var about = new About()
                {
                    Content = "<div><h1>这里是关于我的内容</h1><p>这里支持Html元素</p> </div>"
                };
                _Context.Abouts.Add(about);
            }

            // 轮播图
            if (!_Context.Banners.Any())
            {
                var banners = new List<Banner>()
                {
                    new Banner(){IsBanner=true, Title="首页轮播图名称1",Description="首页轮播图描述",Url="/images/main/article_image_002.jpg",Href="http://超链接",IsShow=true},
                    new Banner(){IsBanner=true, Title="首页轮播图名称2",Description="首页轮播图描述",Url="/images/main/article_image_001.jpg",Href="http://超链接",IsShow=true},
                    new Banner(){IsBanner=true, Title="首页轮播图名称3",Description="首页轮播图描述",Url="/images/main/article_image_002.jpg",Href="http://超链接",IsShow=true},
                    new Banner(){IsBanner=false, Title="广告名称1",Description="广告描述",Url="/images/main/article_image_001.jpg",Href="http://超链接",IsShow=true}
                };
                _Context.Banners.AddRange(banners);
            }

            // 文章标签
            List<Label> labels = null;
            if (!_Context.Labels.Any())
            {
                labels = new List<Label>()
                {
                    new Label(){ Name="主题"},
                    new Label(){ Name="前端技术"},
                };
                _Context.Labels.AddRange(labels);
            }

            // 文章分类
            List<Category> categories = null;
            if (!_Context.Categories.Any())
            {
                categories = new List<Category>()
                {
                    new Category(){ Name="前端"},
                    new Category(){ Name="后端"},
                    new Category(){ Name="JavaScript"},
                    new Category(){ Name="jQuery"},
                };
                _Context.Categories.AddRange(categories);
            }

            if (!_Context.Articles.Any())
            {
                var article = new Article()
                {
                    Title = "剪一段旧时光，暗香绽放",
                    Abstract = @"当花瓣离开花朵，暗香残留，香消在风起雨后，无人来嗅”忽然听到沙宝亮的这首《暗香》，似乎这香味把整间屋子浸染。我是如此迷恋香味，吸进的是花儿的味道，吐出来的是无尽的芬芳。轻轻一流转，无限风情，飘散，是香，是香，它永远不会在我的时光中走丢。
旧的东西其实极好。学生时代喜欢写信，只是今天书信似乎早已被人遗忘，那些旧的记忆，被尘埃轻轻覆盖，曾经的笔端洇湿了笔锋，告慰着那时的心绪。现在读来，仿佛嗅到时光深处的香气，一朵墨色小花晕染了眼角，眉梢，是飞扬的青春，无知年少的轻狂，这份带不走的青涩，美丽而忧伤。",
                    Content = @"<div>“当花瓣离开花朵，暗香残留，香消在风起雨后，无人来嗅”忽然听到沙宝亮的这首《暗香》，似乎这香味把整间屋子浸染。我是如此迷恋香味，吸进的是花儿的味道，吐出来的是无尽的芬芳。轻轻一流转，无限风情，飘散，是香，是香，它永远不会在我的时光中走丢。
旧的东西其实极好。学生时代喜欢写信，只是今天书信似乎早已被人遗忘，那些旧的记忆，被尘埃轻轻覆盖，曾经的笔端洇湿了笔锋，告慰着那时的心绪。现在读来，仿佛嗅到时光深处的香气，一朵墨色小花晕染了眼角，眉梢，是飞扬的青春，无知年少的轻狂，这份带不走的青涩，美丽而忧伤。
小心翼翼珍藏着，和母亲在一起的美好时光。母亲身体一直不好，最后的几年光景几乎是在医院渡过，然而和母亲在一起的毎一刻都是温暖美好的。四年前，母亲还是离开了这个世界，离开了我。生命就是如此脆弱，逝去和別离，陈旧的情绪某年某月的那一刻如水泻闸。水在流，云在走，聚散终有时，不贪恋一生，有你的这一程就是幸运。那是地久天长的在我的血液中渗透，永远在我的心中，在我的生命里。
时光就是这么不经用，很快自己做了母亲，我才深深的知道，这样的爱，不带任何附加条件，不因万物毁灭而更改。只想守护血浓于水的旧时光，即便峥嵘岁月将容颜划伤，相信一切都是最好的安排。那时的时光无限温柔，当清水载着陈旧的往事，站在时光这头，看时光那头，一切变得分明。执笔书写，旧时光的春去秋来，欢喜也好，忧伤也好，时间窖藏，流光曼卷里所有的宠爱，疼惜，活色生香的脑海存在。
回忆的老墙，偶尔依靠，黄花总开不败，所有囤积下来的风声雨声，天晴天阴，都是慈悲。时光不管走多远，不管有多老旧，含着眼泪，伴着迷茫，读了一页又一页，一直都在，轻轻一碰，就让内心温软。旧的时光被揉进了岁月的折皱里，藏在心灵的沟壑，直至韶华已远，才知道走过的路不能回头，错过的已不可挽留，与岁月反复交手，沧桑中变得更加坚强。
是的，折枝的命运阻挡不了。人世一生，不堪论，年华将晚易失去，听几首歌，描几次眉，便老去。无论天空怎样阴霾，总会有几缕阳光，总会有几丝暗香，温暖着身心，滋养着心灵。就让旧年花落深掩岁月，把心事写就在素笺，红尘一梦云烟过，把眉间清愁交付给流年散去的烟山寒色，当冰雪消融，自然春暖花开，拈一朵花浅笑嫣然。
听这位老友，絮絮叨叨地讲述老旧的故事，试图找回曾经的踪迹，却渐渐明白了流年，懂得了时光。过去的沟沟坎坎，风风雨雨，也装饰了我的梦，也算是一段好词，一幅美卷，我愿意去追忆一些旧的时光，有清风，有流云，有朝露晚霞，我确定明亮的东西始终在。静静感念，不着一言，百转千回后心灵又被唤醒，于一寸笑意中悄然绽放。
唯用一枝瘦笔，剪一段旧时光，剪掉喧嚣尘世的纷纷扰扰，剪掉终日的忙忙碌碌。情也好，事也罢，细品红尘，文字相随，把寻常的日子，过得如春光般明媚。光阴珍贵，指尖徘徊的时光唯有珍惜，朝圣的路上做一个谦卑的信徒，听雨落，嗅花香，心上植花田，蝴蝶自会来，心深处自有广阔的天地。旧时光难忘，好的坏的一一纳藏，不辜负每一寸光阴，自会花香满径，盈暗香满袖。</div>",
                    Cover = "https://ftp.bmp.ovh/imgs/2020/04/2b4f7d1190727150.jpg",
                    IsOwn = true,
                    IsRecommend = true,
                    IsNotOwnDesctiption = "如文章无特殊说明，均为本站原创！",
                    CategoryId = categories.FirstOrDefault().Id
                };

                _Context.Articles.Add(article);

                foreach (var item in labels)
                {
                    _Context.ArticleAndLabels.Add(new ArticleAndLabel() { ArticleId = article.Id, LabelId = item.Id });
                }
            }

            _Context.SaveChanges();
        }
    }
}
