// @codekit-prepend "navigation.js"
// @codekit-prepend "slide.js"
// @codekit-prepend "intro.js"
//  
About = {
    init: function(data) {
        this.cachedElements();


        slide = new Slide();
        intro = new Intro({
            onComplete: bindAll
        });
        navigation = new Navigation();
        slide.init();

        function toggleCenterNavigation() {
            $About.find(".right-back, .left-back").toggle();
        };

        function handleCustomResize() {
            $(window).trigger("resize.bindToWindowSize");
        };
        // INIT SLIDE
        var currentSlide = "";
        var currentBioPage = 1;
        // var currentHistoryPage = 1;
        var currentSectionPage = "";

        function slideTo(direction, reset, callback) {
            if (direction == currentSlide && !reset) {

                return false;
            }

            function resetNavArrows() {
                $About.find("a.up").addClass("null");
                $About.find("a.down").removeClass("null");
            }
            //toggleCenterNavigation();
            switch (direction) {
                case 'left':
                    if (currentSlide != 'left') {
                        resetNavArrows();
                        slide.animate({
                            key: "slideLeft",
                            callback: callback
                        });
                        $(".active-left-block.active").trigger("click");
                    } else {
                        slide.snap('left');
                        slide.repositionBackground('left');
                    }
                    break;
                case 'right':
                    if (currentSlide != 'right') {
                        resetNavArrows();
                        slide.animate({
                            key: "slideRight",
                            callback: callback
                        });
                        $(".active-right-block.active").trigger("click");
                    } else {
                        slide.snap('right');
                        slide.repositionBackground('right');
                    }
                    break;
                default:
                    //var backgroundOffset = 
                    if (reset == true) {
                        TweenMax.to("#section-about", this.duration, {
                            scrollTo: {
                                x: ($(window).width() - 95) / 2,

                            },
                        });
                        // $(document).scrollLeft(($(window).width() - 95) / 2);

                    } else {
                        slide.animate({
                            key: "slideCenter",
                            onComplete: callback
                        });
                    }
                    //
                    //$About.find(".slide").css("left", "-50%");
                    //$About.find(".center-image").css("background-position", "");
            }
            currentSlide = direction;
            // console.log(currentSlide);
        }

        function addButtonTriggers() {
            //Left Navigation Menu
            $About.find("#link-mision, #link-historia, #link-manifiesto, #link-pilares").click(onLeftNavigationClick);
            $About.find(".left-slide a.down, .left-slide a.up").click(onLeftVerticalNavClick);
            //Right Navigation Menu
            $About.find("#link-bio, #link-filosofia, #link-logros").click(onRightNavigationClick);
            $About.find(".right-slide a.down, .right-slide a.up ").click(onRightVerticalNavClick);
            //Side controls
            $About.find("#left-back-link, #right-back-link").click(function(e) {
                e.preventDefault();
                slideTo('center');
            });
            //Center controls
            $About.find("#center-go-right, #center-go-left").click(function(e) {
                e.preventDefault();
                // console.log("hua!", currentSlide);
                if (currentSlide != 'center') {
                    slideTo('center');
                    return false;
                }else{

                    slideTo(this.id.replace("center-go-", ""));
                }
            });
            // $About.find("#center-go-right").click(function(e) {
            //     e.preventDefault();
            //     console.log("hua!", currentSlide);
            //     if (currentSlide != 'center') {
            //         slideTo('center');
            //         return false;
            //     }
            //     slideTo(this.id.replace("center-go-", ""));
            // });

        }


        //RIGHT NAVIGATION
        function onRightNavigationClick(event) {
            var page = event.target.id.replace("link-", "");
            event.preventDefault();
        }

        function onRightVerticalNavClick(event) {
            event.preventDefault();
            var parentA = $(event.target).parent();
            var isEnabled = !parentA.hasClass('null');
            if (isEnabled) {
                $About.find(".right-slide a.down, .right-slide a.up").removeClass("null");
                switch (parentA[0].id) {
                    case 'bio-down':
                        // console.log("bio-down");
                        currentBioPage = currentBioPage + 1;
                        navigation.animations({
                            action: "right-show-bio-2"
                        });
                        break;
                    case 'bio-up':
                        // console.log("bio-up");
                        currentBioPage = currentBioPage - 1;
                        navigation.animations({
                            action: "right-show-bio-1"
                        });
                        break;
                }
                parentA.addClass("null");
            }
        }

        //LEFT NAVIGATION
        function onLeftNavigationClick(event) {
            event.preventDefault();
            var page = event.target.id.replace("link-", "");
        }

        function onLeftVerticalNavClick(event) {
            event.preventDefault();
            var parentA = $(event.target).parent();
            var isEnabled = !parentA.hasClass('null');

            if (isEnabled) {
                $About.find(".left-slide a.down, .left-slide a.up").removeClass("null");
                switch (parentA[0].id) {
                    case 'historia-down':
                        // console.log("historia-down");
                        // currentHistoryPage = currentHistoryPage + 1;
                        navigation.animations({
                            action: "left-show-history-2"
                        });
                        break;
                    case 'historia-up':
                        // console.log("historia-up");
                        // currentHistoryPage = currentHistoryPage - 1;
                        navigation.animations({
                            action: "left-show-history-1"
                        });
                        break;
                }
                parentA.addClass("null");
            }

        }

        _onLoadImages(data.images, function() {
            var loadIntro = function() {
                setTimeout(function() {
                    intro.init({});
                }, 1);
            }


            $About.removeLoader({
                onComplete: function() {
                    slideTo('center', false, loadIntro);
                    navigation.init();
                }
            });
        });


        function bindAll() {
            addButtonTriggers();
            toggleCenterNavigation();
            handleCustomResize();
            //span center
            $(window).resize(function() {
                slideTo(currentSlide, true);
            });
        };


    },
    cachedElements: function() {
        $About = $("#section-about");
        $Window = $(window);
    }
};