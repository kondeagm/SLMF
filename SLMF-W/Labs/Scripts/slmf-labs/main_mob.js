List = function() {

	this.settings = {

		a: $SlmfTeam.find("ul.team a"),
		li: $SlmfTeam.find("ul.team li"),
		header: $SlmfTeam.find("header"),
		container: $SlmfTeam.find("#slmf-team-home-container"),
		is_Mobile: false,
		is_TabletLandscape: false,
		slider: {
			item: $(".slmf_slider_container"),
			mover: $("#labs_slider"),
			fullcontainer : $("#fullContainer"),
			busy: false,
			current: 0,
			sections: [
				{
					name     : "home",
					type     : "custom",
					timeline : null,
					element  : $("#labs_slider_home"),
					height   : 0,
					top      : 0,
					menuTl   : null,
					menuEl   : $("#bullet_home"),
				},
				{
					name     : "energia",
					type     : "std",
					timeline : null,
					element  : $("#labs_slider_energia"),
					height   : 0,
					top      : 0,
					menuTl   : null,
					menuEl   : $("#bullet_item1"),
				},
				{
					name     : "desarrollo",
					type     : "std",
					timeline : null,
					element  : $("#labs_slider_desarrollo"),
					height   : 0,
					top      : 0,
					menuTl   : null,
					menuEl   : $("#bullet_item2"),
				},
				{
					name     : "regula",
					type     : "std",
					timeline : null,
					element  : $("#labs_slider_regula"),
					height   : 0,
					top      : 0,
					menuTl   : null,
					menuEl   : $("#bullet_item3"),
				},
				{
					name     : "especiales",
					type     : "custom",
					timeline : null,
					element  : $("#labs_slider_especiales"),
					height   : 0,
					top      : 0,
					menuTl   : null,
					menuEl   : $("#bullet_item4"),
				},
			],
		},
	};

	var totalOffset = 0;
	this.lastActive = null;
};

List.prototype.init = function() {

	this.settings.is_Mobile= (isMobile.apple.phone || isMobile.android.phone || isMobile.seven_inch || isMobile.apple.tablet );
	if (this.settings.is_Mobile && $(document).width()>1023)
	{
		this.settings.is_TabletLandscape = true;
	}
	
	this.initTimeline();
	this.bind();
	this.wResize(null);
	slider = this.settings.slider;
	self = this;
	var controller = new ScrollMagic.Controller();
	

	var scene = new ScrollMagic.Scene({
			triggerElement: "#labs_slider_energia"
		})
		.on("start",function(event){
			self.reachSection(event,slider.sections[1],slider.sections[0]);
		})
		.addIndicators({name: "1 (duration: 0)"}) // add indicators (requires plugin)		
		.addTo(controller);

	var scene2 = new ScrollMagic.Scene({
			triggerElement: "#labs_slider_desarrollo"
		})
		.on("start",function(event){
			self.makeStdTimeline(slider.sections[2].element);
			self.reachSection(event,slider.sections[2],slider.sections[1]);
		})
		.addIndicators({name: "2 (duration: 0)"}) // add indicators (requires plugin)		
		.addTo(controller);

	var scene3 = new ScrollMagic.Scene({
			triggerElement: "#labs_slider_regula"
		})
		.on("start",function(event){
			self.reachSection(event,slider.sections[3],slider.sections[2]);
		})
		.addIndicators({name: "3 (duration: 0)"}) // add indicators (requires plugin)
		//.reverse(false)
		.addTo(controller);

	var scene4 = new ScrollMagic.Scene({
			triggerElement: "#team_slider_especiales"
		})
		.on("start",function(event){
			self.reachSection(event,slider.sections[4],slider.sections[3]);
		})
		.addIndicators({name: "4 (duration: 0)"}) // add indicators (requires plugin)
		//.reverse(false)
		.addTo(controller);
	
};

