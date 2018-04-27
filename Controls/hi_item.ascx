<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeFile="hi_item.ascx.cs" 
            Inherits="Controls_hi_item" %>
    <section class="progress">
  <div class="progress-radial progress-95 setsize">
    <div class="overlay setsize">
      <p>95</p>
    </div>
  </div>
  <div class="clear"></div>
</section>
<div class="grid-100 grid-parent">
    <div class="row">
        <ul class="vol-tile list-unstyled">
            <li class="vol-ruler">
                <div style="border: 1px solid blue;">
                    <span class="fa fa-check"></span>
                </div>
                <div class="vol-info">
                    <p class="vol-tile-name"><%= AreaName %></p>
                    <span class="vol-tile-status"><i class="fa fa-tasks"></i>&nbsp;<%= _statusOutput %></span>
                    <span class="vol-tile-status"><i class="fa fa-clock-o"></i>&nbsp;<%= _timeOutput %></span>
                </div>
            </li>
        </ul>
    </div>
</div>