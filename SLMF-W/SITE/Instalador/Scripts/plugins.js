(function() {
    var method;
    var noop = function() {};
    var methods = [
        'assert', 'clear', 'count', 'debug', 'dir', 'dirxml', 'error',
        'exception', 'group', 'groupCollapsed', 'groupEnd', 'info', 'log',
        'markTimeline', 'profile', 'profileEnd', 'table', 'time', 'timeEnd',
        'timeStamp', 'trace', 'warn'
    ];
    var length = methods.length;
    var console = (window.console = window.console || {});

    while (length--) {
        method = methods[length];

        // Only stub undefined methods.
        if (!console[method]) {
            console[method] = noop;
        }
    }
}());

/*
 *
 * Verifica si hay algo en el selector
 *
 **/
$.fn.exists = function() { return this.length !== 0; }
/**
 *
 * Verifica si existe la funcion, sino la crea (solo existe en firefox)
 *
 **/
if (!('contains' in String.prototype)) {
    String.prototype.contains = function(str, startIndex) {
        return -1 !== String.prototype.indexOf.call(this, str, startIndex);
    };
};

String.prototype.contains = function(it) {
    return this.indexOf(it) != -1;
};

String.prototype.trunc = function(n, useWordBoundary) {
    var toLong = this.length > n,
        s_ = toLong ? this.substr(0, n - 1) : this;
    s_ = useWordBoundary && toLong ? s_.substr(0, s_.lastIndexOf(' ')) : s_;
    return toLong ? s_ + ' ...' : s_;
};

/*
 *
 * Find character and wrap the word(s) containing it
 *
 * @params character        - string
 * @params tag              - string, nombre de etiqueta html
 *
 **/
String.prototype.wrapp = function(character, tag) {
    var text = "",
        split = this.split(" ");
    for (var i = 0; i <= split.length - 1; i++) {
        if (split[i].contains(character)) {
            split[i] = "<" + tag + ">" + split[i] + "</" + tag + ">";
        };
        text = text + split[i] + " ";
    };
    return text;
};
$.fn.btBack = function() {
    TweenLite.set($(this).find("span.arrow-content"), {
        x: 18
    });
    $(this)
        .hover(function() {
            TweenLite.to($(this).find("span.arrow-content"), 0.15, {
                x: 0
            });

        }, function() {
            TweenLite.to($(this).find("span.arrow-content"), 0.15, {
                x: 18
            });
        });
};
$.fn.btNext = function() {
    TweenLite.set($(this).find("span.arrow-content"), {
        x: -18
    });
    $(this)
        .hover(function() {
            TweenLite.to($(this).find("span.arrow-content"), 0.15, {
                x: 0
            });

        }, function() {
            TweenLite.to($(this).find("span.arrow-content"), 0.15, {
                x: -18
            });
        });
};

var _onLoadSvgs = function(svgs, onComplete) {

    var count = 0;
    $.each(svgs, function(index, el) {

        el.on("load", function() {

            count = count + 1;
            if (count >= svgs.length) {

                onComplete();
            };
        });
    });
};

/**
 *
 * Loader de imágenes
 *
 * _onLoadImages( { 'img/image-name-1.jpg', 'img/image-name-2.jpg' }, function(){ } );
 **/

var _onLoadImages = function(images, onComplete) {

    var count = 0,
        loaderContent = $(".loader2 span"),
        counter = {
            var: 0
        };
    $(".loader2 span").css({
        opacity: 0.1
    });
    // console.log(images);
    $.each(images, function(index, src) {

        var img = new Image();
        img.src = src;
        
        img.onload = function() {

            count = count + 1;
            // console.log("percent loaded", images.length, (count / images.length) * 99)

            TweenMax.to(counter, .50, {
                var: Math.round((count / images.length) * 99),
                onUpdate: function() {
                    // console.log(Math.round(counter.var), Math.round((count / images.length) * 99));
                    loaderContent.html(Math.round(counter.var))
                    TweenLite.to(loaderContent, 0.25, {
                        opacity: counter.var / 100
                    })
                },
                ease: Circ.easeOut
            });

            if (count >= images.length) {
                setTimeout(function() {

                    onComplete();
                    return true;
                }, 350);
            };
        };
    });
    return true;
};