List.prototype.initHomeAnimation = function()
{
	this.settings.slider.sections[0].timeline.play(0);
	this.settings.slider.sections[0].menuTl.play(0);	
};
List.prototype.reachSection = function(event,sectionEnter,sectionExit)
{
	
	if(event.progress==1)
	{
		if(!sectionEnter.menuEl.hasClass("hover"))
			sectionEnter.menuTl.play(0);
		

		sectionExit.menuTl.reverse(1);

		if(sectionEnter.type=="custom")
			sectionEnter.timeline.play(0);
		else
			self.makeStdTimeline(sectionEnter.element);

		if(sectionExit.type=="custom")
			sectionExit.timeline.reverse(1);
		else
			this.makeStdReverseTimeline(sectionExit.element,false);

		sectionEnter.menuEl.addClass("active");
		sectionExit.menuEl.removeClass("active");
		
	}
	else
	{
		sectionEnter.menuTl.reverse(1);
		
		if(sectionExit.type=="custom")
			sectionExit.timeline.play(0);
		else
			self.makeStdTimeline(sectionExit.element);

		if(!sectionExit.menuEl.hasClass("hover"))
			sectionExit.menuTl.play(0);

		if(sectionEnter.type=="custom")
			sectionEnter.timeline.reverse(1);
		else
			this.makeStdReverseTimeline(sectionEnter.element,false);

		sectionEnter.menuEl.removeClass("active");
		sectionExit.menuEl.addClass("active");
	}
};
List.prototype.bind = function() {

	var self = this;
	//$("body").on("keydown",function(event){self.keydown(event,self);});
	// Hover
	$(".bullets a").on("mouseenter",function(event){self.playTimelimeMenu(event);});
	$(".bullets a").on("mouseleave",function(event){self.reverseTimelimeMenu(event);});
	
	$(".bullets a").on("click",function(event){self.menuClick(event,self);});
	

	$(".labs_changer_menu a").on("click",function(event){self.changer(event,self);});
	
	$(".home_menu_tab_btn").on("click",function(event){self.showInfo(event,self);});
	
	if(!this.settings.is_Mobile)
		$("#team_slider_especiales .btn_container div").on("mouseenter",function(event){self.specialsProductPlay(event,self);});

	$(".labs_changer_menu_specials a").on("click",function(event){self.specialsProductPlay(event,self);});
	
	$(window).on("resize",function(event){self.wResize(event);});
	//$(this.settings.slider.fullcontainer).on("scroll",function(event){self.scrollMove();});
		
};

List.prototype.initTimeline = function()
{
	slider= this.settings.slider;
	// HOME
	this.makeHomeTimeline(slider);
	this.makeEspecialesTimeline(slider);
	
	//menu timeline	
	for(var i=0; i<slider.sections.length;i++)
	{
		slider.sections[i].menuTl = this.makeStdMenuTimeline(slider.sections[i].menuEl);
		this.initSectionsTimeline(slider.sections[i].el);

	}
		
};

List.prototype.makeHomeTimeline= function(slider)
{
	homeBg = $("#labs_slider_home .bg_lights",slider.item);
	homeFigure1 = $("#labs_slider_home .bg_figure",slider.item);
	
	homeLogo    = $("#labs_home_logo svg",slider.item);
	homeSpacer  = $(".header_spacer",slider.item);
	
	homeText    = $(".main_container p",slider.item);
	
	slider.sections[0].timeline = new TimelineLite({});
	slider.sections[0].timeline.fromTo(homeFigure1, 0.45,{ autoAlpha: 0},
		{
			autoAlpha: 1,
			ease: Linear.easeNone,
		});
	slider.sections[0].timeline.fromTo(homeBg, 0.80,{ autoAlpha: 0},
		{
			autoAlpha: 1,
			ease: Linear.easeNone,
		},'=-0.2');
	
	slider.sections[0].timeline.fromTo(homeLogo, 0.45,{ autoAlpha: 0,x: -50},
		{
			autoAlpha: 1,
			x: 0,
			ease: Cubic.easeInOut,
		},'=-0.60');

	slider.sections[0].timeline.fromTo(homeText, 0.30,{ autoAlpha: 0,y: 45},
		{
			autoAlpha: 1,
			y: 0,
			ease: Cubic.easeOut,
		},'=-0.2');

	
	slider.sections[0].timeline.pause(0);
};
List.prototype.makeEspecialesTimeline= function(slider)
{
	// Kai
	/*
	kaiBg = $("#team_slider_kai .bg_lights",slider.item);
	kaiFigure = $("#team_slider_kai .bg_figure",slider.item);

	kaiTitle1    = $("#team_slider_kai .bg_text",slider.item);
	kaiTitle2    = $("#team_slider_kai_txt_2",slider.item);
	kaiH3    = $("#team_slider_kai h3",slider.item);
	kaiText    = $("#team_slider_kai p",slider.item);
	kaiBtn    = $(".home_content_btn ",slider.item);*/
	
	slider.tlEspeciales = new TimelineLite({});
	/*
	slider.tlEspeciales.fromTo(kaiFigure, 0.45,{ autoAlpha: 0},
		{
			autoAlpha: 1,
			ease: Linear.easeNone,
		});
	slider.tlKai.fromTo(kaiBg, 0.45,{ autoAlpha: 0},
		{
			autoAlpha: 1,
			ease: Linear.easeNone,
		},'=-0.2');
	slider.tlKai.fromTo(kaiTitle1, 0.45,{ autoAlpha: 0,x: -45},
		{
			autoAlpha: 1,
			x: 0,
			ease: Cubic.easeOut,
		},'=-0.3');
	slider.tlKai.fromTo(kaiTitle2, 0.45,{ autoAlpha: 0,x: -45},
		{
			autoAlpha: 1,
			x: 0,
			ease: Cubic.easeOut,
		},'=-0.3');

	slider.tlKai.fromTo(kaiH3, 0.45,{ autoAlpha: 0,y: 45},
		{
			autoAlpha: 1,
			y: 0,
			ease: Cubic.easeOut,
		},'=-0.15');

	slider.tlKai.fromTo(kaiText, 0.45,{ autoAlpha: 0},
		{
			autoAlpha: 1,
			ease: Linear.easeNone,
		}, "-=0.15");

	
	slider.tlKai.fromTo(kaiBtn, 0.3, {
				scale: 1.6,
				autoAlpha: 0
			}, {
				
				scale: 1.0,
				autoAlpha: 1,
				ease: Power2.easeIn,
			},'=-0.15');
	slider.tlKai.timeScale(1.3);
	*/
	slider.tlEspeciales.pause(0);
};

