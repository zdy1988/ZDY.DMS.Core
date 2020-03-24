require.config({
    baseUrl: '/assets/global',
    //urlArgs: 'timestamp=' + new Date(),
    paths: {
        "cookie": "plugins/js.cookie.min", //cookies 
        "jquery": "plugins/jquery.min",
        "jquery-migrate": "plugins/jquery-migrate.min",
        "jquery-ui": "plugins/jquery-ui/jquery-ui.min",
        "jquery-slimscroll": "plugins/jquery-slimscroll/jquery.slimscroll.min", //滚动条插件
        "jquery-uniform": "plugins/uniform/jquery.uniform.min", //form 美化插件
        "jquery-select2": "plugins/select2/js/select2.min",//dropdown插件
        "jquery-validation-plugin": "plugins/jquery-validation/js/jquery.validate.min",//数据验证插件
        "jquery-validation-additional-methods": "plugins/jquery-validation/js/additional-methods.min",//数据验证插件
        "jquery-validation": "plugins/jquery-validation/js/localization/messages_zh.min",
        "jquery-backstretch": "plugins/backstretch/jquery.backstretch.min",//背景插件
        "jquery-gritter": "plugins/gritter/js/jquery.gritter.min", //提示控件
        "jquery-hotkeys": "plugins/jquery-hotkeys/jquery.hotkeys",//按键插件
        "jquery-pulsate": "plugins/jquery.pulsate.min",//脉搏提示插件
        "jquery-nestable": "plugins/jquery-nestable/jquery.nestable", //可拖拽树结构
        "jquery-sortable": "plugins/jquery-sortable/jquery.sortable.min",//可拖拽排序
        "jquery-bootstrap-wizard": "plugins/bootstrap-wizard/jquery.bootstrap.wizard.min",
        "jquery-qrcode": "plugins/jquery-qrcode/jquery.qrcode.min",//qrcode生成
        "jquery-barcode": "plugins/jquery-barcode/jquery-barcode.min",//qrcode生成
        "jquery-printArea": "plugins/jquery.printArea",//qrcode生成
        "jquery-mixitup": "plugins/jquery-mixitup/jquery.mixitup.min",//排序插件
        "jquery-freewall": "plugins/jquery.freewall",//桌面快捷方式插件
        "jquery-table2excel": "plugins/jquery-table2excel/jquery.table2excel",//table 导出 excal
        "jquery-fancybox": "plugins/fancybox/source/jquery.fancybox", //看图插件
        "jquery-fancybox-thumbs": "plugins/fancybox/source/helpers/jquery.fancybox-thumbs",
        "jquery-fullcalendar": "plugins/fullcalendar/fullcalendar.min",//时间规划
        "jquery-fullcalendar-zh-cn": "plugins/fullcalendar/lang/zh-cn",
        "jquery-blockui": "plugins/jquery.blockui.min",

        "bootstrap": "plugins/bootstrap/js/bootstrap.min",
        "bootstrap-hover-dropdown": "plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min",// bootstrap 下拉菜单
        "bootstrap-switch": "plugins/bootstrap-switch/js/bootstrap-switch.min",//switch checkbox 插件
        "bootstrap-datepicker-plugin": "plugins/bootstrap-datepicker/js/bootstrap-datepicker",//日期控件中文包
        "bootstrap-datepicker": "plugins/bootstrap-datepicker/locales/bootstrap-datepicker.zh-CN.min",//日期控件
        "bootstrap-datetimepicker-plugin": "plugins/bootstrap-datetimepicker/js/bootstrap-datetimepicker.min",//日期时间控件中文包
        "bootstrap-datetimepicker": "plugins/bootstrap-datetimepicker/js/locales/bootstrap-datetimepicker.zh-CN",//日期时间控件
        "bootstrap-markdown": "plugins/bootstrap-markdown/js/bootstrap-markdown",//markdown 插件
        "bootstrap-summernote": "plugins/bootstrap-summernote/summernote.min",//markdown 插件
        "bootstrap-sweetalert": "plugins/bootstrap-sweetalert/sweetalert",//alert插件
        "bootstrap-toastr": "plugins/bootstrap-toastr/toastr",
        "bootstrap-contextmenu": "plugins/bootstrap-contextmenu/bootstrap-contextmenu",
        "bootstrap-citypicker-data": "plugins/bootstrap-citypicker/js/city-picker.data.min",
        "bootstrap-citypicker": "plugins/bootstrap-citypicker/js/city-picker.min",
        "bootstrap-fileinput": "plugins/bootstrap-fileinput/bootstrap-fileinput",//上传插件

        "jstree": "plugins/jstree/dist/jstree",
        "pace": "plugins/pace/pace.min",

        "markdown": "plugins/bootstrap-markdown/lib/markdown",
        "moment": "plugins/fullcalendar/lib/moment.min",
        "zrender": "plugins/zrender/zrender.min",

        "flow-designer": "/assets/global/scripts/workflow/flow-designer.min",
        "form-builder": "/assets/global/scripts/workflow/form-builder.min",
        "form-render": "/assets/global/scripts/workflow/form-render.min",

        //图表插件
        "echarts": "plugins/echart/echarts.min",
        "echarts-theme-macarons": "plugins/echart/theme/macarons",

        //通信组件
        "signalr": "/signalr/hubs?ak",
        "jquery-signalr": "/scripts/jquery.signalR-2.2.0.min",

        "app": "scripts/app.min",
        "ko": "scripts/core/knockout.min",
        "zdy": "scripts/core/zdy.amd.min",

        "_Layout": "/assets/layouts/layout/scripts/demo",
        "_Layout2": "/assets/layouts/layout2/scripts/demo",
        "_Layout3": "/assets/layouts/layout3/scripts/demo"
    },
    map: {
        "*": {
            "css": "scripts/core/css.min"
        }
    },
    shim: {
        "zdy": ["jquery", "ko"],

        "jquery-migrate": ["jquery"],
        "jquery-ui": ["jquery", "css!plugins/jquery-ui/jquery-ui.min.css"],
        "jquery-slimscroll": ["jquery"],
        "jquery-uniform": ["jquery", "css!plugins/uniform/css/uniform.default.css"],
        "jquery-select2": ["jquery", "css!plugins/select2/css/select2.min.css", "css!plugins/select2/css/select2-bootstrap.min.css"],
        "jquery-validation": ["jquery", "jquery-validation-plugin"],
        "jquery-validation-additional-methods": ["jquery", "jquery-validation"],
        "jquery-backstretch": ["jquery"],
        "jquery-gritter": ["jquery", "css!plugins/gritter/css/jquery.gritter.css"],
        "jquery-hotkeys": ["jquery", "jquery-migrate"],
        "jquery-pulsate": ["jquery", "jquery-migrate"],
        "jquery-nestable": ["jquery", "css!plugins/jquery-nestable/jquery.nestable.css"],
        "jquery-sortable": ["jquery", "css!plugins/jquery-sortable/jquery.sortable.css"],
        "jquery-bootstrap-wizard": ["jquery"],
        "jquery-qrcode": ["jquery"],
        "jquery-barcode": ["jquery"],
        "jquery-printArea": ["jquery"],
        "jquery-mixitup": ["jquery"],
        "jquery-freewall": ["jquery"],
        "jquery-table2excel": ["jquery"],
        "jquery-fancybox": ["jquery", "jquery-migrate", "css!plugins/fancybox/source/jquery.fancybox.css"],
        "jquery-fancybox-thumbs": ["jquery", "jquery-migrate", "jquery-fancybox", "css!plugins/fancybox/source/helpers/jquery.fancybox-thumbs.css"],
        "jquery-fullcalendar": ["jquery", "moment", "css!plugins/fullcalendar/fullcalendar.min.css"],
        "jquery-fullcalendar-zh-cn": ["jquery-fullcalendar"],

        "bootstrap": ["jquery"],
        "bootstrap-hover-dropdown": ["jquery"],
        "bootstrap-switch": ["jquery", "css!plugins/bootstrap-switch/css/bootstrap-switch.min.css"],
        "bootstrap-datepicker-plugin": ["jquery", "css!plugins/bootstrap-datepicker/css/bootstrap-datepicker3.min.css"],
        "bootstrap-datepicker": ["jquery", "bootstrap-datepicker-plugin"],
        "bootstrap-datetimepicker-plugin": ["jquery", "css!plugins/bootstrap-datetimepicker/css/bootstrap-datetimepicker.min.css"],
        "bootstrap-datetimepicker": ["jquery", "bootstrap-datetimepicker-plugin"],
        "bootstrap-markdown": ["jquery", "markdown", "css!plugins/bootstrap-markdown/css/bootstrap-markdown.min.css"],
        "bootstrap-summernote": ["jquery", "css!plugins/bootstrap-summernote/summernote.css"],
        "bootstrap-sweetalert": ["css!plugins/bootstrap-sweetalert/sweetalert.css"],
        "bootstrap-toastr": ["css!plugins/bootstrap-toastr/toastr.css"],
        "bootstrap-contextmenu": ["jquery", "css!plugins/bootstrap-toastr/toastr.css"], 
        "bootstrap-citypicker": ["jquery", "bootstrap-citypicker-data", "css!plugins/bootstrap-citypicker/css/city-picker.css"],
        "bootstrap-fileinput": ["jquery", "moment","css!plugins/bootstrap-fileinput/bootstrap-fileinput.css"],

        "jstree": ["css!plugins/jstree/dist/themes/default/style.min.css"],
        "pace": ["css!plugins/pace/themes/pace-theme-flash.css"],

        "flow-designer": ["zrender"],
        "form-builder": ["jquery", "jquery-ui"],
        "form-render": ["jquery"],

        "jquery-signalr": ["jquery"],
        "signalr": ["jquery-signalr"]
    }
});