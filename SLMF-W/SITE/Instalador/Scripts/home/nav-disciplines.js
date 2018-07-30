NavDisciplines = function() {

    this.settings = {
        slmf: {
            background: $Home.find(".home-bg .slmf"),
            header: $Home.find(".home-content header .slmf"),
            headerSubs: $Home.find(".home-content .slmf .logo, .home-content .slmf .bottom"),
            description: $Home.find(".home-content header .slmf span .description"),
        },
        bbd: {
            background: $Home.find(".home-bg .bbd"),
            button: $Home.find(".d-bbd"),
            header: $Home.find(".home-content header .bbd"),
            headerSubs: $Home.find(".home-content .bbd .bottom, .home-content .bbd .description-mask"),
            description: $Home.find(".home-content header .bbd span .description"),
        },
        cft: {
            background: $Home.find(".home-bg .cft"),
            button: $Home.find(".d-cft"),
            header: $Home.find(".home-content header .cft"),
            headerSubs: $Home.find(".home-content .cft .bottom, .home-content .cft .description-mask"),
            description: $Home.find(".home-content header .cft span .description"),
        },
        mma: {
            background: $Home.find(".home-bg .mma"),
            button: $Home.find(".d-mma"),
            header: $Home.find(".home-content header .mma"),
            headerSubs: $Home.find(".home-content .mma .bottom, .home-content .mma .description-mask"),
            description: $Home.find(".home-content header .mma span .description"),
        },

        allBackgrounds: $Home.find(".home-bg .slide"),
        allButtons: $Home.find(".d-cft, .d-mma, .d-bbd"),
        navigationBar: $Home.find("#nav-disciplines"),
        //all background stripes
        diagonals: $Home.find(".header-content .top .diagonal"),
        navPilars: $("#navPilars"),
        btNavPilars: $("#navPilars a"),
        btSlmfTeam: $Home.find("#bt-slmf-team a"),
    };
    this.frontSlide = null;
    this.backSlide = null;
    this.enabled = false;
    this.init();
};

NavDisciplines.prototype.init = function() {
    this.bind();
};