List.prototype.specialsProductPlay= function(event,_self)
{
	event.preventDefault();
	btn = $(event.currentTarget);
	if(btn.hasClass("current")) return false;
	
	slider_item = $("#team_slider_especiales");
	if(_self.settings.is_Mobile)
		$(".labs_changer_menu_specials .current",slider_item).removeClass("current");
	
		$(".btn_container .current",slider_item).removeClass("current");

	btn.addClass("current");
	if(_self.settings.is_Mobile)
		newItem= btn.attr("id").replace("menu_labs_","");
	else
		newItem= btn.attr("id").replace("_over","");
		
	console.log(newItem);
	
	timeline = new TimelineLite({});
	_current_Dark = $(".current.dark",slider_item);
	
	
	_current_Text = $(".current.paddingl_150",slider_item);
	
	//slideItem_Bg = $(".bg_lights",slider_item);
	slideItem_Dark = $("."+newItem+".dark",slider_item);
	

	slideItem_Text = $("."+newItem+".paddingl_150",slider_item);
	
	/*
	slideItem_Title1 = $(" .bg_text",slider_item);
	slideItem_menu = $(" .labs_changer_menu",slider_item);
	slideItem_H3        = $("h3",_current);
	slideItem_Text      = $(".description",_current);
	slideItem_Card      = $(".team_ficha_container",_current);
	slideItem_CardItem1 = $("ul li",_current).eq(0);
	slideItem_CardItem2 = $("ul li",_current).eq(1);
	slideItem_CardItem3 = $("ul li",_current).eq(2);
	slideItem_btn     = $(".home_content_btn",_current);
	*/
	timeline.timeScale(1.5);
	//timeline.set(slideItem_Title1,{opacity:0,visibily: 'hidden'});
	//bulletLeft = 100;
	//if(slider_item.hasClass("labs_left")) bulletLeft = -100;

	timeline.fromTo(_current_Dark, 0.25,{ autoAlpha: 0},
		{ autoAlpha: 1,ease: Linear.easeNone,});
	timeline.fromTo(_current_Text, 0.25,{ autoAlpha: 1},
		{ autoAlpha: 0,ease: Linear.easeNone,},"-=0.25");

	timeline.fromTo(slideItem_Dark, 0.25,{ autoAlpha: 1},
		{ autoAlpha: 0,ease: Linear.easeNone,},"-=0.25");
	timeline.fromTo(slideItem_Text, 0.25,{ autoAlpha: 0},
		{ autoAlpha: 1,ease: Linear.easeNone,},"-=0.25");

	slideItem_Dark.addClass("current");
	slideItem_Text.addClass("current");
	if(_self.settings.is_Mobile)
		$(".btn_container #"+newItem+"_over",slider_item).addClass("current");

	_current_Dark.removeClass("current");
	_current_Text.removeClass("current");
	
	/*timeline.fromTo(slideItem_Bg, 0.45,{ autoAlpha: 0},
		{ autoAlpha: 1,ease: Linear.easeNone,},'=-0.2');
	
	

	timeline.fromTo(slideItem_Title1, 1.2,{ autoAlpha: 0.5,y: "-100%"},
		{ autoAlpha: 1,y: 0,ease: Cubic.easeOut,},'=-0.3');
	
	timeline.fromTo(slideItem_menu, 0.45,{ autoAlpha: 0},
		{ autoAlpha: 1,
			ease: Cubic.easeOut,
		},'=-0.65');

	timeline.fromTo(slideItem_H3, 0.45,{ autoAlpha: 0,y: 45},
		{ autoAlpha: 1,y: 0,
			ease: Cubic.easeOut,
		},'-=0.2');
		timeline.fromTo(slideItem_Text, 0.30,{ autoAlpha: 0},
		{
			autoAlpha: 1,
			ease: Linear.easeNone,
		}, "-=0.15");

	timeline.fromTo(slideItem_Card, 0.30,{ autoAlpha: 0,height: 0},
		{
			autoAlpha: 1,
			height: 97,
			ease: Linear.easeNone,
		}, "-=0.15");

	timeline.fromTo(slideItem_CardItem1, 0.30,{ autoAlpha: 0,x: bulletLeft},
		{
			autoAlpha: 1,
			x: 0,
			ease: Cubic.easeOut,
		}, "-=0.15");
	timeline.fromTo(slideItem_CardItem2, 0.30,{ autoAlpha: 0,x: bulletLeft},
		{
			autoAlpha: 1,
			x: 0,
			ease: Cubic.easeOut,
		}, "-=0.15");
	timeline.fromTo(slideItem_CardItem3, 0.30,{ autoAlpha: 0,x: bulletLeft},
		{
			autoAlpha: 1,
			x: 0,
			ease: Cubic.easeOut,
		});

	timeline.fromTo(slideItem_btn, 0.3, {
				scale: 1.6,
				autoAlpha: 0
			}, {
				
				scale: 1.0,
				autoAlpha: 1,
				ease: Power2.easeIn,
			},'=-0.15');
			*/
};

