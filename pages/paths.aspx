<%@ Page Language="C#" AutoEventWireup="true" CodeFile="paths.aspx.cs" Inherits="pages_paths" MasterPageFile="~/MasterPage.master" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
    <title>Маршруты - Краулер</title>
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
            Маршруты<small class="on-right">шаг третий</small>
        </h1>
        <p class="description slider_" style="position: absolute; top: 90px; left: 20px; opacity: 0;">
            Страница для определения маршрутов краулера.
        </p>
    </div>
    </div>
    <div style="margin-top: 20px; width:100%; height: 1px;"></div>
    <div class="grid fluid" style ="margin-bottom:0px;" runat="server" id="grid">
    <div class="row" style="margin-top:0px;">
        <div class="span2" style="min-height: 600px;  padding-left: 15px; min-width: 300px;">
            <div class="example1 padding20" style="padding-top:0px; padding-left: 7px;" id="pathm">
                <strong style="font-family: Roboto Regular;">Область</strong> 
                <br />
                <div class="small_description" style="font-family: Roboto Regular;">Web ресурс, объединенный общей темой или смыслом</div>
                <br/>
                <div class ="input-control select">
                    <asp:DropDownList runat ="server" ID ="areas_list" required="required">
                    </asp:DropDownList>
                </div>
                <strong style="font-family: Roboto Regular;">Тип маршрутизации</strong> 
                <br />
                <div class="small_description" style="font-family: Roboto Regular;">Способ навигации по страницам</div>
                <br/>
                <div class ="input-control select">
                    <asp:DropDownList runat ="server" ID ="P_type" >
                        <asp:ListItem Value="0" Text ="Паджинатор"></asp:ListItem>
                        <asp:ListItem Value="1" Text ="Ajax"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <hr />
                <div id="pgn" runat="server" style ="margin-bottom: 10px; display: block;">
                    <div class ="input-control number">
                        <strong style="font-family: Roboto Regular;">Строка пути</strong>
                        <br />
                        <div class="small_description" style="font-family: Roboto Regular;">Адрес соотвествующий маске</div>
                        <br/> 
                        <input type ="text" value ="" placeholder="В формате http://link.com/{step}" runat ="server" id="P_url" required="required"/>
                        <br />
                        <br />
                        <input type ="text" value ="" placeholder="Пример: ([A-Z])\w+" runat ="server" id="Pattern_url"/>
                        <br /><br />
                        <strong style="font-family: Roboto Regular;">Шаг</strong>
                        <br />
                        <div class="small_description" style="font-family: Roboto Regular;">Изменение url</div>
                        <br/>
                        <input type ="text" value ="" min ="1" runat="server" id="P_step" placeholder="1" required="required"/>
                        <br /><br />
                        <strong style="font-family: Roboto Regular;">Начать с</strong> 
                        <br />
                        <div class="small_description" style="font-family: Roboto Regular;">Первая страница сайта</div>
                        <br/>
                        <input type ="text" value ="" min ="1" runat="server" id="P_start" placeholder="1" required="required"/>
                        <br /><br />
                        <strong style="font-family: Roboto Regular;">Закончить на</strong> 
                        <br />
                        <div class="small_description" style="font-family: Roboto Regular;">Последняя страница сайта</div>
                        <br/>
                        <input type ="text" value ="" min ="1" runat="server" id="P_end" placeholder="1" required="required"/>
                    </div>
                    <br />
                    <br />
                    <br />
                    <asp:Button CssClass ="button primary large" runat="server" ID="savepg" Text="Сохранить" OnClick="savepg_Click"></asp:Button>
                </div>
                <div id="ajax" runat="server" style ="display: none;">
                    <div class ="input-control number">
                        <strong style="font-family: Roboto Regular;">Тескт элемента</strong>
                        <br />
                        <div class="small_description" style="font-family: Roboto Regular;">Объект страницы, подгружающий следующий контент</div>
                        <br/> 
                        <input type ="text" value ="" placeholder="Здесь будет тескст выбранного элемента" disabled="disabled" runat="server" id="P_text" />
                        <br /><br />
                        <strong style="font-family: Roboto Regular;">Тип элемента</strong>
                        <br />
                        <div class="small_description" style="font-family: Roboto Regular;">Тип DOM элемента (Например: div, span, a и т.д.)</div>
                        <br/> 
                        <input type ="text" value ="" placeholder="Как правило это DIV" disabled="disabled" runat="server" id="P_type_el" />
                        <br /><br />
                        <strong style="font-family: Roboto Regular;">Количество иттераций</strong>
                        <br />
                        <div class="small_description" style="font-family: Roboto Regular;">Переменная, указывающая какое количество раз будет выполненна подгрузка контента</div>
                        <br/> 
                        <input type ="text" value ="" min ="1" runat="server" id="P_count" placeholder="1" />
                    </div>
                    <br />
                    <br />
                    <hr />
                    <asp:Button CssClass ="button primary large" runat="server" ID="saveajax" Text="Сохранить" OnClick="saveajax_Click"></asp:Button>
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
<asp:FileUpload ID="fileBrow" runat="server" CssClass="fileUpload" />
<p runat ="server" id="debuging" visible="false" class="caution" style="position:absolute; top: 150px; right: 10px; width: auto; font-size: 10pt;"><strong style="font-size: 10pt;">Предупреждение:</strong> Данная страница находится в стадии разработки.</p>
<script type="text/javascript" src="../js/core.js" ></script>
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

    $("#MP_P_type").change(function () {
        if ($(this).val() == 0)
        {
            $("#MP_pgn").css('display', 'block');
            $("#MP_pgn").find("input").attr('required', 'required');
            $("#MP_ajax").css('display', 'none');
            $("#MP_pgn").find("input").removeAttr('required');
        }
        else {
            $("#MP_pgn").css('display', 'none');
            $("#MP_pgn").find("input").removeAttr('required');
            $("#MP_ajax").css('display', 'block');
            $("#MP_ajax").find("input").attr('required', 'required');
        }
    });

    function loadet(obj) {
        document.getElementById('loaded_page').src = 'http://10.10.9.132:777/images/round.png';
    }

    function setpage(obj)
    {
        if (obj != null)
        {
            var val = $(obj).val();
            if (val != '')
            {
                location.href = "?" + val;
            }
        }
    }

    window.selectelemt = function (elem, xpath) {
        var tp = $("#MP_P_type").val();
        var areaid = '<%=this.areaid%>';


        if (tp == 0) {
            $.ajax({
                url: "/Handler.ashx",
                type: "get",
                dataType: '',
                async: false,
                data: {
                    type: "pgntr",
                    mapid: areaid,
                    xpath: xpath
                },
                success: function (response) {
                    $("#MP_P_url").val(response.url);
                    $("#MP_P_step").val(response.step);
                },
                error: function (xhr, ajaxOptions, thrownError) { AlertM('Парсер HTML', 'По указанному пути ничего не найдено. Возможно система не смогла правильно определить путь до элемента.'); }
            });
        }
        else {
            $.ajax({
                url: "/Handler.ashx",
                type: "get",
                dataType: '',
                async: false,
                data: {
                    type: "ajax",
                    mapid: areaid,
                    xpath: xpath
                },
                success: function (response) {
                    $("#MP_P_text").val(response.text);
                    $("#MP_P_type_el").val(response.element);
                },
                error: function (xhr, ajaxOptions, thrownError) { AlertM('Парсер HTML', 'По указанному пути ничего не найдено. Возможно система не смогла правильно определить путь до элемента.'); }
            });
        }
    }

    window.setheigth = function (h) {
        $("#P_site").height(h + 1000);
        $("#P_site").css('visibility', 'visible')
    }

    $("#reset").click(function () {
        var Ereset = $("#P_site").contents().find("#restart");
        Ereset.click();
    })

</script>

</asp:Content>
