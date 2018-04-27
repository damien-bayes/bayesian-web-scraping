<%@ Page Language="C#" AutoEventWireup="true" CodeFile="areas.aspx.cs" Inherits="pages_areas" MasterPageFile="~/MasterPage.master" %>
<%@ Register TagPrefix="lsti" TagName="lsti" Src="../Controls/listitem.ascx" %>
<%@ Register TagPrefix="dr" TagName="dr" Src="../Controls/dropdown.ascx" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <title>Области поиска - Краулер</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="MP" runat="server">

    <script type="text/javascript" src="../js/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="../js/jquery.widget.js"></script>
    <script type="text/javascript" src="../js/metro.min.js"></script>

    <div class="container">
    <br />
    <div style="position:relative; height: 110px;">
        <h1 class="slider_" style="position: absolute; top: 0px; left: 30px; opacity: 0;">
            <a href-data="/Home.aspx"><i class="icon-arrow-left-3 smaller black"></i></a>
            Область<small class="on-right">шаг второй</small>
        </h1>
        <p class="description slider_" style="position: absolute; top: 90px; left: 20px; opacity: 0;">
            Страница для создания и редактирования областей поиска.
        </p>
    </div>
    <hr />
    <div class="grid" runat="server" id="grid">
        <div class="row">
        <div class="span6" style="border-right: 1px #d9d9d9 solid; min-height: 600px; margin-right:20px;">
            <h1><small class="on-right" runat ="server" id="TitleType">Новая область</small><img id="loadGIF" src="../images/W_load_1.GIF" style="margin-top:13px; margin-left: 10px; visibility:hidden;" /></h1>
            <div class="example1 padding20" style="padding-top:0px; padding-left: 7px;">
                    <p><strong style="font-family: Roboto Regular;">Выберите карту</strong></p>
                    <div class="input-control select">
                        <asp:DropDownList runat="server" ID="mapslist" AutoPostBack="true" required="" OnSelectedIndexChanged="maps_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                    <p><strong style="font-family: Roboto Regular;">Категория</strong></p>
                    <div class="input-control select">
                        <asp:DropDownList runat="server" ID="topic" AutoPostBack="false" required="" ></asp:DropDownList>
                    </div>  
                    <hr />
                    <strong style="font-family: Roboto Regular;">Введите адрес сайта</strong>
                    <br />
                    <div class="small_description" style="font-family: Roboto Regular;">Несколько веб-страниц, объединенных общей темой</div>
                    <br/>
                    <div class="input-control text">
                        <input id="address" runat="server" placeholder="Адрес сайта" required="" style="font-family:  RobotoCondensed Regular;"/>
                        <div class="btn-search" style="padding-top: 3px; padding-left: 5px;" id="getinfo"></div>
                    </div>

                    <div style="border: 1px solid #D8D8D8;border-radius: 3px; background-color: white; overflow: hidden; margin-bottom: 15px; display:none;" runat ="server" id="panelinfo" class="showpanel">
                        <div style="padding: 5px;">
                            <div style ="border-bottom: 1px solid #e5e5e5; position: relative; padding: 0 0 5px 0; overflow: hidden; padding-right: 0; text-overflow: ellipsis; white-space: nowrap;">
                                <strong style="font-family: Roboto Regular;" id="title_finder" runat ="server">Наименование</strong>
                                <span style="float:right; height: 16px; width: 16px; margin-top: 2px; color: red; cursor: pointer" class="icon-earth" onclick="removeinten(this)"></span>
                            </div>
                            <div style="position:relative; max-height: 130px; margin-bottom: 2px; overflow: hidden; padding-top: 7px; text-align: center;">
                                <img id="P_logo" runat="server" src="../images/images.jpg" style="z-index: 900"/>
                                <div id="foter_img_logo" style="background-image: url('http://10.10.9.132:777/images/embeds_article_v2-aa76e8105f4ad58f58b33da824ba2dc0.png'); background-repeat: repeat-x; width: 100%; height: 16px; position:absolute; top: 65px; z-index: 1000">&nbsp</div>
                            </div>
                        </div>
                        <div style="border-style: solid; border-width: 1px 0px 0px 0px; border-color: #E5E5E5; background-color: #F5F5F5; padding: 10px;">
                            <span style="font-family: Roboto Regular;"  id="description_finder" runat ="server" >Описание</span>
                        </div>
                    </div>
                    <br />
                    <br />
                    <input type="hidden" runat="server" id="S_title" />
                    <input type="hidden" runat="server" id="S_description" />
                    <input type="hidden" runat="server" id="S_image" />
                    <div class="button large primary" onclick="loadImageForMap();" ><span class="icon-camera"></span></div>
                    <asp:Button runat="server" ID="create" CssClass="button large primary" Text="Создать" OnClick="create_Click"></asp:Button>
                    <asp:Button runat="server" ID="save" CssClass="button large primary" Text="Сохранить" OnClick="save_Click"></asp:Button>
                    
            </div>
        </div>
        <div class="span8">
            <h1><small class="on-right">Список областей карты</small></h1>
            <div class="example1 padding5" runat="server" id="P_arealist">

            </div>
        </div>
        </div>
    </div>
    </div>

<script type="text/javascript" src="../js/core.js" ></script>
<script type="text/javascript" src="../js/jquery.uploadify.js" ></script>
<script type="text/javascript">

    Win8_effectHeader();

    var obj_ = null;

    $("#getinfo").click(function () {
        var obj = document.getElementById('MP_address');
        search(obj, true);
    });

    $('#MP_address').keypress(function (event) {
        if (event.which == '13') {
            search(this, true);
            return false;
        }
    });

    $(".changeelement").click(function () {
        location.href = '/pages/areas.aspx?edit=' + $(this).attr('data-element');
    });

    $(".deleteelement").click(function () {
        ConfM('Удаление области', 'Вы действительно хотите удалить выбранную область?', $(this).attr('data-element'));
        obj_ = this;
    });

    function NoClick () {
        $("#MsgBoxBack3").css('display', 'none');
        $("#Cnf3").css('display', 'none');
    };

    function YesClick () {
        var id = $(obj_).attr('data-element');
        $.ajax({
            url: "/Handler.ashx",
            type: "get",
            async: false,
            data: {
                type: "deletearea",
                id: id,
            },
            success: function (response) {
                location.reload();
            },
            error: function (xhr, ajaxOptions, thrownError) { }
        });
    };

    var top_img_logo = $("#MP_P_logo").height();
    $("#foter_img_logo").css('top', (top_img_logo - 7 > 114 ? 114 : top_img_logo - 7) + 'px');

</script>

</asp:Content>