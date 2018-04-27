<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Awesomium.aspx.cs" Inherits="Bin_Ajax_Awesomium" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script type="text/javascript" src="http://10.10.9.132:777/js/jquery-1.10.2.js" ></script>
    <script type="text/javascript" src="http://10.10.9.132:777/js/bootstrap.js" ></script>
    <script type="text/javascript" src="http://10.10.9.132:777/js/canvas.js" ></script>
    <link rel="stylesheet" href="http://10.10.9.132:777/css/smoothness/jquery-ui-1.10.4.custom.css" />
    <style type="text/css">
        .exr
        {
            font-size:13px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div runat="server" id="panelAwesomium">
       
    </div>
    </form>
    <div id="informer-top" style="border-top: 1px dashed #333333; position:absolute; width:0px; height:1px; top:0px; left:0px;"></div>
    <div id="informer-bottom" style="border-top: 1px dashed #333333; position:absolute; width:0px; height:1px; top:0px; left:0px;"></div>

    <div id="informer-left" style="border-left: 1px dashed #333333; position:absolute; width:1px; height:0px; top:0px; left:0px;"></div>
    <div id="informer-right" style="border-right: 1px dashed #333333; position:absolute; width:1px; height:0px; top:0px; left:0px;"></div>
    <div id="tagName" class="tooltip exr" style="position:absolute; z-index:9999999; visibility:hidden;"><div class="tooltip-arrow"></div>
        <div class="tooltip-inner exr" style="background-color: #2C3742; padding:5px; border-radius: 3px; box-shadow: 0 0 20px rgba(0,0,0,0.5); line-height:18px;">
            <span style="border-width: 1px; border-color: #666666; margin-right:5px; display: none; border-right-style: solid; padding-right: 3px;" id="restart" class="exr"><img id="resetimage" class="exr" src="../images/none_active_arrow_out.png" style="float:left;" /></span>
            <span id="tagnameElement" style="color: white" class="exr">div</span>
            <span id="tagnameElementId" style="color: #ED4DFF;" class="exr">pagenator</span>
            <span id="tagnameElementClass" style="color: #4CC3FF; margin-left:2px;" class="exr">pagenator</span></div>
        <img src="http://10.10.9.132:777/images/dark_blue_arrow_example1.png" style="position:relative; top:-5px; left: 10px;"  class="exr"/>
    </div>
</body>
</html>

<script>
    parent.setheigth($(document).height());
    
    var index = 1;
    var element_back = null;
    var move = true;
    var ttt = 1;

    $("a").attr('href', '');
    $("a").attr('target', '');
    $("*").css('cursor', 'pointer');
    
    $("*").not(".exr").on('click', function (e) {

        if ($(this).css("border") != '1px dashed rgb(58, 75, 112)') {
            $(this).css("border", "1px dashed #3A4B70");
            try
            {
                
                var xpath = getElementXPath(this).replace('form[1]/div[2]/', '');

                var load_element = $('#load_element', window.parent.document)
                if (load_element != null)
                {
                    try{
                        load_element.css('visibility', 'visible');
                    }
                    catch(err){}
                }

                var el_ = xpath.split('/');
                var end_el = el_[el_.length - 1];
                var isHref = false;
                if (end_el.length > 1)
                {
                    if (end_el.indexOf('a[') != -1)
                    {
                        isHref = true;
                    }
                }
                else {
                    isHref = end_el == 'a';
                }
                if (!isHref) {
                    parent.selectelemt(null, xpath);
                }
                else {
                    parent.selectelemt(getAllAttributes(this, true), xpath);
                }

                /*var innerHtml = $(this).html();
                $(this).html("<canvas id='graph' style='width:300px; height: 200px; position: absolute;'></canvas>" + innerHtml);

                var canvas = document.getElementById('graph');
                var ctx = canvas.getContext('2d');

                var doc = document.implementation.createHTMLDocument("");
                doc.write(innerHtml);
                doc.documentElement.setAttribute("xmlns", doc.documentElement.namespaceURI);
                html = (new XMLSerializer).serializeToString(doc);
                
                var data = '<svg xmlns="http://www.w3.org/2000/svg" width="200" height="200">' +
                           '<foreignObject width="100%" height="100%">'
                           + html +
                           '</foreignObject>' +
                           '</svg>';

                var DOMURL = window.URL || window.webkitURL || window;

                var img = new Image();
                var svg = new Blob([data], { type: 'image/svg+xml;charset=utf-8' });
                var url = DOMURL.createObjectURL(svg);

                img.onload = function () {
                    ctx.drawImage(img, 0, 0);
                    DOMURL.revokeObjectURL(url);
                }

                img.src = url;

                saveCanvas();*/
            }
            catch (err) { }
            index++;
        }
        else {
            $(this).css("border", "");
            getAllAttributes(this, false)
        }

        if (move)
            $("#restart").css('display', 'inline');

        move = false;

        var informer_top = document.getElementById('informer-top');
        var informer_bottom = document.getElementById('informer-bottom');
        var informer_left = document.getElementById('informer-left');
        var informer_right = document.getElementById('informer-right');

        informer_top.style.visibility = 'hidden';
        informer_bottom.style.visibility = 'hidden';
        informer_left.style.visibility = 'hidden';
        informer_right.style.visibility = 'hidden';
        e.stopPropagation();

    });

    $("#restart").on('click', function (e) {

        if (element_back != null)
        {
            $(element_back).css("border", "");
        }
        move = true;
        $(this).css('display', 'none');
        e.stopPropagation();
    });

    $("*").on('mouseover', function (e) {

        if (element_back != null)
            $(element_back).stop();

        if (this.tagName.toLowerCase() == 'body' || this.tagName.toLowerCase() == 'html')
            return;

        if (move) {

            var element = this;
            element_back = element;
            var informer_top = document.getElementById('informer-top');
            var informer_bottom = document.getElementById('informer-bottom');
            var informer_left = document.getElementById('informer-left');
            var informer_right = document.getElementById('informer-right');
            
            if (element_back == element)
                setTimeout(second_passed, 200);

            e.stopPropagation();
        }
    });

    $("#resetimage").on('mouseover', function (e) {
        $(this).attr('src', 'http://localhost:777/images/active_arrow_in.png');
    });

    $("#resetimage").on('mouseleave', function (e) {
        $(this).attr('src', 'http://localhost:777/images/none_active_arrow_out.png');
    });

    function second_passed() {
        var informer_top = document.getElementById('informer-top');
        var informer_bottom = document.getElementById('informer-bottom');
        var informer_left = document.getElementById('informer-left');
        var informer_right = document.getElementById('informer-right');

        if (informer_top != null) {
            informer_top.style.visibility = 'visible';
            informer_bottom.style.visibility = 'visible';
            informer_left.style.visibility = 'visible';
            informer_right.style.visibility = 'visible';


            $("#tagName").css('visibility', 'visible');
            $("#tagnameElement").text(element_back.tagName);
            $("#tagnameElementClass").text("." + $(element_back).attr('class'));
            $("#tagnameElementId").text( ( $(element_back).attr('id') != undefined ? "[" + $(element_back).attr('id') + "]" : ''));
            $("#tagName").css('top', ($(element_back).offset().top - 42 > 0 ? $(element_back).offset().top - 42 : 0 ) + 'px');
            $("#tagName").css('left', $(element_back).offset().left + ($(element_back).width() / 2) - 20 + 'px');

            $(informer_top).css("left", $(element_back).offset().left - 2 + 'px');
            $(informer_top).css("width", $(element_back).width() + 4 + 'px');
            $(informer_top).css("top", $(element_back).offset().top - 2 + 'px');

            $(informer_bottom).css("left", $(element_back).offset().left - 2 + 'px');
            $(informer_bottom).css("width", $(element_back).width() + 4 + 'px');
            $(informer_bottom).css("top", $(element_back).height() + 4 + $(element_back).offset().top - 2 + 'px');

            $(informer_left).css("left", $(element_back).offset().left - 2 + 'px');
            $(informer_left).css("height", $(element_back).height() + 4 + 'px');
            $(informer_left).css("top", $(element_back).offset().top - 2 + 'px');

            $(informer_right).css("left", $(element_back).width() + 4 + $(element_back).offset().left - 2 + 'px');
            $(informer_right).css("height", $(element_back).height() + 4 + 'px');
            $(informer_right).css("top", $(element_back).offset().top - 2 + 'px');
        }
    }


    function getElementXPath(element) {
        if ($(element).attr("id") == null) {
            return "//" + $(element).parents().andSelf().map(function () {
                var $this = $(this);
                var tagName = this.nodeName;
                if ($this.siblings(tagName).length > 0) {
                    tagName += '[' + ($this.prevAll(tagName).length + 1) + ']';
                }
                else {
                    tagName += '[1]';
                }
                return tagName;
            }).get().join("/").toLowerCase();
        }
        else {
            
            return '//' + element.tagName + '[@id="' + $(element).attr("id") + '"]';
        }
    }

    function getAllAttributes(element, select) {
        var arr = element.attributes, attributes = [];
        for (var i = 0; i < arr.length; i++) {
            attributes.push(arr[i].name + ':' + $(element).attr(arr[i].name) + '; ');
            var split = $(element).attr(arr[i].name).split(' ');
            
            if (select == true) {
                if (arr[i].name == "class")
                {
                    //$("." + split[0]).css("border", "1px solid #3A4B70").not($(element));
                }
            }
            else {
                if (arr[i].name == "class") {
                    $("." + split[0]).css("border", "");
                }
            }
        }
        return attributes;
    }

    function reblock(type)
    {
        alert(type);
        move = true;
    }

    function getXPath(element) {
        var xpath = '';
        for (; element && element.nodeType == 1; element = element.parentNode) {
            var id = $(element.parentNode).children(element.tagName).index(element) + 1;
            id > 1 ? (id = '[' + id + ']') : (id = '');
            xpath = '/' + element.tagName.toLowerCase() + id + xpath;
        }
        return xpath;
    }

</script>
