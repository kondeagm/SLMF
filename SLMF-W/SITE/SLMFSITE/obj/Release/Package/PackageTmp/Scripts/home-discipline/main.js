// @codekit-prepend "../vendor/underscore-min.js"
// @codekit-prepend "../home/nav-disciplines.js"
// @codekit-prepend "nav-pilars.js"
HomeDiscipline = {

    init: function(params) {

        this.cachedElements();
        navPilars = new NavPilars();

        _onLoadImages(params.images, function() {
            $(".bg img").each(function(idx, el) {
                $(el).attr("data-natural-width", el.width);
                $(el).attr("data-natural-height", el.height);
                //console.log($(el).attr("data-ml", el.style.marginLeft));
            });

            var resizeBackgrounds = _.debounce(function() {
                var w = Math.max(1024, $(window).width()),
                    h = Math.max(800, $(window).height()),
                    parentRatio = w / h;

                $(".bg img").each(function(idx, el) {
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
                        var ml = (w - 95) < elWidth ? (w - elWidth) / 2 - 45 : "";
                        //console.log(w, elWidth);
                        $(el).css({
                            "height": h,
                            "width": Math.round(naturalWidth * (h / naturalHeight)),
                            "margin-left": ml,
                        });
                    }

                    //console.log(parentRatio, ratio);
                });
            }, 10);
            $(window).resize(resizeBackgrounds).trigger("resize");


            $HomeDiscipline.removeLoader({
                onComplete: function() {
                    //add active class
                    $(".header-content").addClass("active");
                    TweenMax.set("#home-discipline-bg", {
                        autoAlpha: 1,
                    });
                    TweenMax.fromTo($("#home-discipline-bg img"), .8, {
                        autoAlpha: 0,
                    },{
                        autoAlpha: 1,
                        ease: Linear.easeNone,
                        onComplete: function() {

                            TweenMax.to($(".home-content .header-content .top"), 0, {
                                autoAlpha: 1
                            });
                            TweenMax.fromTo($(".home-content .header-content .top"), .5, {
                                left: -200
                            }, {
                                left: -22
                            });

                            TweenMax.to($(".home-content .header-content h1"), 0, {
                                autoAlpha: 1,
                                delay: .2
                            });
                            TweenMax.to($(".home-content .header-content h1"), .3, {
                                left: 0,
                                delay: .2
                            });

                            TweenMax.to($(".home-content .header-content h3"), 0, {
                                autoAlpha: 1,
                                delay: .3
                            });
                            TweenMax.to($(".home-content .header-content h3"), .3, {
                                left: 0,
                                delay: .3
                            });

                            TweenMax.to($(".home-content .header-content p"), .3, {
                                top: 0,
                                autoAlpha: 1,
                                delay: .4
                            });

                            TweenMax.to($(".home-content .description"), .15, {
                                delay: 4.55,
                                ease: Power2.easeIn,
                                autoAlpha: 1
                            });

                            var btNavPilars = $("#nav-pilars a");

                            var delay = 0.5,
                                imgScale0 = btNavPilars.eq(0),
                                imgScale1 = btNavPilars.eq(1),
                                imgScale2 = btNavPilars.eq(2);

                            TweenMax.to(imgScale0, .3, {
                                autoAlpha: 1,
                                scale: 1,
                                marginTop: 0,
                                delay: delay + 0.1,
                                ease: Power2.easeIn,
                            });
                            TweenMax.to(imgScale1, .3, {
                                autoAlpha: 1,
                                scale: 1,
                                marginTop: 0,
                                delay: delay + 0.2,
                                ease: Power2.easeIn,
                            });
                            TweenMax.to(imgScale2, .3, {
                                autoAlpha: 1,
                                scale: 1,
                                marginTop: 0,
                                delay: delay + 0.3,
                                ease: Power2.easeIn,
                            });

                            TweenMax.to($(".bt-slmf-team"), .3, {
                                marginTop: -66,
                                delay: delay + 0.7,
                                // ease: Cubic.easeIn,
                            });
                        }
                    })
                },
            });
        });


    },
    cachedElements: function() {

        $HomeDiscipline = $("#home-discipline");
        $HeaderContent = $HomeDiscipline.find(".header-content");
        $NavPilars = $HomeDiscipline.find("#nav-pilars");
    },
};

$(function() {


});