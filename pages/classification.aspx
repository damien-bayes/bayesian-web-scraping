<%@ Page Language="C#" AutoEventWireup="true" CodeFile="classification.aspx.cs" Inherits="pages_classification" MasterPageFile="~/MasterPage.master" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <title>Классификация - Краулер</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="MP" runat="server">

    <script type="text/javascript" src="../js/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="../js/jquery.widget.js"></script>
    <script type="text/javascript" src="../js/metro.min.js"></script>

    <asp:ScriptManager ID="ScriptManagerForAjax" runat="server"></asp:ScriptManager>
    <asp:Timer ID="TimerAjax" runat="server" Interval="15000" OnTick="TimerAjax_Tick" Enabled="false"></asp:Timer>
    <div class="container">
    <br />
    <div style="position:relative; height: 105px;">
            <h1 class="slider_" style="position: absolute; top: 0px; left: 30px; opacity: 0;">
                <a href-data="/Home.aspx"><i class="icon-arrow-left-3 smaller black"></i></a>
                Классификация<small class="on-right">определение класса темы</small>
            </h1>
            <p class="description slider_" style="position: absolute; top: 90px; left: 20px; opacity: 0;">
                Страница для определение класса темы.
            </p>
        </div>
        <hr />
        <div class="grid" runat="server" id="grid">
            <div class="row">
            <div class="span5" style="border-right: 1px #d9d9d9 solid; min-height: 600px; margin-right:20px;">
                <div runat="server" id="warning" style="height:0px; visibility:hidden"><p class="caution"><strong>Предупреждение:</strong> В данный момент уже выполняется классификация. Запуск еще одного процесса увеличит время выполнения.</p></div>
                <h1><small class="on-right">Новая классификация</small></h1>
                <div class="example1 padding20" style="padding-top:0px; padding-left: 7px;">
                    <strong style="font-family: Roboto Regular;">Области для тренировки</strong>
                    <br />
                    <div class="small_description" style="font-family: Roboto Regular;">Несколько веб-страниц, объединенных общей темой</div>
                    <br/>
                    <div class="input-control select" style="height:110px; overflow-y: auto;">
                        <asp:CheckBoxList ID="CheckBoxList1" runat="server">
                        </asp:CheckBoxList>
                    </div>
                    <hr />
                    <strong style="font-family: Roboto Regular;">Области для тестирования</strong>
                    <br />
                    <div class="small_description" style="font-family: Roboto Regular;">Несколько веб-страниц, объединенных общей темой</div>
                    <br/>
                    <div class="input-control select" style="height:110px; overflow-y: auto;">
                        <asp:CheckBoxList ID="CheckBoxList2" runat="server">
                        </asp:CheckBoxList>
                    </div>
                    <hr />
                    <strong style="font-family: Roboto Regular; ">Категория</strong>
                    <br/>
                    <div class="input-control select" style="margin-top: 10px;">
                        <asp:DropDownList runat="server" ID="topic_s" AutoPostBack="true" required="required" >
                            
                        </asp:DropDownList>
                    </div>
                    <br />
                    <br />
                    <br />
                    <asp:Button runat="server" ID="now" CssClass="button large primary" Text="Классифицировать" OnClick="now_Click" ></asp:Button>
                </div>
            </div>
            <div class="span9">
                <%--<asp:UpdatePanel ID="UpdatePanelInfo" runat="server">
                <ContentTemplate>--%>
                    <h1><small class="on-right">Результат классификации&nbsp;&nbsp;&nbsp;<%--<img runat="server" id="load_logo" src="../images/W_load_1.GIF" />--%></small></h1>
                        <div class="example1 padding5" runat="server" id="P_arealist">
                            <asp:chart id="Chart1" runat="server" Height="300px" Width="400px">
                              <titles>
                                <asp:Title ShadowOffset="3" Name="Title1" />
                              </titles>
                              <legends>
                                <asp:Legend Alignment="Center" Docking="Bottom"
                                            IsTextAutoFit="False" Name="Default"
                                            LegendStyle="Row" />
                              </legends>
                              <series>
                                <asp:Series Name="Default" />
                              </series>
                              <chartareas>
                                <asp:ChartArea Name="ChartArea1"
                                                 BorderWidth="0" />
                              </chartareas>
                            </asp:chart>
                            <%--<div style="float:right;" runat="server" id="cloud_cl"></div>--%>
                            <div style="height: 330px; overflow-y: auto;" runat="server" id ="statistic" visible="true">
                            <table class="table striped hovered dataTable" id="dataTables-1" aria-describedby="dataTables-1_info" style="font-family: RobotoCondensed Regular; font-size: 10pt;">
                                <thead>
                                    <tr role="row">
                                        <%--<th class="text-left" tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" aria-label="Browser: activate to sort column ascending" style="width: 286px;">P</th>--%>
                                        <th class="text-left" tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" aria-label="Platform: activate to sort column ascending" style="width: 267px;">Документ</th>
                                        <th class="text-left" tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" aria-label="Platform: activate to sort column ascending" style="width: 267px;">Категория</th>
                                        <th class="text-left" tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" aria-label="Platform: activate to sort column ascending" style="width: 267px;">Вер-ть</th>
                                    </tr>
                                </thead>
                                <tbody id="his" runat="server">
                            
                                </tbody>
                                <%--<tfoot>
                                    <tr>
                                        <th class="text-left sorting" tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" aria-label="Browser: activate to sort column ascending" style="width: 286px;">PROB</th>
                                        <th class="text-left sorting" tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" aria-label="Platform: activate to sort column ascending" style="width: 267px;">DOC</th>
                                        <th class="text-left sorting" tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" aria-label="Platform: activate to sort column ascending" style="width: 267px;">CLASS</th>
                                    </tr >
                                </tfoot>--%>
                            </table>
                            </div>
                    </div>
                    <%--</ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="TimerAjax" EventName="Tick" />
                    </Triggers>
                </asp:UpdatePanel>--%>
            </div>
            </div>
        </div>
    </div>