var BrowserDetect = {
    init: function() {
        this.browser = this.searchString(this.dataBrowser) || "Other";
        this.version = this.searchVersion(navigator.userAgent) || this.searchVersion(navigator.appVersion) || "Unknown";
    },

    searchString: function(data) {
        for (var i = 0; i < data.length; i++) {
            var dataString = data[i].string;
            this.versionSearchString = data[i].subString;

            if (dataString.indexOf(data[i].subString) != -1) {
                return data[i].identity;
            }
        }
    },

    searchVersion: function(dataString) {
        var index = dataString.indexOf(this.versionSearchString);
        if (index == -1) return;
        return parseFloat(dataString.substring(index + this.versionSearchString.length + 1));
    },

    dataBrowser: [{
        string: navigator.userAgent,
        subString: "Chrome",
        identity: "Chrome"
    }, {
        string: navigator.userAgent,
        subString: "MSIE",
        identity: "Explorer"
    }, {
        string: navigator.userAgent,
        subString: "Firefox",
        identity: "Firefox"
    }, {
        string: navigator.userAgent,
        subString: "Safari",
        identity: "Safari"
    }, {
        string: navigator.userAgent,
        subString: "Opera",
        identity: "Opera"
    }]
};
BrowserDetect.init();
$("html").addClass(BrowserDetect.browser)

$.fn.loader = function() {

    $(this)
    .wrapInner('<div class="allLoader"><div class="hidden"></div><div class="loader"> <span></span> </div></div>');
};

$.fn.removeLoader = function(params) {
    
    params = params || {};
    params.onComplete = params.onComplete || function() {};

    params.hiddenContent = false;

    var el = $(this),
        loader = params.loader || el.find(".loader, .loader2").first(),
        hidden = el.find(".hidden").first(),
        allLoader = el.find(".allLoader").first();
    if (params.hiddenContent) {

        el.find(".hidden-content")
            .toggleClass("isHidden");

        el.find(".hidden-header")
            .toggleClass("isHidden");
    } else {

        el
            .find(".hidden-content")
            .contents()
            .unwrap()
            .unwrap();
    };

    TweenMax.to(loader, .5, {
        autoAlpha: 0,
        onComplete: function() {
            loader
                .html('')
                .remove();

            hidden
                .contents()
                .unwrap();

            allLoader
                .contents()
                .unwrap();

            setTimeout(function() {
                params.onComplete();
            }, 250);
        }
    });
};


