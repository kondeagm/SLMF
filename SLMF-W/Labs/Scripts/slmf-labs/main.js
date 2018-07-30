List = function() {

	this.settings = {

		a: $SlmfTeam.find("ul.team a"),
		li: $SlmfTeam.find("ul.team li"),
		header: $SlmfTeam.find("header"),
		container: $SlmfTeam.find("#slmf-team-home-container"),
		is_Mobile: false,
		is_TabletLandscape: false,
		scrollDisabled: false,
		previousOrientation: '',
		slider: {
			item: $(".slmf_slider_container"),
			mover: $("#labs_slider"),
			fullcontainer : $("#fullContainer"),
			busy: false,
			productsBusy: false,
			current: 0,
			new: 0,
			sections: [
				{
					id       : 0,
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
					id       : 1,
					name     : "energia",
					type     : "std",
					timeline : null,
					element  : $("#labs_slider_energia"),
					height   : 0,
					top      : 0,
					menuTl   : null,
					menuEl   : $("#bullet_item1"),
					current  : 0,
					products : [
						{
							id     : 0,
							name   : "hysteria",
						},						
						{
							id     : 1,
							name   : "provoke",
						},
						{
							id     : 2,
							name   : "hysteria_l",
						},
					],
					

				},
				{
					id       : 2,
					name     : "desarrollo",
					type     : "std",
					timeline : null,
					element  : $("#labs_slider_desarrollo"),
					height   : 0,
					top      : 0,
					menuTl   : null,
					menuEl   : $("#bullet_item2"),
					current  : 0,
					products : [
						{
							id     : 0,
							name   : "pride",
						},
						{
							id     : 1,
							name   : "tenacious",
						},
					],
				},
				{
					id       : 3,
					name     : "regula",
					type     : "std",
					timeline : null,
					element  : $("#labs_slider_regula"),
					height   : 0,
					top      : 0,
					menuTl   : null,
					menuEl   : $("#bullet_item3"),
					current  : 0,
					products : [
						{
							id     : 0,
							name   : "revival",
						},
						{
							id     : 1,
							name   : "temper",
						},
						{
							id     : 2,
							name   : "relief",
						},
						{
							id     : 3,
							name   : "vital",
						},
						{
							id     : 4,
							name   : "revival_c",
						},
					],
				},
				{
					id       : 4,
					name     : "especiales",
					type     : "custom",
					timeline : null,
					element  : $("#labs_slider_especiales"),
					height   : 0,
					top      : 0,
					menuTl   : null,
					menuEl   : $("#bullet_item4"),
					current  : 1,
					products : [
						{
							id     : 0,
							name   : "boldness",
						},
						{
							id     : 1,
							name   : "mad",
						},
						{
							id     : 2,
							name   : "ripped",
						},
					],
				},
			],
		},
	};

	
};

List.prototype.init = function() {

	this.settings.is_Mobile= (isMobile.apple.phone || isMobile.android.phone || isMobile.seven_inch || isMobile.apple.tablet );
	if (this.settings.is_Mobile && $(document).width()>1023)
	{
		this.settings.is_TabletLandscape = true;
	}
	
	this.bind();
	this.wResize(null);

	slider= this.settings.slider;
	self = this;

	// HOME
	this.makeHomeTimeline(slider);
	this.makeEspecialesTimeline(slider);
	
	var controller = new ScrollMagic.Controller();
	
	for(var i=0; i<slider.sections.length;i++)
	{
		section = slider.sections[i];
		//menu timeline	
		section.menuTl = this.makeStdMenuTimeline(section.menuEl);
		if(i<slider.sections.length-1)
			this.initSectionsTimeline(section.element);
		
		if(i>0)
		{
			this.setupScroll(controller,i);
		}
	}

	$(".team_slider_item").each(function(index,element)
	{
		//index 0 = home
		if(index!==0)
		{
			var swipeItem = element;
			var mcItem = new Hammer(swipeItem);
			mcItem.on("swipeleft swiperight", function(ev){self.swipes(ev);});
		}
	});
	if($(document).width()>1023)
	{
		this.previousOrientation="desktop";
	}
	else
		this.previousOrientation="mobile";

};



