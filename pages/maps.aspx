<%@ Page Language="C#" AutoEventWireup="true" CodeFile="maps.aspx.cs" Inherits="pages_maps" Title="Cube Crawler: Карты" MasterPageFile="~/MasterPage.master" %>
<%@ Register TagPrefix="lsti" TagName="lsti" Src="../Controls/listitem.ascx" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <meta name="description" 
          content="" />

    <meta name="keywords" 
          content="" />

    <link rel="stylesheet" type="text/css" href="/css/cube-crawler-supervisor-css/pages/maps-page.css"/>
</asp:Content>

<asp:Content ContentPlaceHolderID="MP" runat="server">

    <div class="grid-100 grid-parent">
        <div class="grid-40 shadow-z-1">
            <h3 title=""><span class="fa fa-file"></span>&nbsp;Укажите доступный URL адрес</h3>
            <input type ="text" 
                   runat ="server" 
                   id="addressInput" 
                   class="form-control floating-label" 
                   required="required" 
                   placeholder="Введите URL адрес источника"/>
        </div>

        <div class="grid-60">
            <h3 title=""><span class="fa fa-list"></span>&nbsp;Полный список карт</h3>

            <div runat="server" id="P_maplist" style="overflow-y: auto; min-height: 300px; ">
            </div>
        </div>
    </div>

    
<script type="text/javascript" src="../js/core.js" ></script>
<!--<script type="text/javascript" src="../js/jquery.uploadify.js" ></script>-->
<script type="text/javascript" charset="utf-8">
    Win8_effectHeader();
    TileEffects();


</script>

</asp:Content>