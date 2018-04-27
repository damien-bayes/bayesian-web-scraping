<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wordcloud.aspx.cs" Inherits="SHost_wordcloud" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Облако тегов</title>
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
                Облако<small class="on-right">тегов</small>
            </h1>
            <p class="description slider_" style="position: absolute; top: 90px; left: 20px; opacity: 0;">
                Страница вывода облак тегов.
            </p>
        </div>
        <hr />
         <div class="grid">
             <%=new TagCloud.TagCloud(cloud, new TagCloud.TagCloudGenerationRules{ Order = TagCloud.TagCloudOrder.Random, TagToolTipFormatString = "Слово {0}", TagUrlFormatString = "#{0}" }) %>
         </div>
    </div>
    </form>
</body>
</html>
<script type="text/javascript" src="../js/core.js" ></script>
<script type ="text/javascript">
    Win8_effectHeader();

</script>
