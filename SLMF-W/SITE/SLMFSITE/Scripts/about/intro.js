Intro = function(data) {

    this.settings = {

        bg: $About.find("#slide,.background-tiling"),
        logo: $About.find("#center-logo img"),

        leftArrow: $About.find("#left-arrow"),
        rightArrow: $About.find("#right-arrow"),

        leftH2: $About.find("#center-go-left .nav-title"),
        rightH2: $About.find("#center-go-right .nav-title"),

        leftH1: $About.find("#center-go-left .nav-sub-title"),
        rightH1: $About.find("#center-go-right .nav-sub-title"),

        leftArrowSmall: $About.find("#center-go-left .arrow-left"),
        rightArrowSmall: $About.find("#center-go-right .arrow-right"),
    };
    this.onComplete = data.onComplete || {};
};

Intro.prototype.init = function(data) {

    this.onComplete = this.onComplete || data.onComplete || {};
    this.animate();
};

Intro.prototype.animate = function() {

    var self = this;

    var titles = function() {
        self.onComplete();

        this.duration = .55;
        this.delay1 = 0.5;
        this.delay2 = 0.4;
        this.delay3 = 0.75;

        // Secundario
        TweenLite.fromTo(self.settings.leftH2, this.duration, {
            x: -$Window.width() - 300,
            autoAlpha: 1,
        }, {
            x: 0,
            delay: this.delay2,
            ease: Cubic.easeOut,
        });
        TweenLite.fromTo(self.settings.rightH2, this.duration, {
            x: $Window.width() + 300,
            autoAlpha: 1,
        }, {
            x: 0,
            delay: this.delay2,
            ease: Cubic.easeOut,
        });




        // Principal
        TweenLite.fromTo(self.settings.leftH1, this.duration, {
            x: -$Window.width() - 300,
            autoAlpha: 1,
        }, {
            x: 0,
            delay: this.delay1,
            ease: Cubic.easeOut,
        });
        TweenLite.fromTo(self.settings.rightH1, this.duration, {
            x: $Window.width() + 300,
            autoAlpha: 1,
        }, {
            x: 0,
            delay: this.delay1,
            ease: Cubic.easeOut,
        });




        //Flechas
        TweenLite.set(self.settings.leftArrowSmall, {
            x: 50,
            autoAlpha: 1,
            top: 0,
            delay: this.delay3,
        });
        TweenLite.to(self.settings.leftArrowSmall, this.duration, {
            x: 0,
            delay: this.delay3,
            ease: Cubic.easeOut,
        });


        TweenLite.set(self.settings.rightArrowSmall, {
            x: -50,
            top: 0,
            delay: this.delay3,
        });
        TweenLite.to(self.settings.rightArrowSmall, this.duration, {
            x: 0,
            delay: this.delay3,
            ease: Cubic.easeOut,
        });
    };

    var arrows = function() {

        this.duration = .55;
        TweenLite.fromTo(self.settings.leftArrow, this.duration, {
            left: "-100%"
        }, {
            left: 0,
            ease: Cubic.easeOut,
        });

        TweenLite.fromTo(self.settings.rightArrow, this.duration, {
            left: "100%"
        }, {
            left: 0,
            ease: Cubic.easeOut,
        });
    };

    var logo = function() {

        this.duration = .35;

        TweenLite.fromTo(self.settings.logo, this.duration, {
            autoAlpha: 1,
        }, {
            scale: 1,
            ease: Cubic.easeOut,
        });
    };

    var bg = function() {

        this.duration = .75;
        TweenLite.fromTo(self.settings.bg, this.duration, {
            autoAlpha: 0
        }, {
            autoAlpha: 1,
            ease: Linear.easeNone,
            onComplete: function() {
                logo();
                arrows();
                titles();
            }
        });
    };

    bg();
};