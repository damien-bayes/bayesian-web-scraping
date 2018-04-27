<%@ Page Language="C#" AutoEventWireup="true" CodeFile="events.aspx.cs" Inherits="SHost_events" %>

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
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
     <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Interval="30000" Enabled="true"></asp:Timer>
     <div class="container">
        <br />
        <div style="position:relative; height: 110px;">
            <h1 class="slider_" style="position: absolute; top: 0px; left: 30px; opacity: 0;">
                События<small class="on-right">действия сервера</small>
            </h1>
            <p class="description slider_" style="position: absolute; top: 90px; left: 20px; opacity: 0;">
                Страница описания действий сервера парсера.
            </p>
        </div>
        <hr />
         <div class="grid">
             <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                 <ContentTemplate>
                     <table class="table striped hovered dataTable" id="dataTables-1" aria-describedby="dataTables-1_info" style="width: 100%">
                    <thead>
                        <tr role="row">
                            <th class="text-left " tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" >ID</th>
                            <th class="text-left " tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1">ID задачи</th>
                            <th class="text-left " tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1">Состояние</th>
                            <th class="text-left " tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1">Дата начала</th>
                            <th class="text-left " tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1">Клиентов</th>
                            <th class="text-left " tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1">Дата окончания</th>
                        </tr>
                    </thead>

                    <tbody id="content_events" runat="server">
                    </tbody>
            </table>
                 </ContentTemplate>
                 <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                 </Triggers>
             </asp:UpdatePanel>
        </div>
    </div>
    </form>
</body>
</html>
<script type="text/javascript" src="../js/core.js" ></script>
<script type ="text/javascript">
    Win8_effectHeader();
</script>