List.prototype.swipes= function(ev)
{
	
	if(this.settings.slider.productsBusy) return false;
	this.settings.slider.productsBusy=true;
	slider= this.settings.slider;

	_container = $(ev.target).closest(".team_slider_item");
	// not proud of this but works
	section = _container.attr("id");
	sectionNumber =0;
	switch(section)
	{
		case "labs_slider_energia":sectionNumber = 1;
		break;
		case "labs_slider_desarrollo":sectionNumber = 2;
		break;
		case "labs_slider_regula":sectionNumber = 3;
		break;
		case "labs_slider_especiales":sectionNumber = 4;
		break;
	}
	current = slider.sections[sectionNumber].current;
	
	next= 0;
	
	if(ev.type=="swiperight")
	{
		next= current -1;
		if(next<0)
		{
			next= slider.sections[sectionNumber].products.length-1;
		}
	}
	else
	{
		if(ev.type=="swipeleft")
		{
			next= current +1;
			if(next>slider.sections[sectionNumber].products.length-1)
			{
				next= 0;
			}
		}
	}
	productToShow= slider.sections[sectionNumber].products[next];
	
	slider.sections[sectionNumber].current = next;
	$("#menu_labs_"+productToShow.name).click();
	ev.preventDefault();
	
	
};
List.prototype.touchEspeciales= function(event, self)
{
	
	event.preventDefault();
	var productToShow = $(event.target).attr("id").replace("touch_","");
	
	$("#menu_labs_"+productToShow).click();
};
List.prototype.setupScroll= function(controller,i)
{
	self = this;
	slider= this.settings.slider;

	var sliderEnter = slider.sections[i];
	var sliderExit = slider.sections[i-1];
	var triger = slider.sections[i].element.attr("id");
	
	var scene = new ScrollMagic.Scene({
		triggerElement: "#"+triger
	})
	.on("start",function(event){
		if(self.settings.scrollDisabled) return false;
		if(event.progress===1)
		{
			self.sectionChange(sliderEnter,sliderExit,false);
		}
		else
		{
			self.sectionChange(sliderExit,sliderEnter,false);
		}
	})
	.addTo(controller);
};

List.prototype.initHomeAnimation = function()
{
	this.settings.slider.sections[0].timeline.play(0);
	this.settings.slider.sections[0].menuTl.play(0);
};

List.prototype.sectionChange= function(sectionEnter,sectionExit,needsToScroll)
{
	
	slider = this.settings.slider;
	if(!sectionEnter.menuEl.hasClass("hover"))
		sectionEnter.menuTl.play(0);
	
	this.exitAnimation(sectionExit);
	slider.current = sectionEnter.id;

	if(needsToScroll)
	{
		if(this.settings.is_Mobile && !this.settings.is_TabletLandscape)
		{
			TweenMax.to(slider.fullcontainer, 0.7, {
				scrollTo: { y: sectionEnter.top},
				ease: Cubic.easeInOut,
				onComplete:function()
				{
					self.settings.slider.busy = false;
					self.entranceAnimation(sectionEnter);
					self.settings.scrollDisabled=false;
				}
			});
		}
		else
		{
			TweenMax.to(slider.mover, 0.7, {
				top: sectionEnter.top,
				ease: Cubic.easeInOut,
				onComplete:function()
				{
					self.settings.slider.busy = false;
					self.entranceAnimation(sectionEnter);
					self.settings.scrollDisabled=false;
				}
			});
		}
	}
	else
	{
		self.entranceAnimation(sectionEnter);
	}
	
};

List.prototype.entranceAnimation= function(sectionEnter)
{
	
	if(sectionEnter.type=="custom")
		sectionEnter.timeline.play(0);
	else
		self.makeStdTimeline(sectionEnter.element);
};

