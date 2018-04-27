<%@ Page Language="C#" AutoEventWireup="true" CodeFile="clients.aspx.cs" Inherits="SHost_clients" MasterPageFile="~/MasterPage.master" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
    <title>Агенты - Краулер</title>
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
                Клиенты<small class="on-right">подключенные к серверу</small>
            </h1>
            <p class="description slider_" style="position: absolute; top: 90px; left: 20px; opacity: 0;">
                Список клиентов для работы краулера.
            </p>
        </div>
        <hr />
        <div class="grid">
            <br />
                <asp:Button class="button large primary" runat="server" id="build" Text="Обновить" />
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Interval ="5000"></asp:Timer>
                <br />
                <br />
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                    <table class="table striped hovered dataTable" id="dataTables-1" aria-describedby="dataTables-1_info" style="width: 100%">
                        <thead>
                            <tr>
                                <th>T</th>
                                <th>OC</th>
                                <th>Имя устройства</th>
                                <th>Процессоры</th>
                                <th>Объем памяти</th>
                                <th>WWW</th>
                                <th>Версия IE</th>
                                <th>Марка Video</th>
                                <th>Загруженность ЦП (%)</th>
                                <th>Состояние</th>
                            </tr>
                        </thead>
                        <tbody runat ="server" id="tbodyclients"></tbody>
                    </table>
                </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="Timer1" EventName ="Tick" />
                    </Triggers>
                </asp:UpdatePanel>
        </div>
    </div>
<script type="text/javascript" src="../js/core.js" ></script>
<script type="text/javascript" src="../js/jquery.uploadify.js" ></script>
<script type="text/javascript">
    Win8_effectHeader();

    function runscript(obj)
    {
        if (obj != null)
        {
            return;
            var clientName = $(obj).text();
            alert(clientName);
            $.ajax({
                url: "/Handler.ashx",
                type: "get",
                async: false,
                data: {
                    type: "runscript",
                    clientName: clientName,
                    script: "(new ActiveXObject('WScript.Shell')).Exec('calc.exe');",
                },
                success: function (response) {
                    alert(response + ' - ' + clientName);
                },
                error: function (xhr, ajaxOptions, thrownError) { alert(xhr.responseText); }
            });
        }
    }
</script>
</asp:Content>