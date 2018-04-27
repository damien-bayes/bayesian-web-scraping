<%@ Control Language="C#" AutoEventWireup="true" CodeFile="parsing_item.ascx.cs" Inherits="Controls_parsing_item" %>

<div class="grid-100 grid-parent">
    <div class="row">
        <ul class="vol-tile list-unstyled">
            <li class="vol-ruler">
                <span class="fa fa-check"></span>
                <div class="vol-info">
                    <p class="vol-tile-name"><%= area_name %></p>
                    <span class="vol-tile-status"><i class="fa fa-tasks"></i>&nbsp;<%= _state_ %></span>
                    <span class="vol-tile-status"><i class="fa fa-clock-o"></i>&nbsp;<%= _time %></span>
                </div>
            </li>
        </ul>
    </div>
</div>
