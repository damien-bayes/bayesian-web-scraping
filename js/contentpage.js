var index = 1;
var element_back = null;
var move = true;
var ttt = 1;

$(document).ready(function () {
    contentClass.init();
});

var contentClass = 
{
    init: function () {
        $("*").not(".exr").on('click', function (e) {

            if ($(this).css("border") != '1px dashed rgb(58, 75, 112)') {
                $(this).css("border", "1px dashed #3A4B70");
                try {

                    var xpath = getElementXPath(this).replace('form[1]/div[2]/', '');

                    var load_element = $('#load_element', window.parent.document)
                    if (load_element != null) {
                        try {
                            load_element.css('visibility', 'visible');
                        }
                        catch (err) { }
                    }

                    var el_ = xpath.split('/');
                    var end_el = el_[el_.length - 1];
                    var isHref = false;
                    if (end_el.length > 1) {
                        if (end_el.indexOf('a[') != -1) {
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

            if (element_back != null) {
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
    },
}

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
        $("#tagnameElementId").text(($(element_back).attr('id') != undefined ? "[" + $(element_back).attr('id') + "]" : ''));
        $("#tagName").css('top', ($(element_back).offset().top - 42 > 0 ? $(element_back).offset().top - 42 : 0) + 'px');
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
            if (arr[i].name == "class") {
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

function reblock(type) {
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