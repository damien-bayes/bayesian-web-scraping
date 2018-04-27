<%@ Page Language="C#" AutoEventWireup="true" CodeFile="lemmas.aspx.cs" Inherits="pages_lemmas" MasterPageFile="~/MasterPage.master" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
    <title>Лемматизация - Краулер</title>
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
                Лемматизация<small class="on-right">результат работы краулера</small>
            </h1>
            <p class="description slider_" style="position: absolute; top: 90px; left: 20px; opacity: 0;">
                Страница выполнения пробной лемматизации.
            </p>
        </div>
        <hr />
        <div class="grid">
            <nav class="navigation-bar">
                <div class="navigation-bar-content">
                    <a href="/" class="element"><span class="icon-grid-view"></span>&nbsp;&nbsp;СБРОСИТЬ</a>
                    <span class="element-divider"></span>

                    <a class="pull-menu" href="#"></a>
                    <ul class="element-menu">
                        <li>
                            <a class="dropdown-toggle" href="#" style="font-size: 20px; padding-right: 50px;" runat="server" id="selected"></a>
                            <ul class="dropdown-menu" data-role="dropdown" runat="server" id="menus" style="white-space: nowrap">
                                    
                            </ul
                        </li>
                    </ul>
                </div>
            </nav>
            <br />
            <button type="button" class="button large primary" runat="server" id="build"  onclick ="lemmas();" >Выполнить</button>
            <img id="loadGIF" src="../images/W_load_1.GIF" style="margin-top:3px; margin-left: 10px; visibility:hidden;" />
            <br />
            <br />
            <table class="table striped hovered dataTable" id="dataTables-1" aria-describedby="dataTables-1_info" style="width: 100%">
                <thead>
                    <tr role="row">
                        <th class="text-left " tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1"  style="width: 33%;" >Исходное слово</th>
                        <th class="text-left " tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" style="width: 33%;">Лемматизированное слово</th>
                         <th class="text-left " tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" style="width: 33%;">Количество повторений</th>
                    </tr>
                </thead>

                <tbody id="content_lemmas">
                </tbody>
            </table>
        </div>
    </div>
<script type="text/javascript" src="../js/core.js" ></script>
<script type="text/javascript" src="../js/jquery.uploadify.js" ></script>
<script type ="text/javascript">

    Win8_effectHeader();

    function lemmas() {
        var url = '<%=this.url%>';
        if (url != '') {
            $("#loadGIF").css('visibility', 'visible');
            $.get(
                "/Handler.ashx",
                {
                    type: "lemmas",
                    url_: url,
                },
                function (data) {
                    $("#content_lemmas").html(data);
                    $("#loadGIF").css('visibility', 'hidden');
                }
           );
        }
        else {
            AlertM('Лемматизация', 'Введенный адрес не являеться ссылкой');
            $("#loadGIF").css('visibility', 'hidden');
        }
    }
</script>

</asp:Content>