// generateDetails('paper1', 25, '1', 'NIVEL');
// generateDetails('paper2', 75, '30', 'DÍAS');
_generateCircle = function(_holder, _percent, _textNumber, _textDescription, _circleSize) {
    // Creates canvas 320 × 200 at 10, 50
    if (_circleSize == 'dashboard-training') {
        var paper = Raphael(document.getElementById(_holder), 94, 94);
    } else {
        var paper = Raphael(document.getElementById(_holder), 62, 80);
    }



    paper.customAttributes.arc = function(xloc, yloc, value, total, R) {
        var alpha = 360 / total * value,
            a = (90 - alpha) * Math.PI / 180,
            x = xloc + R * Math.cos(a),
            y = yloc - R * Math.sin(a),
            path;
        if (total == value) {
            path = [
                ["M", xloc, yloc - R],
                ["A", R, R, 0, 1, 1, xloc - 0.01, yloc - R]
            ];
        } else {
            path = [
                ["M", xloc, yloc - R],
                ["A", R, R, 0, +(alpha > 180), 1, x, y]
            ];
        }
        return {
            path: path
        };
    };





    _circleSize = _circleSize || "train";
    var arc_0_0;
    var arc_0_1;
    var t0_0_y = 30;
    var t0_1_y = 39;
    var t0_0_x = 31;
    var t0_0_fontSize = 40;
    var arc_0_0_x = 31;
    var arc_0_1_y = 31;
    //make an arc at 50,50 with a radius of 30 that grows from 0 to 40 of 100 with a bounce
    switch (_circleSize) {
        case "train":

            arc_0_0 = paper.path().attr({
                "stroke": "white",
                "stroke-width": 1.7,
                arc: [arc_0_0_x, arc_0_1_y, 100, 100, 30]
            });
            arc_0_1 = paper.path().attr({
                "stroke": "#222324",
                "stroke-width": 1.7,
                arc: [arc_0_0_x, arc_0_1_y, _percent, 100, 30]
            });
            if (BrowserDetect.browser === 'Firefox') {
                t0_0_y = 46;
                t0_1_y = 78;
            };
            break;
        case "dashboard-info":

            arc_0_0 = paper.path().attr({
                "stroke": "#222224",
                "stroke-width": 1.7,
                arc: [arc_0_0_x, arc_0_1_y, 100, 100, 30]
            });
            arc_0_1 = paper.path().attr({
                "stroke": "#f6d641",
                "stroke-width": 1.7,
                arc: [arc_0_0_x, arc_0_1_y, _percent, 100, 30]
            });
            t0_1_y = 40;
            if (BrowserDetect.browser === 'Firefox') {
                t0_0_y = 30;
                t0_1_y = 80;
            };
            break;

        case "dashboard-training":
            arc_0_0_x = 46;
            arc_0_1_y = 45;
            t0_0_x = 46
            t0_0_y = 32;
            t0_0_fontSize = 52;
            if (BrowserDetect.browser === 'Firefox') {
                t0_0_y = 63;
                arc_0_0_x = 46;
                arc_0_1_y = 46;
            };

            arc_0_0 = paper.path().attr({
                "stroke": "white",
                "stroke-width": 1.7,
                arc: [arc_0_0_x, arc_0_1_y, 100, 100, 45]
            });
            arc_0_1 = paper.path().attr({
                "stroke": "#222324",
                "stroke-width": 1.7,
                arc: [arc_0_0_x, arc_0_1_y, _percent, 100, 45]
            });


            break;
    };


    // var t0_0 = paper.text(t0_0_x, t0_0_y, _textNumber);
    // t0_0.attr({
    //     fill: '#fff',
    //     "font-size": t0_0_fontSize,
    //     "font-family": '"Tungsten A", "Tungsten B"'
    // });

    var t0_1 = paper.text(t0_0_x, t0_1_y, _textDescription);
    t0_1.attr({
        fill: '#383838',
        "font-size": 11,
        "font-family": '"PlexesMediumPro"'
    });
};

$.fn.refresh = function() {
    return $(this.selector);
};


$.fn.bindToWindowSize = function(viewPort, params) {
    return this.each(function(idx, el) {
        var self = $(el);
        params = params || {}
        //console.log(el, attr);
        var vP = viewPort;
        var att = params.attr;
        var multiplier = params.multiplier || 1;
        var isPercent = (params.min && params.min.contains("%")) && (params.max && params.max.contains("%")) || false;
        var min = parseInt(params.min) || false;
        var max = parseInt(params.max) || false;

        var minWidth = parseInt($('body').css('min-width')) || 1024;
        var minHeight = parseInt($('body').css('min-height')) || 600;

        var ratio = 0;
        if (vP == 'width') {
            ratio = parseInt(self.css(att)) / minWidth;
        } else {
            ratio = parseInt(self.css(att)) / minHeight;
        }

        // console.log("RATIO:", ratio, "MINWIDTH:", minWidth, "MINHEIGHT", minHeight, "ATTRIBUTE", self.css(att));

        $(window).on('resize.bindToWindowSize', function() {
            var viewSize = Math.max(vP == 'width' ? minWidth : minHeight,
                vP == 'width' ? $(window).width() : $(window).height());
            var size = Math.round(ratio * viewSize);
            // console.log(size);
            if (size < min) size = min;
            if (size > max) size = max;
            self.css(att, size * multiplier);
        });
    });
};

$.fn.set = function(params) {

    this.css(params);
};

$.fn.centerToWindowSize = function(viewPort) {
    return this.each(function(idx, el) {
        var self = $(el);
        var vP = viewPort;
        $(window).on('resize.centerToWindowSize', function() {
            var w = $(window).width();
            var h = $(window).height();
            var at = vP == 'width' ? 'margin-left' : 'margin-top';
            var max = 0;

            self.children("*").each(function(idx, el) {
                max = vP == 'width' ? Math.max(max, $(el).width()) : Math.max(max, $(el).height());
            });
            // console.log("MAX:", max);
            var margin = vP == 'width' ? w / 2 - max / 2 : h / 2 - max / 2;
            self.css(at, margin);
        });
    });
};