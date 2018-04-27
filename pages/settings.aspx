<%@ Page Language="C#" AutoEventWireup="true" CodeFile="settings.aspx.cs" Inherits="pages_settings" MasterPageFile="~/MasterPage.master" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
    <title>Карты - Краулер</title>
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
            Настройки<small class="on-right">свойства системы</small>
        </h1>
        <p class="description slider_" style="position: absolute; top: 90px; left: 20px; opacity: 0;">
            Страница упраления системой.
        </p>
    </div>
    <hr />
    <div class="grid" runat="server" id="grid"></div>
        <h1><small class="on-left" style="color:#16499a">Общие</small></h1>
        <div style="font-size:16px;">
            <div style="float:left; height:41px;line-height:30px;">Обновлять контент автоматически</div>
            <div style="float:left; margin-left:20px;">
                <div class="input-control switch">
                    <label>
                        <input type="checkbox" checked="checked"/>
                        <span class="check"></span>
                    </label>
                </div>
            </div>
        </div>
        <br />
        <br /><hr />
        <div style="font-size:16px;">
            <div style="float:left; height:41px;line-height:30px;">Период обновления в сек</div>
            <div class="input-control select">
                <select>
                    <option>5</option>
                    <option>10</option>
                    <option>15</option>
                    <option>30</option>
                    <option>45</option>
                    <option>60</option>
                </select>
            </div>
        </div>
        <hr />
        <div style="font-size:16px;">
            <div style="float:left; height:41px;line-height:30px;">Маскимальное количество клиентов</div>
            <div class="input-control select">
                <select>
                    <option>Без ограничений</option>
                    <option>100</option>
                    <option>150</option>
                    <option>300</option>
                    <option>500</option>
                    <option>1000</option>
                </select>
            </div>
        </div>
        <hr />
        <input class="button primary large" style="width:150px; height: 40px" value="Сохранить"></input>
    </div>

    <script type="text/javascript" src="../js/core.js" ></script>
    <script type="text/javascript" charset="utf-8">
        Win8_effectHeader();
    </script>
</asp:Content>
