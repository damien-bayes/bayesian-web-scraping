<%@ Page Language="C#" AutoEventWireup="true" CodeFile="elements.aspx.cs" Inherits="pages_elements" MasterPageFile="~/MasterPage.master" %>
<%@ Register TagPrefix="cl_item" TagName="cl_item" Src="~/Controls/elements_item.ascx"%>
<asp:Content ContentPlaceHolderID="head" runat="server">
    <title>Цели поиска - Краулер</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="MP" runat="server">

    <script type="text/javascript" src="../js/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="../js/jquery.widget.js"></script>
    <script type="text/javascript" src="../js/metro.min.js"></script>

    <style type ="text/css">
        html
        {
            overflow:hidden;
        }

        .reset_Class:hover
        {
            cursor: pointer;
            border-radius: 5px; border: 1px solid #BFBFBF !imported; 
            box-shadow: 0 0.5px 2px rgba(0,0,0,1);
        }

    </style>

    <div class="container">
    <br />
        <div style="position:relative; height: 110px;">
            <h1 class="slider_" style="position: absolute; top: 0px; left: 30px; opacity: 0;">
                <a href-data="/Home.aspx"><i class="icon-arrow-left-3 smaller black"></i></a>
                Цели<small class="on-right">шаг четвертый</small>
            </h1>
            <p class="description slider_" style="position: absolute; top: 90px; left: 20px; opacity: 0;">
                Страница для определения целей парсинга.
            </p>
        </div>
    </div>
    <div style="margin-top: 20px; width:100%; height: 1px;"></div>
    <div class="grid fluid" style = "margin-bottom:0px; padding-right: 0px;" runat="server" id="grid">
    <div class="row" style="margin-top: 0px;">
        <div class="span2" style="min-height: 600px; text-align:left; padding-left: 15px; min-width: 300px;">
            <div class="example1 padding20" style="padding-top:0px; padding-left: 7px; " >
                <div>
                    <strong style="font-family: Roboto Regular;">Область</strong> 
                    <br />
                    <div class="small_description" style="font-family: Roboto Regular;">Web ресурс, объединенный общей темой или смыслом</div>
                    <br/>
                    <div class ="input-control select">
                        <asp:DropDownList runat ="server" ID ="areas_list" >
                        </asp:DropDownList>
                    </div>
                </div>
                <hr />
                <div id="arhive" runat ="server" style="overflow-y:auto; width: 260px;">
                    
                </div>
                <div style="text-align:center; margin-right: 20px; margin-top: 10px; visibility: hidden;" id="load_element">
                    <img src="../images/W_load_1.GIF" />
                    <br />
                    <div class="small_description" style="font-family: Roboto Regular;">Определение и анализ цели. Это может занять несколько секунд</div>
                </div>
            </div>
        </div>
        <div class="span9" style="position:relative; margin-left:0px; margin-top: 0px; margin-right: 0px;" id="pagecontaner_" >
            <div class="row" id="contentfrm" style="overflow-y: hidden; position:relative; margin-top: 0px; border: 1px solid #C1C1C1; background-color: white; border-radius: 7px; box-shadow: 0 0 10px rgba(0,0,0,0.3);" runat="server">
                <div style="border-style: solid; border-width: 0px 0px 1px 0px; border-color: #979797; height: 35px; background-image: url('http://10.10.9.132:777/images/browser_head.png'); background-repeat: repeat-x;">
                    &nbsp;
                </div>
                <div style="width:216px; height: 26px; position:absolute; top: 10px; left: 50px; background-image: url('http://10.10.9.132:777/images/browser_tile.png'); background-repeat: no-repeat;">
                    <div style="margin-left: 40px; margin-top: 4px;" runat="server" id="browser_title">Без имени</div>
                </div>
                <div style="border-style: solid; padding: 5px; border-width: 0px 0px 1px 0px; border-color: #7A7A7A; height: 35px; background-image: url('http://10.10.9.132:777/images/browser_string.png'); background-repeat: repeat-x;">
                    <div id="reset" title="Сбросить" style="border: 1px solid transparent; float:left; font-size: 10pt; margin-left: 5px; padding: 5px;" class="icon-pointer reset_Class"></div>
                    <div style="border-radius: 5px; border: 1px solid #BFBFBF; background-color: white; height: 25px; margin-left: 5px; margin-right: 10px; padding-left:3px; padding-right: 3px; float:left;">
                        <table style="width:100%; border-width:0px;">
                            <tr>
                                <td>
                                    <img src="../images/load.gif" style="" id="loaded_page" />
                                </td>
                                <td style="width:98%">
                                    <input type="text" style="border-width: 0px; width: 90%" placeholder="http://" readonly="readonly" runat="server" id="address_browser"/>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="contentfrm_inner" style="overflow-y: auto;" runat="server">
                    <%=this.default_text %>
                    <iframe id="P_site" onload="loadet(this);" style="border: solid 0px grey; width:100%; height:600px; visibility:hidden; overflow: hidden;" src="../ajax/Awesomium.aspx?id=<%=this.area %>&mapid=<%=this.areaid %>&replace=1" ></iframe>
                </div>
              </div>
        </div>
    </div>
    </div>
