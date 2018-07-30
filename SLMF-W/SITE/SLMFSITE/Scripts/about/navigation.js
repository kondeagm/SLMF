Navigation = function(data) {
    this.settings = {
        btActiveSectionLeft: $About.find(".left-slide .active-left-block"),
        leftBlock: $About.find(".left-slide .active-section-block"),

        rightBlock: $About.find(".right-slide .active-section-block"),
        btActiveSectionRight: $About.find(".right-slide .active-right-block"),
    };
};

Navigation.prototype.init = function(data) {

    this.bind();
};

Navigation.prototype.bind = function() {

    var self = this;

    self.settings.btActiveSectionLeft.on('click', function(event) {
        event.preventDefault();

        if (!$(this).hasClass('null')) {

            self.animations({
                el: $(this),
                action: "left-show-block",
            });
            self.animations({
                el: $(this),
                action: "bt-left-list-active",
            });
        }
    });

    self.settings.btActiveSectionRight.on('click', function(event) {
        event.preventDefault();

        if (!$(this).hasClass('null')) {

            self.animations({
                el: $(this),
                action: "right-show-block",
            });
            self.animations({
                el: $(this),
                action: "bt-right-list-active",
            });
        }
    });
};

Navigation.prototype.animations = function(params) {

    var self = this;
    switch (params.action) {

        case "bt-left-list-active":

            TweenLite.to(self.settings.btActiveSectionLeft, 0.15, {
                color: "#b0b0b0",
                right: 0,
                ease: Cubic.easeOut
            });
            TweenLite.to(self.settings.btActiveSectionLeft.parent().find("span"), 0.1, {
                autoAlpha: 0
            });

            TweenLite.to(params.el, 0.15, {
                color: "#f7d72b",
                right: -12,
                ease: Cubic.easeOut
            });
            TweenLite.fromTo(params.el.parent().find("span"), 0.15, {
                autoAlpha: 0,
                right: -35
            }, {
                right: -27,
                autoAlpha: 1,
                ease: Cubic.easeOut
            });
            break;
        case "bt-right-list-active":

            TweenLite.to(self.settings.btActiveSectionRight, 0.15, {
                color: "#b0b0b0",
                left: 0,
                ease: Cubic.easeOut
            });
            TweenLite.to(self.settings.btActiveSectionRight.parent().find("span"), 0.1, {
                autoAlpha: 0
            });

            TweenLite.to(params.el, 0.15, {
                color: "#f7d72b",
                left: -12,
                ease: Cubic.easeOut
            });
            TweenLite.fromTo(params.el.parent().find("span"), 0.15, {
                autoAlpha: 0,
                left: -35
            }, {
                left: -27,
                autoAlpha: 1,
                ease: Cubic.easeOut
            });
            break;
        case "left-hidde-block":

            self.settings.leftBlock.css('display', 'none');
            TweenLite.to([self.settings.leftBlock, self.settings.leftBlock.find(".step-show"), self.settings.leftBlock.find(".step-big")], 0.15, {
                autoAlpha: 0
            });
            break;
        case "left-show-history-1":

            var block1 = self.settings.leftBlock.eq(1).find(".part-1");
            var block2 = self.settings.leftBlock.eq(1).find(".part-2");

            block2.css('display', 'none');
            TweenLite.to([block2, block2.find(".step-show")], 0.15, {
                autoAlpha: 0
            });


            block1.css('display', 'block');
            TweenLite.to(block1, 0.15, {
                autoAlpha: 1,
                onComplete: function() {
                    self.animations({
                        action: "left-block-1",
                        block: self.settings.leftBlock.eq(1),
                    })
                }
            });
            break;
        case "left-show-history-2":

            var block1 = self.settings.leftBlock.eq(1).find(".part-1");
            var block2 = self.settings.leftBlock.eq(1).find(".part-2");

            block1.css('display', 'none');
            TweenLite.to([block1, block1.find(".step-show")], 0.15, {
                autoAlpha: 0
            });

            block2.css('display', 'block');
            TweenLite.to(block2, 0.15, {
                autoAlpha: 1,
                onComplete: function() {
                    self.animations({
                        action: "left-block-1.1",
                        block: block2,
                    })
                }
            });
            break;
        case "left-show-block":

            self.animations({
                action: "left-hidde-block"
            });

            var number = params.el.data("block") || 0;
            var block = self.settings.leftBlock.eq(number);

            block.css('display', 'block');
            TweenLite.to(block, 0.15, {
                autoAlpha: 1,
                onComplete: function() {
                    self.animations({
                        action: "left-block-" + number,
                        block: block,
                    })
                }
            });
            break;
        case "left-block-0":
            var duration = 0.15;
            var delay = 0;
            var elements = params.block.find(".step-show");

            TweenLite.set(params.block.find(".step-big"), {
                scale: 1.3,
                delay: 0.2,
                autoAlpha: 1,
            });
            TweenLite.to(params.block.find(".step-big"), 0.15, {
                scale: 1,
                delay: 0.2,
                ease: Linear.easeNone
            });

            for (var i = 0; i < elements.length; i++) {

                TweenLite.fromTo(elements[i], 0.15, {
                    autoAlpha: 0,
                    y: 25
                }, {
                    autoAlpha: 1,
                    y: 0,
                    delay: delay,
                    ease: Cubic.easeOut
                });
                delay = delay + 0.1;
            };
            break;
        case "left-block-1":

            var block1 = self.settings.leftBlock.eq(1).find(".part-1");
            var block2 = self.settings.leftBlock.eq(1).find(".part-2");

            block2.css('display', 'none');
            TweenLite.to([block2, block2.find(".step-show")], 0.15, {
                autoAlpha: 0,
                onComplete: function(){
                    $("#historia-up span").removeAttr("style");
                    $("#historia-down span").removeAttr("style");
                },
            });
            
            $("#historia-down").removeClass('null');
            $("#historia-up").addClass('null');

            block1.css('display', 'block');
            TweenLite.to(block1, 0, {
                autoAlpha: 1,
                onComplete: function() {

                    var duration = 0.15;
                    var delay = 0;
                    var elements = block1.find(".step-show");
                   
                    for (var i = 0; i < elements.length; i++) {
                        TweenLite.fromTo(elements[i], 0.15, {
                            autoAlpha: 0,
                            y: 25
                        }, {
                            autoAlpha: 1,
                            y: 0,
                            delay: delay,
                            ease: Cubic.easeOut
                        });
                        delay = delay + 0.1;
                    };
                }
            });


            break;
        case "left-block-1.1":
            var duration = 0.15;
            var delay = 0;
            var elements = params.block.find(".step-show");

            for (var i = 0; i < elements.length; i++) {

                TweenLite.fromTo(elements[i], 0.15, {
                    autoAlpha: 0,
                    y: 25
                }, {
                    autoAlpha: 1,
                    y: 0,
                    delay: delay,
                    ease: Cubic.easeOut
                });
                delay = delay + 0.1;
            };
            break;
        case "left-block-2":

            var duration = 0.15;
            var delay = 0.1;
            var elements = params.block.find(".step-show");

            TweenLite.fromTo(params.block.find(".step-big"), 0.15, {
                scale: 1.3,
                autoAlpha: 1
            }, {
                scale: 1,
                ease: Linear.easeNone
            });
            for (var i = 0; i < elements.length; i++) {

                TweenLite.fromTo(elements[i], 0.15, {
                    autoAlpha: 0,
                    y: 25
                }, {
                    autoAlpha: 1,
                    y: 0,
                    delay: delay,
                    ease: Cubic.easeOut
                });
                delay = delay + 0.1;
            };
            break;
        case "left-block-3":
            var duration = 0.15;
            var delay = 0;
             
            var elements = params.block.find(".step-show");
            for (var i = 0; i < elements.length; i++) {

                TweenLite.fromTo(elements[i], 0.15, {
                    autoAlpha: 0,
                    y: 25
                }, {
                    autoAlpha: 1,
                    y: 0,
                    delay: delay,
                    ease: Cubic.easeOut
                });
                delay = delay + 0.1;
            };
            break;


        case "right-hidde-block":

            self.settings.rightBlock.css('display', 'none');
            TweenLite.to([self.settings.rightBlock, self.settings.rightBlock.find(".step-show")], 0.15, {
                autoAlpha: 0
            });
            break;
        case "right-show-block":

            self.animations({
                action: "right-hidde-block"
            });

            var number = params.el.data("block") || 0;
            var block = self.settings.rightBlock.eq(number);

            block.css('display', 'block');
            TweenLite.to(block, 0.15, {
                autoAlpha: 1,
                onComplete: function() {
                    self.animations({
                        action: "right-block-" + number,
                        block: block,
                    })
                }
            });
            break;
        case "right-show-bio-1":

            var block1 = self.settings.rightBlock.eq(0).find(".bio-section-p1");
            var block2 = self.settings.rightBlock.eq(0).find(".bio-section-p2");

            block2.css('display', 'none');
            TweenLite.to([block2, block2.find(".step-show")], 0.15, {
                autoAlpha: 0
            });

            block1.css('display', 'block');
            TweenLite.to(block1, 0.15, {
                autoAlpha: 1,
                onComplete: function() {
                    self.animations({
                        action: "right-block-0",
                        block: self.settings.rightBlock.eq(0),
                    })
                }
            });
            break;
        case "right-show-bio-2":

            var block1 = self.settings.rightBlock.eq(0).find(".bio-section-p1");
            var block2 = self.settings.rightBlock.eq(0).find(".bio-section-p2");

            block1.css('display', 'none');
            TweenLite.to([block1, block1.find(".step-show")], 0.15, {
                autoAlpha: 0
            });

            block2.css('display', 'block');
            TweenLite.to(block2, 0.15, {
                autoAlpha: 1,
                onComplete: function() {
                    self.animations({
                        action: "right-block-0.1",
                        block: block2,
                    })
                }
            });
            break;
        case "right-block-0":
            var block1 = self.settings.rightBlock.eq(0).find(".bio-section-p1");
            var block2 = self.settings.rightBlock.eq(0).find(".bio-section-p2");

            block2.css('display', 'none');
            TweenLite.to([block2, block2.find(".step-show")], 0.15, {
                autoAlpha: 0
            });
            $("#bio-down").removeClass('null');
            $("#bio-up").addClass('null');

            block1.css('display', 'block');
            TweenLite.to(block1, 0, {
                autoAlpha: 1,
                onComplete: function() {

                    var duration = 0.15;
                    var delay = 0;
                    var elements = block1.find(".step-show");

                    for (var i = 0; i < elements.length; i++) {

                        TweenLite.fromTo(elements[i], 0.15, {
                            autoAlpha: 0,
                            y: 25
                        }, {
                            autoAlpha: 1,
                            y: 0,
                            delay: delay,
                            ease: Cubic.easeOut
                        });
                        delay = delay + 0.1;
                    };
                    TweenLite.set(params.block.find(".step-big"), {
                        scale: 1.2,
                        delay: 0.2,
                        autoAlpha: 1,
                    });
                    TweenLite.to(params.block.find(".step-big"), 0.15, {
                        scale: 1,
                        delay: 0.2,
                        ease: Linear.easeNone
                    });
                }
            });
            break;
        case "right-block-0.1":
            var duration = 0.15;
            var delay = 0;
            var elements = params.block.find(".step-show");
            for (var i = 0; i < elements.length; i++) {

                TweenLite.fromTo(elements[i], 0.15, {
                    autoAlpha: 0,
                    y: 25
                }, {
                    autoAlpha: 1,
                    y: 0,
                    delay: delay,
                    ease: Cubic.easeOut
                });
                delay = delay + 0.1;
            };
            break;
        case "right-block-1":
            var duration = 0.15;
            var delay = 0;
            var elements = params.block.find(".step-show");
            for (var i = 0; i < elements.length; i++) {

                TweenLite.fromTo(elements[i], 0.15, {
                    autoAlpha: 0,
                    y: 25
                }, {
                    autoAlpha: 1,
                    y: 0,
                    delay: delay,
                    ease: Cubic.easeOut
                });
                delay = delay + 0.1;
            };
            break;
        case "right-block-2":
            var duration = 0.15;
            var delay = 0;
            var elements = params.block.find(".step-show");
            for (var i = 0; i < elements.length; i++) {

                TweenLite.fromTo(elements[i], 0.15, {
                    autoAlpha: 0,
                    y: 25
                }, {
                    autoAlpha: 1,
                    y: 0,
                    delay: delay,
                    ease: Cubic.easeOut
                });
                delay = delay + 0.1;
            };
            break;
    };
};