NavDisciplines.prototype.bind = function() {
    var self = this,
        settings = self.settings,
        frame = 1 / 30,
        isRunning = false,
        tl = false,
        animationQueue = [];

    setTimeout(processAnimationQueue, 4000);

    function setFrontSlide(slide) {
        self.frontSlide = slide;
        slide && slide.background.css("z-index", 1);
        slide && slide.background.find(".diagonal-1, .diagonal-2, .diagonal-3, .diagonal-4").css("display", "block");
    }

    function setBackSlide(slide) {
        self.backSlide = slide;
        //reset css properties

        slide && slide.background.css("z-index", 0);
        slide && slide.background.find(".diagonal").css("clip", '');
        slide && slide.background.find(".diagonal-1, .diagonal-2, .diagonal-3, .diagonal-4").css("display", "none");
    }

    function hideInactiveSlides() {
        //self.settings.allBackgrounds.css("opacity", 0);
        TweenMax.set(self.settings.allBackgrounds, {
            autoAlpha: 0
        });
        self.frontSlide && TweenMax.set(self.frontSlide.background, {
            autoAlpha: 1
        });
        self.backSlide && TweenMax.set(self.backSlide.background, {
            autoAlpha: 1
        });
    }

    function processAnimationQueue() {
        animationQueue.length > 0 && onButtonEvent(null, animationQueue.pop());
    }

    function queueAnimation(element) {
        animationQueue[0] = element; //only 1 animation queue
    }

    function startFrontTransition() {
        hideInactiveSlides();
        if (self.frontSlide && self.backSlide) {

            tl = new TimelineLite({
                onComplete: function(e) {
                    setFrontSlide(self.backSlide);
                    self.settings.diagonals.css({
                        overflow: "visible"
                    });
                    //console.log("tl Done");
                    isRunning = false;
                    processAnimationQueue();
                }
            });

            function getDiagonalTransition(diagonal, mode, top, left, time) {
                var easing = mode == "in" ? Power2.easeIn : Power2.easeOut;
                var alpha = mode == "in" ? 1 : 0;
                top = mode == "in" ? 0 : top;
                left = mode == "in" ? 0 : left;
                //console.log(easing, alpha, top, left);
                return new TweenMax.to(diagonal, time, {
                    ease: easing,
                    top: top,
                    left: left,
                    autoAlpha: alpha,
                    onComplete: function() {
                        //restore original values if went out (inverted signs)
                        if (mode == "out") {
                            diagonal.css("top", top * -1).css("left", left * -1);
                        }
                    }
                });
            }

            function appendHeaderTransitionToTimeLine(slide, mode, label) {
                var d0 = slide.header.find('.top .d1'),
                    d1 = slide.header.find('.top .d2'),
                    d2 = slide.header.find('.top .d3'),
                    d3 = slide.header.find('.top .d4');
                //Fade header
                if (slide != self.frontSlide) {
                    tl.append(TweenMax.set(slide.header.find('.top'), {
                        autoAlpha: mode == "in" ? 1 : 0
                    }), label);
                };

                if (mode == "in") {
                    //time is slower on slmf slide
                    var time = slide == self.settings.slmf ? 0.25 : frame * 7;
                    tl.append(getDiagonalTransition(d0, mode, 90, -52, time), label);
                    tl.append(getDiagonalTransition(d1, mode, -90, 52, time), label);
                    tl.append(getDiagonalTransition(d2, mode, 90, -52, time), label);
                    tl.append(getDiagonalTransition(d3, mode, -90, 52, time), label);
                } else if (mode == "out") {
                    tl.append(getDiagonalTransition(d0, mode, -90, 52, frame * 7), label);
                    tl.append(getDiagonalTransition(d1, mode, 90, -52, frame * 7), label);
                    tl.append(getDiagonalTransition(d2, mode, -90, 52, frame * 7), label);
                    tl.append(getDiagonalTransition(d3, mode, 90, -52, frame * 7), label);
                };

                // logo and titles
                tl.append(TweenMax.to(slide.headerSubs, frame * 5, {
                    autoAlpha: mode == "in" ? 1 : 0
                }), label);
            };

            // F R O N T - S L I D E
            var fSlide = self.frontSlide;
            // HEADER
            appendHeaderTransitionToTimeLine(fSlide, "out", "front_header_out");
            //BACKGROUND
            var bg_d0 = fSlide.background.find(".diagonal-0"),
                bg_d1 = fSlide.background.find(".diagonal-1"),
                bg_d2 = fSlide.background.find(".diagonal-2"),
                bg_d3 = fSlide.background.find(".diagonal-3"),
                bg_d4 = fSlide.background.find(".diagonal-4");

            var time = frame * 20;

            function resetRect(element) {
                element.css("clip", "rect({0}px, {1}px, {2}px, {3}px".format(0, element.width(), element.height(), 0));
            }
            resetRect(bg_d0);
            resetRect(bg_d1);
            resetRect(bg_d2);
            resetRect(bg_d3);
            resetRect(bg_d4);

            var rectUp = "rect({0}px, {1}px, {2}px, {3}px)".format(0, bg_d2.width(), 0, 0);
            var rectDown = "rect({0}px, {1}px, {2}px, {3}px)".format(bg_d2.height(), bg_d2.width(), bg_d2.height(), 0);
            //var rectDown = 

            if (self.backSlide == self.settings.slmf) {
                rectUp = "rect({0}px, {1}px, {2}px, {3}px)".format(bg_d2.height(), bg_d2.width(), bg_d2.height(), 0);
                rectDown = "rect({0}px, {1}px, {2}px, {3}px)".format(0, bg_d2.width(), 0, 0);
            }

            //console.log(rectUp);
            tl.append(new TweenMax.to(bg_d0, time, {
                ease: Cubic.easeInOut,
                css: {
                    clip: rectUp
                }
            }), "bg_diagonals");
            tl.append(new TweenMax.to(bg_d1, time, {
                ease: Cubic.easeInOut,
                css: {
                    clip: rectUp
                }
            }), "bg_diagonals");
            tl.append(new TweenMax.to(bg_d2, time, {
                ease: Cubic.easeInOut,
                css: {
                    clip: rectDown
                }
            }), "bg_diagonals");
            tl.append(new TweenMax.to(bg_d3, time, {
                ease: Cubic.easeInOut,
                css: {
                    clip: rectUp
                }
            }), "bg_diagonals");
            tl.append(new TweenMax.to(bg_d4, time, {
                ease: Cubic.easeInOut,
                css: {
                    clip: rectUp
                }
            }), "bg_diagonals");

            //B A C K - S L I D E
            var bSlide = self.backSlide;
            appendHeaderTransitionToTimeLine(bSlide, "in", "back_header_in");

            tl.timeScale(1);
            tl.gotoAndPlay("head_diagonals");
            isRunning = true;
        }
    }

    function showIconAnimation(slide) {
        var btn = slide.button;
        var frame = 1 / 30;
        if (btn.find(".hover").css("opacity") == "0") {
            hideIconsAnimation();
            TweenMax.set(btn.find(".hover"), {
                autoAlpha: 1
            });
            TweenMax.fromTo(btn.find(".hover-left"), frame * 10, {
                top: -20,
                left: 20
            }, {
                top: 0,
                left: 0,
                ease: Power2.easeOut
            });
            TweenMax.fromTo(btn.find(".hover-right"), frame * 10, {
                top: 20,
                left: -20
            }, {
                top: 0,
                left: 0,
                ease: Power2.easeOut
            });
            TweenMax.to(btn.find("span.text"), frame * 1, {
                autoAlpha: 1
            });
        }
    }

    function hideIconsAnimation() {
        TweenMax.set(self.settings.allButtons.find(".hover, span.text"), {
            autoAlpha: 0
        });
    }


    //NEED TO REFACTOR!
    function showDisciplineButtons() {
        tl.currentProgress = 1;



        $(".diagonal-1, .diagonal-2, .diagonal-3, .diagonal-4").css("display", "none");
        var delay0 = 1.3;

        TweenMax.to(self.settings.navPilars, 0, {
            height: 285,
            delay: delay0,
        });

        var imgScale0 = settings.btNavPilars.eq(0);
        TweenMax.to(imgScale0, 0, {
            autoAlpha: 1,
            delay: delay0,
        });
        TweenMax.to(imgScale0.find("svg"), 0.15, {
            scale: 1,
            delay: delay0,
        });

        var imgScale1 = settings.btNavPilars.eq(1);
        TweenMax.to(imgScale1, 0, {
            autoAlpha: 1,
            delay: delay0 + 0.1
        });
        TweenMax.to(imgScale1.find("svg"), 0.15, {
            scale: 1,
            delay: delay0 + 0.1,
        });

        var imgScale2 = settings.btNavPilars.eq(2);
        TweenMax.to(imgScale2, 0, {
            autoAlpha: 1,
            delay: delay0 + 0.2
        });
        TweenMax.to(imgScale2.find("svg"), 0.15, {
            scale: 1,
            delay: delay0 + 0.2,
        });

        $("section.submenu-disciplines a.available").unbind('mouseenter mouseleave');
        $("#nav-disciplines").unbind('mouseenter mouseleave');

        TweenMax.to($("section.submenu-disciplines a").eq(0), .15, {
            scale: 0,
            marginTop: -22
        });
        TweenMax.to($("section.submenu-disciplines a").eq(1), .15, {
            scale: 0,
            delay: 0.1,
            marginTop: -22
        });
        TweenMax.to($("section.submenu-disciplines a").eq(2), .15, {
            scale: 0,
            delay: 0.2,
            marginTop: -22
        });

        self.frontSlide.header.addClass('active');
        $('.from-none').css({
            display: "block"
        });

        TweenMax.to(self.frontSlide.description, .15, {
            delay: 0.55,
            ease: Power2.easeIn,
            autoAlpha: 1
        });

        var delay1 = 0.5;
        TweenMax.fromTo($("section.submenu-pilars a").eq(0), .3, {
            scale: 1.6,

        }, {
            delay: delay1,
            scale: 1.0,
            marginTop: 0,
            autoAlpha: 1,
            ease: Power2.easeIn,

        });
        TweenMax.fromTo($("section.submenu-pilars a").eq(1), .3, {
            scale: 1.6,

        }, {
            scale: 1.0,
            marginTop: 0,
            delay: delay1 + 0.2,
            autoAlpha: 1,
            ease: Power2.easeIn,
        });
        TweenMax.fromTo($("section.submenu-pilars a").eq(2), .3, {
            scale: 1.6,
            ease: Power2.easeIn,

        }, {
            scale: 1.0,
            marginTop: 0,
            autoAlpha: 1,
            delay: delay1 + 0.3,
            ease: Power2.easeIn,
        });

        TweenMax.to(settings.btSlmfTeam, .15, {
            marginTop: -66,
            ease: Linear.easeIn,
            delay: 1.5,
            onComplete: function() {
                TweenMax.set(self.frontSlide.description, {
                    autoAlpha: 1
                });
                self.settings.diagonals.css({
                    overflow: "visible"
                });
            }
        });
    }

    //BUTTON LISTENERS
    function onButtonEvent(event, slide) {

        //update icons state
        if (slide != settings.slmf) {
            showIconAnimation(slide);
        } else {
            hideIconsAnimation();
        }
        //same slide?
        if (self.backSlide != slide) {
            //is not running
            if (!isRunning && self.enabled == true) {
                setBackSlide(slide);
                self.settings.diagonals.css({
                    overflow: "hidden"
                });
                startFrontTransition();
            } else if (isRunning == true || !self.enabled) {

                //console.log("To Queue");
                // if busy then queueAnimation
                queueAnimation(slide);
                //speedup current transition!
                // if (isRunning) {
                //     tl.timeScale(1.5);
                // }
            }
        }

        event && event.stopPropagation();
    }

    settings.bbd.button.mouseenter(function(e) {
        onButtonEvent(e, settings.bbd);
    });
    settings.mma.button.mouseenter(function(e) {
        onButtonEvent(e, settings.mma);
    });
    settings.cft.button.mouseenter(function(e) {
        onButtonEvent(e, settings.cft);
    });
    settings.navigationBar.mouseleave(function(e) {
        if (self.backSlide) {
            onButtonEvent(e, settings.slmf);
        }
    });
    settings.allButtons.mouseleave(function(e) {
        hideIconsAnimation();
    });

    settings.allButtons.click(function(e) {
        if (self.enabled) {
            //settings.allButtons.off();
            if ($(this).hasClass("available")) {
                //build the new path
                var hrefPath = "#";
                if ($(this).hasClass('d-cft')) hrefPath = '/crossfit';
                if ($(this).hasClass('d-mma')) hrefPath = '/mixedmartialarts';
                if ($(this).hasClass('d-bbd')) hrefPath = '/bodybuilding';

                $("#pilar-button-competir").attr('href', hrefPath + "/competir");
                $("#pilar-button-entrenar").attr('href', hrefPath + "/entrenar");
                $("#pilar-button-potenciar").attr('href', hrefPath + "/potenciar");

                $("#menu-pilar-button-competir").attr('href', hrefPath + "/competir");
                $("#menu-pilar-button-entrenar").attr('href', hrefPath + "/entrenar");
                $("#menu-pilar-button-potenciar").attr('href', hrefPath + "/potenciar");

                tl.currentProgress = 1;
                setTimeout(showDisciplineButtons, 100);
            }
        }
        e.preventDefault();
    });
    //set default slide
    setFrontSlide(settings.slmf);
    hideInactiveSlides();
};