<%@ Page Language="C#" AutoEventWireup="true" CodeFile="createtask.aspx.cs" Inherits="pages_createTask" MasterPageFile="~/MasterPage.master" %>
<%@ Register TagPrefix="tsk" TagName="tsk" Src="../Controls/task_item.ascx" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
    <title>Задачи - Краулер</title>
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
            Расписание<small class="on-right">задачи</small>
        </h1>
        <p class="description slider_" style="position: absolute; top: 90px; left: 20px; opacity: 0;">
            Страница для создания и редактирования расписания для краулера.
        </p>
    </div>
    <hr />
    <div class="grid" runat="server" id="grid">
        <div class="row">
        <div class="span5" style="border-right: 1px #d9d9d9 solid; min-height: 600px; margin-right:20px;">
            <h1><small class="on-right">Новая задача</small></h1>
            <div class="example1 padding20" style="padding-top:0px; padding-left: 7px;">
                    <strong style="font-family: Roboto Regular;">Выберите карту</strong>
                    <br />
                    <div class="small_description" style="font-family: Roboto Regular;">Несколько веб-страниц, объединенных общей темой</div>
                    <br/>
                    <div class="input-control select">
                        <asp:DropDownList runat="server" ID="P_maps" AutoPostBack="true" OnSelectedIndexChanged="P_maps_SelectedIndexChanged" required="required"></asp:DropDownList>
                    </div>
                    <hr />
                    <strong style="font-family: Roboto Regular;">Выберите область</strong>
                    <br />
                    <div class="small_description" style="font-family: Roboto Regular;">Несколько веб-страниц, объединенных общей темой</div>
                    <br/>
                    <div class="input-control select">
                        <asp:DropDownList runat="server" ID="P_areas" AutoPostBack="true" required="required" ></asp:DropDownList>
                    </div>
                    <strong style="font-family: Roboto Regular;">Интервал</strong>
                    <br />
                    <div class="small_description" style="font-family: Roboto Regular;">Переодичность выполнения задачи</div>
                    <br />
                    <div class="input-control select">
                        <asp:DropDownList runat="server" ID="interval" AutoPostBack="true" required="required" OnSelectedIndexChanged ="interval_SelectedIndexChanged">
                            <asp:ListItem Value="1" Text="Каждый день"></asp:ListItem>
                            <asp:ListItem Value="7" Text="Раз в неделю"></asp:ListItem>
                            <asp:ListItem Value="30" Text="Раз в месяц"></asp:ListItem>
                            <asp:ListItem Value="0" Text="Однажды"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <p runat ="server" id="namepr"></p>
                    <div class="input-control text" runat ="server" id="panel1" style="display:none;">
                        <div class="span3">
                            <div class="input-control text" data-role="datepicker" data-locale="ru" data-date="2013-11-13" data-effect="slide" data-other-days="1" runat="server" id="P_day_div">
                                <input type="text" readonly="readonly"  required="" runat="server" id="P_day" />
                                <div class="btn-date" style="padding-left: 5px; padding-top: 3px;"></div>
                            <div class="calendar calendar-dropdown" style="position: absolute; max-width: 260px; z-index: 1000; top: 100%; left: 0px; display: none;"><table class="bordered"><tbody><tr class="calendar-header"><td class="text-center"><a class="btn-previous-year" href="#"><i class="icon-previous"></i></a></td><td class="text-center"><a class="btn-previous-month" href="#"><i class="icon-arrow-left-4"></i></a></td><td colspan="3" class="text-center"><a class="btn-select-month" href="#">November 2013</a></td><td class="text-center"><a class="btn-next-month" href="#"><i class="icon-arrow-right-4"></i></a></td><td class="text-center"><a class="btn-next-year" href="#"><i class="icon-next"></i></a></td></tr><tr class="calendar-subheader"><td class="text-center day-of-week">Su</td><td class="text-center day-of-week">Mo</td><td class="text-center day-of-week">Tu</td><td class="text-center day-of-week">We</td><td class="text-center day-of-week">Th</td><td class="text-center day-of-week">Fr</td><td class="text-center day-of-week">Sa</td></tr><tr><td class="empty"><small class="other-day">27</small></td><td class="empty"><small class="other-day">28</small></td><td class="empty"><small class="other-day">29</small></td><td class="empty"><small class="other-day">30</small></td><td class="empty"><small class="other-day">31</small></td><td class="text-center day"><a href="#">1</a></td><td class="text-center day"><a href="#">2</a></td></tr><tr><td class="text-center day"><a href="#">3</a></td><td class="text-center day"><a href="#">4</a></td><td class="text-center day"><a href="#">5</a></td><td class="text-center day"><a href="#">6</a></td><td class="text-center day"><a href="#">7</a></td><td class="text-center day"><a href="#">8</a></td><td class="text-center day"><a href="#">9</a></td></tr><tr><td class="text-center day"><a href="#">10</a></td><td class="text-center day"><a href="#">11</a></td><td class="text-center day"><a href="#">12</a></td><td class="text-center day"><a href="#" class="selected">13</a></td><td class="text-center day"><a href="#">14</a></td><td class="text-center day"><a href="#">15</a></td><td class="text-center day"><a href="#">16</a></td></tr><tr><td class="text-center day"><a href="#">17</a></td><td class="text-center day"><a href="#">18</a></td><td class="text-center day"><a href="#">19</a></td><td class="text-center day"><a href="#">20</a></td><td class="text-center day"><a href="#">21</a></td><td class="text-center day"><a href="#">22</a></td><td class="text-center day"><a href="#">23</a></td></tr><tr><td class="text-center day"><a href="#">24</a></td><td class="text-center day"><a href="#">25</a></td><td class="text-center day"><a href="#">26</a></td><td class="text-center day"><a href="#">27</a></td><td class="text-center day"><a href="#">28</a></td><td class="text-center day"><a href="#">29</a></td><td class="text-center day"><a href="#">30</a></td></tr></tbody></table></div></div>
                        </div>
                    </div>
                    <div class="input-control select" runat ="server" id="panel2" style="display:none;">
                        <asp:DropDownList runat="server" ID="dayofweek" required="" >
                            <asp:ListItem Value="1" Text="Понедельник"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Вторник"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Среда"></asp:ListItem>
                            <asp:ListItem Value="4" Text="Четверг"></asp:ListItem>
                            <asp:ListItem Value="5" Text="Пятница"></asp:ListItem>
                            <asp:ListItem Value="6" Text="Суббота"></asp:ListItem>
                            <asp:ListItem Value="7" Text="Воскресенье"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="input-control select" runat ="server" id="panel3" style="display:none;">
                        <asp:DropDownList runat="server" ID="days" required="" ></asp:DropDownList>
                    </div>
                    <div class="input-control number">
                        <div class="span1">
                            <strong style="font-family: Roboto Regular;">Час:</strong><br /><br /><input type="number" onkeypress='validate(event)' min ="0" max="23" value="0" step="1" required="" runat="server" id="P_hour" />
                        </div>
                        <div class="span1">
                            <strong style="font-family: Roboto Regular;">Минута:</strong><br /><br /><input type="number" onkeypress='validate(event)' min ="0" max="59" value="0" step="1"  required="" runat="server" id="P_minute" />
                        </div>
                    </div>
                    <br />
                    <br />
                    <br />
                    <div class="input-control check">
                        <label><input type="checkbox" runat="server" id="removeresult" checked="checked" style="font-family: Roboto Regular;"/>&nbsp;Удалять предыдущий результат</label>
                    </div>
                    <br />
                    <br />
                    <asp:Button runat="server" ID="now" CssClass="button large primary" Text="Выполнить сейчас" OnClick="now_Click"></asp:Button>
                    <asp:Button runat="server" ID="create" CssClass="button large primary" Text="Создать" OnClick   ="create_Click"></asp:Button>
            </div>
        </div>
        <div class="span9">
            <h1><small class="on-right">Список задач</small></h1>
            <div class="example1 padding5" runat="server" id="P_arealist" style="overflow-y:auto; height: 600px;">
                <div class="listview-outlook" runat ="server" id="tasks">
                        
                </div>
            </div>
        </div>
        </div>
    </div>
    </div>
