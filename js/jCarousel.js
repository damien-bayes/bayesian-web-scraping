﻿/* jCarousel */
(function (e) { e.fn.jcarousel = function (t) { if (typeof t == "string") { var r = e(this).data("jcarousel"), i = Array.prototype.slice.call(arguments, 1); return r[t].apply(r, i) } else return this.each(function () { e(this).data("jcarousel", new n(this, t)) }) }; var t = { vertical: false, start: 1, offset: 1, size: null, scroll: 3, visible: null, animation: "normal", easing: "swing", auto: 0, wrap: null, initCallback: null, reloadCallback: null, itemLoadCallback: null, itemFirstInCallback: null, itemFirstOutCallback: null, itemLastInCallback: null, itemLastOutCallback: null, itemVisibleInCallback: null, itemVisibleOutCallback: null, buttonNextHTML: "<div></div>", buttonPrevHTML: "<div></div>", buttonNextEvent: "click", buttonPrevEvent: "click", buttonNextCallback: null, buttonPrevCallback: null }; e.jcarousel = function (n, r) { this.options = e.extend({}, t, r || {}); this.locked = false; this.container = null; this.clip = null; this.list = null; this.buttonNext = null; this.buttonPrev = null; this.wh = !this.options.vertical ? "width" : "height"; this.lt = !this.options.vertical ? "left" : "top"; var i = "", s = n.className.split(" "); for (var o = 0; o < s.length; o++) { if (s[o].indexOf("jcarousel-skin") != -1) { e(n).removeClass(s[o]); i = s[o]; break } } if (n.nodeName == "UL" || n.nodeName == "OL") { this.list = e(n); this.container = this.list.parent(); if (this.container.hasClass("jcarousel-clip")) { if (!this.container.parent().hasClass("jcarousel-container")) this.container = this.container.wrap("<div></div>"); this.container = this.container.parent() } else if (!this.container.hasClass("jcarousel-container")) this.container = this.list.wrap("<div></div>").parent() } else { this.container = e(n); this.list = this.container.find("ul,ol").eq(0) } if (i != "" && this.container.parent()[0].className.indexOf("jcarousel-skin") == -1) this.container.wrap('<div class=" ' + i + '"></div>'); this.clip = this.list.parent(); if (!this.clip.length || !this.clip.hasClass("jcarousel-clip")) this.clip = this.list.wrap("<div></div>").parent(); this.buttonNext = e(".jcarousel-next", this.container); if (this.buttonNext.size() == 0 && this.options.buttonNextHTML != null) this.buttonNext = this.clip.after(this.options.buttonNextHTML).next(); this.buttonNext.addClass(this.className("jcarousel-next")); this.buttonPrev = e(".jcarousel-prev", this.container); if (this.buttonPrev.size() == 0 && this.options.buttonPrevHTML != null) this.buttonPrev = this.clip.after(this.options.buttonPrevHTML).next(); this.buttonPrev.addClass(this.className("jcarousel-prev")); this.clip.addClass(this.className("jcarousel-clip")).css({ overflow: "hidden", position: "relative" }); this.list.addClass(this.className("jcarousel-list")).css({ overflow: "hidden", position: "relative", top: 0, left: 0, margin: 0, padding: 0 }); this.container.addClass(this.className("jcarousel-container")).css({ position: "relative" }); var u = this.options.visible != null ? Math.ceil(this.clipping() / this.options.visible) : null; var a = this.list.children("li"); var f = this; if (a.size() > 0) { var l = 0, o = this.options.offset; a.each(function () { f.format(this, o++); l += f.dimension(this, u) }); this.list.css(this.wh, l + "px"); if (!r || r.size === undefined) this.options.size = a.size() } this.container.css("display", "block"); this.buttonNext.css("display", "block"); this.buttonPrev.css("display", "block"); this.funcNext = function () { f.next() }; this.funcPrev = function () { f.prev() }; this.funcResize = function () { f.reload() }; if (this.options.initCallback != null) this.options.initCallback(this, "init"); try { if (e.browser.safari) { this.buttons(false, false); e(window).bind("load.jcarousel", function () { f.setup() }) } else this.setup() } catch (c) { this.setup() } }; var n = e.jcarousel; n.fn = n.prototype = { jcarousel: "0.2.4" }; n.fn.extend = n.extend = e.extend; n.fn.extend({ setup: function () { this.first = null; this.last = null; this.prevFirst = null; this.prevLast = null; this.animating = false; this.timer = null; this.tail = null; this.inTail = false; if (this.locked) return; this.list.css(this.lt, this.pos(this.options.offset) + "px"); var t = this.pos(this.options.start); this.prevFirst = this.prevLast = null; this.animate(t, false); e(window).unbind("resize.jcarousel", this.funcResize).bind("resize.jcarousel", this.funcResize) }, reset: function () { this.list.empty(); this.list.css(this.lt, "0px"); this.list.css(this.wh, "10px"); if (this.options.initCallback != null) this.options.initCallback(this, "reset"); this.setup() }, reload: function () { if (this.tail != null && this.inTail) this.list.css(this.lt, n.intval(this.list.css(this.lt)) + this.tail); this.tail = null; this.inTail = false; if (this.options.reloadCallback != null) this.options.reloadCallback(this); if (this.options.visible != null) { var t = this; var r = Math.ceil(this.clipping() / this.options.visible), i = 0, s = 0; e("li", this.list).each(function (e) { i += t.dimension(this, r); if (e + 1 < t.first) s = i }); this.list.css(this.wh, i + "px"); this.list.css(this.lt, -s + "px") } this.scroll(this.first, false) }, lock: function () { this.locked = true; this.buttons() }, unlock: function () { this.locked = false; this.buttons() }, size: function (e) { if (e != undefined) { this.options.size = e; if (!this.locked) this.buttons() } return this.options.size }, has: function (e, t) { if (t == undefined || !t) t = e; if (this.options.size !== null && t > this.options.size) t = this.options.size; for (var n = e; n <= t; n++) { var r = this.get(n); if (!r.length || r.hasClass("jcarousel-item-placeholder")) return false } return true }, get: function (t) { return e(".jcarousel-item-" + t, this.list) }, add: function (e, t) { var r = this.get(e), i = 0, s = 0; if (r.length == 0) { var o, r = this.create(e), u = n.intval(e); while (o = this.get(--u)) { if (u <= 0 || o.length) { u <= 0 ? this.list.prepend(r) : o.after(r); break } } } else i = this.dimension(r); r.removeClass(this.className("jcarousel-item-placeholder")); typeof t == "string" ? r.html(t) : r.empty().append(t); var a = this.options.visible != null ? Math.ceil(this.clipping() / this.options.visible) : null; var f = this.dimension(r, a) - i; if (e > 0 && e < this.first) this.list.css(this.lt, n.intval(this.list.css(this.lt)) - f + "px"); this.list.css(this.wh, n.intval(this.list.css(this.wh)) + f + "px"); return r }, remove: function (e) { var t = this.get(e); if (!t.length || e >= this.first && e <= this.last) return; var r = this.dimension(t); if (e < this.first) this.list.css(this.lt, n.intval(this.list.css(this.lt)) + r + "px"); t.remove(); this.list.css(this.wh, n.intval(this.list.css(this.wh)) - r + "px") }, next: function () { this.stopAuto(); if (this.tail != null && !this.inTail) this.scrollTail(false); else this.scroll((this.options.wrap == "both" || this.options.wrap == "last") && this.options.size != null && this.last == this.options.size ? 1 : this.first + this.options.scroll) }, prev: function () { this.stopAuto(); if (this.tail != null && this.inTail) this.scrollTail(true); else this.scroll((this.options.wrap == "both" || this.options.wrap == "first") && this.options.size != null && this.first == 1 ? this.options.size : this.first - this.options.scroll) }, scrollTail: function (e) { if (this.locked || this.animating || !this.tail) return; var t = n.intval(this.list.css(this.lt)); !e ? t -= this.tail : t += this.tail; this.inTail = !e; this.prevFirst = this.first; this.prevLast = this.last; this.animate(t) }, scroll: function (e, t) { if (this.locked || this.animating) return; this.animate(this.pos(e), t) }, pos: function (e) { var t = n.intval(this.list.css(this.lt)); if (this.locked || this.animating) return t; if (this.options.wrap != "circular") e = e < 1 ? 1 : this.options.size && e > this.options.size ? this.options.size : e; var r = this.first > e; var i = this.options.wrap != "circular" && this.first <= 1 ? 1 : this.first; var s = r ? this.get(i) : this.get(this.last); var o = r ? i : i - 1; var u = null, a = 0, f = false, l = 0, c; while (r ? --o >= e : ++o < e) { u = this.get(o); f = !u.length; if (u.length == 0) { u = this.create(o).addClass(this.className("jcarousel-item-placeholder")); s[r ? "before" : "after"](u); if (this.first != null && this.options.wrap == "circular" && this.options.size !== null && (o <= 0 || o > this.options.size)) { c = this.get(this.index(o)); if (c.length) this.add(o, c.children().clone(true)) } } s = u; l = this.dimension(u); if (f) a += l; if (this.first != null && (this.options.wrap == "circular" || o >= 1 && (this.options.size == null || o <= this.options.size))) t = r ? t + l : t - l } var h = this.clipping(); var p = []; var d = 0, o = e, v = 0; var s = this.get(e - 1); while (++d) { u = this.get(o); f = !u.length; if (u.length == 0) { u = this.create(o).addClass(this.className("jcarousel-item-placeholder")); s.length == 0 ? this.list.prepend(u) : s[r ? "before" : "after"](u); if (this.first != null && this.options.wrap == "circular" && this.options.size !== null && (o <= 0 || o > this.options.size)) { c = this.get(this.index(o)); if (c.length) this.add(o, c.find(">*").clone(true)) } } s = u; var l = this.dimension(u); if (l == 0) { /*alert("jCarousel: No width/height set for items. This will cause an infinite loop. Aborting...");*/ return 0 } if (this.options.wrap != "circular" && this.options.size !== null && o > this.options.size) p.push(u); else if (f) a += l; v += l; if (v >= h) break; o++ } for (var m = 0; m < p.length; m++) p[m].remove(); if (a > 0) { this.list.css(this.wh, this.dimension(this.list) + a + "px"); if (r) { t -= a; this.list.css(this.lt, n.intval(this.list.css(this.lt)) - a + "px") } } var g = e + d - 1; if (this.options.wrap != "circular" && this.options.size && g > this.options.size) g = this.options.size; if (o > g) { d = 0, o = g, v = 0; while (++d) { var u = this.get(o--); if (!u.length) break; v += this.dimension(u); if (v >= h) break } } var y = g - d + 1; if (this.options.wrap != "circular" && y < 1) y = 1; if (this.inTail && r) { t += this.tail; this.inTail = false } this.tail = null; if (this.options.wrap != "circular" && g == this.options.size && g - d + 1 >= 1) { var b = n.margin(this.get(g), !this.options.vertical ? "marginRight" : "marginBottom"); if (v - b > h) this.tail = v - h - b } while (e-- > y) t += this.dimension(this.get(e)); this.prevFirst = this.first; this.prevLast = this.last; this.first = y; this.last = g; return t }, animate: function (e, t) { if (this.locked || this.animating) return; this.animating = true; var n = this; var r = function () { n.animating = false; if (e == 0) n.list.css(n.lt, 0); if (n.options.wrap == "circular" || n.options.wrap == "both" || n.options.wrap == "last" || n.options.size == null || n.last < n.options.size) n.startAuto(); n.buttons(); n.notify("onAfterAnimation") }; this.notify("onBeforeAnimation"); if (!this.options.animation || t == false) { this.list.css(this.lt, e + "px"); r() } else { var i = !this.options.vertical ? { left: e } : { top: e }; this.list.animate(i, this.options.animation, this.options.easing, r) } }, startAuto: function (e) { if (e != undefined) this.options.auto = e; if (this.options.auto == 0) return this.stopAuto(); if (this.timer != null) return; var t = this; this.timer = setTimeout(function () { t.next() }, this.options.auto * 1e3) }, stopAuto: function () { if (this.timer == null) return; clearTimeout(this.timer); this.timer = null }, buttons: function (e, t) { if (e == undefined || e == null) { var e = !this.locked && this.options.size !== 0 && (this.options.wrap && this.options.wrap != "first" || this.options.size == null || this.last < this.options.size); if (!this.locked && (!this.options.wrap || this.options.wrap == "first") && this.options.size != null && this.last >= this.options.size) e = this.tail != null && !this.inTail } if (t == undefined || t == null) { var t = !this.locked && this.options.size !== 0 && (this.options.wrap && this.options.wrap != "last" || this.first > 1); if (!this.locked && (!this.options.wrap || this.options.wrap == "last") && this.options.size != null && this.first == 1) t = this.tail != null && this.inTail } var n = this; this.buttonNext[e ? "bind" : "unbind"](this.options.buttonNextEvent + ".jcarousel", this.funcNext)[e ? "removeClass" : "addClass"](this.className("jcarousel-next-disabled")).attr("disabled", e ? false : true); this.buttonPrev[t ? "bind" : "unbind"](this.options.buttonPrevEvent + ".jcarousel", this.funcPrev)[t ? "removeClass" : "addClass"](this.className("jcarousel-prev-disabled")).attr("disabled", t ? false : true); if (this.buttonNext.length > 0 && (this.buttonNext[0].jcarouselstate == undefined || this.buttonNext[0].jcarouselstate != e) && this.options.buttonNextCallback != null) { this.buttonNext.each(function () { n.options.buttonNextCallback(n, this, e) }); this.buttonNext[0].jcarouselstate = e } if (this.buttonPrev.length > 0 && (this.buttonPrev[0].jcarouselstate == undefined || this.buttonPrev[0].jcarouselstate != t) && this.options.buttonPrevCallback != null) { this.buttonPrev.each(function () { n.options.buttonPrevCallback(n, this, t) }); this.buttonPrev[0].jcarouselstate = t } }, notify: function (e) { var t = this.prevFirst == null ? "init" : this.prevFirst < this.first ? "next" : "prev"; this.callback("itemLoadCallback", e, t); if (this.prevFirst !== this.first) { this.callback("itemFirstInCallback", e, t, this.first); this.callback("itemFirstOutCallback", e, t, this.prevFirst) } if (this.prevLast !== this.last) { this.callback("itemLastInCallback", e, t, this.last); this.callback("itemLastOutCallback", e, t, this.prevLast) } this.callback("itemVisibleInCallback", e, t, this.first, this.last, this.prevFirst, this.prevLast); this.callback("itemVisibleOutCallback", e, t, this.prevFirst, this.prevLast, this.first, this.last) }, callback: function (t, n, r, i, s, o, u) { if (this.options[t] == undefined || typeof this.options[t] != "object" && n != "onAfterAnimation") return; var a = typeof this.options[t] == "object" ? this.options[t][n] : this.options[t]; if (!e.isFunction(a)) return; var f = this; if (i === undefined) a(f, r, n); else if (s === undefined) this.get(i).each(function () { a(f, this, i, r, n) }); else { for (var l = i; l <= s; l++) if (l !== null && !(l >= o && l <= u)) this.get(l).each(function () { a(f, this, l, r, n) }) } }, create: function (e) { return this.format("<li></li>", e) }, format: function (t, n) { var r = e(t).addClass(this.className("jcarousel-item")).addClass(this.className("jcarousel-item-" + n)).css({ "float": "left", "list-style": "none" }); r.attr("jcarouselindex", n); return r }, className: function (e) { return e + " " + e + (!this.options.vertical ? "-horizontal" : "-vertical") }, dimension: function (t, r) { var i = t.jquery != undefined ? t[0] : t; var s = !this.options.vertical ? i.offsetWidth + n.margin(i, "marginLeft") + n.margin(i, "marginRight") : i.offsetHeight + n.margin(i, "marginTop") + n.margin(i, "marginBottom"); if (r == undefined || s == r) return s; var o = !this.options.vertical ? r - n.margin(i, "marginLeft") - n.margin(i, "marginRight") : r - n.margin(i, "marginTop") - n.margin(i, "marginBottom"); e(i).css(this.wh, o + "px"); return this.dimension(i) }, clipping: function () { return !this.options.vertical ? this.clip[0].offsetWidth - n.intval(this.clip.css("borderLeftWidth")) - n.intval(this.clip.css("borderRightWidth")) : this.clip[0].offsetHeight - n.intval(this.clip.css("borderTopWidth")) - n.intval(this.clip.css("borderBottomWidth")) }, index: function (e, t) { if (t == undefined) t = this.options.size; return Math.round(((e - 1) / t - Math.floor((e - 1) / t)) * t) + 1 } }); n.extend({ defaults: function (n) { return e.extend(t, n || {}) }, margin: function (t, r) { if (!t) return 0; var i = t.jquery != undefined ? t[0] : t; try { if (r == "marginRight" && e.browser.safari) { var s = { display: "block", "float": "none", width: "auto" }, o, u; e.swap(i, s, function () { o = i.offsetWidth }); s["marginRight"] = 0; e.swap(i, s, function () { u = i.offsetWidth }); return u - o } } catch (a) { } return n.intval(e.css(i, r)) }, intval: function (e) { e = parseInt(e); return isNaN(e) ? 0 : e } }) })(jQuery);