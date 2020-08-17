

// 导航js
var headerNav = {
    _$navs: $('.nav-list').children(),
    get: function () {
        return JSON.parse(localStorage.getItem('CurrentPage'));
    },
    set: function (page) {
        localStorage.setItem('CurrentPage', JSON.stringify(page));
    },
    clear: function () {
        localStorage.removeItem('CurrentPage');
    },
    default: function () {
        if (!this.get()) {
            this.set({ index: 0, name: 'home', height: 2000 });
        }
        this.active();
    },
    active: function () {
        var self = this;
        var _elements = self._$navs;
        var _current = self.get();
        var _index = _current.index;
        //layer.msg('当前页面标识：' + _current.name);
        if (_index !== null) {
            _elements.eq(Number(_index)).removeClass('active').addClass('active').siblings().removeClass('active');
        }
    },
    selected: function () {
        var self = this;
        $('.nav-item').click(function (e) {
            self.set({
                index: $(this).index(),
                name: $(this).data('name'),
                height: $(this).data('height')
            });
            $(this).removeClass('active').addClass('active').siblings().removeClass('active');
            //模拟点击导航 动态切换 demo.html 中的iframe框架
            toggleIframe.do(window.parent.document.getElementById('ContentIframe'));
        });

    },
    init: function () {
        this.selected();
        this.default();
    }
};


// 搜索js

var search = {
    _$container: $('#HomeSearch'),
    _$showBtn: $('#ShowSearchBtn'),
    _$input: $('#HomeSearchInput'),
    _$closeBtn: $('#HomeCloseSearchBtn'),
    _$submitBtn: $('#HomeToSearchBtn'),
    state: false,
    get: function () {
        var _state = localStorage.getItem('IsShowSearch') === 'true' ? true : false;
        return _state;
    },
    set: function (state) {
        localStorage.setItem('IsShowSearch', state);
    },
    check: function () {
        // if (this.get()) {
        //     _self._$container.show();
        // }else{

        // }
    },
    show: function () {
        var _self = this;
        _self.state = this.get();
        console.log('is show search ', _self.state);
        if (_self.state) {
            _self._$container.show();
        } else {
            _self._$container.hide();
        }
        this._$showBtn.click(function (e) {
            _self._$container.toggle();
            _self.state = !_self.state;
            _self.set(_self.state);
        });
        this._$closeBtn.click(function (e) {
            if (_self.state) {
                _self._$container.hide();
                _self.state = !_self.state;
                _self.set(_self.state);
            }
        });
    },
    search: function () {
        var _self = this;
        _self._$submitBtn.click(function (e) {
            if (_self._$input.val() === "") {
                layer.msg("请输入搜索内容");
                return;
            }
            //var index = layer.load(0, { shade: false });

            //window.location.href = '../../Home/Search?key=' + _self._$input.val();

            //setTimeout(function () {
            //    //调用搜索
            //    searchPage.isInitPaging = false;
            //    searchPage.query(_self._$input.val());
            //    //layer.close(index);
            //}, 500); 

            if (searchPage.isSearch) {
                //调用搜索
                searchPage.isInitPaging = false;
                searchPage.query(_self._$input.val());
            } else {
                headerNav.set({ index: null, name: 'search' });
                window.location.href = '../../Home/Search?key=' + _self._$input.val();
            }
        });
    },
    init: function () {
        this.show();
        this.search();
    }
};