<script type="text/javascript" src="../js/core.js" ></script>
<script type="text/javascript" src="../js/jquery.uploadify.js" ></script>
<script type ="text/javascript">
    var obj_ = null;

    Win8_effectHeader();


    function show_hi(ths) {
        var attr_ = $(ths).attr('area');
        $.Dialog({
            overlay: true,
            shadow: true,
            flat: true,
            icon: '<span class = "icon-info" ></span>',
            title: '',
            content: '<iframe style = "min-width: 800px; min-height: 600px; border: solid 0px grey; width:' + (windowWidth - 400) + 'px; height:' + (windowHeight - 200) + 'px;" src = "/shost/hi_result.aspx?hi=' + attr_ + '"></iframes>',
            onShow: function (_dialog) {

            }
        });
    }

    var c_chackBox = Number('<%=this.count_checkbox1%>');
    for (var i = 0; i < c_chackBox; i++) {
        $("[for = MP_CheckBoxList1_" + i + "]")[0].style.display = 'inline';
        $("[for = MP_CheckBoxList1_" + i + "]")[0].style.marginLeft = '7px';

        $("#MP_CheckBoxList1_" + i)[0].style.marginBottom = '7px';
        $("#MP_CheckBoxList1_" + i).change(function () {
            var index = $(this).attr('id').replace('MP_CheckBoxList1_', 'MP_CheckBoxList2_');
            if ($(this).prop('checked') == true) {
                $("#" + index).prop('checked', false);
                $("#" + index).prop('disabled', true);
            }
            else {
                $("#" + index).prop('checked', false);
                $("#" + index).prop('disabled', false);
            }
        });
    }

    var c_chackBox2 = Number('<%=this.count_checkbox2%>');
    for (var i = 0; i < c_chackBox2; i++) {
        $("[for = MP_CheckBoxList2_" + i + "]")[0].style.display = 'inline';
        $("[for = MP_CheckBoxList2_" + i + "]")[0].style.marginLeft = '7px';

        $("#MP_CheckBoxList2_" + i)[0].style.marginBottom = '7px';
        $("#MP_CheckBoxList2_" + i).click(function () {
            var index = $(this).attr('id').replace('MP_CheckBoxList2_', 'MP_CheckBoxList1_');
            if ($(this).prop('checked') == true) {
                $("#" + index).prop('checked', false);
                $("#" + index).prop('disabled', true);
            }
            else {
                $("#" + index).prop('checked', false);
                $("#" + index).prop('disabled', false);
            }
        });
    }
</script>

</asp:Content>