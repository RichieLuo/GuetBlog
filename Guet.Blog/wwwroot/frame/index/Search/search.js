var searchPage = {
    key: '',
    isInitPaging: false,
    isSearch: false,
    setCurrentPage: function () {

    },
    queryVariable: function (variable) {
        var self = this;
        var query = window.location.search.substring(1);
        var vars = query.split("&");
        for (var i = 0; i < vars.length; i++) {
            var pair = vars[i].split("=");
            if (pair[0] == variable) {
                if (variable === 'key') { 
                    self.key = decodeURI(pair[1]);
                    search._$input.val(self.key);
                }
                return decodeURI(pair[1]);
            }
        }
        return ('');
    },
    query: function (key, paging) {
        var self = this;
        var _paging = {
            limit: 10,
            index: 1
        };
        $.extend(_paging, paging);
        if (key) {
            layer.msg("搜索中...");
            self.key = key || self.key;
            $.ajax({
                url: '../../Home/Search',
                type: 'POST',
                dataType: 'json',
                data: { key: self.key, index: _paging.index, limit: _paging.limit },
                success: function (res) {
                    layer.closeAll();
                    var t = '';
                    if (res.length <= 0) {
                        t = `<li style="padding: 200px 0; text-align: center; font-weight: 700;" class="articles-item-line margin-top-xs padding-bottom">没有搜索到相关的文章！</li>`;
                    } else {
                        res.forEach((item, index) => {
                            console.log(item)
                            t += `
 <li class="articles-item-line margin-top-xs padding-bottom">
                                                        <a href="/Home/ArticleDetail/${item.id}">
                                                            <div class="layui-row articles-item">
                                                                <div class="layui-col-md12">
                                                                    <div class="layui-row articles-item-content padding">
                                                                        <div class="layui-col-md4 articles-item-cover">
                                                                            <img src="${item.cover}" title="${item.title}" alt="${item.title}">
                                                                            <span class="article-item-category">${item.category.name}</span>
                                                                        </div>
                                                                        <div class="layui-col-md8 articles-list-items">
                                                                            <div class="layui-row">
                                                                                <div class="layui-col-md12">
                                                                                    <span class="layui-btn layui-btn-xs is-original">${item.isOwn ? "原创" : "转载"}</span>
                                                                                    <a href="/Home/ArticleDetail/${item.id}" title="${item.title}" class="articles-item-title margin-t-none">${item.title}</a>
                                                                                </div>
                                                                                <div class="layui-col-md12 articles-item-description">
                                                                                    <p> ${item.abstract}</p>
                                                                                </div>
                                                                                <div class="layui-col-md12">
                                                                                    <div class="layui-row">
                                                                                        <div class="layui-col-md8">
                                                                                            <img title="官方认证：站长" class="articles-item-avatar margin-right" src="${item.user.avatar === null ? "/images/defaultAvatar.jpg" : item.user.avatar}" alt="">
                                                                                            <span>${item.cTime}</span>
                                                                                            <span>【 <a class="" href="/Home/Category/${item.category.id}" target="_blank">${item.category.name}</a>】</span>
                                                                                        </div>
                                                                                        <div class="layui-col-md4 text-right">
                                                                                            <a href="/Home/ArticleDetail/${item.id}" class="to-article-detail layui-btn layui-btn-xs">阅读更多</a>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </a>
                                                    </li>
                        `;
                        });
                    }
                    $("#SearchList").html(t);
                    if (!self.isInitPaging) {
                        self.pagination();
                        self.isInitPaging = true;
                    }
                }
            })
        }
    },
    showTips: function (index) {
        var _$searchKeyArea = $('#SearchKeyArea'),
            _$searchCount = $('#SearchCount');

        _$searchKeyArea.text(this.key);
        _$searchCount.text(9999);

        $('.search-tips').children().eq(index).show().siblings().hide();
    },
    pagination: function () {
        var self = this;
        layui.use(['element', 'laypage'], function () {
            var element = layui.element,
                laypage = layui.laypage;
            $.ajax({
                url: '../../Home/GetArticleCount',
                type: 'POST',
                dataType: 'json',
                data: { key: self.key },
                success: function (res) {
                    console.log(res)
                    //完整分页功能  
                    laypage.render({
                        elem: 'PaginationSearch',
                        count: res.count, //从后台获取值
                        layout: ['count', 'prev', 'page', 'next', 'limit', 'refresh', 'skip'],
                        jump: function (obj) {
                            console.log(obj);
                            if (!self.isInitPaging) {
                                self.query(self.key, { limit: obj.limit, index: obj.curr });
                            }
                        }
                    });
                }
            });

        });
    },
    init: function () {
        this.isSearch = true;
        this.queryVariable('key'); 
        //调用搜索
        this.query(this.key);
        //this.pagination();
        this.setCurrentPage();
        console.log('初始化成功')
    }
}
