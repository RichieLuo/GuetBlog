$(function () {

    //插件初始化和使用Demo

    //时间日期插件：datetimepicker
    //<input type="text" value="2018/07/30 12:30:05" title="时间" id="taskRemindTime" class="btn btn-default task-remindTime" />
    //$("input[name=remindTime]").datetimepicker();


    //分页插件：pagination
    //<div class="task-list-pagination"></div>
    //$(".task-list-pagination").pagination({
    //    totalData: 1000,
    //    showData: 5,
    //    pageCount: 5,
    //    jump: true,
    //    coping: true,
    //    homePage: '首页',
    //    endPage: '末页',
    //    prevContent: '上页',
    //    nextContent: '下页',
    //    callback: function (api) {
    //        //console.log(api.getCurrent());
    //        //TODO
    //    }
    //});

    //防xss注入插件：xss
    //filterXSS(值)


    //消息提醒插件：toastr
    //toastr.options.positionClass = 'toast-bottom-right';
    //toastr.success("提示消息"); //还有很多的方法：消息、警告


    //弹窗插件：layer
    //查看官网：http://layer.layui.com/

    //表单校验
    //$("form").validate(); //初始化
    //if (!form.valid()) return; //校验开始（表单元素必须加上require
    

    //封装一个获取表单数据为json对象的扩展 //调用：$("form").serializeJson();
    (function (window, $) {
        $.fn.serializeJson = function () {
            var serializeObj = {};
            var array = this.serializeArray();
            var str = this.serialize();
            $(array).each(
                function () {
                    if (serializeObj[this.name]) {
                        if ($.isArray(serializeObj[this.name])) {
                            serializeObj[this.name].push(this.value);
                        } else {
                            serializeObj[this.name] = [
                                serializeObj[this.name], this.value
                            ];
                        }
                    } else {
                        serializeObj[this.name] = this.value;
                    }
                });
            return serializeObj;
        };
    })(window, jQuery); 
 
})