layui.use(['jquery', 'layer', 'form', 'element','upload'], function () {
    let $ = layui.jquery,
        layer = layui.layer,
        form = layui.form,
       upload= layui.upload,
        element = layui.element;

    const UserLogin = {
        _$loginBtn: $('#ToLoginBtn'),
        _$registerBtn: $('#ToRegisterBtn'),
        _$logoutBtn: $('#ToLogoutBtn'),
        _$hadLoginEl: $('.has-login-btns'),
        _$toLoginEl: $('.to-login-btns'),
        template: function () {
            return `
            <div class="user-login-register">
                <div class="layui-tab layui-tab-brief" lay-filter="UserLoginRegisterTab">
                    <ul class="layui-tab-title text-center">
                        <li lay-id="TabLogin" class="layui-this">登录</li>
                        <li lay-id="TabRegister">注册</li>
                    </ul>
                    <div class="layui-tab-content">
                        <div class="layui-tab-item layui-show">
                            <form class="layui-form layui-form-pane" action="">
                                <div class="layui-form-item">
                                    <label class="layui-form-label">用户名</label>
                                    <div class="layui-input-block">
                                        <input type="text" name="UserName" required lay-verify="required" placeholder="请输入用户名" autocomplete="off" class="layui-input">
                                    </div>
                                </div>
                                <div class="layui-form-item">
                                    <label class="layui-form-label">密  码</label>
                                    <div class="layui-input-block">
                                        <input type="password" name="Password" required lay-verify="required" placeholder="请输入密码" autocomplete="off" class="layui-input">
                                    </div>
                                </div>
                                <div class="layui-form-item text-center">
                                    <button class="layui-btn user-login-btn" lay-submit lay-filter="UserLogin">立即登录</button>
                                    <a href="javascript:" class="user-to-toggle user-to-login" title="没有账号？立即注册">没有账号？立即注册</a>
                                </div>
                            </form>
                        </div>
                        <div class="layui-tab-item">
                            <form class="layui-form layui-form-pane" action="">
                                <div class="layui-form-item">
                                    <label class="layui-form-label">昵  称</label>
                                    <div class="layui-input-block">
                                        <input type="text" name="NickName" required lay-verify="required" placeholder="请输入您的昵称" autocomplete="off" class="layui-input">
                                    </div>
                                </div>
                                <div class="layui-form-item">
                                    <label class="layui-form-label">用户名</label>
                                    <div class="layui-input-block">
                                        <input type="text" name="UserName" required lay-verify="required" placeholder="请输入用户名" autocomplete="off" class="layui-input">
                                    </div>
                                </div>
                                <div class="layui-form-item">
                                    <label class="layui-form-label">密  码</label>
                                    <div class="layui-input-block">
                                        <input type="password" name="Password" required lay-verify="required" placeholder="请输入密码" autocomplete="off" class="layui-input">
                                    </div>
                                </div>
                                <div class="layui-form-item">
                                    <label class="layui-form-label">确认密码</label>
                                    <div class="layui-input-block">
                                        <input type="password" name="ConfirmPassword" required lay-verify="required" placeholder="请再次确认密码" autocomplete="off" class="layui-input">
                                    </div>
                                </div>
                                <div class="layui-form-item text-center">
                                    <button class="layui-btn user-login-btn" lay-submit lay-filter="UserRegister">立即注册</button>
                                    <a href="javascript:" class="user-to-toggle user-to-register" title="已有账号？立即登录">已有账号？立即登录</a>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        `;
        },
        render: function (type) {
            let _this = this;
            let _loginDialogIndex = layer.open({
                id: 'UserLoginDialog',
                skin: 'user-logindialog',
                type: 1,
                closeBtn: true,
                title: 0,
                // area: ['400px'],
                content: _this.template(),
                zIndex: layer.zIndex,
                success: function (layero) {
                    if (type === 2) {
                        element.tabChange('UserLoginRegisterTab', 'TabRegister');
                    }
                    form.render();
                    form.on('submit(UserLogin)', function (data) {
                        $.ajax({
                            url: '/Account/UserLogin',
                            type: 'post',
                            data: data.field,
                            dataType: 'json',
                            success: function (res) {
                                if (res.status && res.code === 200) {
                                    _this._$hadLoginEl.show();
                                    _this._$hadLoginEl.removeClass("display-none");
                                    _this._$toLoginEl.hide();
                                    _this.set(true);

                                    _this.showInfo(res.data);
                                    layer.close(_loginDialogIndex);
                                }
                                alert(res.msg);
                            }
                        });
                        return false;
                    });
                    form.on('submit(UserRegister)', function (data) {
                        if (data.field.password !== data.field.confirmPassword) {
                            alert("两次密码输入不匹配");
                            return false;
                        }
                        $.ajax({
                            url: '/Account/UserRegister',
                            type: 'post',
                            data: data.field,
                            dataType: 'json',
                            success: function (res) {
                                alert(res.msg);
                            }
                        });
                        return false;
                    });
                    $('.user-to-login').off('click').on('click', function () {
                        element.tabChange('UserLoginRegisterTab', 'TabRegister');
                    });
                    $('.user-to-register').off('click').on('click', function () {
                        element.tabChange('UserLoginRegisterTab', 'TabLogin');
                    });
                }
            });
        },
        //显示信息窗口
        showInfo: function (data) {
            var _this = this; 
            if (data.isAdmin) {
                $(".has-login-nickname.admin").show();
                $(".has-login-nickname.user").hide();
            } else {
                $(".has-login-nickname.admin").hide();
                $(".has-login-nickname.user").show();

                _this.bindShowInfo();
            }
            $(".has-login-nickname").data("userid", data.id);
            $(".has-login-nickname").attr("title", data.nickname);
            $(".has-login-nickname.user").text(data.nickName);
            $(".has-login-avatar").attr("src", data.avatar);

        },
        bindShowInfo: function () {
            var _this = this; 
            $(".has-login-nickname.user").off("click").on("click", function () {
                $.ajax({
                    url: '/Account/GetInfoByUserId',
                    type: 'post',
                    data: { UserId: $(this).data("userid") },
                    dataType: 'json',
                    success: function (res) {
                        layer.open({
                            type: 1,
                            title: "我的资料",
                            area: ['600px', '650px'], //宽高
                            content: _this.formTemplate(res),
                            success: function () {
                                form.render();
                                _this.uploadAvatar();
                                _this.saveInfo();
                            }
                        });
                    }
                });
            });
        },
        formTemplate: function (res) {
            return `
                <form class="layui-form layui-form-pane" action="" style="padding: 30px;">
                    <input type="hidden" name="Id" value="${res.id}"></input>
                  <div class="layui-form-item">
                    <label class="layui-form-label">头像预览</label>
                    <div class="layui-input-block">
                       <img id="MyAvatar" src="${res.avatar}" style="width:38px;height:38px;border-radius:50%;margin-left: 30px;"/>
                    </div>
                  </div>
                  <div class="layui-form-item">
                    <label class="layui-form-label">头像</label>
                    <div class="layui-input-inline">
                      <input type="text" name="Avatar" value="${res.avatar}"  placeholder="请输入头像地址（支持网络图片）" autocomplete="off" class="layui-input">
                    </div>
                    <div class="layui-form-mid layui-word-aux" style="padding:0!important;"><button type="button" class="layui-btn layui-btn-normal" id="UploadAvatar">修改头像</button></div>
                  </div> 
                  <div class="layui-form-item">
                    <label class="layui-form-label">昵称</label>
                    <div class="layui-input-block">
                      <input type="text" name="NickName" required  lay-verify="required" value="${res.nickName}" placeholder="请输入您的昵称" autocomplete="off" class="layui-input">
                    </div>
                  </div>
                    <div class="layui-form-item">
                    <label class="layui-form-label">邮箱</label>
                    <div class="layui-input-block">
                      <input type="text" name="EMail" required  lay-verify="required" value="${res.eMail}" placeholder="请输入您的邮箱" autocomplete="off" class="layui-input">
                    </div>
                  </div>
                  <div class="layui-form-item layui-form-text ">
                    <label class="layui-form-label">个人简介</label>
                    <div class="layui-input-block">
                      <textarea name="Remark" placeholder="请输入个人简介" class="layui-textarea">${res.remark}</textarea>
                    </div>
                  </div>
                  <div class="layui-form-item">
                    <label class="layui-form-label">新密码</label>
                    <div class="layui-input-inline">
                      <input type="password" name="Password"  placeholder="请输入新密码" autocomplete="off" class="layui-input">
                    </div>
                    <div class="layui-form-mid layui-word-aux">如果密码为空则不修改</div>
                  </div> 
                    <div class="layui-form-item">
                    <label class="layui-form-label">再次确认密码</label>
                    <div class="layui-input-inline">
                      <input type="password" name="ConfirmPassword"  placeholder="请再次确认新密码" autocomplete="off" class="layui-input">
                    </div>
                    <div class="layui-form-mid layui-word-aux">如果密码为空则不修改</div>
                  </div>  
                  <div class="layui-form-item">
                    <div class="text-center">
                      <button class="layui-btn "  style="width:60%;" lay-submit lay-filter="SaveBtn">保存修改</button> 
                    </div>
                  </div>
                </form>
                `;
        },
        saveInfo: function () {
            form.on('submit(SaveBtn)', function (data) {
                if (data.field.password !== data.field.confirmPassword) {
                    alert("两次密码输入不匹配");
                    return false;
                }
                $.ajax({
                    url: '/Account/UpdateInfo',
                    type: 'post',
                    data: data.field,
                    dataType: 'json',
                    success: function (res) {
                        alert(res.msg);
                        layer.closeAll();
                    },
                    error: function (err) {
                        if (err.status === 401) {
                            alert("您未登录或登录失效，请刷新页面后重试！");
                        }
                    }
                });
                return false;
            });
        },
        uploadAvatar: function () {
            upload.render({
                elem: '#UploadAvatar',
                url: '/File/Upload',
                method: "post",
                data: { IsRich: false },
                accept: "images",
                done: function (res, index, upload) {
                    if (res.status && res.code === 200) {
                        $('#MyAvatar').attr('src', res.fullUrl);
                        $('input[name=Avatar]').val(res.fullUrl);
                    } else {
                        console.log(res.msg)
                    };
                }
            });
        },
        get: function () {
            var _loginState = localStorage.getItem('hadLogin');
            return _loginState === "true" ? true : false;
        },
        set: function (state) {
            localStorage.setItem('hadLogin', state);
        },
        logout: function () {
            var _this = this;
            _this._$logoutBtn.click(function () {
                var _index = layer.confirm('您确定要注销吗？', {
                    btn: ['确定', '点错了'],
                    btnAlign: 'c'
                }, function () {
                    $.ajax({
                        url: '/Account/Logout',
                        type: 'post',
                        data: false,
                        dataType: 'json',
                        success: function (res) {
                            if (res.status) {
                                _this._$hadLoginEl.hide();
                                _this._$toLoginEl.show();
                                _this._$toLoginEl.removeClass("display-none");
                                //$("#SearchContainer").removeClass("layui-col-md2").addClass("layui-col-md12");
                                //$("#SearchContainer").find("span").removeClass("display-none").show();
                                _this.set(false);
                                layer.msg('注销成功，您已退出系统！', { icon: 1 });
                            } else {
                                layer.msg('注销失败，请刷新页面！', { icon: 2 });
                            }
                        },
                        error: function (err) {
                            console.log(err)
                            console.log(err.status)
                            if (Number(err.status) === 401) {
                                _this._$hadLoginEl.hide();
                                _this._$toLoginEl.show();
                                _this.set(false);
                            }
                            layer.closeAll();
                        }
                    });

                });
            });
        },
        login: function () {
            var _this = this;
            _this._$loginBtn.click(function () {
                _this.render(1);
            });
            _this._$registerBtn.click(function () {
                _this.render(2);
            });
        },
        checked: function () {
            var _state = this.get();
            if (_state) {
                this._$hadLoginEl.show();
                _this._$hadLoginEl.removeClass("display-none");
                this._$toLoginEl.hide();
            } else {
                this._$hadLoginEl.hide();
                this._$toLoginEl.show();
            }
        },
        test: function () { },
        init: function () {
            this.test();
            this.bindShowInfo();
            //this.checked();
            this.login();
            this.logout();
        }
    };
    window.UserLogin = UserLogin;
});


search.init();
headerNav.init();
UserLogin.init();
//webSite.logo.load();