List.prototype.makeStdTimeline=function (slider_item,isLeft)
{
	timeline = new TimelineLite({});
	_current = $(".current",slider_item);
	_info_current = $(".labs_info_container.current",_current);
	
	slideItem_Figure = $(".bg_figure",_current);
	slideItem_Title1 = $(" .bg_text",slider_item);
	slideItem_menu = $(" .labs_changer_menu",slider_item);
	slideItem_H3        = $("h3",_info_current);
	slideItem_Text      = $(".description",_info_current);
	slideItem_Card      = $(".team_ficha_container",_info_current);
	slideItem_CardItem1 = $("ul li",_info_current).eq(0);
	slideItem_CardItem2 = $("ul li",_info_current).eq(1);
	slideItem_CardItem3 = $("ul li",_info_current).eq(2);
	slideItem_btn     = $(".home_content_btn",_current);
	
	timeline.timeScale(1.5);
	//timeline.set(slideItem_Title1,{opacity:0,visibily: 'hidden'});
	bulletLeft = 100;
	if(slider_item.hasClass("labs_left")) bulletLeft = -100;

	timeline.fromTo(slideItem_Figure, 0.45,{ autoAlpha: 0},
		{ autoAlpha: 1,ease: Linear.easeNone,});
	
	/*timeline.fromTo(slideItem_Bg, 0.45,{ autoAlpha: 0},
		{ autoAlpha: 1,ease: Linear.easeNone,},'=-0.2');*/
	
	

	timeline.fromTo(slideItem_Title1, 1.2,{ autoAlpha: 0.5,y: "-100%"},
		{ autoAlpha: 1,y: 0,ease: Cubic.easeOut,},'=-0.3');
	
	timeline.fromTo(slideItem_menu, 0.45,{ autoAlpha: 0},
		{ autoAlpha: 1,
			ease: Cubic.easeOut,
		},'=-0.65');

	timeline.fromTo(slideItem_H3, 0.45,{ autoAlpha: 0,y: 45},
		{ autoAlpha: 1,y: 0,
			ease: Cubic.easeOut,
		},'-=0.2');
		timeline.fromTo(slideItem_Text, 0.30,{ autoAlpha: 0},
		{
			autoAlpha: 1,
			ease: Linear.easeNone,
		}, "-=0.15");

	timeline.fromTo(slideItem_Card, 0.30,{ autoAlpha: 0,height: 0},
		{
			autoAlpha: 1,
			height: 97,
			ease: Linear.easeNone,
		}, "-=0.15");

	timeline.fromTo(slideItem_CardItem1, 0.30,{ autoAlpha: 0,x: bulletLeft},
		{
			autoAlpha: 1,
			x: 0,
			ease: Cubic.easeOut,
		}, "-=0.15");
	timeline.fromTo(slideItem_CardItem2, 0.30,{ autoAlpha: 0,x: bulletLeft},
		{
			autoAlpha: 1,
			x: 0,
			ease: Cubic.easeOut,
		}, "-=0.15");
	timeline.fromTo(slideItem_CardItem3, 0.30,{ autoAlpha: 0,x: bulletLeft},
		{
			autoAlpha: 1,
			x: 0,
			ease: Cubic.easeOut,
		});

	timeline.fromTo(slideItem_btn, 0.3, {
				scale: 1.6,
				autoAlpha: 0
			}, {
				
				scale: 1.0,
				autoAlpha: 1,
				ease: Power2.easeIn,
			},'=-0.15');
	
	//timeline.pause(0);
	
	//return timeline;
};
List.prototype.initSectionsTimeline= function (slider_item)
{
	_current = $(".current",slider_item);
	_info_current = $(".labs_info_container.current",_current);
	//slideItem_Bg = $(".bg_lights",slider_item);
	slideItem_Figure = $(".bg_figure",_current);
	slideItem_Title1 = $(" .bg_text",slider_item);
	slideItem_menu = $(" .labs_changer_menu",slider_item);
	slideItem_H3        = $("h3",_info_current);
	slideItem_Text      = $(".description",_info_current);
	slideItem_Card      = $(".team_ficha_container",_info_current);
	slideItem_CardItem1 = $("ul li",_info_current).eq(0);
	slideItem_CardItem2 = $("ul li",_info_current).eq(1);
	slideItem_CardItem3 = $("ul li",_info_current).eq(2);
	slideItem_btn     = $(".home_content_btn",_current);

	slideItem_Figure.css("opacity",0);
	slideItem_Title1.css("opacity",0);
	slideItem_menu.css("opacity",0);
	slideItem_H3.css("opacity",0);
	slideItem_Text.css("opacity",0);
	slideItem_Card.css("opacity",0);
	slideItem_CardItem1.css("opacity",0);
	slideItem_CardItem2.css("opacity",0);
	slideItem_CardItem3.css("opacity",0);
	slideItem_btn.css("opacity",0);

};

