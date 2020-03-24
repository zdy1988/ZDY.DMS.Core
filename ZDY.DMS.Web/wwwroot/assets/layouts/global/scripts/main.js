require([
    "pace",
    "jquery",
    "ko",
    "zdy",
    "app",
    "bootstrap",
    ui], function (pace, $, ko, zdy, app) {
        var ui = arguments[arguments.length - 1];
        ui.init();

        var user = zdy.user.get();

        var main = zdy.module().import(function () {

            this.openedPages = ko.observableArray();
            this.activePage = ko.observable({
                MenuName: ""
            });

            this.openPage = function (data, e) {
                var json = $(e.target).attr("data-json");
                if (e.target.nodeName === "SPAN") {
                    json = $(e.target.parentNode).attr("data-json");
                }
                if (json === undefined) {
                    return false;
                }
                window.openWindow(JSON.parse(json), true);
                return false;
            };
            this.closePage = function (data, e) {
                window.closeWindow(data);
                return false;
            };

            this.user = ko.observable(user);

            this.profile = function () {
                window.gotoPage("/Home/UserProfile");
            };

            this.calendar = function () {
                window.gotoPage("/Home/Calendar");
            };

            this.inbox = function () {
                window.gotoPage("/Home/Inbox");
            };

            this.task = function () {
                window.gotoPage("/Home/Task");
            };

            this.logout = function () {
                zdy.confirm("注销账号？").done(function () {
                    $.when(zdy.token.set(""), zdy.user.set("")).done(function () {
                        window.location = "/Index";
                    });
                });
            };

            this.searchText = ko.observable();

            this.handleSearch = function () {
                zdy.alert.info("查询功能尚未完善");
            };

            $('.search-form input').keypress(function (e) {
                if (e.which === 13) {
                    main.handleSearch();
                }
            });

        }).bind();

        window.setInterval(function () {
            var $avtivePage = $('.page-content-inner div.page:visible');
            var $avtiveIframe = $avtivePage.find("iframe");
            if ($avtiveIframe.length > 0) {
                if ($avtiveIframe[0].style.visibility !== "hidden") {
                    var $contentHeight = $avtiveIframe.contents().height();
                    $avtiveIframe.height($contentHeight);
                }
            }
        }, 200);

        window.openWindow = function (page, isRefresh) {
            var pageId = page.PageId;
            var pageTitle = page.MenuName;
            var pageSrc = page.PageSrc;

            main.openedPages.remove(function (item) {
                return item.PageId === page.PageId;
            });
            main.openedPages.unshift(page);
            main.activePage(page);

            var $pageContent = $(".page-content-inner");

            if (isRefresh === true) {
                $(".page-content-inner").find("#" + page.PageId).remove();
            }

            var $page = $pageContent.find("#" + pageId);

            if ($page.length === 0) {
                $page = $('<div class="page" id="{pageId}"><iframe src="{pageSrc}" style="width:100%; min-height:1000px; border:none; visibility:hidden;" scrolling="no" frameborder="0"></iframe></div>'.replace("{pageId}", pageId).replace("{pageSrc}", pageSrc));
                $page.find('iframe').on("load", function () {
                    this.style.visibility = "";
                });
                $pageContent.append($page);
            }

            $pageContent.find('div.page').hide();
            $page.show();
            pace.restart();

            window.location = "/Home/Main#" + pageSrc;

            app.scrollTop();
        };

        window.onhashchange = function (e) {
            var url = e.newURL.split("#")[1];
            if (url !== undefined) {
                var data = main.openedPages().find(function (page) {
                    return String(page.PageSrc).toLowerCase() === url.toLowerCase();
                });
                if (data === undefined) {
                    var $link = $("#menu a[href='" + url + "']");
                    if ($link.length > 0) {
                        data = $link.data("json");
                    }
                }
                if (data !== undefined) {
                    window.openWindow(data);
                }
            }
        };

        window.closeWindow = function (page) {
            if (main.openedPages().length === 1) {
                return false;
            }

            main.openedPages.remove(function (item) {
                return item.PageId === page.PageId;
            });

            $(".page-content-inner").find("#" + page.PageId).remove();

            var candidatePage = main.openedPages()[0];

            window.openWindow(candidatePage);
        };

        window.gotoIndex = function () {
            window.location = "/Index";
        };

        window.gotoPage = function (url) {
            if (url === undefined || url === "") {
                return false;
            }
            var info = url.split("?");
            var href = info[0];
            var data;
            var $link = $("#menu a[href='" + href + "']");
            if ($link.length > 0) {
                var json = $link.attr("data-json");
                data = JSON.parse(json);
            }
            if (data !== undefined) {
                data.PageSrc = url;
                window.openWindow(data, true);
            }
        };

        window.closePage = function (url) {
            if (url === undefined || url === "") {
                return false;
            }
            var info = url.split("?");
            var href = info[0];
            var data;
            var $link = $("#menu a[href='" + href + "']");
            if ($link.length > 0) {
                data = $link.data("json");
            }
            if (data !== undefined) {
                data.PageSrc = url;
                window.closeWindow(data);
            }
        };

        window.toastr = {
            error: function (text) {
                app.showToastr("error", text, "错误提示");
            },
            warning: function (text) {
                app.showToastr("warning", text, "警告提示");
            },
            success: function (text) {
                app.showToastr("success", text, "成功提示");
            },
            info: function (text) {
                app.showToastr("warning", text, "信息提示");
            }
        };

        window.alert = {
            error: function (text) {
                return app.sweetAlert({
                    text: text,
                    type: "error",
                    confirmButtonClass: 'btn-danger'
                });
            },
            warning: function (text) {
                return app.sweetAlert({
                    text: text,
                    type: "warning",
                    confirmButtonClass: 'btn-danger'
                });
            },
            success: function (text) {
                return app.sweetAlert({
                    text: text,
                    type: "success",
                    confirmButtonClass: 'btn-danger'
                });
            },
            info: function (text) {
                return app.sweetAlert({
                    text: text,
                    type: "info",
                    confirmButtonClass: 'btn-danger'
                });
            }
        };

        window.confirm = function (text) {
            return app.sweetAlert({
                text: text,
                allowOutsideClick: true,
                showConfirmButton: true,
                showCancelButton: true,
                confirmButtonClass: 'btn-info',
                cancelButtonClass: 'btn-danger',
                type: 'warning',
                confirmButtonText: '确认',
                cancelButtonText: '取消'
            });
        };

        window.gotoPage("/Home/DashBoard");
    });