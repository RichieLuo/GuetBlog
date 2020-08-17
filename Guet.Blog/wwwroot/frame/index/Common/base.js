var pages = {
    get: function () {
        return JSON.parse(localStorage.getItem('CurrentPage')) || { index: 0, name: "home", height: 1940 };
    },
    set: function (page) {
        var _page = page ? page : { index: 0, name: 'home', height: 1940 };
        localStorage.setItem('CurrentPage', JSON.stringify(_page));
    }
};



var loading = {
    index: 0,
    show: function () {
        this.index = top.layer.msg('页面加载中，请稍等...', {
            icon: 16,
            time: false,
            shade: 0.8
        });
    },
    close: function () {
        layer.close(this.index);
    }
};

var notice = {
    index: 0,
    show: function (msg) {
        var self = this;
        this.index = layer.open({
            type: 1,
            title: false, //不显示标题栏 
            closeBtn: false,
            area: '300px;',
            shade: 0.8,
            id: 'LAY_layuipro', //设定一个id，防止重复弹出 
            resize: false,
            btn: ['确定', '关闭'],
            btnAlign: 'c',
            moveType: 1, //拖拽模式，0或者1
            content: `<div style="padding: 50px; line-height: 22px; background-color: #393D49; color: #fff; font-weight: 300;">
            搜索的关键字：${msg}</div>`,
            success: function (layero) {
                layer.close(self.index);
                var btn = layero.find('.layui-layer-btn');
                // btn.find('.layui-layer-btn0').attr({
                //     href: 'javascript:',
                //     target: '_blank'
                // });
            }
        });
    }
};

var webHelper = {
    create: function () {
        var _el = `
        <div id="WebSiteHelper" class="web-site-helper text-center">   
            <div id="WebBackTop" class="back-top"  title="返回顶部">
            <i class="layui-icon layui-icon-release"></i> 
            <span class="back-top-text">顶部</span>
            </div>         
            <div id="WebSitting" class="setting display-none" title="网站设置">
                <i class="layui-icon layui-icon-set"></i>
            </div>
        </div>
        `;
        var _$body = $('body');
        _$body.append(_el);
        this.listenSetting();
    },
    listenSetting: function () {
        $('#WebBackTop').click(function () {
            $("html,body").animate({
                scrollTop: 0
            }, 500);
        });
        $('#WebSitting').click(function () {
            webSite.logo.set(!webSite.logo.get());
        });
    },
    isShowBackTop: function () {
        // TODO:
    },
    init: function () {
        this.create();
    }
};


//模拟点击导航 动态加载 demo.html 页面中的 iframe 框架
var toggleIframe = {
    _$iframe: $('#ContentIframe'),
    _$waitEmpty: $('#IframeEmpty'),
    do: function (iframe) {
        var self = this;
        console.log(iframe);
        var _iframe = iframe || this._$iframe;
        console.log(_iframe);
        this._$iframe = $(_iframe);
        console.log(this._$iframe);
        var _currentPage = pages.get();
        this._$iframe.height(_currentPage.height);
        this._$iframe.attr('src', `../${_currentPage.name}/${_currentPage.name}.html`);
        var _timer = setTimeout(function () {
            self._$waitEmpty.hide();
            self._$iframe.show();
            clearTimeout(_timer);
        }, 300);
    }
};

// 设置
var webSite = {
    logo: {
        _$textEl: $('#TitleText'),
        _$imgEl: $('#TitleImage'),
        set: function (state) {
            state = state || false;
            console.log('set :', state);
            localStorage.setItem('LogoUseText', state);
        },
        get: function () {
            var _state = localStorage.getItem('LogoUseText');
            _state = _state === 'true' ? true : false;
            console.log('get :', _state);
            return _state;
        },
        load: function () {
            console.log(this._$textEl);
            console.log('load :', this.get());
            if (this.get()) {
                this._$textEl.show();
                this._$imgEl.hide();
            } else {
                this._$textEl.hide();
                this._$imgEl.show();
            }
        }
    }
};


loading.show(); //显示加载中...

webHelper.init();
console.log(pages.get());

toggleIframe.do();

setTimeout(function () {
    loading.close();
}, 1000);

    // this.webSite.logo.load();
    // this.pages.set({ index: null, name: 'login' })
