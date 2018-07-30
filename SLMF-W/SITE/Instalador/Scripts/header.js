Header = function() {

    $Header = $("#header-main");
    this.settings = {

        logo: $Header.find("#logo"),
        disciplines: $Header.find("nav.submenu-hidden"),
        hoverDisciplines: $Header.find(".hover-disciplines"),
        // hoverSocial: $Header.find(".hover-social"),
        showingDisciplines: false,

        btPilars: $Header.find("ul.menu-left-2 a"),
        btDisciplines: $Header.find("#show-disciplines a"),
        containerDisciplines: $Header.find("#show-disciplines"),
        // containerSocial: $Header.find("#show-social"),
        // btSocial: $Header.find("#bt-social"),
    };

    this.init();
};


Header.prototype.init = function() {

    this.bind();
};

Header.prototype.bind = function() {

    var self = this;

    // Submenu disciplinas
    this.settings.hoverDisciplines
        .hover(function() {

                if (!self.settings.showingDisciplines) {
                    self.animations({
                        action: "show-disciplines",
                        container: self.settings.containerDisciplines
                    });
                };
                self.settings.showingDisciplines = true;
            },
            function() {
                self.settings.showingDisciplines = false;
                setTimeout(function() {

                    if (!self.settings.showingDisciplines) {
                        self.animations({
                            action: "hidde-disciplines",
                            container: self.settings.containerDisciplines
                        });
                    };
                }, 50);
            });

    // Botones menú pilares
    this.settings.btPilars
        .each(function() {
            var el = $(this),
                text = el.find("span"),
                icon = el.find("svg"),
                path = el.find("path");

            el.hover(function() {
                if ($(this).hasClass('overed')) {
                    return false;
                }
                $(this).addClass('overed');
                console.log(self.settings.showingDisciplines);
                var mirrorId = $(this).data('mouse-mirror');
                $(mirrorId).trigger('mouseover');
                self.animations({
                    action: "show-data-bt-pilar",
                    el: el,
                    text: text,
                    icon: icon,
                    path: path
                });
            }, function() {
                if ($(this).hasClass('overed') === false) {
                    return false;
                }
                $(this).removeClass('overed');
                var mirrorId = $(this).data('mouse-mirror');
                $(mirrorId).trigger('mouseleave');
                self.animations({
                    action: "hidde-data-bt-pilar",
                    el: el,
                    text: text,
                    icon: icon,
                    path: path
                });
            });
        });

    // Botones menú disciplinas
    this.settings.btDisciplines
        .each(function() {

            var el = $(this),
                lineTop = el.find(".line-top"),
                lineBottom = el.find(".line-bottom"),
                toHidde = el.find(".to-hidde"),
                toShow = el.find(".to-show");

            el.hover(function() {
                var isDisabled = $(this).hasClass('inactive');
                var actionName = isDisabled ? "show-disabled-bt-discipline" : "show-data-bt-discipline";
                
                self.animations({
                    action: actionName,
                    el: el,
                    toShow: toShow,
                    toHidde: toHidde,
                    lineTop: lineTop,
                    lineBottom: lineBottom,
                });
            }, function() {

                self.animations({
                    action: "hidde-data-bt-discipline",
                    el: el,
                    toShow: toShow,
                    toHidde: toHidde,
                    lineTop: lineTop,
                    lineBottom: lineBottom,
                });
            });
        });
};

Header.prototype.animations = function(params) {

    switch (params.action) {

        case "show-disabled-bt-discipline":
            TweenMax.to(params.toHidde, 0.15, {
                    top: -30
                });
            TweenMax.to(params.el.find('.soon-link'), 0.15, {
                    top: 35
                });
            break;
        case "show-disciplines":

            TweenMax.to(params.container, 0, {
                autoAlpha: 1,
                marginLeft: 1
            });
            TweenMax.staggerTo(params.container.find("a"), 0.15, {
                autoAlpha: 1,
            }, 0.1)
            TweenMax.staggerTo(params.container.find("svg"), 0.15, {
                scale: params.scale || 1,
            }, 0.1)

            break;
        case "hidde-disciplines":

            TweenMax.to(params.container, 0.1, {
                autoAlpha: 0,
                onComplete: function() {

                    TweenMax.set(params.container, {
                        autoAlpha: 0,
                        marginLeft: -288
                    });

                    TweenMax.set(params.container.find("a"), {
                        autoAlpha: 0,
                    });

                    TweenMax.set(params.container.find("svg"), {
                        scale: params.scale || 1.6
                    });
                }
            });
            break;

        case "show-data-bt-pilar":

            params.el.removeClass("normal");

            TweenMax.set(params.el, {
                backgroundColor: "#f6d641"
            });
            TweenMax.set(params.path, {
                fill: "#282828",
            });
            TweenMax.set(params.icon, {
                marginTop: 18,
            });

            TweenMax.to(params.icon, 0.2, {
                marginTop: 12,
                ease: Quart.easeOut
            });

            TweenMax.to(params.text, 0.15, {
                delay: 0.10,
                autoAlpha: 1,
                ease: Linear.easeNone
            });
            break
        case "hidde-data-bt-pilar":

            params.el.addClass("normal");

            TweenMax.set(params.el, {
                backgroundColor: "#666666"
            });
            TweenMax.set(params.path, {
                fill: "#fff",
            });
            TweenMax.to(params.icon, 0.1, {
                marginTop: 23,
            });

            TweenMax.to(params.text, 0.1, {
                autoAlpha: 0
            });
            break

        case "show-data-bt-discipline":

            TweenMax.set(params.toShow, {
                autoAlpha: 1
            });

            TweenMax.set(params.lineTop, {
                autoAlpha: 1,
            });
            TweenMax.set(params.lineBottom, {
                autoAlpha: 1,
            });

            TweenMax.to(params.lineTop, .2, {
                easing: Quart.easeOut,
                marginTop: 27
            });
            TweenMax.to(params.lineBottom, .2, {
                easing: Quart.easeOut,
                marginTop: 62
            });

            TweenMax.set(params.toHidde, {
                autoAlpha: 0
            });
            break;
        case "hidde-data-bt-discipline":

            TweenMax.to(params.toShow, 0, {
                autoAlpha: 0
            });

            TweenMax.to(params.lineTop, 0, {
                autoAlpha: 0,
                marginTop: 22,
            });
            TweenMax.to(params.lineBottom, 0, {
                autoAlpha: 0,
                marginTop: 67
            });
            TweenMax.to(params.toHidde, 0, {
                autoAlpha: 1,
                top: 0
            });

            TweenMax.to(params.el.find('.soon-link'), 0.15, {
                    top: 50
                });
            break;
    };
};

$(document).ready(function() {
    header = new Header();
});