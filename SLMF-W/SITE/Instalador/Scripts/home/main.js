// @codekit-prepend "../vendor/underscore-min.js"
// @codekit-prepend "nav-disciplines.js"
// @codekit-prepend "../home-discipline/nav-pilars.js"
Home = {

    init: function(images) {

        this.cachedElements();
        var navPilars = new NavPilars();
        var navDisciplines = new NavDisciplines();
        navDisciplines.enabled = false;

        var frame = 1 / 30;
        _onLoadImages(images, function() {

            $(".diagonal-1, .diagonal-2, .diagonal-3, .diagonal-4").css("display", "none");

            $(".home-bg img").each(function(idx, el) {
                $(el).attr("data-natural-width", el.width);
                $(el).attr("data-natural-height", el.height);
                //console.log($(el).attr("data-ml", el.style.marginLeft));
            });

            var resizeBackgrounds = _.debounce(function() {
                var w = Math.max(1024, $(window).width()),
                    h = Math.max(800, $(window).height()),
                    parentRatio = w / h;

                $(".home-bg .bg img").each(function(idx, el) {
                    var naturalWidth = $(el).attr("data-natural-width"),
                        naturalHeight = $(el).attr("data-natural-height"),
                        sizeMode = $(el).attr("data-size-mode"),
                        elWidth = $(el).width(),
                        elHeight = $(el).height(),
                        ratio = naturalWidth / naturalHeight;

                    //console.log(sizeMode);

                    //load low res if available    
                    if (w <= 1800) {
                        var lowResAvailable = $(el).attr("data-low-res") == "true";
                        var src = $(el).attr("src");

                        if (lowResAvailable && src.indexOf("-medium") == -1) {
                            $(el).attr("src", src.replace(".jpg", "-medium.jpg"));
                        }

                    }

                    if (sizeMode == "cover") {
                        //console.log(naturalWidth, naturalHeight, parentRatio, ratio);
                        if (w > h && parentRatio > ratio) {
                            $(el).css({
                                "width": w,
                                "height": Math.round(naturalHeight * (w / naturalWidth)),
                            });
                        } else {
                            $(el).css({
                                "width": Math.round(naturalWidth * (h / naturalHeight)),
                                "height": h,
                            });
                        }
                        // //margins to center
                        var ml = "";
                        var mt = "";

                        if (elHeight > h) {
                            mt = -(elHeight - h) / 2;
                        }

                        if (elWidth > (w - 95) && parentRatio < ratio) {
                            ml = ((w + 90) - elWidth) / 2 - 90;
                        }

                        $(el).css({
                            "margin-left": ml,
                            "margin-top": mt,
                        });
                    } else if (sizeMode == "contain") {
                        var ml = w < elWidth ? (w - elWidth) / 2 : 0;

                        $(el).css({
                            "height": h,
                            "width": Math.round(naturalWidth * (h / naturalHeight)),
                            "margin-left": ml + "px",
                        });
                    }

                    //console.log(parentRatio, ratio);
                });
            }, 10);
            $(window).resize(resizeBackgrounds).trigger("resize");
            var tlIntro = new TimelineLite();
            $Home.removeLoader({
                onComplete: function() {

                    tlIntro.to($(".home-bg"), .45, {
                        autoAlpha: 1,
                        ease: Linear.easeNone,
                    });
                    tlIntro.to($(".slmf .top .diagonal"), 0.85, {
                        top: 0,
                        left: 0,
                        autoAlpha: 1,
                        ease: Power2.easeInOut,
                        onComplete: function() {
                            $Home.find(".header-content .top .diagonal").css({
                                overflow: "visible"
                            })
                        }
                    });

                    tlIntro.to($(".slmf .bottom h3"), 0.45, {
                        top: 0,
                        autoAlpha: 1,
                        ease: Cubic.easeOut,
                    });

                    tlIntro.to($(".slmf .bottom p"), 0.45, {
                        top: 0,
                        autoAlpha: 1,
                        ease: Power2.easeIn,
                    }, "-=0.15");

                    tlIntro.to($("section.submenu-disciplines a").eq(0), .35, {
                        scale: 1,
                        autoAlpha: 1,
                        marginTop: 0,
                        ease: Power2.easeIn,
                    }, "-=0.15");
                    tlIntro.to($("section.submenu-disciplines a").eq(1), .35, {
                        scale: 1,
                        autoAlpha: 1,
                        marginTop: 0,
                        ease: Power2.easeIn,
                    }, "-=0.15");
                    tlIntro.to($("section.submenu-disciplines a").eq(2), .35, {
                        scale: 1,
                        autoAlpha: 1,
                        marginTop: 0,
                        ease: Power2.easeIn,
                        onComplete: function() {
                            setTimeout(function() {
                                $(".diagonal-1, .diagonal-2, .diagonal-3, .diagonal-4").css("display", "");
                                navDisciplines.enabled = true;
                            }, 100);
                        }
                    }, "-=0.15");
                }
            });
        });
    },
    cachedElements: function() {

        $Home = $("#home-discipline");
        $NavPilars = $Home.find("#nav-pilars");
        $NavDisciplines = $Home.find("#nav-disciplines");
    },
};