<%@ Control Language="C#" AutoEventWireup="true" CodeFile="listitem.ascx.cs" Inherits="Controls_listitem" %>


<div class="row listelement" style="margin-top:0px;padding-left:25px; <%=this.ColorBorder%>">
    <div class="span7">
    <a href ="#"  runat="server" id="P_item">
        <div class="row" style="margin-top: 0px; color: black;">
            <a runat="server" id="P_head" style="font-size: 14px; color: #555555 !important; font-family: Roboto Regular">Список существующих карт</a>
        </div>
        <div class="row" style="margin-top: 0px;">
            <span style="color: #0067cb; font-size:11px; font-family: RobotoCondensed Regular"  runat="server" id="P_decription">Описание</span>
        </div>
    </a>
    </div>
    <div class="span1" style="width: 0px !important; margin-left: 6px;">
        
        <a class="dropdown-toggle" style="color: black;">
            <div class="icon-button blue" style="width: 30px; height: 30px; ">
                <core-icon icon="more-horiz" aria-label="more-horiz" role="img" style="height: 18px; width: 18px; margin: 7px;"><svg viewBox="0 0 24 24" height="100%" width="100%" preserveAspectRatio="xMidYMid meet" style="pointer-events: none; display: block;" fit=""><g><path d="M6 10c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2zm12 0c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2zm-6 0c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2z"></path></g></svg></core-icon>
                <paper-ripple class="circle recenteringTouch" fit=""></paper-ripple>
            </div>
        </a>
        <ul class="dropdown-menu" data-role="dropdown" style="box-shadow: 0 0 6px rgba(0,0,0,.16),0 6px 12px rgba(0,0,0,.32); border-radius: 2px; border-width: 0px;">
            <li style = "border-width: 0px; border-bottom: 1px solid #e5e5e5; background-color: white;">
                <div style = "margin: 10px; margin-left: 30px;">
                Выполнить с...
                </div>
            </li>
            <li>
                <a class="item changeelement" style="height: 30px;" runat ="server" id="edit">
                    <div class="label" fit="" style ="height: 30px; padding-top: 7px !important; margin-left: 27px; background: none;">
                        <span class="icon-pencil fg-emerald" style="margin-right: 15px;"></span>Изменить
                    </div>
                    <paper-ripple fit=""></paper-ripple>
                </a>
            </li>
            <li>
                <a class="item deleteelement" style="height: 30px;" runat ="server" id="delete">
                    <div class="label" fit="" style ="height: 30px; padding-top: 7px !important; margin-left: 27px; background: none;">
                        <span class="icon-remove fg-orange" style="margin-right: 15px;"></span>Удалить
                    </div>
                    <paper-ripple fit=""></paper-ripple>
                </a>
            </li>
        </ul>
    </div>
</div>
<div style="border-top:dotted 1px #b4b4b4; margin-top:4px;margin-bottom:4px;"></div>