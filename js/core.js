
var windowWidth = 1000;
try {
    windowWidth = $(window).width();
}
catch (err) { }

var windowHeight = 800;
try {
    windowHeight = $(window).height();
}
catch (err) { }

$(document).ready(function () {

    $("#help").click(function () {
        $.Dialog({
            overlay: true,
            shadow: true,
            flat: true,
            icon: '<span class = "icon-info" ></span>',
            title: '',
            content: '<div style = "width: 800px; height: 600px">&nbsp</div>',
            onShow: function (_dialog) {

            }
        });
    });

    $("#events_").click(function () {
        $.Dialog({
            overlay: true,
            shadow: true,
            flat: true,
            icon: '<span class = "icon-info" ></span>',
            title: '',
            content: '<iframe style = "min-width: 800px; min-height: 600px; border: solid 0px grey; width:' + (windowWidth - 400) + 'px; height:' + (windowHeight - 200) + 'px;" src = "/shost/events.aspx"></iframes>',
            onShow: function (_dialog) {

            }
        });
    });

    $("#userprofile").click(function () {
        $.Dialog({
            overlay: true,
            shadow: true,
            flat: true,
            icon: '<span class = "icon-info" ></span>',
            title: '',
            content: '<iframe style = "min-width: 800px; min-height: 600px; border: solid 0px grey; width:' + (windowWidth - 600) + 'px; height:' + (windowHeight - 200) + 'px;" src = "/pages/user.aspx"></iframes>',
            onShow: function (_dialog) {

            }
        });
    });

    $(".icon-arrow-left-3").hover(
        function() {
            $(this).css(
            {
                'color': '#16499a !important',
                'cursor': 'pointer'
            });
            this.style.color = '#16499a';
        }, function() {
            this.style.color = '#000';
        }
    );

    $(".icon-home").hover(
       function () {
           $(this).css(
           {
               'color': '#16499a !important',
               'cursor': 'pointer'
           });
           this.style.color = '#16499a';
       }, function () {
           this.style.color = '#000';
       }
   );
});

function getParameterByName(name) {
    return decodeURIComponent((new RegExp('[?|&]' + name + '=' + '([^&;]+?)(&|#|;|$)').exec(location.search) || [, ""])[1].replace(/\+/g, '%20')) || null;
}

function changeCategory(num) {
    $("#cat_1").addClass('Circle');
    $("#cat_2").addClass('Circle');
    $("#cat_3").addClass('Circle');

    $("#cat_1").removeClass('Circlehover');
    $("#cat_2").removeClass('Circlehover');
    $("#cat_3").removeClass('Circlehover');

    $("#cat_" + num).toggleClass('Circlehover');
}

//MAP
function getdata() {
    var p = $('#xpaths')[0].rows[1].cells[1].innerText;
    $.get(
        "/ajax/spider.aspx",
        {
            xpath: p,
        },
        function (data) {

            $("#data_").html(data);
        }
   );
}

function search(obj, isclick) {
    if (isclick != null) {
        if ($(obj).val().indexOf('http') == -1) {
            $(obj).val('http://' + $(obj).val());
        }
        getsiteinfo(obj);
    }

    if (event.keyCode == 13) {
        if ($(obj).val().indexOf('http') == -1) {
            $(obj).val('http://' + $(obj).val());
        }
        getsiteinfo(obj);
    }
}

function AlertM(title_, text) {
    $(".shd").text(title_);
    $(".Ptext").text(text);
    $("#MsgBoxBack3").css('display', 'block');

    var h = $(document).height();
    h = (40 * h) / 100;

    $("#Msg3").css('display', 'block');
    $("#Msg3").css('top', h + 'px');
    $("#bot1-Msg3").focus();
}

function ConfM(title_, text, id) {
    $(".shd").text(title_);
    $(".Ptext").text(text);
    $("#MsgBoxBack3").css('display', 'block');

    var h = $(document).height();
    h = (40 * h) / 100;

    $("#Cnf3").css('display', 'block');
    $("#Cnf3").css('top', h + 'px');
    $("#bot1-Cnf2").focus();
    $("#bot1-Cnf3").attr('data-element', id);
}

function CloseAlertM()
{
    $("#MsgBoxBack3").css('display', 'none');
    $("#Msg3").css('display', 'none');
}