List.prototype.makeStdReverseTimeline=function (slider_item,isLeft)
{
	timeline = new TimelineLite({});
	_current = $(".current",slider_item);
	_info_current = $(".labs_info_container.current",_current);
	//slideItem_Bg = $(".bg_lights",slider_item);
	slideItem_Figure = $(".bg_figure",_current);
	slideItem_Title1 = $(" .bg_text",slider_item);
	slideItem_menu = $(" .labs_changer_menu",slider_item);
	slideItem_H3        = $("h3",_info_current);
	slideItem_Text      = $(".description",_info_current);
	slideItem_Card      = $(".team_ficha_container",_info_current);
	slideItem_CardItem1 = $("ul li",_info_current).eq(0);
	slideItem_CardItem2 = $("ul li",_info_current).eq(1);
	slideItem_CardItem3 = $("ul li",_info_current).eq(2);
	slideItem_btn     = $(".home_content_btn",_current);
	
	timeline.timeScale(1.5);
	//timeline.set(slideItem_Title1,{opacity:0,visibily: 'hidden'});
	bulletLeft = 100;
	if(slider_item.hasClass("labs_left")) bulletLeft = -100;

	timeline.fromTo(slideItem_Figure, 0.45,{ autoAlpha: 1},
		{ autoAlpha: 0,ease: Linear.easeNone,});
	
	/*timeline.fromTo(slideItem_Bg, 0.45,{ autoAlpha: 0},
		{ autoAlpha: 1,ease: Linear.easeNone,},'=-0.2');*/
	
	

	timeline.fromTo(slideItem_Title1, 1.2,{ autoAlpha: 1},
		{ autoAlpha: 0},'=-0.3');
	
	timeline.fromTo(slideItem_menu, 0.45,{ autoAlpha: 1},
		{ autoAlpha: 0,
			ease: Cubic.easeOut,
		},'=-0.65');

	timeline.fromTo(slideItem_H3, 0.45,{ autoAlpha: 1,y: 0},
		{ autoAlpha: 0,y: 45,
			ease: Cubic.easeOut,
		},'-=0.2');
	timeline.fromTo(slideItem_Text, 0.30,{ autoAlpha: 1},
		{
			autoAlpha: 0,
			ease: Linear.easeNone,
		}, "-=0.15");

	timeline.fromTo(slideItem_Card, 0.30,{ autoAlpha: 1,height: 97},
		{
			autoAlpha: 0,
			height: 0,
			ease: Linear.easeNone,
		}, "-=0.15");

	timeline.fromTo(slideItem_CardItem1, 0.30,{ autoAlpha: 1,x: 0},
		{
			autoAlpha: 0,
			x: bulletLeft,
			ease: Cubic.easeOut,
		}, "-=0.15");
	timeline.fromTo(slideItem_CardItem2, 0.30,{ autoAlpha: 1,x: 0},
		{
			autoAlpha: 0,
			x: bulletLeft,
			ease: Cubic.easeOut,
		}, "-=0.15");
	timeline.fromTo(slideItem_CardItem3, 0.30,{ autoAlpha: 1,x: 0},
		{
			autoAlpha: 0,
			x: bulletLeft,
			ease: Cubic.easeOut,
		});

	timeline.fromTo(slideItem_btn, 0.3, {
				
				autoAlpha: 1
			}, {
								
				autoAlpha: 0,
				ease: Power2.easeIn,
			},'=-0.15');
	
	//timeline.pause(0);
	
	//return timeline;
};
List.prototype.makeStdMenuTimeline=function (menu_item)
{
	timeline = new TimelineLite({});

	menu_item_p = $(".p_container p",menu_item);
	menu_item_under = $(".under",menu_item);
	//timeline.set(menu_item_under,{opacity:0,visibily: 'hidden'});
	if(!this.settings.is_Mobile)
	{
		timeline.fromTo(menu_item_p, 0.25,{ autoAlpha: 0,y: 25},
			{ autoAlpha: 1,y: 0,ease: Cubic.easeOut,});
		timeline.fromTo(menu_item_under, 0.25,{ width: 15,backgroundColor: "#ffffff"},
		{ width: 50,backgroundColor: "#e7d133",ease: Cubic.easeOut,},'=-0.25');
	}
	else
	{
		timeline.fromTo(menu_item_under, 0.25,{ backgroundColor: "#ffffff"},
		{ backgroundColor: "#e7d133",ease: Cubic.easeOut,},'=-0.25');
	}

	timeline.pause();
	return timeline;
};
List.prototype.keydown = function(event,env)
{
	switch(event.which)
	{
		case 38:env.prev();
		break;
		case 40:env.next();
		break;
		/* shortcuts */
		case 49:this.settings.slider.current= 0;
			this.gotoSlide(false);
		break;
		case 50:this.settings.slider.current= 1;
			this.gotoSlide(false);
		break;
		case 51:this.settings.slider.current= 2;
			this.gotoSlide(false);
		break;
		case 52:this.settings.slider.current= 3;
			this.gotoSlide(false);
		break;
		case 53:this.settings.slider.current= 4;
			this.gotoSlide(false);
		break;
	}
};
List.prototype.next= function()
{
	if(this.settings.slider.busy){return false;}

	if(this.settings.slider.current==4){return false;}

	this.settings.slider.busy= true;
	this.settings.slider.current++;
	this.gotoSlide(false);
};
List.prototype.prev= function()
{
	
	if(this.settings.slider.busy){return false;}
	if(this.settings.slider.current===0){return false;}

	this.settings.slider.busy= true;
	this.settings.slider.current--;
	this.gotoSlide(false);
};
List.prototype.gotoSlide= function(isFromBtn)
{
	
	slider= this.settings.slider;
	self = this;

	btn= $(".bullet_item.active");
	btn.removeClass("active");
		
	current = this.settings.slider.current;
	newSlide = slider.sections[current];
	slider.sections[current].menuEl.addClass("active");
	/*
	switch(btn.attr("id"))
	{
		case 'bullet_home':slider.sections[0].menuTl.reverse(1);
		break;
		case 'bullet_item1':slider.sections[1].menuTl.reverse(1);
		break;
		case 'bullet_item2':slider.sections[2].menuTl.reverse(1);
		break;
		case 'bullet_item3':slider.sections[3].menuTl.reverse(1);
		break;
		case 'bullet_item4':slider.sections[4].menuTl.reverse(1);
		break;
	}
	*/
	
	if(current==1 || current==4 )
	{
		timeline.pause(0);
	}
	if(this.settings.is_Mobile && !this.settings.is_TabletLandscape)
	{
		TweenMax.to(slider.fullcontainer, 0.7, {
			scrollTo: { y: newSlide.top},
			ease: Cubic.easeInOut,
			onComplete:function()
			{
				self.settings.slider.busy = false;
				//timeline.play(0);
				if(!isFromBtn)
					newSlide.menuTl.play(0);
			}
		});
	}
	else
	{
		TweenMax.to(slider.mover, 0.7, {
			top: newSlide.top,
			ease: Cubic.easeInOut,
			onComplete:function()
			{
				self.settings.slider.busy = false;
				//timeline.play(0);
				if(!isFromBtn)
					newSlide.menuTl.play(0);
			}
		});
	}
	
	/*
	if(current>0 && current <4)
	{
		setTimeout(function(){self.makeStdTimeline(newSlide.element);},0.3);
	}
	else
	{
		setTimeout(function(){newSlide.timeline.play(0);},0.3);
	}
	*/
	
};

