<%@ Control Language="C#" AutoEventWireup="true" CodeFile="dropdown.ascx.cs" Inherits="Controls_dropdown" %>
<div>
    <div style="border: 1px solid #D9D9D9; display: block; overflow: hidden; text-align: left; white-space: nowrap; height: 30px; font-family: RobotoCondensed Regular; margin-bottom: 7px;" class="mm_menu">
        <input type="text" class="cui-cb-input" autocomplete="off" id="fontName" title="Карта поиска" readonly="true" style="width: 90%; border-style:none; margin: 5px; margin-left: 8px; margin-right: 8px; margin-top: 0px;" value="Значение по умолчанию" runat="server"/>
        <span class ="down_menu icon-arrow-down-4">
            &nbsp;
        </span>
        <div style="visibility:hidden; position: absolute; top: 150px; left: 150px; padding-top: 10px; padding-bottom:10px; border: 1px solid rgba(0,0,0,.2); background: #fff; box-shadow: 0 2px 4px rgba(0,0,0,0.2);" runat="server" id="MMMM" class="content_menu">
                                
        </div>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {

        $(".down_menu").next('div').hide();

        $('.content_menu').hover(function () {

        }, function () {
            $.data(this, 'timer', setTimeout($.proxy(function () {
                $(".content_menu").stop(true, true).slideUp(200);
            }, this), 200));
        });

    });

    $(".down_menu").click(function () {
        var width = $(this).closest("div").width();
        var offset = $(this).closest("div").offset();
        $(this).next().css('min-width', width);
        $(this).next().css('top', offset.top + $(this).width() + 12 + 'px');
        $(this).next().css('left', offset.left + 'px');
        $(this).next().css('visibility', 'visible');
        $(this).next().css('z-index', '99999');
        $(this).next().slideDown(100);
    });

    $(".item_hover").click(function () {
        var ww = $(this).parent("div").height() / 2;
        var ob = $(this).parent("div");
        if (ww > 50)
            ww = 50;

        $(this).parent("div").slideUp(100);

        var key = $(this).attr('attr-key');
        var value = $(this).attr('attr-value');

        $(this).parent("div").prevAll('.cui-cb-input').val($(this).text());

        location.href = '?' + key + "=" + value;
    });
</script>