<script type="text/javascript" src="../js/core.js" ></script>
<script type="text/javascript" src="../js/jquery.uploadify.js" ></script>

<script type ="text/javascript">
    var obj_ = null;

    Win8_effectHeader();

    $(".icon-pause-2").click(function () {
        var id = $(this).attr('val');
        $.ajax({
            url: "/Handler.ashx",
            type: "get",
            async: false,
            data: {
                type: "changetaskstate",
                id: id,
                state: 6,
            },
            success: function (response) {
                location.reload();
            },
            error: function (xhr, ajaxOptions, thrownError) { }
        });
    });

    $(".icon-play-2").click(function () {
        var id = $(this).attr('val');
        $.ajax({
            url: "/Handler.ashx",
            type: "get",
            async: false,
            data: {
                type: "changetaskstate",
                id: id,
                state: 3,
            },
            success: function (response) {
                location.reload();
            },
            error: function (xhr, ajaxOptions, thrownError) { }
        });
    });

    $(".removetask").click(function () {
        ConfM('Удаление области', 'Вы действительно хотите удалить выбранную область?', $(this).attr('data-element'));
        obj_ = this;
    });

    function NoClick() {
        $("#MsgBoxBack3").css('display', 'none');
        $("#Cnf3").css('display', 'none');
    };

    function YesClick() {
        var _th = $(obj_).attr('data-element');
        $.ajax({
            url: "/Handler.ashx",
            type: "get",
            async: false,
            data: {
                type: "deletetask",
                id: _th,
            },
            success: function (response) {
                location.reload();
            },
            error: function (xhr, ajaxOptions, thrownError) { }
        });
    };
    
    function removetask(obj)
    {
        ConfM('Удаление задачи', 'Вы действительно хотите удалить выбранную задачу?', $(obj).attr('date-element'));
        obj_ = obj;
    }

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
                type: "deletetask",
                id: id,
            },
            success: function (response) {
                location.reload();
            },
            error: function (xhr, ajaxOptions, thrownError) { }
        });
    };

    function changetime(o)
    {
        if (o != null)
        {
            var result = prompt('Введите время выпполнения задачи', $(o).text());
            if (result != null)
            {
                var id = $(o).attr('id');
                if (id != null)
                {
                    $.ajax({
                        url: "/Handler.ashx",
                        type: "get",
                        async: false,
                        data: {
                            type: "changetime",
                            id: id,
                            value: result
                        },
                        success: function (response) {
                            location.reload();
                        },
                        error: function (xhr, ajaxOptions, thrownError) { }
                    });
                }
            }
        }
    }

    function validate(evt) {
        var theEvent = evt || window.event;
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
        var regex = /[0-9]|\./;
        if (!regex.test(key)) {
            theEvent.returnValue = false;
            if (theEvent.preventDefault) theEvent.preventDefault();
        }
    }

</script>

</asp:Content>

