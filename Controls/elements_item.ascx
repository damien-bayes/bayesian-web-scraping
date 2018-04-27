<%@ Control Language="C#" AutoEventWireup="true" CodeFile="elements_item.ascx.cs" Inherits="Controls_elements_item" %>
<div style="border: 1px solid #D8D8D8;border-radius: 3px; background-color: white; margin-right: 15px; overflow: hidden; margin-bottom: 15px" id="<%=this.ID_ %>_element">
    <div style="padding: 5px;">
        <div style ="border-bottom: 1px solid #e5e5e5; position: relative; padding: 0 0 5px 0; overflow: hidden; padding-right: 0; text-overflow: ellipsis; white-space: nowrap;">
            <strong style="font-family: Roboto Regular;"><%=this.nameElement %></strong>
            <span style="float:right; height: 16px; width: 16px; margin-top: 2px; color: red; cursor: pointer" class="icon-remove" remove="<%=this.ID_ %>" onclick="removeinten(this)"></span>
        </div>
        <!-- Progress -->
        <div style="position:relative; max-height: 130px; margin-bottom: 2px; overflow: hidden; ">
            <img src ="<%=this.Src_ %>" style="z-index: 900"/>
            <div style="background-image: url('images/embeds_article_v2-aa76e8105f4ad58f58b33da824ba2dc0.png'); background-repeat: repeat-x; width: 100%; height: 16px; position:absolute; top: 114px; z-index: 1000">&nbsp</div>
        </div>
    </div>
    <div style="border-style: solid; border-width: 1px 0px 0px 0px; border-color: #E5E5E5; background-color: #F5F5F5; padding: 10px;">
        <span style="font-family: Roboto Regular;"><%=this.Description %></span>
    </div>
</div>