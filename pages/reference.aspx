<%@ Page Language="C#" AutoEventWireup="true" CodeFile="reference.aspx.cs" Inherits="pages_reference" MasterPageFile="~/MasterPage.master" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
    <title>Справочники - Краулер</title>
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
                Справочники<small class="on-right">записи базы</small>
            </h1>
            <p class="description slider_" style="position: absolute; top: 90px; left: 20px; opacity: 0;">
                Страница для создания и редактирования записей справочников.
            </p>
        </div>
        <br />
        <br />
        <div class="tab-control" data-role="tab-control">
            <ul class="tabs">
                <li class="active"><a href="#_page_1">Стоп слова</a></li>
                <li><a href="#_page_2">Состояние задачи</a></li>
                <li><a href="#_page_3">Категории</a></li>
                <li class="place-right"><a href="#_page_4"><i class="icon-spin"></i></a></li>
            </ul>
 
            <div class="frames">
                <div class="frame" id="_page_1">
                     <div class="grid">
                        <div class="row">
                        <div class="span6" style="border-right: 1px #d9d9d9 solid; min-height: 600px; margin-right:20px;">
                            <h1><small class="on-right">Новое слово</small></h1>
                            <div class="example1 padding20" style="padding-top:0px; padding-left: 7px;">
                                
                                    <p>Значение</p>
                                    <div class="input-control text">
                                        <input id="stopwords_word" placeholder="Новое значение справочника" required="required" />
                                    </div>
                                    <br />
                                    <br />
                                    <asp:Button runat="server" ID="create" data-text ="stopwords_word" CssClass="button large primary sv_ref" Text="Сохранить" ></asp:Button>

                            </div>
                        </div>
                        <div class="span7">
                            <p class="subheader">Элементы справочника</p>
                            <div class="example1 padding5" runat="server" id="P_maplist" style ="height: 600px; overflow-y: auto;">
                                <table class="table striped hovered dataTable" aria-describedby="dataTables-1_info" style="width: 100%">
                                    <thead>
                                        <tr role="row">
                                            <th class="text-left " tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1"  >ID</th>
                                            <th class="text-left " tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" >Значение</th>
                                        </tr>
                                    </thead>

                                    <tbody id="content_log" runat ="server" >
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        </div>
                    </div>
                </div>
                <div class="frame" id="_page_2">
                     <div class="grid">
                        <div class="row">
                        <div class="span6" style="border-right: 1px #d9d9d9 solid; min-height: 600px; margin-right:20px;">
                            <h1><small class="on-right">Новое состояние</small></h1>
                            <div class="example1 padding20" style="padding-top:0px; padding-left: 7px;">

                                    <p>Значение</p>
                                    <div class="input-control text">
                                        <input id="taskstate_state"  placeholder="Новое значение справочника" required="required" />
                                    </div>
                                    <br />
                                    <br />
                                    <asp:Button runat="server" ID="Button1" data-text ="taskstate_state" CssClass="button large primary sv_ref" Text="Сохранить" ></asp:Button>

                            </div>
                        </div>
                        <div class="span7">
                            <p class="subheader">Элементы справочника</p>
                            <div class="example1 padding5" runat="server" id="Div1" style ="height: 600px; overflow-y: auto;">
                                <table class="table striped hovered dataTable" aria-describedby="dataTables-1_info" style="width: 100%">
                                    <thead>
                                        <tr role="row">
                                            <th class="text-left " tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1"  >ID</th>
                                            <th class="text-left " tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" >Значение</th>
                                        </tr>
                                    </thead>

                                    <tbody id="P_states" runat ="server" >
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        </div>
                    </div>
                </div>
                <div class="frame" id="_page_3">
                     <div class="grid">
                        <div class="row">
                        <div class="span6" style="border-right: 1px #d9d9d9 solid; min-height: 600px; margin-right:20px;">
                            <h1><small class="on-right">Новая категория</small></h1>
                            <div class="example1 padding20" style="padding-top:0px; padding-left: 7px;">

                                    <p>Значение</p>
                                    <div class="input-control text">
                                        <input id="topics_topic"  placeholder="Новое значение справочника" required="required" />
                                    </div>
                                    <br />
                                    <br />
                                    <asp:Button runat="server" ID="topic" data-text ="topics_topic" CssClass="button large primary sv_ref" Text="Сохранить" ></asp:Button>

                            </div>
                        </div>
                        <div class="span7">
                            <p class="subheader">Элементы справочника</p>
                            <div class="example1 padding5" runat="server" id="Div2" style ="height: 600px; overflow-y: auto;">
                                <table class="table striped hovered dataTable" aria-describedby="dataTables-1_info" style="width: 100%">
                                    <thead>
                                        <tr role="row">
                                            <th class="text-left " tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1"  >ID</th>
                                            <th class="text-left " tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" >Значение</th>
                                        </tr>
                                    </thead>

                                    <tbody id="topics" runat ="server" >
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
       
    </div>
<script type="text/javascript" src="../js/core.js" ></script>
<script type="text/javascript" src="../js/jquery.uploadify.js" ></script>

<script type="text/javascript">
    Win8_effectHeader();

    $(".sv_ref").click(function () {
       
        var data_text = $(this).attr('data-text');
        if (data_text != null)
        {
            var sp = data_text.split('_');
            $.ajax({
                url: "/Handler.ashx",
                type: "get",
                dataType: '',
                async: false,
                data: {
                    type: "reference",
                    name: sp[0],
                    column: sp[1],
                    value: $("#" + data_text).val()
                },
                success: function (response) {
                    $("#" + data_text).val('');
                    location.reload();
                },
                error: function (xhr, ajaxOptions, thrownError) { AlertM('Система', 'Невозможно записать элемент'); }
            });
        }
    });
</script>
</asp:Content>