List.prototype.exitAnimation= function(sectionExit)
{
	
	sectionExit.menuTl.reverse(1);
	if(sectionExit.type=="custom")
		sectionExit.timeline.reverse(1);
	else
		this.makeStdReverseTimeline(sectionExit.element,false);

	sectionExit.menuEl.removeClass("active");

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
		$("#labs_slider_especiales .btn_container div").on("mouseenter",function(event){self.specialsProductPlay(event,self);});

	$(".labs_changer_menu_specials a").on("click",function(event){self.specialsProductPlay(event,self);});

	$("#especiales_mobile_touch .especiales_touch_changer").on("touchstart",function(event){self.touchEspeciales(event,self);});
	
	$(window).on("resize",function(event){self.wResize(event);});

	//analitycs
	$(".home_content_btn.comprar","#labs_hysteria").on("click",function(event){self.pushclick('hysteria-tienda');});
	$(".home_content_btn.comprar","#labs_provoke").on("click",function(event){self.pushclick('provoke-tienda');});
	$(".home_content_btn.comprar","#labs_hysteria_l").on("click",function(event){self.pushclick('hysteria_l-tienda');});
	
	$(".home_content_btn.comprar","#labs_pride").on("click",function(event){self.pushclick('pride-tienda');});
	$(".home_content_btn.comprar","#labs_tenacious").on("click",function(event){self.pushclick('tenacious-tienda');});
	

	$(".home_content_btn.comprar","#labs_revival").on("click",function(event){self.pushclick('revival-tienda');});
	$(".home_content_btn.comprar","#labs_temper").on("click",function(event){self.pushclick('temper-tienda');});
	$(".home_content_btn.comprar","#labs_relief").on("click",function(event){self.pushclick('relief-tienda');});
	$(".home_content_btn.comprar","#labs_vital").on("click",function(event){self.pushclick('vital-tienda');});

	$(".home_content_btn","#boldness_over").on("click",function(event){self.pushclick('boldness-tienda');});
	$(".home_content_btn","#mad_over").on("click",function(event){self.pushclick('mad-tienda');});
	$(".home_content_btn","#ripped_over").on("click",function(event){self.pushclick('ripped-tienda');});
		
};

List.prototype.restartMobile= function(self)
{
	
	location.reload();
	return false;

	/*
	slider= self.settings.slider;
	//self.resizew(null);

	for(var i=0; i<self.settings.slider.sections.length;i++)
	{
		self.settings.slider.sections[i].height = self.settings.slider.sections[i].element.height();
		if(i===0)
		{
			self.settings.slider.sections[i].top = 0;
		}
		else
		{
			self.settings.slider.sections[i].top = self.settings.slider.sections[i-1].top+ self.settings.slider.sections[i-1].element.height();
		}
	}
	sectionEnter = slider.sections[slider.current];
	slider.mover.css("top","0%");
	TweenMax.to(slider.mover, 0, {
		top: "0%",
		onComplete: function()
		{
			TweenMax.to(slider.fullcontainer, 0, {
				scrollTo: { y: sectionEnter.top},
			});
		}
	});
	*/
};
List.prototype.restartDesktop= function()
{
	
	return false;

	/*
		for(var j=0; j<this.settings.slider.sections.length;j++)
		{
			
			this.settings.slider.sections[j].top = (j*-100)+"%";
			
		}
	*/
};
List.prototype.pushclick= function(liga)
{
	ga('send', 'event', liga);
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
	
	slider.sections[4].timeline = new TimelineLite({});
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
	slider.sections[4].timeline.pause(0);
};