<p runat ="server" id="debuging" visible="false" class="caution" style="position:absolute; top: 150px; right: 10px; width: auto;"><strong>Предупреждение:</strong> Данная страница находится в стадии разработки.</p>

<script type="text/javascript" src="../js/core.js"></script>
<script type="text/javascript" src="../js/jquery.uploadify.js" ></script>
<script type="text/javascript">

    var h = $(document).height();
    var w = $(document).width();
    $("#MP_contentfrm").width(w - 310);

    var add = $("#MP_address_browser");
    if (add.val() == '') {
        $("#MP_contentfrm").height(h - 370);
        $("#MP_contentfrm_inner").height(h - 400);
        $("#pathm").height(h - 355);
    }
    else {
        $("#MP_contentfrm").height(h - 210);
        $("#MP_contentfrm_inner").height(h - 210);
        $("#pathm").height(h - 275);
    }

    Win8_effectHeader();

    function loadet(obj) {
        document.getElementById('loaded_page').src = '../images/round.png';
    }

    var index = Number(1);
    window.selectelemt = function (elem, xpath) {
        if (xpath == null)
            return;
        var mapid = '<%=this.areaid%>';
        if (elem != null) {
            var attr_ = String(elem).split(';');
            for (var i = 0; i < attr_.length; i++) {

                var s = attr_[i].replace(',', '').trim();
                if (s.indexOf('class:') != -1) {
                    var className = s.substr(6, s.length - s.indexOf('class:'));

                    $.ajax({
                        url: "/Handler.ashx",
                        type: "get",
                        dataType: '',
                        async: false,
                        data: {
                            type: "savehref",
                            mapid: mapid,
                            hrf: className
                        },
                        success: function (response) {
                            if (response != 'errorxpath') {
                                var sp = response.split(":");
                                $("#MP_arhive").html($("#MP_arhive").html() + getContayner(1, sp[0], sp[1]));
                            }
                            else {
                                AlertM('Парсер HTML', 'По указанному пути ничего не найдено. Возможно система не смогла правильно определить путь до элемента.');
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            AlertM('Парсер HTML', 'По указанному пути ничего не найдено. Возможно система не смогла правильно определить путь до элемента.');
                        }
                    });
                }
            }
        }
        else {
            xpath = xpath.toLowerCase();
            xpath = xpath.replace('//td[', '//*[');


            //$("#MP_address_browser").val("/Handler.ashx?" + "type=screenshot&" + "mapid:" + mapid + "&xpath:" + xpath);


            $.ajax({
                url: "/Handler.ashx",
                type: "get",
                dataType: '',
                async: false,
                data: {
                    type: "screenshot",
                    mapid: mapid,
                    xpath: xpath
                },
                success: function (response) {
                    if (response != 'errorxpath') {
                        var sp = response.split(":");
                        $("#MP_arhive").html($("#MP_arhive").html() + getContayner(2, sp[0], sp[1]));
                        index++;
                    }
                    else {
                        AlertM('Парсер HTML', 'По указанному пути ничего не найдено. Возможно система не смогла правильно определить путь до элемента. (' + response.responseText + ')');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) { AlertM('Парсер HTML', 'По указанному пути ничего не найдено. Возможно система не смогла правильно определить путь до элемента. (' + xhr.responseText + ')'); }
            });
        }

        $('#load_element').css('visibility', 'hidden');
    }

    var obj_ = null;

    function setpage(obj) {
        if (obj != null) {
            var val = $(obj).val();
            if (val != '') {
                location.href = "?" + val;
            }
        }
    }

    function removeinten(obj, id) {
        ConfM('Удаление цели', 'Вы действительно хотите удалить выбранную цель?', $(obj).attr('src'));
        obj_ = obj;
    };

    function NoClick() {
        $("#MsgBoxBack3").css('display', 'none');
        $("#Cnf3").css('display', 'none');
    };

    function YesClick() {
        var id = $(obj_).attr('remove');
        $.ajax({
            url: "/Handler.ashx",
            type: "get",
            async: false,
            data: {
                type: "removeintent",
                id: id,
            },
            success: function (response) {
                $("#" + id + "_element").remove();
                $("#MsgBoxBack3").css('display', 'none');
                $("#Cnf3").css('display', 'none');
            },
            error: function (xhr, ajaxOptions, thrownError) { AlertM('Ошибка Web приложения', xhr.responseText); }
        });
    };

    window.setheigth = function (h) {
        $("#P_site").height(h + 30);
        $("#P_site").css('visibility', 'visible')
    }

    function getContayner(type, id, url)
    {
        return result = "<div style=\"border: 1px solid #D8D8D8;border-radius: 3px; background-color: white; margin-right: 15px;margin-bottom: 15px\" id=\"" + id + "_element\">" +
                    "<div style=\"padding: 5px;\">" +
                    "<div style =\"border-bottom: 1px solid #e5e5e5; position: relative; padding: 0 0 5px 0; overflow: hidden; padding-right: 0; text-overflow: ellipsis; white-space: nowrap;\">" +
                        "<strong style=\"font-family: Roboto Regular;\">" + (type == 2 ? "Блок" : "Ссылка") + "</strong>" +
                        "<span style=\"float:right; height: 16px; width: 16px; margin-top: 2px; color: red; cursor: pointer\" class=\"icon-remove\" remove=\"" + id + "\" onclick=\"removeinten(this)\"></span>" +
                    "</div>" +
                    "<div style=\"position:relative; max-height: 130px; margin-bottom: 2px;overflow: hidden; \">" +
                        "<img src =\"/screenshots/" + url + "\" />" +
                        "<div style=\"background-image: url('../images/embeds_article_v2-aa76e8105f4ad58f58b33da824ba2dc0.png'); background-repeat: repeat-x; width: 100%; height: 16px; position:absolute; top: 114px; z-index: 1000\">&nbsp</div>" +
                    "</div>" +
                    "</div>" +
                    "<div style=\"border-style: solid; border-width: 1px 0px 0px 0px; border-color: #E5E5E5; background-color: #F5F5F5; padding: 10px;\">" +
                    "<span style=\"font-family: Roboto Regular;\">" + (type == 2 ? "Поиск ссылок в данном блоке" : "Выбираються все ссылки аналогичные этой") + "</span>" +
                    "</div>" +
                    "</div>"
    }

    $("#reset").click(function () {
        var Ereset = $("#P_site").contents().find("#restart");
        Ereset.click();
    });

</script>

</asp:Content>