List.prototype.wResize= function(event)
{
	
	if(this.settings.is_Mobile && !this.settings.is_TabletLandscape)
	{
		for(var i=0; i<this.settings.slider.sections.length;i++)
		{
			this.settings.slider.sections[i].height = this.settings.slider.sections[i].element.height();
			if(i===0)
			{
				this.settings.slider.sections[i].top = 0;
			}
			else
			{
				this.settings.slider.sections[i].top = this.settings.slider.sections[i-1].top+ this.settings.slider.sections[i-1].element.height();
			}
		}
	}
	else
	{
		for(var j=0; j<this.settings.slider.sections.length;j++)
		{
			
			this.settings.slider.sections[j].top = (j*-100)+"%";
			
		}
	}
	
	
	console.log(this.settings.slider.sections);
};
List.prototype.SlidermoveTo = function(to_,timeline,menuTimeline,isFromBtn,isStdAnimation)
{
	self = this;
	if(!isStdAnimation)
	{
		timeline.pause(0);
	}
	TweenMax.to(this.settings.slider.item, 0.7, {
		top: to_,
		ease: Cubic.easeInOut,
		onComplete:function()
		{
			self.settings.slider.busy = false;
			//timeline.play(0);
			if(!isFromBtn)
				menuTimeline.play(0);
		}
	});
	if(isStdAnimation)
	{
		setTimeout(function(){self.makeStdTimeline(timeline);},0.3);
	}
	else
	{
		setTimeout(function(){timeline.play(0);},0.3);
	}
};