//Получить информацию о сайте
function getsiteinfo(obj) {
    if (document.getElementById('loadGIF') != null) {
        $("#loadGIF").css('visibility', 'visible');
    }
    $(".showpanel").slideUp();
    var url = $(obj).val();
    if (checkURL(url)) {

        var url_buf = url.replace('//', '@');
        var split_array = url_buf.split('/');
        url_buf = split_array[0].replace('@', '//');
        $.get(
            "/Handler.ashx",
            {
                type: "info",
                info: url,
            },
            function (data) {
                try {
                    var logo = data.logo;
                    if (logo != '') {
                        logo = (data.logo.indexOf('http') == -1 ? url_buf + data.logo : data.logo);
                    }
                    else {
                        logo = "/images/images.jpg";
                    }

                    $("#MP_P_logo").attr('src', logo);
                    $("#MP_title_finder").html('<strong>' + data.title.length > 30 ? data.title.substr(0, 30) + '...' : data.title + '</strong>');

                    var dscr = data.desc;
                    if (dscr.length != 0)
                    {
                        if (dscr.length > 50)
                        {
                            dscr = data.desc.substr(0, 48) + '...';
                        }
                    }

                    $("#MP_description_finder").text(dscr);

                    $("#MP_S_image").val(logo);
                    $("#MP_S_title").val(data.title);
                    $("#MP_S_description").val(data.desc);

                    $("#MP_create").removeClass('aspNetDisabled');
                    $("#MP_create").removeAttr('disabled');

                    $(".showpanel").slideDown();

                    if (document.getElementById('loadGIF') != null) {
                        $("#loadGIF").css('visibility', 'hidden');
                    }

                    var top_img_logo = $("#MP_P_logo").height();
                    $("#foter_img_logo").css('top', (top_img_logo - 7 > 114 ? 114 : top_img_logo - 7) + 'px');
                }
                catch (err) {

                    $("#MP_S_image").val("");
                    $("#MP_S_title").val("");
                    $("#MP_S_description").val("");

                    $("#MP_create").attr('disabled', 'disabled');
                    $(".showpanel").slideUp();

                    if (document.getElementById('loadGIF') != null) {
                        $("#loadGIF").css('visibility', 'hidden');
                    }
                }
            }
       );
    }
    else {
        AlertM('Информация системы', 'Введенный адрес не являеться ссылкой. Адрес должен соответсвовать формату exsaple.com');
        if (document.getElementById('loadGIF') != null) {
            $("#loadGIF").css('visibility', 'hidden');
        }
    }
}

function opencloaseMapFinder(id_ob) {
    $("#loading").css("visibility", "hidden");
    if (id_ob != null) {
        var open_ = $("#" + id_ob).attr("isopen");
        if (open_ == 0) {
            $("#" + id_ob).slideDown("normal");
            $("#" + id_ob).attr("isopen", "1");
        }
        else {
            $("#" + id_ob).slideUp("normal");
            $("#" + id_ob).attr("isopen", "0");
        }
    }
}

