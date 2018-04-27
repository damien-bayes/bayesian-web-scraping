<%@ Page Language="C#" AutoEventWireup="true" CodeFile="calculate_hi.aspx.cs" Inherits="pages_calculate_hi" MasterPageFile="~/MasterPage.master" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <title>Вычисления - Краулер</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="MP" runat="server">

    <script type="text/javascript" src="../js/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="../js/jquery.widget.js"></script>
    <script type="text/javascript" src="../js/metro.min.js"></script>

    <asp:ScriptManager ID="ScriptManagerForAjax" runat="server"></asp:ScriptManager>
    <asp:Timer ID="TimerAjax" runat="server" Interval="10000" OnTick="TimerAjax_Tick"></asp:Timer>
    <div class="container">
    <br />
    <div style="position:relative; height: 110px;">
            <h1 class="slider_" style="position: absolute; top: 0px; left: 30px; opacity: 0;">
                <a href-data="/Home.aspx"><i class="icon-arrow-left-3 smaller black"></i></a>
                Фильтрация<small class="on-right">хи-квадрат</small>
            </h1>
            <p class="description slider_" style="position: absolute; top: 90px; left: 20px; opacity: 0;">
                Страница для выполнения расчетов с результатом парсинга.
            </p>
        </div>
        <hr />
        <div class="grid" runat="server" id="grid">
            <div class="row">
            <div class="span5" style="border-right: 1px #d9d9d9 solid; min-height: 600px; margin-right:20px;">
                <h1><small class="on-right">Новый расчет</small></h1>
                <div class="example1 padding20" style="padding-top:0px; padding-left: 7px;">
                    <strong style="font-family: Roboto Regular;">Выберите категорию</strong>
                    <br />
                    <div class="small_description" style="font-family: Roboto Regular;">Несколько веб-страниц, объединенных общей темой</div>
                    <br/>
                    <div class="input-control select">
                        <asp:DropDownList runat="server" ID="P_areas" AutoPostBack="true" required="required" OnSelectedIndexChanged="P_areas_SelectedIndexChanged" ></asp:DropDownList>
                    </div>
                    <div style="font-family: RobotoCondensed Regular; color: grey;" id="countclass1" runat="server">Выбранно документов: 0</div>
                    <hr />
                    <strong style="font-family: Roboto Regular;">Области не относящихся к теме</strong>
                    <br />
                    <div class="small_description" style="font-family: Roboto Regular;">Несколько веб-страниц, объединенных общей темой</div>
                    <br/>
                    <div class="input-control select">
                        <asp:CheckBoxList ID="CheckBoxList1" runat="server">
                        </asp:CheckBoxList>
                    </div>
                    <div style="font-family: RobotoCondensed Regular; color: grey;" id="countselected">Выбранно документов: 0</div>
                    <hr />
                    <br />
                    <asp:Button runat="server" ID="now" CssClass="button large primary" Text="Выполнить расчет" OnClick="now_Click" ></asp:Button>
                </div>
                
            </div>
            
            <div class="span9">
                <h1><small class="on-right">Расчитанные области</small></h1>
                <%--<p class="caution padding5"><strong>Внимание:</strong> Для просмотра результата нажмите на значек <span class="icon-eye" style="color:#16499a"></span> выбранной области.</p>--%>
                <asp:UpdatePanel ID="UpdatePanelInfo" runat="server">
                <ContentTemplate>
                        <div class="example1 padding5 " runat="server" id="P_arealist">
                        <table class="table striped hovered dataTable" id="dataTables-1" aria-describedby="dataTables-1_info">
                            <thead>
                                <tr role="row">
                                    <th class="text-left" tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" aria-label="Engine: activate to sort column ascending" aria-sort="ascending">ИД</th>
                                    <th class="text-left sorting" tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" aria-label="Browser: activate to sort column ascending" style="width: 286px;">Наименование</th>
                                    <th class="text-left sorting" tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" aria-label="Platform: activate to sort column ascending" style="width: 267px;">Адрес</th>
                                    <th class="text-left" aria-controls="dataTables-1" rowspan="1" colspan="1"></th>
                                </tr>
                            </thead>

                            <tbody id="his" runat="server">
                            
                            </tbody>
                           <%-- <tfoot>
                                <tr>
                                    <th class="text-left sorting_asc" tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" aria-label="Engine: activate to sort column ascending" style="width: 162px;" aria-sort="ascending">ИД</th>
                                    <th class="text-left sorting" tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" aria-label="Browser: activate to sort column ascending" style="width: 286px;">Наименование</th>
                                    <th class="text-left sorting" tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" aria-label="Platform: activate to sort column ascending" style="width: 267px;">Адрес</th>
                                    <th class="text-left" aria-controls="dataTables-1" rowspan="1" colspan="1"></th>
                                </tr >
                            </tfoot>--%>
                        </table>
                    </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="TimerAjax" EventName="Tick" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            </div>
        </div>
    </div>
<script type="text/javascript" src="../js/core.js" ></script>
<script type="text/javascript" src="../js/jquery.uploadify.js" ></script>
<script type ="text/javascript">
    var obj_ = null;

    Win8_effectHeader();


    function show_hi(ths)
    {
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

    function show_cloud(ths) {
        var attr_ = $(ths).attr('area');
        $.Dialog({
            overlay: true,
            shadow: true,
            flat: true,
            icon: '<span class = "icon-info" ></span>',
            title: '',
            content: '<iframe style = "min-width: 800px; min-height: 600px; border: solid 0px grey; width:' + (windowWidth - 400) + 'px; height:' + (windowHeight - 200) + 'px;" src = "/shost/wordcloud.aspx?id=' + attr_ + '"></iframes>',
            onShow: function (_dialog) {

            }
        });
    }

    var c_chackBox = Number('<%=this.count_checkbox%>');
    for (var i = 0; i < c_chackBox; i++) {
        $("[for = MP_CheckBoxList1_" + i + "]")[0].style.display = 'inline';
        $("[for = MP_CheckBoxList1_" + i + "]")[0].style.marginLeft = '7px';

        $("#MP_CheckBoxList1_" + i)[0].style.marginBottom = '7px';


        $("#MP_CheckBoxList1_" + i).click(function () {
            var summa = 0;
            for (var i = 0; i < c_chackBox; i++) {
                if ($("#MP_CheckBoxList1_" + i)[0].checked) {
                    var label_ = $("[for = MP_CheckBoxList1_" + i + "]").text();
                    var label_mas = label_.split('[');
                    var num = label_mas[1].replace(']', '').trim();
                    num = Number(num);
                    summa = summa + num;
                }
            }

            $("#countselected").text('Выбранно документов: ' + summa);
        });
    }
</script>

</asp:Content>