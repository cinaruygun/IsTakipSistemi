var aaaa777 = 0;
var isReload = false;
$.ErcanAyhan = {};
$.ErcanAyhan.options = {
    colors: {
        red: '#F44336',
        pink: '#E91E63',
        purple: '#9C27B0',
        deepPurple: '#673AB7',
        indigo: '#3F51B5',
        blue: '#2196F3',
        lightBlue: '#03A9F4',
        cyan: '#00BCD4',
        teal: '#009688',
        green: '#4CAF50',
        lightGreen: '#8BC34A',
        lime: '#CDDC39',
        yellow: '#ffe821',
        amber: '#FFC107',
        orange: '#FF9800',
        deepOrange: '#FF5722',
        brown: '#795548',
        grey: '#9E9E9E',
        blueGrey: '#607D8B',
        black: '#000000',
        white: '#ffffff'
    },
    leftSideBar: {
        scrollColor: 'rgba(0,0,0,0.5)',
        scrollWidth: '4px',
        scrollAlwaysVisible: false,
        scrollBorderRadius: '0',
        scrollRailBorderRadius: '0'
    },
    dropdownMenu: {
        effectIn: 'fadeIn',
        effectOut: 'fadeOut'
    }
}
//Left Sidebar
var activeMenu;
var splitPath = window.location.pathname.split("/");
var locPath = "/" + splitPath[1] + (splitPath[2] != undefined ? "/" + splitPath[2] + (splitPath[3] != undefined ? "/" + splitPath[3] : "") : "");
$.ErcanAyhan.LeftSideBar = {
    activate: function () {
        var _this = this;
        var $body = $('body');
        var $overlay = $('.overlay');
        //Close sidebar
        $(window).click(function (e) {
            var $target = $(e.target);
            if (e.target.nodeName.toLowerCase() === 'i') { $target = $(e.target).parent(); }
            if (!$target.hasClass('bars') && _this.isOpen() && $target.parents('#leftsidebar').length === 0) {
                if (!$target.hasClass('js-right-sidebar')) $overlay.fadeOut();
                $body.removeClass('overlay-open');
            }
        });
        //Set Active Menu
        activeMenu = $("div.menu").find("[href='" + locPath + "']")
        if (activeMenu.length != 0) {
            activeMenu.addClass((activeMenu.parent().parent().parent().find("i")[0].className.replace("material-icons", "")));
            activeMenu.parent().addClass('active');
            activeMenu.parent().parent().parent().addClass('active');
            activeMenu.parent().parent().parent().parent().parent().addClass('active');
            activeMenu.parent().parent().parent().parent().parent().parent().parent().addClass('active');
            activeMenu.parent().parent().parent().find("span")[0].className += (activeMenu.parent().parent().parent().find("i")[0].className.replace("material-icons", ""))
            activeMenu.parent().parent().parent().parent().parent().find("span")[0].className += (activeMenu.parent().parent().parent().parent().parent().find("i")[0].className.replace("material-icons", ""))
            if (activeMenu.parent().find("i")[0] != undefined)
                activeMenu.parent().find("span").addClass(activeMenu.parent().find("i")[0].className.replace("material-icons", ""))
        }
        var userMenu = $(".user-helper-dropdown").find("[href='" + locPath + "']");
        if (userMenu.find("i")[0] != undefined) {
            userMenu.addClass(userMenu.find("i")[0].className.replace("material-icons", ""));
            $(".user-helper-dropdown").addClass(userMenu.find("i")[0].className.replace("material-icons", ""))
            $(".user-helper-dropdown i").addClass("circle").attr("style", "background-color:#fff")
        }

        // set active menu scroll

        $.each($('.menu-toggle.toggled'), function (i, val) {
            $(val).next().slideToggle(0);
        });
        //When page load
        $.each($('.menu .list li.active'), function (i, val) {
            var $activeAnchors = $(val).find('a:eq(0)');
            $activeAnchors.addClass('toggled');
            $activeAnchors.next().show();
        });
        //Collapse or Expand Menu
        $('.menu-toggle').on('click', function (e) {
            var $this = $(this);
            var $content = $this.next();
            if ($($this.parents('ul')[0]).hasClass('list')) {
                var $not = $(e.target).hasClass('menu-toggle') ? e.target : $(e.target).parents('.menu-toggle');
                $.each($('.menu-toggle.toggled').not($not).next(), function (i, val) {
                    if ($(val).is(':visible')) {
                        $(val).prev().toggleClass('toggled');
                        $(val).slideUp();
                    }
                });
            }
            $this.toggleClass('toggled');
            $content.slideToggle(320);
        });
        //Set menu height
        _this.setMenuHeight();
        _this.checkStatuForResize(true);
        $(window).resize(function () {
            _this.setMenuHeight();
            _this.checkStatuForResize(false);
        });
        //Set Waves
        Waves.attach('.menu .list a', ['waves-block']);
        Waves.init();
    },
    setMenuHeight: function () {
        if (typeof $.fn.slimScroll != 'undefined' && $.ErcanAyhan.Browser.IsMobileDevice() == false) {
            var configs = $.ErcanAyhan.options.leftSideBar;
            var height = ($(window).height() - ($('.legal').outerHeight() + $('.user-info').outerHeight() + $('.navbar').innerHeight()));
            var $el = $('.list');
            $el.slimscroll({
                height: height + "px",
                color: configs.scrollColor,
                size: configs.scrollWidth,
                alwaysVisible: configs.scrollAlwaysVisible,
                borderRadius: configs.scrollBorderRadius,
                railBorderRadius: configs.scrollRailBorderRadius,
                allowPageScroll: true,
                wheelStep: 10,
                touchScrollStep: 75
            });
        }
    },
    checkStatuForResize: function (firstTime) {
        var $body = $('body');
        var $openCloseBar = $('.navbar .navbar-header .bars');
        var width = $body.width();
        if (firstTime) {
            $body.find('.content, .sidebar').addClass('no-animate').delay(1000).queue(function () {
                $(this).removeClass('no-animate').dequeue();
            });
        }
        if (width < 1170) {
            $body.addClass('ls-closed');
            $openCloseBar.fadeIn();
        }
        else {
            $body.removeClass('ls-closed');
            $openCloseBar.fadeOut();
        }
    },
    isOpen: function () {
        return $('body').hasClass('overlay-open');
    }
};
//Navbar
$.ErcanAyhan.Navbar = {
    activate: function () {
        var $body = $('body');
        var $overlay = $('.overlay');
        //Open left sidebar panel
        $('.bars').on('click', function () {
            $body.toggleClass('overlay-open');
            if ($body.hasClass('overlay-open')) { $overlay.fadeIn(); } else { $overlay.fadeOut(); }
        });
        //Close collapse bar on click event
        $('.nav [data-close="true"]').on('click', function () {
            var isVisible = $('.navbar-toggle').is(':visible');
            var $navbarCollapse = $('.navbar-collapse');
            if (isVisible) {
                $navbarCollapse.slideUp(function () {
                    $navbarCollapse.removeClass('in').removeAttr('style');
                });
            }
        });
    }
}
//Input
$.ErcanAyhan.Input = {
    activate: function () {
        //On focus event
        $('.form-control').focus(function () {
            $(this).parent().addClass('focused');
        });
        //On focusout event
        $('.form-control').focusout(function () {
            var $this = $(this);
            if ($this.parents('.form-group').hasClass('form-float')) {
                if ($this.val() == '') { $this.parents('.form-line').removeClass('focused'); }
            }
            else {
                $this.parents('.form-line').removeClass('focused');
            }
        });
        //On label click
        $('body').on('click', '.form-float .form-line .form-label', function () {
            $(this).parent().find('input').focus();
        });
    }
}
// Form - Select
$.ErcanAyhan.Select = {
    activate: function () {
        if ($.fn.selectpicker) { $('select:not(.ms)').selectpicker(); }
    }
}
// DropdownMenu
$.ErcanAyhan.DropdownMenu = {
    activate: function () {
        var _this = this;
        $('.dropdown, .dropup, .btn-group').on({
            "show.bs.dropdown": function () {
                var dropdown = _this.dropdownEffect(this);
                _this.dropdownEffectStart(dropdown, dropdown.effectIn);
            },
            "shown.bs.dropdown": function () {
                var dropdown = _this.dropdownEffect(this);
                if (dropdown.effectIn && dropdown.effectOut) {
                    _this.dropdownEffectEnd(dropdown, function () { });
                }
            },
            "hide.bs.dropdown": function (e) {
                var dropdown = _this.dropdownEffect(this);
                if (dropdown.effectOut) {
                    e.preventDefault();
                    _this.dropdownEffectStart(dropdown, dropdown.effectOut);
                    _this.dropdownEffectEnd(dropdown, function () {
                        dropdown.dropdown.removeClass('open');
                    });
                }
            }
        });
        //Set Waves
        Waves.attach('.dropdown-menu li a', ['waves-block']);
        Waves.init();
    },
    dropdownEffect: function (target) {
        var effectIn = $.ErcanAyhan.options.dropdownMenu.effectIn, effectOut = $.ErcanAyhan.options.dropdownMenu.effectOut;
        var dropdown = $(target), dropdownMenu = $('.dropdown-menu', target);
        if (dropdown.size() > 0) {
            var udEffectIn = dropdown.data('effect-in');
            var udEffectOut = dropdown.data('effect-out');
            if (udEffectIn !== undefined) { effectIn = udEffectIn; }
            if (udEffectOut !== undefined) { effectOut = udEffectOut; }
        }
        return {
            target: target,
            dropdown: dropdown,
            dropdownMenu: dropdownMenu,
            effectIn: effectIn,
            effectOut: effectOut
        };
    },
    dropdownEffectStart: function (data, effectToStart) {
        if (effectToStart) {
            data.dropdown.addClass('dropdown-animating');
            data.dropdownMenu.addClass('animated dropdown-animated');
            data.dropdownMenu.addClass(effectToStart);
        }
    },
    dropdownEffectEnd: function (data, callback) {
        var animationEnd = 'webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend';
        data.dropdown.one(animationEnd, function () {
            data.dropdown.removeClass('dropdown-animating');
            data.dropdownMenu.removeClass('animated dropdown-animated');
            data.dropdownMenu.removeClass(data.effectIn);
            data.dropdownMenu.removeClass(data.effectOut);
            if (typeof callback == 'function') {
                callback();
            }
        });
    }
}
String.prototype.turkishToUpper = function () {
    var string = this;
    var letters = { "i": "İ", "ş": "Ş", "ğ": "Ğ", "ü": "Ü", "ö": "Ö", "ç": "Ç", "ı": "I" };
    string = string.replace(/(([iışğüçö]))/g, function (letter) { return letters[letter]; })
    return string.toUpperCase();
}
String.prototype.turkishToLower = function () {
    var string = this;
    var letters = { "İ": "i", "I": "ı", "Ş": "ş", "Ğ": "ğ", "Ü": "ü", "Ö": "ö", "Ç": "ç" };
    string = string.replace(/(([İIŞĞÜÇÖ]))/g, function (letter) { return letters[letter]; })
    return string.toLowerCase();
}
$.ErcanAyhan.ToTitleCase = function (txt) {
    if (txt != null) {

        var splitTxt = txt.split(" ");
        var text = "";
        for (var i = 0; i < splitTxt.length; i++) {
            text += splitTxt[i].charAt(0).turkishToUpper() + splitTxt[i].substr(1).turkishToLower() + (i + 1 == splitTxt.length ? "" : " ");
        }
        return text;
    }
    else
        return "";
}
$.ErcanAyhan.ToEmpty = function (text) {
    return text == null ? "" : text;
}
$.ErcanAyhan.Loading;
$.ErcanAyhan.WaitMeIsShow = true;
$.ErcanAyhan.Initialize = function () {
    $.ErcanAyhan.Loading = $('[data-loading="cardloading"]').parents('.card').waitMe({
        effect: "pulse",
        text: '',
        bg: 'rgba(255,255,255,0.90)',
        color: '#555'
    });
}
$.ErcanAyhan.CreateChart = function (chartId, title, field, series, data, maxAxis) {
    $("#" + chartId).kendoChart({
        dataSource: data,
        title: {
            text: title
        },
        series: series,
        categoryAxis: {
            min: 0,
            max: maxAxis,
            field: field,
            majorGridLines: {
                visible: false
            }
        },
        valueAxis: {
            labels: {
                format: "N0"
            },
            line: {
                visible: false
            }
        },
        tooltip: {
            visible: true,
            format: "N0",
            template: "#= series.name #: #:kendo.toString(value,'n0')#"
        },
        pannable: {
            lock: "y"
        },
        zoomable: {
            mousewheel: {
                lock: "y"
            },
            selection: {
                lock: "y"
            }
        }
    });
}
$.ErcanAyhan.Server = {
    PostAsync: function (model, url, func) {
        $.ErcanAyhan.Initialize()
        $.ajax({
            type: "POST",
            async: true,
            url: url,
            data: JSON.stringify(model),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: func,
            error: function (xhr, errorType, exception) {
                $.ErcanAyhan.Loading.waitMe('hide');
                $("[data-btn]").removeAttr("disabled");
                $("[data-btn]").waitMe("hide");
                var rText = xhr.responseText;
                var rTextSplit = rText.split("##");
                if (xhr.statusText == "warning" || xhr.statusText == "info" ||
                    xhr.statusText == "success" || xhr.statusText == "danger" ||
                xhr.statusText == "func" || xhr.statusText == "funcAndMessage") {
                    if (xhr.statusText != "success")
                        isReload = false;
                    $.ErcanAyhan.Notify.Show(rTextSplit[1], rTextSplit[0], xhr.statusText);
                }
            }
        });
    },
    Post: function (model, url, func) {
        $.ajax({
            type: "POST",
            async: false,
            url: url,
            data: JSON.stringify(model),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: func,
            error: function (xhr, errorType, exception) {
                var rText = xhr.responseText;
                var rTextSplit = rText.split("##");
                if (xhr.statusText == "warning" || xhr.statusText == "info" ||
                    xhr.statusText == "success" || xhr.statusText == "danger" ||
                xhr.statusText == "func" || xhr.statusText == "funcAndMessage") {
                    if (xhr.statusText != "success")
                        isReload = false;
                    $.ErcanAyhan.Notify.Show(rTextSplit[1], rTextSplit[0], xhr.statusText);
                }
            }
        });
    },
    GetAsync: function (model, url, func) {
        if ($.ErcanAyhan.WaitMeIsShow)
            $.ErcanAyhan.Initialize()
        $.ajax({
            type: "GET",
            async: true,
            url: url,
            data: JSON.stringify(model),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: func,
            error: function (xhr, errorType, exception) {
                if ($.ErcanAyhan.WaitMeIsShow)
                    $.ErcanAyhan.Loading.waitMe('hide');
                var rText = xhr.responseText;
                var rTextSplit = rText.split("##");
                if (xhr.statusText == "warning" || xhr.statusText == "info" ||
                    xhr.statusText == "success" || xhr.statusText == "danger" ||
                xhr.statusText == "func" || xhr.statusText == "funcAndMessage")
                    $.ErcanAyhan.Notify.Show(rTextSplit[1], rTextSplit[0], xhr.statusText);
            }
        });
    },
    Get: function (model, url, func) {
        $.ajax({
            type: "GET",
            async: false,
            url: url,
            data: JSON.stringify(model),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: func,
            error: function (xhr, errorType, exception) {
                var rText = xhr.responseText;
                var rTextSplit = rText.split("##");
                if (xhr.statusText == "warning" || xhr.statusText == "info" ||
                    xhr.statusText == "success" || xhr.statusText == "danger" ||
                xhr.statusText == "func" || xhr.statusText == "funcAndMessage")
                    $.ErcanAyhan.Notify.Show(rTextSplit[1], rTextSplit[0], xhr.statusText);
            }
        });
    },
    UploadAsync2: function (model, url) {
        if (model != undefined) {
            var formdata = new FormData();
            for (var i = 0; i < model.length; i++) {
                formdata.append("file", model[i]);
            }
            $.ErcanAyhan.Initialize()
            $.ajax({
                async: true,
                url: url,
                type: "POST",
                data: formdata,
                contentType: false,
                cache: false,
                processData: false,
                success: function (e) {
                    $.ErcanAyhan.Loading.waitMe('hide');
                    var rTextSplit = e.split("##");
                    $.ErcanAyhan.Notify.Show(rTextSplit[1], rTextSplit[0], "info");
                },
                error: function (xhr, errorType, exception) {
                    $.ErcanAyhan.Loading.waitMe('hide');
                    var rText = xhr.responseText;
                    var rTextSplit = rText.split("##");
                    if (xhr.statusText == "warning" || xhr.statusText == "info" ||
                        xhr.statusText == "success" || xhr.statusText == "danger" ||
                    xhr.statusText == "func" || xhr.statusText == "funcAndMessage") {
                        if (xhr.statusText != "success")
                            isReload = false;
                        $.ErcanAyhan.Notify.Show(rTextSplit[1], rTextSplit[0], xhr.statusText);
                    }
                }
            });
        }
    },
    UploadAsync: function (model, url, func) {
        if (model != undefined) {
            var formdata = new FormData();
            for (var i = 0; i < model.length; i++) {
                formdata.append("file", model[i]);
            }
            $.ErcanAyhan.Initialize()
            $.ajax({
                async: true,
                url: url,
                type: "POST",
                data: formdata,
                contentType: false,
                cache: false,
                processData: false,
                success: func,
                error: function (xhr, errorType, exception) {
                    $.ErcanAyhan.Loading.waitMe('hide');
                    var rText = xhr.responseText;
                    var rTextSplit = rText.split("##");
                    if (xhr.statusText == "warning" || xhr.statusText == "info" ||
                        xhr.statusText == "success" || xhr.statusText == "danger" ||
                    xhr.statusText == "func" || xhr.statusText == "funcAndMessage") {
                        if (xhr.statusText != "success")
                            isReload = false;
                        $.ErcanAyhan.Notify.Show(rTextSplit[1], rTextSplit[0], xhr.statusText);
                    }
                }
            });
        }
    }
};
//Notify
var waitingSeconds = 4000;
var notification;
$.ErcanAyhan.Notify = {
    Show: function (title, message, type) {
        if (type == "func")
            eval(message)
        else {
            $.growl({
                title: title != "" ? "<strong style='font-size:16px;'>" + title + "</h4></br>" : "",
                message: type == "funcAndMessage" ? message.split("$$")[0] : message
            }, {
                type: type == "funcAndMessage" ? "success" : type,
                allow_dismiss: true,
                label: 'Cancel',
                className: 'btn-xs btn-inverse',
                placement: {
                    from: 'top',
                    align: 'center'
                },
                delay: isReload ? 2000 : waitingSeconds,
                animate: {
                    enter: 'animated fadeInDown',
                    exit: 'animated fadeOutUp'
                },
                offset: {
                    x: 0,
                    y: 0
                },
            });
            setTimeout(function (e) {
                if (type == "success" || type == "funcAndMessage")
                    $(".modal").modal("hide");
                if (type == "funcAndMessage")
                    eval(message.split("$$")[1])
            }, 2000)
            setTimeout(function (e) {
                if (isReload) {
                    window.location.reload();
                }
            }, 2000)
        }
    },
    Send: function (title, body) {
        if (Notification.permission !== "granted")
            Notification.requestPermission();
        else {
            notification = new Notification(title, {
                icon: '/assets/images/push.png',
                body: body,
            });

            notification.onclick = function () {
                window.open("/Dashboard");
            };

        }

    }
}
var funcAcceptDelete;
function funcDeleteModalShow(e) {
    funcAcceptDelete = e;
    $("#DeleteAccept").attr("onclick", "funcAcceptDelete()");
    $("#deleteModal").modal("show");
}
function formSendModel(e) {
    var model = {};
    var modelId = $("[data-id='" + e.id + "'] [data-attribute]");
    $("[data-btn]").attr("disabled");
    var trimByIpnuts = $("[data-id='" + e.id + "'] input");
    for (var c = 0; c < trimByIpnuts.length; c++) {
        if (trimByIpnuts[c].type == "text") {
            trimByIpnuts[c].value = trimByIpnuts[c].value.trim();
        }
    }
    for (var i = 0; i < modelId.length; i++) {
        model[modelId[i].attributes["data-attribute"].value] = GetValue(modelId[i]);
    }
    var dataId = $("[data-id='" + e.id + "']")[0].dataset;
    var ajaxType = dataId.ajaxType;
    var url = dataId.modelUrl;
    var func = dataId.ajaxFunction;

    isReload = isReload ? true : (dataId.reload === "true");

    $.ErcanAyhan.Server.Post(model, url, eval(func));
    $("[data-btn]").removeAttr("disabled");
}
function GetValue(e) {
    if (e.type == "text" || e.type == "email" || e.type == "password" || e.type == "date" || e.type == "datetime" || e.type == "datetime-local") {
        return e.value.trim();
    } else if (e.type == "file") {
        return e.files;
    }
    else if (e.type == "checkbox")
        return e.checked;
    else if (e.className == "g-recaptcha")
        return grecaptcha.getResponse();
    else if (e.className.indexOf("percent") != -1)
        return parseInt(e.noUiSlider.get());
    else if (e.id == "tinymce")
        return tinyMCE.activeEditor.getContent();
    else if (e.tagName.toLowerCase() == "select" && e.attributes["multiple"] != undefined) {
        var data = [];
        for (var i = 0; i < e.selectedOptions.length; i++) {
            data.push(e.selectedOptions[i].value);
        }
        return data;
    }
    else if (e.nodeName == "TR") {
        return e.dataset.value;
    }
    else if (e.nodeName == "TD") {
        return e.dataset.value;
    }
    else if (e.nodeName == "A") {
        return e.innerHTML;
    }
    else
        return e.value.trim();
}
function SetValue(v) {
    var e = $("[data-attribute]");
    for (var i = 0; i < e.length; i++) {
        if (e[i].className.indexOf("select2-offscreen") != -1)
            $("[data-attribute]:eq(" + i + ")").select2("val", v);
        else
            if (e[i].type == "text" || e[i].type == "email" || e[i].type == "password" || e[i].type == "date" || e[i].type == "datetime" || e[i].type == "datetime-local") {
                e[i].value = v;
            }
            else if (e[i].type == "checkbox")
                e[i].checked = false;
            else if (e[i].id == "tinymce")
                tinyMCe[i].activeEditor.setContent(v);
            else if (e[i].tagName[i].toLowerCase() == "select" && e[i].attributes["multiple"] != undefined) {
                e[i].selectedOptions[i].value = v;
            }
            else
                e[i].value = v;
    }
}
var $MaskedInput = $('.mask');
$MaskedInput.find('.mobile-phone-number').inputmask('(999) 999-99-99', { placeholder: '(___) ___-__-__' });
$MaskedInput.find('.identity-number').inputmask('99999999999', { placeholder: '' });
$MaskedInput.find('.numeric').inputmask('9999999999999999999', { placeholder: '' });
$MaskedInput.find('.verify-code').inputmask('999999', { placeholder: '' });
$MaskedInput.find('.tprice').inputmask("9{0,3},9{0,6}", { placeholder: '' });
$MaskedInput.find('.multiplier').inputmask("9{0,3},9{0,3}", { placeholder: '' });
$MaskedInput.find('.decimal').inputmask("9{0,15},9{0,3}", { placeholder: '' });
$MaskedInput.find('.vouchercode').inputmask('a/999999999999999999', { placeholder: '' });
//Browser
var edge = 'Microsoft Edge';
var ie10 = 'Internet Explorer 10';
var ie11 = 'Internet Explorer 11';
var opera = 'Opera';
var firefox = 'Mozilla Firefox';
var chrome = 'Google Chrome';
var safari = 'Safari';
$.ErcanAyhan.Browser = {
    activate: function () {
        var _this = this;
        var className = _this.getClassName();
        if (className !== '') $('html').addClass(_this.getClassName());
    },
    getBrowser: function () {
        var userAgent = navigator.userAgent.toLowerCase();
        if (/edge/i.test(userAgent)) {
            return edge;
        } else if (/rv:11/i.test(userAgent)) {
            return ie11;
        } else if (/msie 10/i.test(userAgent)) {
            return ie10;
        } else if (/opr/i.test(userAgent)) {
            return opera;
        } else if (/chrome/i.test(userAgent)) {
            return chrome;
        } else if (/firefox/i.test(userAgent)) {
            return firefox;
        } else if (!!navigator.userAgent.match(/Version\/[\d\.]+.*Safari/)) {
            return safari;
        }
        return undefined;
    },
    getClassName: function () {
        var browser = this.getBrowser();
        if (browser === edge) {
            return 'edge';
        } else if (browser === ie11) {
            return 'ie11';
        } else if (browser === ie10) {
            return 'ie10';
        } else if (browser === opera) {
            return 'opera';
        } else if (browser === chrome) {
            return 'chrome';
        } else if (browser === firefox) {
            return 'firefox';
        } else if (browser === safari) {
            return 'safari';
        } else {
            return '';
        }
    },
    IsMobileDevice: function () {
        var isMobile = false; 
        if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|ipad|iris|kindle|Android|Silk|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(navigator.userAgent)
            || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(navigator.userAgent.substr(0, 4))) isMobile = true;
        return isMobile;
    }
}
// Activate
$(function () {
    $.ErcanAyhan.Browser.activate();
    $.ErcanAyhan.LeftSideBar.activate();
    $.ErcanAyhan.Navbar.activate();
    $.ErcanAyhan.DropdownMenu.activate();
    $.ErcanAyhan.Input.activate();
    $.ErcanAyhan.Select.activate();
    setTimeout(function () { $('.page-loader-wrapper').fadeOut(); }, 50);
    $('.list').slimScroll({ scrollTo: activeMenu[0].offsetTop, animate: true });
    $('#leftsidebar .slimScrollBar').css("top", activeMenu[0].offsetTop + "px");
});