List.prototype.specialsProductPlay= function(event,_self)
{
	event.preventDefault();
	btn = $(event.currentTarget);
	if(btn.hasClass("current")) return false;
	
	slider_item = $("#labs_slider_especiales");
	if(_self.settings.is_Mobile)
		$(".labs_changer_menu_specials .current",slider_item).removeClass("current");
	
		$(".btn_container .current",slider_item).removeClass("current");

	btn.addClass("current");
	if(_self.settings.is_Mobile)
		newItem= btn.attr("id").replace("menu_labs_","");
	else
		newItem= btn.attr("id").replace("_over","");
	
	timeline = new TimelineLite({});
	_current_Dark = $(".current.dark",slider_item);
	
	
	_current_Text = $(".current.paddingl_150",slider_item);
	
	//slideItem_Bg = $(".bg_lights",slider_item);
	slideItem_Dark = $("."+newItem+".dark",slider_item);
	

	slideItem_Text = $("."+newItem+".paddingl_150",slider_item);
	
	
	timeline.timeScale(1.5);
	
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
	_self.settings.slider.productsBusy= false;
	
	
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
	
	$(".bg_figure",_current).css("opacity",0);
	$(" .bg_text",slider_item).css("opacity",0);
	$(" .labs_changer_menu",slider_item).css("opacity",0);
	$("h3",_info_current).css("opacity",0);
	$(".description",_info_current).css("opacity",0);
	$(".team_ficha_container",_info_current).css("opacity",0);
	
	ul = $("ul",_info_current);
	$("li",ul).css("opacity",0);
	
	$(".home_content_btn",_current).css("opacity",0);
	
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
	if($(document).width()>1023 && self.previousOrientation=="mobile")
	{
		self.restartDesktop(self);

	}
	if($(document).width()<1024 && self.previousOrientation=="desktop")
	{
		self.restartMobile(self);
	}
	if($(document).width()>1023)
	{
		self.previousOrientation="desktop";
	}
	else
		self.previousOrientation="mobile";
	

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
	slider= this.settings.slider;
	if(slider.busy)return false;
	if($(btn).hasClass("active")) return false;
	this.settings.scrollDisabled = true;
	slider.busy = true;
	
	btn = $(event.currentTarget);
	currentSection = slider.sections[slider.current];
	 
	switch(btn.attr("id"))
	{
		case 'bullet_home':btnSection=0;
		break;
		case 'bullet_item1':btnSection=1;
		break;
		case 'bullet_item2':btnSection=2;
		break;
		case 'bullet_item3':btnSection=3;
		break;
		case 'bullet_item4':btnSection=4;
		break;
	}
	newSection =slider.sections[btnSection];
	newSection.menuEl.addClass("active");

	this.sectionChange(newSection,currentSection,true);
};
List.prototype.changer= function(event)
{
	event.preventDefault();
	slider = this.settings.slider;
	_btn= $(event.currentTarget);
	if(_btn.hasClass("current")) return false;

	if(slider.busy) return false;
	slider.busy= true;
	
	_parent= $(_btn).parent();

	id= _btn.attr('id').replace("menu_labs_","");
	
	$(".labs_changer_menu_item.current",_parent).removeClass("current");
	_btn.addClass("current");
	menuContainer = _parent.parent().parent();
		
	index = $("a",_parent).index( _btn );
	
	switch(menuContainer.attr("id"))
	{
		case "labs_slider_energia":sectionNumber = 1;
		break;
		case "labs_slider_desarrollo":sectionNumber = 2;
		break;
		case "labs_slider_regula":sectionNumber = 3;
		break;
		case "labs_slider_especiales":sectionNumber = 4;
		break;
	}
	slider.sections[sectionNumber].current = index;

	isleft = false;

	if(menuContainer.hasClass('labs_left'))isleft = true;
	this.changeProduct($("#labs_"+id,menuContainer),menuContainer,isleft);

};
List.prototype.changeProduct= function(section,changer,isLeft)
{
	var self = this;
	
	current = $(".labs_changer_item.current",changer);
	
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
				self.settings.slider.productsBusy= false;
				
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

		/*
		$SlmfTeam.removeLoader({
				onComplete: function() {
					
				}
			});
		*/
		
		_onLoadImages(function() {

			$SlmfTeam.removeLoader({
				onComplete: function() {
					list.initHomeAnimation();
				}
			});
		});
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

var _onLoadImages = function( onComplete) {

	var count = 0,
		loaderContent = $("#loader p"),
		counter = {
			var: 0
		};
	$(".loader p").css({
		opacity: 0.1
	});
	width = $(window).width();
	
	size= "";
	if(width < 1024 )
	{
		size="_sm";
	}
	else
	{
		if(width> 1449)
		{
			size="_xl";
		}
	}
	images = [ 'Content/img/slmf-labs/LABS_home_bg.png',
		'Content/img/slmf-labs/LABS_home_figures'+size+'.png',
		
		'Content/img/slmf-labs/LABS_hysteria_figure'+size+'.png',
		'Content/img/slmf-labs/LABS_pride_figure'+size+'.png',
		'Content/img/slmf-labs/LABS_revival_figure'+size+'.png',
		
		'Content/img/slmf-labs/LABS_especiales_figure.png',
		'Content/img/slmf-labs/LABS_shock_figure_dark.png',
		'Content/img/slmf-labs/LABS_mad_figure_dark.png',
		'Content/img/slmf-labs/LABS_ripped_figure_dark.png',
			];
	$.each(images, function(index, src) {

		var img = new Image();
		img.src = src;
		
		img.onload = function() {

			count = count + 1;

			TweenMax.to(counter, 0.50, {
				var: Math.round((count / images.length) * 99),
				onUpdate: function() {
					
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