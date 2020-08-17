

var _layui = {
    install: function () {
        var self = this;
        layui.use(['element', 'laypage'], function () {
            var element = layui.element,
                laypage = layui.laypage;

            //完整分页功能
            laypage.render({
                elem: 'PaginationContainer',
                count: 100,
                layout: ['count', 'prev', 'page', 'next', 'limit', 'refresh', 'skip'],
                jump: function (obj) {
                    console.log(obj);
                }
            });
        });
    },
    init: function () {
        this.install();
    }
};

var homePages = {
    selection: function () {
        // 精选类别推荐
        $(".selection-selected").hover(function () {
            $(this).removeClass('active').addClass('active').siblings().removeClass('active')
        });
    },
    init: function () {
        this.selection();
    }
};

this.homePages.init();
this._layui.init();
