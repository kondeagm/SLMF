Slide = function() {
    this.settings = {
        wnd: $(window),
        slide: $About.find(".slide"),
        navLeft: $About.find(".left-slide .slide-content .navigator span"),
        navRight: $About.find(".right-slide .slide-content .navigator span"),
        centerLogo: $About.find(".center-logo img"),
        flechaLeft: $About.find(".left-slide .wrapper-franja .franja-derecha"),
        flechaRight: $About.find(".right-slide .wrapper-franja .franja-izquierda"),
        linkBackLeft: $About.find(".left-slide .slide-content a.back-button"),
        linkBackRight: $About.find(".right-slide .slide-content a.back-button"),
        activeSectionRight: $About.find("#active-section-right"),
        activeSectionLeft: $About.find("#active-section-left"),

        btLeft: $About.find("#center-go-left .navigator"),
        btRight: $About.find("#center-go-right .navigator"),

        diagonalLeftImage: $About.find(".left-slide .diagonals-left .image"),
        diagonalLeftImageBg: $About.find(".left-slide .diagonals-left .background"),
        diagonalLeftImageYellow: $About.find(".left-slide .diagonals-left .yellow-back"),

        diagonalRightImage: $About.find(".right-slide .diagonals-right .image"),
        diagonalRightImageBg: $About.find(".right-slide .diagonals-right .background"),
        diagonalRightImageYellow: $About.find(".right-slide .diagonals-right .yellow-back"),

        backArrows: $About.find(".bt-scroll-next-slide"),

        btHistoryUp: $About.find("#historia-up"),
        btHistoryDown: $About.find("#historia-down"),

        btBioUp: $About.find("#bio-up"),
        btBioDown: $About.find("#bio-down"),
    }
    this.init();
};

Slide.prototype.init = function() {

    this.bind({
        action: "hoverBtLeft" 
    });
    this.bind({
        action: "hoverBtRight" 
    });
    this.bind({
        action: "scroll",
        up: this.settings.btHistoryUp,
        down: this.settings.btHistoryDown 
    });
    this.bind({
        action: "scroll",
        up: this.settings.btBioUp,
        down: this.settings.btBioDown 
    });
};

Slide.prototype.bind = function(params) {

    var self = this;
    var s = self.settings;

    switch (params.action) {
        case "hoverBtLeft":
            s.btLeft.hover(function() {

                self.animate({
                    key: "hoverBtArrow",
                    el: $(this).find(".arrow-left"),
                    x: 0
                });
            }, function() {

                self.animate({
                    key: "hoverBtArrow",
                    el: $(this).find(".arrow-left"),
                    x: 20
                });
            });
            break;

        case "hoverBtRight":
            s.btRight.hover(function() {

                self.animate({
                    key: "hoverBtArrow",
                    el: $(this).find(".arrow-right"),
                    x: 0
                });
            }, function() {
                self.animate({
                    key: "hoverBtArrow",
                    el: $(this).find(".arrow-right"),
                    x: -20
                });
            });
            break;

        case "scroll":
            params.down
                .hover(function() {
                    TweenLite.to($(this).find("span"), 0.15, {
                        top: 0
                    })
                }, function() {
                    TweenLite.to($(this).find("span"), 0.15, {
                        top: -10
                    })
                });

            params.up
                .hover(function() {
                    TweenLite.to($(this).find("span"), 0.15, {
                        top: 0
                    })
                }, function() {
                    TweenLite.to($(this).find("span"), 0.15, {
                        top: 10
                    })
                });
            break;
    };
};

Slide.prototype.snap = function(side) {
    var duration = 0.55,
        s = this.settings;
    if (side == 'left') {
        TweenMax.to("#section-about", duration, {
            scrollTo: {
                x: 0
            },
            ease: Cubic.easeOut,
            onComplete: function(){
                // console.log("done snap", side);
                // console.log(s.wnd.width());
            }
        });


    } else if (side == 'right') {
        TweenMax.to("#section-about", duration, {
            scrollTo: {
                x: $(window).width()
            },
            ease: Cubic.easeOut,
            onComplete: function(){
                // console.log("done snap", side)
                // console.log(s.wnd.width());
            }
        });

    }
}

