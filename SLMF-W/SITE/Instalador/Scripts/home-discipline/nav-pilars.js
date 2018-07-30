NavPilars = function() {

    this.settings = {
        bt: $NavPilars.find("a"),
    };
    this.init();
};

NavPilars.prototype.init = function() {

    this.bind();
};

NavPilars.prototype.bind = function() {
    var frame = 1 / 30;
    this.settings.bt
        .mouseenter(function() {

            var el = $(this);
            el.settings = {
                display: el.find(".display"),
                text: el.find(".text"),
                title: el.find(".title"),
                subtitle: el.find(".subtitle"),
                ico: el.find(".ico"),
                hover: el.find(".hover")
            };
            var index = el.index();
            $("#navPilars a").eq(el.index()).trigger("mouseenter");

            el.settings.display.css("overflow", "hidden");

            TweenMax.to(el.settings.hover, 0, {
                autoAlpha: 1,
                scale: 1
            });
            TweenMax.to(el.settings.title, 0, {
                paddingTop: 30,
                autoAlpha: 0
            });
            TweenMax.to(el.settings.subtitle, 0, {
                autoAlpha: 0
            });
            TweenMax.to(el.settings.ico, 0, {
                autoAlpha: 0,
                scale: 1.5
            });

            TweenMax.to(el.settings.display, frame * 6, {
                marginTop: 36,
                height: 0,
                ease: Power4.easeOut
            });
            TweenMax.to(el.settings.text, frame * 6, {
                marginTop: -36,
                ease: Power4.easeOut
            });
            TweenMax.to(el.settings.title, 0, {
                delay: frame * 2.8 + frame * 1.7,
                autoAlpha: 1
            });
            TweenMax.to(el.settings.subtitle, 0, {
                delay: frame * 2.8 + frame * 1.7,
                autoAlpha: 1
            });
            TweenMax.to(el.settings.title, frame * 10, {
                delay: frame * 2.8 + frame * 1.7,
                display: 'block',
                paddingTop: 0,
                ease: Power4.easeOut
            });
            TweenMax.to(el.settings.ico, 0, {
                delay: frame * 2.8,
                autoAlpha: 1
            });
            TweenMax.to(el.settings.ico, frame * 14, {
                delay: frame * 2.8,
                scale: 1,
                ease: Power4.easeOut
            });
        })
        .mouseleave(function() {

            var el = $(this);
            el.settings = {
                display: el.find(".display"),
                text: el.find(".text"),
                title: el.find(".title"),
                ico: el.find(".ico"),
                hover: el.find(".hover")
            };
            $("#navPilars a").eq(el.index()).trigger("mouseleave");

            el.settings.display.css("overflow", "visible");


            TweenMax.to(el.settings.hover, frame * 3, {
                scale: .6,
                ease: Power2.easeIn
            });
            TweenMax.to(el.settings.hover, 0, {
                delay: frame * 3,
                autoAlpha: 0
            });


            TweenMax.to(el.settings.display, 0, {
                autoAlpha: 0,
                marginTop: 0,
                height: 36,
                scale: 1.24
            });
            TweenMax.to(el.settings.text, 0, {
                autoAlpha: 0,
                marginTop: 0,
                scale: 1.0,
            });

            TweenMax.to(el.settings.display, 0, {
                delay: frame * 5,
                autoAlpha: 1
            });
            TweenMax.to(el.settings.text, 0, {
                delay: frame * 5,
                autoAlpha: 1
            });

            TweenMax.to(el.settings.display, .15, {
                delay: frame * 5,
                scale: 1,
            });
            TweenMax.to(el.settings.text, .15, {
                delay: frame * 5,
                scale: 1
            });
        });
};