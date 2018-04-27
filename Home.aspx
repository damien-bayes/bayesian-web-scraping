<%@ Page Language="C#" 
         AutoEventWireup="true" 
         CodeFile="Home.aspx.cs" 
         Inherits="Home" 
         MasterPageFile="~/MasterPage.master" 
         Title="Cube Crawler: Панель управления"%>

<%@ Register TagPrefix="prgr" 
             TagName="prgr" 
             Src="~/Controls/parsing_progress.ascx" %>
<%@ Register TagPrefix="prgr_item" 
             TagName="prgr_item" 
             Src="~/Controls/parsing_item.ascx" %>
<%@ Register TagPrefix="hi_item" 
             TagName="hi_item" 
             Src="~/Controls/hi_item.ascx" %>
<%@ Register TagPrefix="cl_item" 
             TagName="cl_item" 
             Src="~/Controls/classification_item.ascx" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <meta name="description" 
          content="" />

    <meta name="keywords" 
          content="" />

    <link rel="stylesheet" type="text/css" href="css/cube-crawler-supervisor-css/pages/home-page.css"/>
</asp:Content>

<asp:Content ContentPlaceHolderID="MP" 
             runat="server" >
    <asp:ScriptManager ID="ScriptManagerForAjax" 
                       runat="server"></asp:ScriptManager>
    <asp:Timer ID="TimerAjax" 
               runat="server" 
               Interval="10000" 
               OnTick="TimerAjax_Tick"></asp:Timer>

    <div class="grid-100 grid-parent indent-from-text-ani">
        <div class="grid-70">
            <h3 title=""><span class="fa fa-cubes"></span>&nbsp;Действия</h3>
            <div class="shadow-z-0 left-content">
                <div class="box-list-wrapper display-animation">
                    <header>
                        <a href="javascript:void(0)" class="box-list-show-list withripple" title="Преобразование в списочную структуру"><i class="fa fa-th-list"></i></a>
                        <a href="javascript:void(0)" class="box-list-hide-list withripple" title="Преобразование в плиточную структуру"><i class="fa fa-th"></i></a>
                    </header>
                    <div class="grid-100 box-list-container shadow-z-0">
                        <a href="pages/maps.aspx" title="Карты">
                            <div class="box-list-b shadow-z-1 withripple">
                                <h4><span class="fa fa-puzzle-piece fa-lg"></span>&nbsp;Карты</h4>
                                <p class="box-content">Элемент навигации по странице сайта, имеющий полный перечень разделов</p>
                                <p class="box-sub-content">Количество: <b>12</b></p>
                            </div>
                        </a>

                        <a href="pages/areas.aspx" title="Области поиска">
                            <div class="box-list-b shadow-z-1 withripple">
                                <h4><span class="fa fa-newspaper-o fa-lg"></span>&nbsp;Области поиска</h4>
                                <p class="box-content">Указание привилегированных ресурсов для синтаксического анализа</p>
                                <p class="box-sub-content">Количество: <b>18</b></p>
                            </div>
                        </a>

                        <a href="pages/paths.aspx" title="Маршруты">
                            <div  class="box-list-b shadow-z-1 withripple">
                                <h4><span class="fa fa-sitemap fa-lg"></span>&nbsp;Маршруты</h4>
                                <p class="box-content">Выбор способа перемещения по заданной области страницы</p>
                            </div>
                        </a>

                        <a href="pages/elements.aspx" title="Цели поиска">
                            <div  class="box-list-b shadow-z-1 withripple">
                                <h4><span class="fa fa-crosshairs fa-lg"></span>&nbsp;Цели поиска</h4>
                                <p class="box-content">Настройка объекта на странице, содержащий ссылки конечных страниц</p>
                            </div>
                        </a>

                        <a href="pages/createtask.aspx" title="Расписание задач">
                            <div class="box-list-b shadow-z-1 withripple">
                                <h4><span class="fa fa-clock-o fa-lg"></span>&nbsp;Расписание задач</h4>
                                <p class="box-content">Автоматизация процесса выполнения задач по установленному расписанию</p>
                                <p class="box-sub-content">Сегодня: <b>0</b></p>
                            </div>
                        </a>

                        <a href="pages/calculate_hi.aspx" title="Фильтрация">
                            <div class="box-list-b shadow-z-1 withripple">
                                <h4><span class="fa fa-filter fa-lg"></span>&nbsp;Фильтрация</h4>
                                <p class="box-content">Выполнение определенных критериев для проверки статистических гипотез</p>
                            </div>
                        </a>

                        <a href="#" title="Классификация">
                            <div class="box-list-b shadow-z-1 withripple" tabindex="5">
                                <h4><span class="fa fa-bar-chart fa-lg"></span>&nbsp;Классификация</h4>
                                <p class="box-content">Распределение однородных предметов или понятий по классам, группам или по какому-либо признаку</p>
                            </div>
                        </a>
                    </div>
                    <hr />
                    <div class="grid-100 box-list-container shadow-z-0">
                        <a href="pages/maps.aspx" title="Отчетность">
                            <div class="box-list-b shadow-z-1">
                                <h4><span class="fa fa-file-text-o fa-lg"></span>&nbsp;Отчетность</h4>
                                <p class="box-content">Особая форма организации сбора данных</p>
                            </div>
                        </a>

                        <a href="pages/areas.aspx" title="Клиенты">
                            <div class="box-list-b shadow-z-1">
                                <h4><span class="fa fa-users fa-lg"></span>&nbsp;Клиенты</h4>
                                <p class="box-content">Перечень подключенных клиентов к службе системы</p>
                            </div>
                        </a>

                        <a href="pages/areas.aspx" title="Справка">
                            <div  class="box-list-b shadow-z-1">
                                <h4><span class="fa fa-bookmark-o fa-lg"></span>&nbsp;Справка</h4>
                                <p class="box-content">Документ содержащий описания и подтверждения тех или иных фактов или событий</p>
                            </div>
                        </a>
                    </div>
                </div>
            </div>
        </div>
        <div class="grid-30" style="padding-left: 8px;">
            <h3 title=""><span class="fa fa-history"></span>&nbsp;Журнал событий</h3>
            <div class="tasks-panel-content">
                <asp:UpdatePanel ID="UpdatePanelInfo" runat="server">
                    <ContentTemplate>
                        <div class="tasks-panel" style="border: 1px solid red;" id="tasks_panel" runat="server"></div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="TimerAjax" EventName="Tick" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <script type="text/javascript" src="js/core.js"></script>
    <script type="text/javascript">
        PanelFadeInitialize();
    </script>
    <!-- Core.js 272 строка закомментирована. File Upload
    <script type="text/javascript" src="js/jquery/jquery.uploadify.js" ></script>-->
</asp:Content>