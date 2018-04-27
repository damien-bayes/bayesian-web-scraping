<%@ Control Language="C#" AutoEventWireup="true" CodeFile="task_item.ascx.cs" Inherits="Controls_task_item" %>
<a class='list marked' href='#' >
    <div class='list-content' style = '<%=this.style_ %>'>
        <span class='list-title' style = 'font-family: Roboto Regular; font-size:14px; '><%=this.state %> <%=this.title %></span>
        <span class='list-subtitle' style = 'font-family: RobotoCondensed Regular; font-size:11px; font-weight: normal'><%=this.day %><span class='place-right' id= '<%=this.id_item %>' onclick = 'javascript: changetime(this);'><%=this.time %></span></span>
        <span class='list-remark'>Выполненно: <%=this.countAll %></span>
    </div>
</a>