List.prototype.playTimelimeMenu= function(event)
{
	btn = $(event.currentTarget);
	btn.addClass("hover");
	if($(btn).hasClass("active")) return false;
	btn.addClass("hover");

	var slider = this.settings.slider;
	switch(btn.attr("id"))
	{
		case 'bullet_home':slider.sections[0].menuTl.play(0);
		break;
		case 'bullet_item1':slider.sections[1].menuTl.play(0);
		break;
		case 'bullet_item2':slider.sections[2].menuTl.play(0);
		break;
		case 'bullet_item3':slider.sections[3].menuTl.play(0);
		break;
		case 'bullet_item4':slider.sections[4].menuTl.play(0);
		break;
	}
};
List.prototype.reverseTimelimeMenu= function(event)
{
	btn = $(event.currentTarget);
	btn.removeClass("hover");
	if($(btn).hasClass("active")) return false;
	
	var slider = this.settings.slider;
	switch(btn.attr("id"))
	{
		case 'bullet_home':slider.sections[0].menuTl.reverse(1);
		break;
		case 'bullet_item1':slider.sections[1].menuTl.reverse(1);
		break;
		case 'bullet_item2':slider.sections[2].menuTl.reverse(1);
		break;
		case 'bullet_item3':slider.sections[3].menuTl.reverse(1);
		break;
		case 'bullet_item4':slider.sections[4].menuTl.reverse(1);
		break;
	}
};
List.prototype.menuClick= function(event)
{
	event.preventDefault();
	if(this.settings.slider.busy)return false;
	if($(btn).hasClass("active")) return false;
	console.log("click");
	this.settings.slider.busy = true;
	
	btn = $(event.currentTarget);
	
	switch(btn.attr("id"))
	{
		case 'bullet_home':this.settings.slider.current=0;
		break;
		case 'bullet_item1':this.settings.slider.current=1;
		break;
		case 'bullet_item2':this.settings.slider.current=2;
		break;
		case 'bullet_item3':this.settings.slider.current=3;
		break;
		case 'bullet_item4':this.settings.slider.current=4;
		break;
	}
	this.gotoSlide(btn.hasClass("hover"));
};
List.prototype.changer= function(event)
{
	event.preventDefault();
	_btn= $(event.currentTarget);
	if(_btn.hasClass("current")) return false;

	if(this.settings.slider.busy) return false;
	this.settings.slider.busy= true;
	
	_parent= $(_btn).parent();

	id= _btn.attr('id').replace("menu_labs_","");
	
	$(".labs_changer_menu_item.current",_parent).removeClass("current");
	_btn.addClass("current");
	menuContainer = _parent.parent().parent();
	console.log("menuContainer");
	console.log(menuContainer);
	isleft = false;
	if(menuContainer.hasClass('labs_left'))isleft = true;
	this.changeProduct($("#labs_"+id,menuContainer),menuContainer,isleft);

};
List.prototype.changeProduct= function(section,changer,isLeft)
{
	var self = this;
	console.log("---------");
	console.log("changeProduct"+this.settings.slider.busy);
	current = $(".labs_changer_item.current",changer);
	console.log(section);
	if(isLeft)
	{
		distanceToMoveOut = -50;
		distanceToMoveIn = 50;
	}
	else
	{
		distanceToMoveOut = 50;
		distanceToMoveIn = -50;
	}
	
	exitTime = 0.15;
	inTime = 0.15;
	delayBetween = "-=0.1";
	timeline = new TimelineLite({});

	timeline.fromTo($(".bg_figure",current), exitTime,{ autoAlpha: 1},
		{
			autoAlpha: 0,
			ease: Linear.easeNone,
		});
	
	

	timeline.fromTo($("h3",current), exitTime,{ autoAlpha: 1,x:0},
		{
			autoAlpha: 0,
			ease: Cubic.easeIn,
			x:distanceToMoveOut,
		},delayBetween);
	timeline.fromTo($(".description",current), exitTime,{ autoAlpha: 1,x:0},
		{
			autoAlpha: 0,
			ease: Cubic.easeIn,
			x:distanceToMoveOut,
		},delayBetween);
	timeline.fromTo($("ul",current), exitTime,{ autoAlpha: 1,x:0},
		{
			autoAlpha: 0,
			ease: Cubic.easeIn,
			x:distanceToMoveOut,
		},delayBetween);
	timeline.fromTo($(".home_content_btn",current), exitTime,{ autoAlpha: 1,x:0},
		{
			autoAlpha: 0,
			ease: Cubic.easeIn,
			x:distanceToMoveOut,
		},delayBetween);
	


	section.addClass("current");





	timeline.fromTo($(".bg_figure",section), inTime,{ autoAlpha: 0},
		{
			autoAlpha: 1,
			ease: Cubic.easeInOut,
		});

	timeline.fromTo($("h3",section), inTime,{ autoAlpha: 0,x:distanceToMoveIn},
		{
			autoAlpha: 1,
			ease: Cubic.easeInOut,
			x:0
		},delayBetween);
	timeline.fromTo($(".description",section), inTime,{ autoAlpha: 0,x:distanceToMoveIn},
		{
			autoAlpha: 1,
			ease: Cubic.easeInOut,
			x:0
		},delayBetween);
	timeline.fromTo($("ul",section), inTime,{ autoAlpha: 0,x:distanceToMoveIn},
		{
			autoAlpha: 1,
			ease: Cubic.easeInOut,
			x:0
		},delayBetween);
	timeline.fromTo($(".home_content_btn",section), inTime,{ autoAlpha: 0,x:distanceToMoveIn},
		{
			autoAlpha: 1,
			ease: Cubic.easeInOut,
			x:0,
			onComplete: function()
			{
				current.removeClass("current");
				self.settings.slider.busy= false;
				
			}
		});
};

