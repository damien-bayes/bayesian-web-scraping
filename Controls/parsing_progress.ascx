<%@ Control Language="C#" AutoEventWireup="true" CodeFile="parsing_progress.ascx.cs" Inherits="Controls_parsing_progress" %>
<a class="list marked" href="#" style="border-left-width: 0px;">
<table style ="width:100%; background-color:transparent;"> 
<tr>
<td style="width: 42px;">
<img style="width: 32px; height: 32px; margin-left: 10px; " src="images/db_parsingLoader.gif" />
</td>
<td>
<div class="list-content" style="border-left: 0px transparent solid !important">
<span class="list-title"><span class="place-right" style="color: black; cursor: pointer; font-size: 10pt;" id="progress_" runat="server"><%=this.progress %>%</span><%=this.area_name %></span>
<span class="list-subtitle">
<div style ="width: 100%; background-color: #eeeeee; height: 5px; position: relative; display: block; margin-bottom: 5px; margin-top: 5px;">
<div style="width:<%=this.progress %>%; height:5px;" class="bg-cyan">&nbsp</div>
</div>
</span>
<span class="list-remark">В работе<span class="place-right"><%=this.url_now %></span></span>
</div></td></tr></table></a>