function checkURL(url) {
    var regURLrf = /^(?:(?:https?|ftp|telnet):\/\/(?:[а-я0-9_-]{1,32}(?::[а-я0-9_-]{1,32})?@)?)?(?:(?:[а-я0-9-]{1,128}\.)+(?:рф)|(?! 0)(?:(?! 0[^.]|255)[ 0-9]{1,3}\.){3}(?! 0|255)[ 0-9]{1,3})(?:\/[a-zа-я0-9.,_@%&?+=\~\/-]*)?(?:#[^ \'\"&<>]*)?$/i;
    var regURL = /^(?:(?:https?|ftp|telnet):\/\/(?:[a-z0-9_-]{1,32}(?::[a-z0-9_-]{1,32})?@)?)?(?:(?:[a-z0-9-]{1,128}\.)+(?:com|net|org|mil|edu|arpa|ru|gov|biz|info|aero|inc|name|[a-z]{2})|(?! 0)(?:(?! 0[^.]|255)[ 0-9]{1,3}\.){3}(?! 0|255)[ 0-9]{1,3})(?:\/[a-zа-я0-9.,_@%&?+=\~\/-]*)?(?:#[^ \'\"&<>]*)?$/i;
    return regURLrf.test(url) || regURL.test(url);
}

function loadImageForMap() {
    $(_file).click();
}

try {
    var _file = $(".fileUpload")[0],
   _progress = document.getElementById('_progress');

    $(function () {
        $(_file).fileupload(
            {
                replaceFileInput: false,
                dataType: "json",
                url: '../Handler.ashx?type=file',
                progressall: function (e, data) {
                    var progress = parseInt(data.loaded / data.total * 100, 10);
                },
                add: function (e, data) {
                    var valid = true;
                    var re = /^.+\.((jpg))$/i;
                    $.each(data.files, function (index, file) {
                        if (!re.test(file.name)) {
                            AlertM('Загрузка изображений', 'Изображения необходимо загружать в формате *.JPG');
                            valid = false;
                        }
                    });

                    if (valid)
                        data.submit();
                },
                done: function (e, data) {
                    $.each(data.result, function (index, file) {
                        if (file != '') {
                            $("#MP_P_logo").attr('src', file);
                            $("#MP_S_image").val(file);
                        }
                    });
                },
                error: function (e, data) {
                    AlertM('Загрузка изображений', 'Ошибка загрузки изображения на сервер. Обратитесь к администратору системы');
                }
            });
    });
}
catch (err) { }

//SUBCATEGORY

function openRegionFind() {
    var paramert = "";
    if (getParameterByName("id") != null) {
        paramert = "?id=" + getParameterByName("id");
        $("#dialog").dialog({
            buttons: [{ text: "Закрыть", click: function () { $(this).dialog("close"); } }],
            modal: true,
            top: "70px",
            height: 700, width: 700,
            open: function (event, ui) {
                $("span.ui-dialog-title").text('Области поиска');
                $("#dialog").html('<iframe frameborder = "0" id="frame" scrolling="no" src="/pages/regionFind.aspx' + paramert + '" width="100%" height="' + (560) + 'px"></iframe>');
                $("#dialog").css("overflow", "hidden");
            },
            close: function (event, ui) {
                $("#dialog").html("<p>Загрузка контента...</p>");
            },
        });
    }
    else {
        alert('Создайте или выберите карту поиска');
    }
};

function openPathFind() {
    var paramert = "";
    if (getParameterByName("id") != null) {
        paramert = "?id=" + getParameterByName("id");
        $("#dialog").dialog({
            buttons: [{ text: "Закрыть", click: function () { $(this).dialog("close"); } }],
            modal: true,
            top: "70px",
            height: 860, width: windowWidth - 60,
            open: function (event, ui) {
                $("div.ui-dialog").css('z-index', '999999');
                $("span.ui-dialog-title").text('Маршрут поиска');
                $("#dialog").html('<iframe frameborder = "0" id="frame" src="/pages/pathFind.aspx' + paramert + '" width="100%" height="770px"></iframe>');
                $("#dialog").css("overflow", "hidden");
            },
            close: function (event, ui) {
                $("#dialog").html("<p>Загрузка контента...</p>");
            },
        });
    }
    else {
        alert('Создайте или выберите карту поиска');
    }
}

function openMissionFind() {
    var paramert = "";
    if (getParameterByName("id") != null) {
        paramert = "?id=" + getParameterByName("id");
        $("#dialog").dialog({
            buttons: [{ text: "Закрыть", click: function () { $(this).dialog("close"); } }],
            modal: true,
            top: "70px",
            height: 860, width: windowWidth - 60,
            open: function (event, ui) {
                $("div.ui-dialog").css('z-index', '999999');
                $("span.ui-dialog-title").text('Цель поиска');
                $("#dialog").html('<iframe frameborder = "0" id="frame" src="/pages/elementFind.aspx' + paramert + '" width="100%" height="760px"></iframe>');
                $("#dialog").css("overflow", "hidden");
            },
            close: function (event, ui) {
                $("#dialog").html("<p>Загрузка контента...</p>");
            },
        });
    }
    else {
        alert('Создайте или выберите карту поиска');
    }
}

function createTasks() {
    var paramert = "";
    if (getParameterByName("id") != null) {
        paramert = "?id=" + getParameterByName("id");
        $("#dialog").dialog({
            buttons: [{ text: "Закрыть", click: function () { $(this).dialog("close"); } }],
            modal: true,
            top: "70px",
            height: 860, width: windowWidth - 60,
            open: function (event, ui) {
                $("div.ui-dialog").css('z-index', '999999');
                $("span.ui-dialog-title").text('Расписание поиска');
                $("#dialog").html('<iframe frameborder = "0" id="frame" src="/pages/createtask.aspx' + paramert + '" width="100%" height="760px"></iframe>');
                $("#dialog").css("overflow", "hidden");
            },
            close: function (event, ui) {
                $("#dialog").html("<p>Загрузка контента...</p>");
            },
        });
    }
    else {
        alert('Создайте или выберите карту поиска');
    }
}

function carousel_itemLoadCallback(carousel, state) {
    $index = carousel.last + 1;
    if (state == 'next') {
        
    }

    if (state == 'prev') {
        if (carousel.first == 1) {
            
        }
    }

    if (state == 'init') {
        carousel.buttonNext[0].style.zIndex = '100';
    }
}

function openothemaps() {
    $("#dialog").dialog({
        buttons: [{ text: "Закрыть", click: function () { $(this).dialog("close"); } }],
        modal: true,
        top: "70px",
        height: 700, width: 900,
        open: function (event, ui) {
            $("span.ui-dialog-title").text('Все карты пользователя');
            $("#dialog").html('<iframe frameborder = "0" id="frame" src="/pages/allMaps.aspx" width="99%" height="' + (560) + 'px"></iframe>');
            $("#dialog").css("overflow", "hidden");
        },
        close: function (event, ui) {
            $("#dialog").html("<p>Загрузка контента...</p>");
        },
    });
}

function clearMapFinder() {
    $("#siteaddress").val('');
    $("#logo_finder").html('');
    $("#title_finder").html('');
    $("#description_finder").text('');
    $("#loading").css("visibility", "hidden");
    $('#_progress').css(
                    'width',
                    '0%'
                );
}

function createMap() {
    var url = $("#siteaddress").val();
    var img_ = $("#logo_finder").find("img").attr("src");
    var name = $("#title_finder").text();
    var description = $("#description_finder").text();
    $("#alertError").css('display', 'none');
    if (url != '') {
        $.get(
                "/Handler.ashx",
                {
                    create: "map",
                    instanse: "map",
                    name: name,
                    img_: img_,
                    description: description,
                    url_: url,
                },
                function (data) {
                    try {
                        var identity = data.identity;
                        if (identity != 'write') {
                            location.href = '/default.aspx?id=' + identity.toString();
                            opencloaseMapFinder('mapfinder');
                            $("#createMapbutton").attr('disabled', '');
                        }
                        else
                            $("#alertError").css('display', 'block');
                    }
                    catch (err) { alert(err.responceText); }

                }
           );
    }
    else {
        alert('Укажите сайт для карты');
    }
}

function deleteMap(id) {
    if (id != null) {
        $.get(
            "/Handler.ashx",
            {
                delete: "map",
                instanse: "submap",
                id: id,
            },
            function (data) {
                try {
                    var identity = data.identity;
                    if (identity != 'write') {
                        location.href = '/default.aspx?id=' + identity.toString();
                        opencloaseMapFinder('mapfinder');
                        $("#createMapbutton").attr('disabled', '');
                    }
                    else
                        $("#alertError").css('display', 'block');
                }
                catch (err) { }

            }
           );
    }
}

function createsubMap() {
    var url = $("#siteaddress").val();
    var img_ = $("#logo_finder").find("img").attr("src");
    var name = $("#title_finder").text();
    var description = $("#description_finder").text();
    var map_id = getParameterByName("id");
    $.get(
            "/Handler.ashx",
            {
                create: "map",
                instanse: "category",
                name: name,
                img_: img_,
                description: description,
                url_: url,
                map_id: map_id,
            },
            function (data) {
                try {
                    var identity = data.identity;
                    if (identity != 'write') {
                        //clearMapFinder();
                        location.reload();
                    }
                    else {
                        $("#alertError").css('display', 'block');
                    }
                }
                catch (err) { }

            }
       );
}

function openlemmas() {
    var paramert = "";
    if (getParameterByName("id") != null) {
        paramert = "?id=" + getParameterByName("id");
        $("#dialog").dialog({
            buttons: [{ text: "Закрыть", click: function () { $(this).dialog("close"); } }],
            modal: true,
            top: "70px",
            height: 860, width: windowWidth - 60,
            open: function (event, ui) {
                $("div.ui-dialog").css('z-index', '999999');
                $("span.ui-dialog-title").text('Нормализация');
                $("#dialog").html('<iframe frameborder = "0" id="frame" src="/pages/lemmas.aspx' + paramert + '" width="100%" height="760px"></iframe>');
                $("#dialog").css("overflow", "hidden");
            },
            close: function (event, ui) {
                $("#dialog").html("<p>Загрузка контента...</p>");
            },
        });
    }
    else {
        alert('Создайте или выберите карту поиска');
    }
}


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function Win8_effectHeader() {
    var elements_ = $(".slider_").length;
    var index = 0;
    _effectHeader(index, elements_);

    $(".slider_").find("a").click(function () {
        Win8_effectHeaderBack($(this).attr("href-data"));
    });
}

function _effectHeader(index, count) {
    $($(".slider_")[index]).animate({
        left: 0,
        opacity: 1,
    }, 200, null, function () {
        if (index < count) {
            index++;
            _effectHeader(index, count);
        }

    });
}

function Win8_effectHeaderBack(url) {
    var elements_ = $(".slider_").length;
    var index = 0;
    _effectHeaderBack(index, elements_, url);
}

function _effectHeaderBack(index, count, url) {
    $($(".slider_")[index]).animate({
        left: 20,
        opacity: 0,
    }, 100, null, function () {
        if (index + 1 < count) {
            index++;
            _effectHeaderBack(index, count, url);
        }
        else {
            location.href = url;
        }

    });
}

function TileEffects() {
    var el = $(".tiles_").length;
    var index = 0;
    for (var i = 0; i < el; i++) {
        tileeffect(i, el);
    }
}

function tileeffect(index, count) {
    $($(".tiles_")[index]).animate({
        opacity: 1,
        left: -10,
    }, 300, null, function () {

    });
}