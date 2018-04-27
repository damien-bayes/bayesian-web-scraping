<%@ Page Language="C#" AutoEventWireup="true" CodeFile="user.aspx.cs" Inherits="pages_user"  %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>События сервера</title>
    <link href="../css/metro-bootstrap.css" rel="stylesheet" />
    <link href="../css/metro-bootstrap-responsive.css" rel="stylesheet" />
    <link href="../css/iconFont.css" rel="stylesheet" />
    <link href="../css/docs.css" rel="stylesheet" />
    <link href="../css/core.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="../js/jquery.widget.js"></script>
    <script type="text/javascript" src="../js/metro.min.js"></script>
</head>
<body class="metro">
    <form id="form1" runat="server">

    <div class="container">
        <br />
        <div style="position:relative; height: 110px;">
            <h1 class="slider_" style="position: absolute; top: 0px; left: 30px; opacity: 0;">
                Пользователь<small class="on-right">системы</small>
            </h1>
            <p class="description slider_" style="position: absolute; top: 90px; left: 20px; opacity: 0;">
                Страница профиля пользователя.
            </p>
            
        </div>
        <hr />
        <div class="grid" runat="server" id="grid"><div class="row">
            <div class="span4" style="border-right: 1px #d9d9d9 solid; min-height: 500px; ">
                <h1><small class="on-right">
                    Аватар</small>
                </h1>
                <img src="../images/default-user.png" id="MP_P_logo" runat="server" style="width:200px;height:200px;" />
                <br />
                <br />
                <input type="button" class="button large primary" value="Обзор" onclick="loadImageForMap(); " />
                <input type="hidden" value="" id="MP_S_image" />
            </div>
            <div class="span6">
                <p>Логин</p>
                <div class="input-control text">
                    <input id="login" runat="server" placeholder="Адрес сайта" required="" />
                </div>
                <p>Фамилия</p>
                <div class="input-control text">
                    <input id="Text1" runat="server" placeholder="Адрес сайта" required="" />
                </div>
                <p>Имя</p>
                <div class="input-control text">
                    <input id="Text2" runat="server" placeholder="Адрес сайта" required="" />
                </div>
                <p>Отчество</p>
                <div class="input-control text">
                    <input id="Text3" runat="server" placeholder="Адрес сайта" required="" />
                </div>
                <p>Немножко о себе</p>
                <div class="input-control text">
                    <textarea style="height:130px; width: 460px;" id="desc"></textarea>
                </div>
                <br /><br /><br /><br /><br /><br /><br />
                <input type="button" class="button large primary" value="Сохранить" onclick="saveavatar(); " />
            </div></div>
        </div>
    </div>
    <asp:FileUpload ID="fileBrow" runat="server" CssClass="fileUpload"/>
    </form>
</body>
</html>
<script type="text/javascript" src="../js/core.js" ></script>
<script type="text/javascript" src="../js/jquery.uploadify.js" ></script>
<script type="text/javascript">
    Win8_effectHeader();

    function saveavatar()
    {
        var url = $("#MP_S_image").val();
        if (url != "")
        {
            $.ajax({
                url: "/Handler.ashx",
                type: "get",
                async: false,
                data: {
                    type: "saveavatar",
                    name: '<%=this.username%>',
                    url: url
                },
                success: function (response) {
                    
                },
                error: function (xhr, ajaxOptions, thrownError) { }
            });
        }
    }
</script>