List.prototype.showInfo= function(event)
{
	event.preventDefault();
	_btn= $(event.currentTarget);
	if(_btn.hasClass("current")) return false;

	if(this.settings.slider.busy) return false;
	this.settings.slider.busy= true;
	
	_parent= $(_btn).parent().parent();

	
	
	$(".home_menu_tab_btn.current",_parent).removeClass("current");
	_btn.addClass("current");
	
	
	console.log("menuContainer");
	console.log(_parent);
	//isleft = false;
	if(_btn.hasClass("info"))
		classToShow = "info";
	else
		if(_btn.hasClass("tabla"))
			classToShow = "tabla";
		else
			classToShow = "dosis";

	this.changeInfo(_parent,classToShow);

};
List.prototype.changeInfo= function(_parent,classToShow)
{
	var self = this;
	console.log("---------");
	console.log("changeInfo"+this.settings.slider.busy);
	current = $(".labs_info_container.current",_parent);
	toShow = $(".labs_info_container."+classToShow,_parent);
	
	
	
	exitTime = 0.15;
	inTime = 0.15;
	delayBetween = "-=0.1";
	timeline = new TimelineLite({});

	timeline.fromTo(current, exitTime,{ autoAlpha: 1},
		{
			autoAlpha: 0,
			ease: Linear.easeNone,
		});

	timeline.fromTo(toShow, exitTime,{ autoAlpha: 0},
		{
			autoAlpha: 1,
			ease: Linear.easeNone,
			onComplete: function()
			{
				current.removeClass("current");
				toShow.addClass("current");
				self.settings.slider.busy= false;
				
			}
		});
	
	

};


List.prototype.animations = function(params) {
	var self = this;


	function hideTooltips(el) {
		if (el) {
			el = el.parent();

			TweenMax.to([$(el).find('.tooltip'), $(el).find('img.back')], 0.25, {
				autoAlpha: 0,
				ease: Linear.easeNone
			});
			self.lastActive = false;
		}
	}
	// console.log(params.action)
	switch (params.action) {
		case "mouseleave":

			hideTooltips(self.lastActive);
			break;
		case "mouseenter":
			var position = $(params.el).attr("data-position");
			hideTooltips(self.lastActive);

			self.lastActive = params.el;

			params.hover.css("z-index", 0);
			params.back.css("z-index", 1);
			TweenMax.to(params.back, 0.25, {
				autoAlpha: 1,
				ease: Linear.easeNone
			});

			$(this.settings.li).each(function(index, el) {
				var distance = $(el).find("a").attr("data-distance");
				TweenMax.to(el, .5, {
					left: position * 18 * distance,
					ease: Quad.easeOut,
					onComplete: function() {
						// Aparece icono de tooltip
						if (self.lastActive && self.lastActive.parent() != $(el)) {

							TweenMax.set(params.icon, {
								autoAlpha: 0
							});
							TweenMax.fromTo(params.icon, .35, {
								autoAlpha: 1,
								x: "300%",
								y: "-300%",
							}, {
								x: "0%",
								y: "0%",
								delay: .1,
								ease: Cubic.easeOut,
							});

							// Aparece nombre de atleta de tooltip
							TweenMax.set(params.header, {
								autoAlpha: 0
							});
							TweenMax.fromTo(params.header, .35, {
								autoAlpha: 1,
								x: "100%",
								y: "-100%",
							}, {
								x: "0%",
								y: "0%",
								ease: Cubic.easeOut,
							});

							// Aparece contenedor de tooltip
							TweenMax.set(params.tooltip, {
								autoAlpha: 1,
								x: "100%",
								y: "-100%",
								ease: Cubic.easeOut,
							});
							TweenMax.to(params.tooltip, .35, {
								x: "0%",
								y: "0%",
								ease: Cubic.easeOut,
							});
						}

					}
				});



			});
			break;
	}
};

SlmfTeam = {

	init: function(images) {

		this.cachedElements();

		list = new List();
		list.init();

		
		$SlmfTeam.removeLoader({
				onComplete: function() {
					
				}
			});
		/*
		_onLoadImages(images, function() {

			$SlmfTeam.removeLoader({
				onComplete: function() {
					list.initHomeAnimation();
				}
			});
		});*/
	},
	cachedElements: function() {

		$SlmfTeam = $("#slmf-team-home");
	},
};

$.fn.removeLoader = function(params) {
	
	loader = $("#loader");
	stage  = $("#stage");

	TweenMax.to(loader, 0.5, {
		autoAlpha: 0,
		onComplete: function() {
			loader
				.html('')
				.remove();
		}
	});

	TweenMax.to(stage, 0.5, {
		autoAlpha: 1,
		delay: 0.5,
		onComplete: function()
		{
			list.initHomeAnimation();
		}
		}
	);
};

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

			TweenMax.to(counter, 0.50, {
				var: Math.round((count / images.length) * 99),
				onUpdate: function() {
					// console.log(Math.round(counter.var), Math.round((count / images.length) * 99));
					loaderContent.html(Math.round(counter.var));
					TweenLite.to(loaderContent, 0.25, {
						opacity: counter.var / 100
					});
				},
				ease: Circ.easeOut
			});

			if (count >= images.length) {
				setTimeout(function() {

					onComplete();
					return true;
				}, 350);
			}
		};
	});
	return true;
};