

layui.use(['form', 'layer'], function () {
    let form = layui.form,
        layer = layui.layer;


    const _blogArticle = {
        /** 评论组件封装 */
        commentArea: function (options) {
            let _cancelBtn = '',
                _options = {
                    level: 1,
                    userId: '',
                    isReply: false,
                    element: '',
                    submitBtn: ''
                };
            $.extend(_options, options);
            _options.userId = _options.userId !== '' ? `@${_options.userId} ` : '';
            if (_options.level === 1) {
                _options.submitBtn = 'TOP'
            } else {
                _options.submitBtn = _options.element;
            }
            if (_options.isReply) {
                _cancelBtn = `<span class="cancel-comment-reply">取消评论</span>`;
            }
            let _template = `
                <div class="blog-article-comment">
                    <h3 class="comment-title text-blue"><i class="layui-icon layui-icon-edit"></i> 发表评论 ${_cancelBtn}</h3>
                    <form name="ArticleCommentForm" class="layui-form layui-form-pane comment-form" action="">
                       <input type="hidden" name="ParentId" value="${_options.parentId}" />
                        <div class="layui-form-item">
                            <div class="layui-row">
                                <div class="layui-col-md4">
                                    <input type="text" name="NickName" placeholder="请填写您的名称" value="" autocomplete="off" class="layui-input">
                                </div>
                                <div class="layui-col-md4">
                                    <input type="text" name="EMail" placeholder="请填写您的邮箱" value=""  autocomplete="off" class="layui-input">
                                </div>
                                <div class="layui-col-md4">
                                    <input type="text" name="SiteUrl" placeholder="http://" autocomplete="off" value="" class="layui-input">
                                </div>
                            </div>
                            <div class="layui-form-item layui-form-text">
                                <label class="layui-form-label">快捷回复</label>
                                <div class="layui-input-block">
                                    <textarea name="Content" required lay-verify="required"  placeholder="和谐网络 文明上网 理性发言！O(∩_∩)O~~" class="layui-textarea">${_options.userId}</textarea>
                                </div>
                            </div>
                            <div class="layui-form-item">
                                <div class="layui-input-inline">
                                    <button class="layui-btn bg-blue" lay-submit lay-filter="${_options.submitBtn}SubmitBtn"  data-level="${options.level}">发表评论</button>
                                </div>
                            </div>
                        </div>
                    </form>
                </div>
            `;
            if (!_options.isReply) {
                $(_options.element).html(_template);
                _options.level = 1;
            }
            setTimeout(() => {
                this.commentSubmit(_options);
            }, 500);
            return _template;
        },
        /** 评论、回复成功后的模板 */
        commentMessage: function (options) {
            let _this = this,
                _template = '',
                _options = {
                    level: 1,
                    comment: {
                        id: '',
                        createTime: '',
                        updateTime: '',
                        content: '没有填写评论',
                        userId: '123',
                        avatar: '/images/header/avatar_002.jpg',
                        nickname: '该用户很懒没有填写昵称',
                        siteUrl: '',
                        email: '',
                        commentItems: [],
                        isAdmin: false,
                    }
                };
            $.extend(_options, options);
            console.log(_options)
            let _indentityName = _options.comment.isAdmin ? '博主' : '游客';
            let _indentityNameColor = _options.comment.isAdmin ? "identity-admin" : "";
            let _deleteBtn = _options.comment.canDelete ? `<button type="button" class="layui-btn layui-btn-xs layui-btn-danger comments-delete-btn" data-commentid="${_options.comment.id}" data-level="${Number(_options.level) + 1}">删除</button>` : '';
            if (_options.level === 1) { //一级评论 
                _template = `
                    <li class="article-comment-item">
                        <div class="layui-row">
                            <div class="layui-col-md1">
                                <img class="blog-radius comments-user-avatar" src="${_options.comment.avatar}" alt="${_options.comment.nickname}">
                            </div>
                            <div class="layui-col-md11 comments-details">
                                <div class="comments-user-info">
                                    <span class="comments-user-identity ${_indentityNameColor} layui-btn layui-btn-xs">${_indentityName}</span>
                                    <a class="comments-user-nickname" href="javascript:" title="${_options.comment.nickname}">${_options.comment.nickname}</a> 
                                    ${_deleteBtn}
                                    <button type="button" class="layui-btn layui-btn-xs comments-reply-btn" data-parentid="${_options.comment.id}" data-userid="${_options.comment.userId}" data-nickname="${_options.comment.nickname}" data-level="2">回复</button>
                                </div>
                                <div class="comment-item-content">
                                     ${_options.comment.content}
                                </div>
                                <span class="article-time-ago">${_options.comment.cTime}</span>
                                <ul class="article-comment-list level2 border-line ${_options.comment.commentItems.length <= 0 ? 'display-none' : ''}">
                                    <!-- 二级评论回复列表 --> 
                                </ul>
                                <!-- 发表评论回复区域 --> 
                            </div> 
                        </div>
                    </li>
                `;
                let _$level1Container = $('.article-comment-list.level1');
                _$level1Container.find('.article-comment-item.not-comments').remove();
                _$level1Container.find('.article-comment-item.display-none').remove();
                _$level1Container.append(_template);
            } else if (_options.level === 2) { //二级评论
                _template = `
                    <li class="article-comment-item">
                        <div class="layui-row">
                            <div class="layui-col-md12 comments-details">
                                <div class="comments-user-info">
                                    <span class="comments-user-identity ${_indentityNameColor} layui-btn layui-btn-xs">${_indentityName}</span>
                                    <a class="comments-user-nickname" href="javascript:" title="${_options.comment.nickname}">${_options.comment.nickname}</a> 
                                       ${_deleteBtn}
                                    <button type="button" class="layui-btn layui-btn-xs comments-reply-btn" data-parentid="${_options.comment.parentId}" data-userid="${_options.comment.userId}" data-nickname="${_options.comment.nickname}" data-level="3">回复</button>
                                </div>
                                <div class="comment-item-content"> 
                                    ${_options.comment.content}
                                </div>
                                <span class="article-time-ago">${_options.comment.cTime}</span>
                                <!-- 发表评论回复区域 -->
                            </div>
                        </div>
                    </li>
                `;
                let _$level2Container = $(_options.container).find('.article-comment-list.level2');
                if (_$level2Container.hasClass('display-none')) {
                    _$level2Container.removeClass('display-none');
                }
                _$level2Container.append(_template);

            } else { //三级评论
                _template = `
                <li class="article-comment-item">
                    <div class="layui-row">
                        <div class="layui-col-md12 comments-details">
                            <div class="comments-user-info"> 
                                <span class="comments-user-identity ${_indentityNameColor} layui-btn layui-btn-xs">${_indentityName}</span>
                                <a class="comments-user-nickname" href="javascript:" title="${_options.comment.nickname}">${_options.comment.nickname}</a> 
                                <button type="button" class="layui-btn layui-btn-xs comments-reply-btn" data-parentid="${_options.comment.parentId}"  data-userid="${_options.comment.userId}" data-nickname="${_options.comment.nickname}" data-level="3">回复</button>
                            </div>
                            <div class="comment-item-content">
                                ${_options.comment.content}
                            </div>
                            <span class="article-time-ago">${_options.comment.cTime}</span>
                            <!-- 发表评论回复区域 -->
                        </div>
                    </div>
                </li>
            `;
                let _$level2Container = $(_options.container);
                if (_$level2Container.hasClass('display-none')) {
                    _$level2Container.removeClass('display-none');
                }
                _$level2Container.append(_template);
            }
            _this.replyBtn(_options);
            return _template;
        },
        /** 提交评论或者回复 */
        commentSubmit: function (options) {
            let _this = this;
            form.render();

            form.on(`submit(${options.submitBtn}SubmitBtn)`, function (data) {
                var _$this = $(this);
                if (options.level === 1) {
                    data.field.articleId = $(options.element).data('articleid');
                }
                $.ajax({
                    url: options.level === 1 ? '/Home/Comment' : '/Home/CommentItem',
                    type: 'POST',
                    data: data.field,
                    dataType: 'json',
                    success: function (res) {
                        console.log(res);
                        //模拟处理成功后添加
                        let _$container = '';
                        switch (options.level) {
                            case 2:
                                _$container = _$this.eq(0).parent().parent().parent().parent().parent().parent();
                                break;
                            case 3:
                                _$container = _$this.eq(0).parent().parent().parent().parent().parent().parent().parent().parent().parent();
                                break;
                        }
                        _this.commentMessage({
                            isReply: options.isReply,
                            level: options.level,
                            container: _$container,
                            comment: {
                                id: res.data.id,
                                cTime: res.data.cTime,
                                createTime: res.data.createTime,
                                updateTime: '',
                                content: res.data.content,
                                userId: res.data.userId,
                                avatar: res.data.avatar,
                                nickname: res.data.nickName,
                                siteUrl: res.data.siteUrl,
                                email: res.data.eMail,
                                commentItems: res.data.children,
                                isAdmin: res.isAdmin,
                                parentId: res.data.parentId,
                                canDelete: res.data.canDelete
                            }
                        });

                        //隐藏评论回复区域
                        if (options.isReply) {
                            _$this.eq(0).parent().parent().parent().parent().parent().remove();
                        }
                        $('form[name=ArticleCommentForm]')[0].reset();

                        console.log(document.body.clientHeight)
                        $('html,body').animate({ scrollTop: document.body.clientHeight }, 800);
                    }
                });
                return false;
            });
        },
        /** 取消评论回复 */
        commentCancel: function (options) {
            $('.cancel-comment-reply').off('click').on('click', function () {
                $(this).parent().parent().remove();
            });
        },
        /** 绑定回复按钮 添加对应评论回复提交区域 */
        replyBtn: function (options) {
            let _this = this;
            $('.comments-reply-btn').off('click').on('click', function () {
                let _$container = $(this).parent().parent();
                if (_$container.find('.blog-article-comment').length > 0) {
                    _$container.find('.blog-article-comment').remove();
                } else {
                    _$container.append(_this.commentArea({
                        //userId: $(this).data('userid') || $(this).data('nickname'),
                        userId: $(this).data('nickname'),
                        isReply: true,
                        element: $(this).index(),
                        level: $(this).data('level'),
                        parentId: $(this).data("parentid")
                    }));
                    _this.commentCancel();
                }
            });

            $('.comments-delete-btn').off('click').on('click', function () {
                let _$this = $(this);
                let _$container = _$this.parent().parent().parent().parent();

                if (confirm("您确定要删除该评论吗？")) {
                    $.ajax({
                        url: '/Home/DeleteComment',
                        type: 'POST',
                        data: { Id: _$this.data("commentid"), IsParent: _$this.data("level") === 2 ? true : false },
                        dataType: 'json',
                        success: function (res) {
                            alert(res.msg)
                            if (res.status) {
                                _$container.remove();
                            }
                        },
                        error: function (err) {
                            if (err.status === 401) {
                                alert("删除失败，未登录或者您不是管理员！")
                            }
                            console.log(err)
                        }
                    })
                }

            });
        },
        /** 评论列表组件封装 */
        commentList: function (options) {


        },
        /** 返回文章列表 */
        backToList: function () {

        },
        /** 文章点赞、打赏、分享 */
        likes: function () {
            $('#ArticleLike').off('click').on('click', function () {
                console.log($(this).attr('title'));

            });
            $('#ArticleReward').off('click').on('click', function () {
                $.ajax({
                    url: '/Home/GetSiteInfo',
                    dataType: 'json',
                    type: 'POST',
                    success: function (res) {
                        layer.open({
                            type: 1,
                            title: false,
                            closeBtn: 0,
                            area: ['350px', '380px'],
                            skin: 'ArticleRewardQrCode', //没有背景色
                            shadeClose: true,
                            content: `
                            <a id="ArticleRewardQrCode" href="javascript:" title="打赏博主">
                                <img src="${res.qrCode}" alt="打赏博主">
                                <p class="text-center">打赏一下博主吧</p>
                            </a>`
                        });
                    }
                })

            });
            // $('#ArticleShare').off('click').on('click', function() {
            //     console.log($(this).attr('title'));
            //     layer.open({
            //         type: 1,
            //         title: false,
            //         closeBtn: true,
            //         area: ['400px', '100px'],
            //         skin: 'ArticleShareArea', //没有背景色
            //         id: 'ArticleShareArea',
            //         shadeClose: true,
            //         content: $('#ArticleShareCode').html(),
            //         success: function() {
            //             setTimeout(() => {

            //             }, 500);
            //         }
            //     });
            // });
        },
        /** 切换右侧栏目 */
        toggleRight: function () {
            let _isShow = localStorage.getItem('IsShowRightArea') == "true" ? true : false || false, _this = this;

            _this.isShowRight(_isShow);
            $('#ToggleRight').off('click').on('click', function () {
                _isShow = !_isShow;
                localStorage.setItem('IsShowRightArea', _isShow);
                _this.isShowRight(_isShow);
            });
        },
        isShowRight: function (_isShow) {
            console.log(_isShow)
            let _$btn = $('#ToggleRight'),
                _$left = $('.article-left'),
                _$right = $('.article-right');
            if (!_isShow) {
                _$left.addClass('article-right-none');
                _$left.removeClass('layui-col-md9').addClass('layui-col-md12');
                _$right.removeClass('layui-col-md3').addClass('display-none');
                _$btn.attr('title', '显示侧边栏');
                _$btn.html('<i class="layui-icon">&#xe668;</i>显示侧边栏');
            } else {
                _$left.removeClass('article-right-none');
                _$left.removeClass('layui-col-md12').addClass('layui-col-md9');
                _$right.removeClass('display-none').addClass('layui-col-md3');
                _$btn.attr('title', '隐藏侧边栏');
                _$btn.html('<i class="layui-icon">&#xe66b;</i>隐藏侧边栏');
            }
        },
        render: function (options) {
            let _this = this;
            _this.likes();
            _this.toggleRight();
            _this.replyBtn();
            _this.commentArea(options);
        },
        init: function (options) {
            this.render(options);
        }
    };

    /** 初始化顶级评论回复输入区域 */
    _blogArticle.init({
        element: '.article-comment-area',
        level: 1
    });
});