Slide.prototype.repositionBackground = function(side) {
    var percent = 57;
    var time = .55;
    var delay = 0.2;
    // console.log(percent);
    var w = $(window).width();
    if (side == 'left') {
        percent = 57;
    } else if (side == 'right') {
        percent = 43;
        time = .48;
    };
    TweenMax.to(this.settings.slide, time, {
        delay: delay,
        backgroundPositionX: percent + "%",
        ease: Cubic.easeOut
    });
}

Slide.prototype.animate = function(params) {
    var self = this,
        s = self.settings,
        key = params.key;

    params.onComplete = params.onComplete || function(){};

    switch (key) {
        case "hoverBtArrow":
            //Firefox Workaround
            var bgPosY = params.el.css('background-position').split(" ")[1];
            TweenLite.to(params.el, 0.15, {
                backgroundPosition: params.x + 'px ' + bgPosY,
                ease: Cubic.easeOut
            });
            break;
        case "slideLeft":
            // Fondo
            this.repositionBackground('left');

            TweenMax.to(s.navLeft, 0.5, {
                marginRight: "6%",
                ease: Cubic.easeOut
            });

            // Logo
            TweenMax.to(s.centerLogo, 0.5, {
                x: 150,
                ease: Cubic.easeOut
            });

            TweenMax.to(s.flechaLeft, .85, {
                left: "95px",
                ease: Cubic.easeOut
            });
            TweenMax.to(s.linkBackLeft, 0.5, {
                opacity: 1,
                ease: Linear.easeNone
            });

            var showBackArrows = function() {

                this.duration = 0.15;
                TweenLite.to(s.backArrows, this.duration, {
                    autoAlpha: 1,
                    delay: 0.25,
                    ease: Cubic.easeOut,
                });
                TweenLite.to($("#left-back-link span.arrow-content"), 0.15, {
                    x: -18,
                    ease: Cubic.easeOut
                });
            };

            var hiddeArrows = function() {

                TweenLite.to([s.btLeft.find(".arrow-left"), s.btRight.find(".arrow-right")], 0.15, {
                    autoAlpha: 0,
                    ease: Linear.easeNone
                })
            };

            // Listas
            var list = function() {
                this.duration = 0.25,
                TweenMax.to(s.activeSectionLeft, this.duration, {
                    opacity: 1,
                    delay: 0.25,
                    ease: Linear.easeNone,
                });
            }

            // Diagonal Left
            var diagonalLeft = function() {
                this.duration = .80;
                TweenLite.to(s.diagonalLeftImage, this.duration, {
                    left: -85,
                    ease: Cubic.easeOut
                });

                TweenLite.to(s.diagonalLeftImageYellow, this.duration, {
                    left: -220,
                    ease: Cubic.easeOut
                });
            };

            showBackArrows();
            hiddeArrows();
            list();
            this.snap('left');
            diagonalLeft();

            break;
        case "slideRight":
            this.repositionBackground('right');

            TweenMax.to(s.navRight, 0.5, {
                marginLeft: "6%",
                ease: Cubic.easeOut
            });
            TweenMax.to(s.centerLogo, 0.5, {
                x: -150,
                ease: Cubic.easeOut
            });

            TweenMax.to(s.flechaRight, 0.85, {
                left: "-160px",
                ease: Cubic.easeOut
            });
            TweenMax.to(s.linkBackRight, 0.5, {
                opacity: 1,
                ease: Linear.easeNone
            });


            var showBackArrows = function() {

                this.duration = 0.15;
                TweenLite.to(s.backArrows, this.duration, {
                    autoAlpha: 1,
                    delay: 0.25,
                    ease: Cubic.easeOut,
                });
                TweenLite.to($("#right-back-link span.arrow-content"), 0.15, {
                    x: 18,
                    ease: Cubic.easeOut
                });
            };

            var hiddeArrows = function() {

                TweenLite.to([s.btLeft.find(".arrow-left"), s.btRight.find(".arrow-right")], 0.15, {
                    autoAlpha: 0,
                    ease: Linear.easeNone
                })
            };

            // Listas
            var list = function() {
                this.duration = 0.25,
                TweenMax.to(s.activeSectionRight, this.duration, {
                    opacity: 1,
                    delay: 0.25,
                    ease: Linear.easeNone,
                });
            }

            // Diagonal Right
            var diagonalRight = function() {
                this.duration = .75;
                TweenLite.to(s.diagonalRightImage, this.duration, {
                    right: -85,
                    ease: Cubic.easeOut
                });

                TweenLite.to(s.diagonalRightImageYellow, this.duration, {
                    right: -220,
                    ease: Cubic.easeOut
                });
            };

            showBackArrows();
            hiddeArrows();
            list();
            this.snap('right');
            diagonalRight();
            break;
        case "slideCenter":

            TweenMax.to(s.slide, 0.75, {
                backgroundPositionX: "50%",
                ease: Cubic.easeOut
            });
            TweenMax.to(s.navLeft, 0.5, {
                marginRight: "31%",
                ease: Cubic.easeOut
            });
            TweenMax.to(s.navRight, 0.5, {
                marginLeft: "31%",
                ease: Cubic.easeOut
            });
            TweenMax.to(s.centerLogo, 0.2, {
                x: 0,
                ease: Cubic.easeOut
            });
            TweenMax.to(s.flechaLeft, 0.5, {
                left: "0px",
                ease: Cubic.easeOut
            });
            TweenMax.to(s.flechaRight, 0.5, {
                left: "0px",
                ease: Cubic.easeOut
            });
            TweenMax.to(s.linkBackLeft, 0.5, {
                opacity: 0,
                ease: Linear.easeNone
            });
            TweenMax.to(s.linkBackRight, 0.5, {
                opacity: 0,
                ease: Linear.easeNone
            });

            var hiddeBackArrows = function() {

                this.duration = 0.15;
                TweenLite.to(s.backArrows, this.duration, {
                    autoAlpha: 0
                })
            };

            var showArrows = function() {

                this.duration = 0.15;
                TweenLite.to([s.btLeft.find(".arrow-left"), s.btRight.find(".arrow-right")], this.duration, {

                    autoAlpha: 1,
                    ease: Linear.easeNone
                });
            };

            var list = function() {

                this.duration = 0.15,
                // Right
                TweenMax.to(s.activeSectionRight, this.duration, {
                    opacity: 0,
                    ease: Linear.easeNone,
                });

                // Left
                TweenMax.to(s.activeSectionLeft, this.duration, {
                    opacity: 0,
                    ease: Linear.easeNone,
                });
            };

            var slide = function() {
                this.duration = 0.5;

                TweenMax.to("#section-about", this.duration, {
                    scrollTo: {
                        x: (s.wnd.width() - 95) / 2,

                    },
                    ease: Cubic.easeOut,
                    onComplete: function(){
                        // console.log("snap center")
                        // console.log(s.wnd.width())
                        params.onComplete();
                    }
                });
            };


            // Diagonal Left
            var diagonalLeft = function() {

                this.duration = 0.5;
                TweenLite.to(s.diagonalLeftImage, this.duration, {
                    left: -400,
                    ease: Cubic.easeOut
                });

                TweenLite.to(s.diagonalLeftImageYellow, this.duration, {
                    left: -265,
                    ease: Cubic.easeOut
                });
            };
            var diagonalRight = function() {

                this.duration = 0.5;
                TweenLite.to(s.diagonalRightImage, this.duration, {
                    right: -400,
                    ease: Cubic.easeOut
                });

                TweenLite.to(s.diagonalRightImageYellow, this.duration, {
                    right: -265,
                    ease: Cubic.easeOut
                });
            };
            hiddeBackArrows();
            showArrows();
            list();
            slide();
            diagonalLeft();
            diagonalRight();
            break;
    };
};