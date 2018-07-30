
//


var User = (function() {
  'use strict';

  function User(args) {

    // enforces new
    if (!(this instanceof User)) {
        return new User(args);
    };

    this.settings = {
      json: false,

      appId: '',

      postLogin: false,
      postInit: false,
      postDeleteUser: false,
      postInitPlan: false,
      postCancelPlan: false,
      postRegisterUser: false,

      setUserState: false,
      getUserState: false,

      isDashboardLoaded: false,

      dataLogged: {
        dayDashboardInit: 1,

        isLogged: false,
        isRegistered: false,

        //datos del dashboard
        isDashboard: false,
        dashboardPlan: '',
        dashboardDiscipline: '',
        
        //datos del plan del usuario
        planDiscipline: false,
        planUser: false,
        planDay: 1,
        planUrl: false,
        currentStatus: 'inactive', //inactive/active/finished

        
        //datos del usuario
        userId: false,
        userName: false,
        userLastName: false,
        userPicture: false,
        userEmail: false,
      },
    };

    $.extend(true, this.settings, args || {});

    var self = this;

    this.cachedElements();
    this.bind();
  };

  var animations = function( params ){

    switch( params.action ) {
      case 'hide-login':

          TweenLite.to('#login', 0.55,{y: -600, x: 600, ease: Cubic.easeOut });
        break;
      case 'show-login':

          TweenLite.to('#login', 0.25,{y: 0, x: 0, ease: Cubic.easeOut });
        break;

      case 'show-bt':
        
        TweenLite.to( params.id, 0.01, { css: { display: 'block' } } );
        break;
      case 'hide-bt':
        
        TweenLite.to( params.id, 0.01, { css: { display: 'none' } } );
        break;

      case 'show-user':

        var btTrainToShow = '';
        
        $('#login-user-train')
          .addClass('login-user-train-hover');

        switch(true) {

          // /no-dashboard/no-plan
          case !params.data.isDashboard && !params.data.planUser:
            // Muestra las dixciplinas
            TweenMax.to( '#login-disciplines', 0.01, { css:{ display: 'block' } } );
            TweenMax.to( '#login-disciplines', 0.28, {y: 0, x: 0, ease: Quart.easeOut, delay: 0.07 } );
            
            $( '#login-user-train' ).removeClass('login-user-train-hover');
            
            btTrainToShow = '#login-user-train-select';
            
            break;

          // /no-dashboard/has-plan
          case params.data.isDashboard === false && (params.data.planDiscipline !== false && params.data.planUser !== false):
            
            var iconTrainToShow = '';
            switch(params.data.planDiscipline) {
              case 'crossfit':      iconTrainToShow = 'cft'; break;
              case 'bodybuilding':    iconTrainToShow = 'bbd'; break;
              case 'mixed-martial-arts':  iconTrainToShow = 'mma'; break;
            }
            btTrainToShow = '#login-user-train-train';

            // Icono de disciplina a mostrar
            TweenMax.set( '#login-user-train-train span', { css:{ display: 'none'} } );
            TweenMax.set( '.login-user-train-' + iconTrainToShow, { css:{ display: 'block'} } );
            break;

          // /in-dashboard/no-plan
          case params.data.isDashboard && !params.data.planUser:

            var iconTrainToShow = '';
            switch(params.data.dashboardDiscipline) {
              case 'crossfit':      iconTrainToShow = 'cft'; break;
              case 'bodybuilding':    iconTrainToShow = 'bbd'; break;
              case 'mixed-martial-arts':  iconTrainToShow = 'mma'; break;
            }
            btTrainToShow = '#login-user-train-dashboardini';
            // Icono de disciplina a mostrar
            TweenMax.set( '#login-user-train-dashboardini span', { css:{ display: 'none'} } );
            TweenMax.set( '.login-user-train-' + iconTrainToShow, { css:{ display: 'block'} } );
            break;
          // /in-dashboard/has-plan/diferent-plan
          case (params.data.isDashboard && !params.data.planUser) ||
             (params.data.isDashboard && params.data.planUser !== false && params.data.planDiscipline !== false && params.data.dashboardPlan != params.data.planUser):

            var iconTrainToShow = '';
            switch(params.data.dashboardDiscipline) {
              case 'crossfit':      iconTrainToShow = 'cft'; break;
              case 'bodybuilding':    iconTrainToShow = 'bbd'; break;
              case 'mixed-martial-arts':  iconTrainToShow = 'mma'; break;
            }
            btTrainToShow = '#login-user-train-dashboardnew';
            // Icono de disciplina a mostrar
            TweenMax.set( '#login-user-train-dashboardnew span', { css:{ display: 'none'} } );
            TweenMax.set( '.login-user-train-' + iconTrainToShow, { css:{ display: 'block'} } );
            break;

          // /in-dashboard/has-plan/same-plan
          case params.data.isDashboard && params.data.planDiscipline !== false && params.data.dashboardPlan === params.data.planUser:
            
            var iconTrainToShow = '';
            switch(params.data.dashboardDiscipline) {
              case 'crossfit':      iconTrainToShow = 'cft'; break;
              case 'bodybuilding':    iconTrainToShow = 'bbd'; break;
              case 'mixed-martial-arts':  iconTrainToShow = 'mma'; break;
            }
            btTrainToShow = '#login-user-train-dashboardcancel';
            // Icono de disciplina a mostrar
            TweenMax.set( '#login-user-train-dashboardcancel span', { css:{ display: 'none'} } );
            TweenMax.set( '.login-user-train-' + iconTrainToShow, { css:{ display: 'block'} } );
            break;
        }

        // Oculta el boton 'entrenar generico'
        TweenMax.set( '.bt-login-train', { css:{ display: 'none'} } );
        // Muestra la opcion correcta en el boton 'entrenar'
        TweenMax.set( btTrainToShow, { css:{ display: 'block'} } );
        
        TweenLite.to( '#login-user', 0.25, { x: 0, y: 0, ease: Cubic.easeOut } );
        TweenMax.staggerTo( ['#login-user-del', '#login-user-avatar', '#login-user-train'], 0.25, { y: 0, ease: Quart.easeOut }, 0.05 );

        break;
      case 'hide-user':
        // no-plan/no-dashboard
        if( !params.data.isDashboard && !params.data.planUser ){
          TweenMax.to( '#login-disciplines', 0.25, {x: -435, y: -435 , onComplete: function(){
            TweenLite.set( '#login-disciplines', { x: 435, y: 435 } );
          } } );
        }

        TweenLite.to( '#login-user', 0.25, { x: -334, y: -334, onComplete: function(){
          TweenLite.set( '#login-user', { x: 334, y: 334 } );
          
          TweenLite.set(['#login-user-del', '#login-user-avatar', '#login-user-train'],
            {y: 250 });
        } } );

        break;

      case 'show-message':

        TweenMax.to( '#login-disciplines', 0.01, { css:{ display: 'none' } } );

        TweenLite.to( '.login-message-box', 0.01, { autoAlpha: 0, css: { display: 'none' } } );
        TweenLite.to( params.id, 0.01, { css: { autoAlpha: 1, display: 'block' } } );

        TweenLite.to( '#login-message', 0.2, { y: 0, x: 0, ease: Quart.easeOut } );
        break;
      case 'hide-message':

        TweenLite.to( '#login-message', 0.25, { y: -180, x: 180, ease: Cubic.easeOut } );
        break;
    
      case 'show-dayini':

        // Bug, no anima opacidad
        TweenLite.fromTo('#message-plan-change', 0.25, 
          {autoAlpha: 1, opacity: 1},
          {autoAlpha: 0, opacity: 0, onComplete: function(){
          TweenLite.set('#message-plan-change', {css:{display:'none'}});
          
          TweenLite.fromTo('#message-plan-ini', 0.25, 
            {autoAlpha: 0, css:{display:'none'}},
            {autoAlpha: 1, css:{display:'block'}}
          );
        }});
        break;
    
      case 'show-loader':
        TweenLite.set('#login-loader', 
          { top: '25px', autoAlpha: 0, rotation:"0", onComplete: function(){
            
            TweenLite.to('#login-loader', 0.25, {autoAlpha: 1});
            TweenMax.to('#login-loader', 1, {ease: Linear.easeNone, rotation:"360", repeat:-1});
          } } );
        break;

      case 'hide-loader':
        TweenLite.to('#login-loader', 0.15,
          { autoAlpha: 0, onComplete: function(){
            TweenLite.set('#login-loader', {top: '-200px', rotation:"0"});
            TweenMax.killTweensOf('#login-loader');
          } } );
        break;
    
      case 'show-form':
        TweenLite.set( '#login-overlay', {css:{ display:'block', autoAlpha: 0 } } );
        TweenLite.to( '#login-overlay', 0.25, { autoAlpha: 0.9 } );

        TweenLite.set( '#login-form', {css:{ display:'block', autoAlpha: 0 } } );
        TweenLite.to( '#login-form', 0.25, { autoAlpha: 1 } );
        break;
      case 'hide-form':

        TweenLite.to( '#login-overlay', 0.25, { autoAlpha: 0, onComplete: function(){
          TweenLite.set( '#login-overlay', {css:{ display: 'none' } } );
        } } );

        TweenLite.to( '#login-form', 0.25, { autoAlpha: 0, onComplete: function(){
          TweenLite.set( '#login-form', {css:{ display: 'none' } } );
        } } );


        break;
    }
  };

  var validationForm = function( params ){

    var error = function(input){
      TweenLite.set( input, { borderColor: '#f6d641', backgroundColor: '#f6d641', color: '#222222' } );
      TweenLite.from( input, 0.35, { x: -20, ease: Elastic.easeOut, yoyo: false, repeat: true, onComplete:
        function(){

          TweenLite.to(input, 0.75, { x: 0, color: '#f6d641', borderColor: '#494949', backgroundColor: '#222222', delay: 0.25 } );
        } } );
      input.focus();
    }

    if(params.firstName.val().trim() === ''){
      error(params.firstName);
      return false;
    }
    if(params.lastName.val().trim() === ''){
      error(params.lastName);
      return false;
    }

    var re = /^[A-Z0-9._%+-]+@([A-Z0-9-]+\.)+[A-Z]{2,4}$/i;
    if (params.email.val() == '' || !re.test(params.email.val().trim())) {
        error(params.email);
        return false;
    }
    return true;
  };

  var updateDisabledButtons = function() {
    $('#login-disciplines .inactive p').text('SOON');
  };

  User.prototype = {

    renderData: function( response, onComplete, status){
      var status = status || true;
      var onComplete = onComplete || function(){};
      var isDashboard = this.settings.dataLogged.isDashboard;
      // console.log('----------');
      // console.log('self.renderData()');

      this.setUserState(status, function(){

        var data = {
          //Datos dashboard
          dayDashboardInit: 1,

          isLogged: true,
          isRegistered: false,
          //datos del dashboard
          isDashboard: isDashboard,
          dashboardPlan: '',
          dashboardDiscipline: '',
          
          //datos del plan del usuario
          planDiscipline: false,
          planUser: false,
          planDay: 1,
          planUrl: false,
          currentStatus: 'inactive',
          
          //datos del usuario
          userId: response.id,
          userName: (response.first_name || '') + ' ' + (response.middle_name || ''),
          userLastName: response.last_name,
          userPicture: 'http://graph.facebook.com/' + response.id + '/picture?width=250&height=250',
          userEmail: response.email || '',
        }
        updateDisabledButtons();
        // console.log('done renderData()');
        onComplete(data);
      });
    },
    cachedElements: function(){
      console.log("User", "cachedElements()");
      this.userImg = $( '#login-user-avatar-img' );
      this.userName = $( '#login-user-avatar-name' );

      // User no logged
      this.btEnter = $( '#login-enter' ); //aparece cuando no se ha logeado
      this.btLogin = $( '#login-login' ); //al RO del boton #login-enter
      
      // User logged
      this.btLogged = $( '#login-logged' ); // aparece cuando ya está logeado
      this.btLogout = $( '#login-logout' ); //al RO del boton #login-logged
      this.btDeleteAccount = $( '#login-user-del' ); //al RO del boton #login-logged
      
      this.btTrain = $( '#login-user-train-train' ); //sho if (!dashboard and has-plan)
      this.btInit = $( '#login-user-train-dashboardini' ); //show if (dashboard and no-plan)
      this.btNew = $( '#login-user-train-dashboardnew' ); //show if (dashboard and has-plan and !dif-plan)
      this.btCancel = $( '#login-user-train-dashboardcancel' ); //show if (dashboard and has-plan and !dif-plan)

      this.panelUserHover = $( '.login-user-hover' );

      this.messageContainer = $( '#login-message' );

      // Messages
      this.btAccountCloseYes = $( '#login-account-close-yes' );
      this.btAccountCloseNo = $( '#login-account-close-no' );
      
      this.btAccountDeleteYes = $( '#login-account-delete-yes' );
      this.btAccountDeleteNo = $( '#login-account-delete-no' );

      this.btPlanChangeYes = $( '#login-plan-change-yes' );
      this.btPlanChangeNo = $( '#login-plan-change-no' );
      
      this.btPlanCancelYes = $( '#login-plan-cancel-yes' );
      this.btPlanCancelNo = $( '#login-plan-cancel-no' );

      this.btPlanInit = $( '.bt-plan-ini' );

      this.inputFirstName = $( '#login-form-firstname' );
      this.inputLastName = $( '#login-form-lastname' );
      this.inputEmail = $( '#login-form-email' );
      this.inputCancel = $( '#bt-login-form-cancel' );
      this.inputOk = $( '#login-form form' );
      this.overlay = $('#login-overlay');

      //for dashboard
      this.btInitPlan = $('#bt-iniciar');
    },
    bind: function(){
      console.log("User", "bind()");
      var self = this;

      //if is dashboard
      this.btInitPlan.on('click', function(e){ self.bindBtInitPlan(e); });

      // User no logged
      this.btEnter.mouseenter( function(){ self.showBtLogIn(); } );
      this.btLogin.mouseleave( function(){ self.hideBtLogin(); } );
      this.btLogin.on( 'click', function(){ self.login(); } );

      // User logged
      this.btLogged.mouseenter( function() { self.showBtLogOut(); } );
      this.btLogout.on( 'click', function(){ self.showMessage(); } );
      this.btDeleteAccount.on( 'click', function(){ self.showMessage( '#message-account-delete' ) })
      
      this.btInit.on( 'click', function(){ self.showMessage( '#message-plan-ini' ); });
      this.btNew.on( 'click', function(){ self.showMessage( '#message-plan-change' ); });
      this.btCancel.on( 'click', function(){ self.showMessage( '#message-plan-cancel' ); });
      this.btPlanInit.on( 'click', function(){ self.planInit( $(this).data('init')); });


      this.panelUserHover.mouseleave(function(){ self.hidePanelUser(); } );
      this.panelUserHover.mouseenter(function(){ self.showBtLogOut(); } );

      //Message box
      this.messageContainer.mouseleave(function(){ self.hideMessage(); });
      $.each([ self.btPlanCancelNo, self.btPlanChangeNo, self.btAccountDeleteNo, self.btAccountCloseNo ], function(i, bt) {
         
        bt.on( 'click', function(event){ 
          event.preventDefault();
          self.hideMessage(); 
        });
      });

      this.btPlanChangeYes.on( 'click', function(event){
        event.preventDefault();
        animations({action:'show-dayini'});
      });

      //Message buttons Account
      this.btAccountCloseYes.on( 'click', function(event){
        event.preventDefault();
        self.logout();
      });

      this.btAccountDeleteYes.on( 'click', function(event){
        event.preventDefault();
        self.delete();
      });

      this.btPlanCancelYes.on( 'click', function(event){
        event.preventDefault();
        self.planCancel();
      });

      this.inputCancel.on( 'click', function(event){
        event.preventDefault();
        self.hideForm();
      });
      this.inputOk.on( 'submit', function(event){
        event.preventDefault();
        var isValid = validationForm({
          firstName:  self.inputFirstName,
          lastName:   self.inputLastName,
          email:      self.inputEmail,
        });
        if( isValid ){
          // console.log('Register!')
          self.registerUser();
        }
      });

      this.overlay.on( 'click', function(){
        self.hideForm();
      });

      window.fbAsyncInit = function() {
        console.log("window.fbAsyncInit")
        FB.init({
          appId      : self.settings.appId,
          status     : true, // check login status
          cookie     : true, // enable cookies to allow the server to access the session
          xfbml      : true,  // parse XFBML
          version    : 'v2.1'
        });

        FB.getLoginStatus(function(response) {
          console.log(response)
          self.show();
          self.fbStatusChangeCallback(response);
        });
      };
    },
    render: function(){
      // console.log('-----------')
      // console.log('render()')
      // console.log('data:')
      // console.log(this.settings.dataLogged)

      var self = this,
        s = self.settings,
        data = s.dataLogged;

      self.userImg.css( { backgroundImage: '' } );
      self.userName.html( '' );
      self.btTrain.attr( 'href', '#' );

      if( data.isLogged === true ){
        self.userImg.css({'background-image': 'url('+data.userPicture+')' });
        self.userName.html( data.userName + ' ' + data.userLastName );
        
        if(data.isDashboard === false && data.planDiscipline !== false && data.planUser !== false){
          self.btTrain.attr('href', data.planUrl);
        }
      }
    },
    clear: function(){

      var self = this, s = self.settings;

      self.setUserState(false, function(){},function(){});
      var isDashboard = this.settings.dataLogged.isDashboard;
      self.settings.dataLogged = {

        dayDashboardInit: 1,

        isLogged: false,
        isRegistered: false,
        //datos del dashboard
        isDashboard: isDashboard,
        dashboardPlan: '',
        dashboardDiscipline: '',
        
        //datos del plan del usuario
        planDiscipline: false,
        planUser: false,
        planDay: 1,
        planUrl: false,
        currentStatus: 'inactive',
        
        //datos del usuario
        userId: false,
        userName: false,
        userLastName: false,
        userPicture: false,
        userEmail: false,
      };
      // console.log('clear data and fields of login')
      self.hideBtLogOut();

      self.userImg.css( { backgroundImage: '' } );
      self.userName.html( '' );
      self.btTrain.attr('href', '#');
    },
    isDashboard: function(callback){

      var self = this;
      callback = callback || function(){};

      $.ajax({url: rootSite + 'getSessionInfo', dataType: 'json'})
        .done(function(dt) {

          console.log(dt)
          self.settings.dataLogged.dashboardDiscipline = dt.dashboardCurrentDiscipline;
          self.settings.dataLogged.isDashboard = dt.dashboardCurrentPlan !== '' && dt.dashboardCurrentDiscipline !== '' ? true : false;
          callback();
        });
    },
    dashboardRender: function(params){
      var self = this,
        s = self.settings,
        data = self.settings.dataLogged;

      // console.log('dashboardRender()');
      // console.log('data.isDashboard', data.isDashboard)
      if(data.isDashboard === true){

        if(s.isDashboardLoaded === true){
          // console.log('DASHBOARD > load in day', data.dayDashboardInit);
          //Cambia dia del plan al dia a iniciar

          Dashboard.changeDay(data.dayDashboardInit-1, data.currentStatus);
          // Dashboard.changeDay(data.dayDashboardInit-1, 'finished');
        }else{
          // console.log('DASHBOARD > init in day', data.dayDashboardInit);
          // console.log(data);
          s.isDashboardLoaded = true;
          console.log(222)
          console.log(data)
          Dashboard.init( { 
            getSessionInfo: rootSite + 'getSessionInfo', 
            getPlanInfo: rootAPI + 'plan',
            initDay: data.dayDashboardInit,
            currentStatus: data.currentStatus,
            disciplineId: data.dashboardDiscipline,
          });
        }
      }
    },
    fbStatusChangeCallback: function(response) {
      // console.log('-----------------')
      // console.log('fbStatusChangeCallback()')
      var self = this,
          s = self.settings,
          data = self.settings.dataLogged;
      response = response || {};
      // console.log(data)

      if (response.status === 'connected') {
        
        console.log('Welcome!  Fetching your information.... ');
        FB.api('/me', function(response) {
          // console.log('Successful login for: ' + response.name);

          self.getUserState(
            function(){
              self.renderData(response, function(data){

                // console.log('data renderData1');
                // console.log(data);
                self.settings.dataLogged = data;
                self.getUser({
                  userNotRegistered: function(){ self.show(); },
                });
              });
            },
            function(){
              // console.log('logeado a facebook pero sesion false')
              data.dayDashboardInit = 1;
              self.isDashboard(function(){self.dashboardRender();});
            });
          return true;
        });

      } else if (response.status === 'not_authorized') {

        console.log('not authorized');
        data.dayDashboardInit = 1;
        self.isDashboard(function(){self.dashboardRender();});
        self.setUserState(false);
        return false;
      } else {
        
        console.log('Please log into Facebook.');
        data.dayDashboardInit = 1;
        self.isDashboard(function(){self.dashboardRender();});
        self.setUserState(false);
        return false;
      }
    },
    getUser: function( params ){
      // console.log('------------')
      // console.log('getUser()')
      params.userNotRegistered = params.userNotRegistered || function(){};

      var self = this,
        s = self.settings,
        data = self.settings.dataLogged;

      if( s.postLogin !== false){

        self.getUserState(function(){
          $.ajax({
            type: "GET",
            url : s.postLogin + '/' + data.userId,
            dataType: 'json',
          }).done(function (dt, textStatus, jqXHR) {

            // console.log('done getUser');
            // console.log(dt);
            data.isRegistered = dt.isRegistered;
            data.isDashboard = dt.isDashboard;
            data.dashboardPlan = dt.dashboardPlan;
            data.dashboardDiscipline = dt.dashboardDiscipline;

            data.planDiscipline = dt.planDiscipline;
            data.planUser = dt.planUser;
            data.planDay = dt.planDay;
            data.planUrl = dt.planUrl;
            data.currentStatus = dt.currentStatus;

            self.render();
            data.dayDashboardInit = 1;
            if(data.isRegistered){
              if((data.dashboardDiscipline === data.planDiscipline) && (data.dashboardPlan === data.planUser)){
                // console.log('data.planDay', data.planDay)
                data.dayDashboardInit = data.planDay <= 0 ? 1 : dt.planDay
              }
              self.show();
            }else{
              params.userNotRegistered();
            };
            self.dashboardRender();
          }).fail(function () {

              self.show();
              self.clear();
          });
        });

      }
    },
    login: function(){
      // console.log('-----------')
      // console.log('login()')
      var self = this,
          s = self.settings,
          data = self.settings.dataLogged;

      self.hide();
      self.showLoader();
      FB.login(function(response) {

        if (response.authResponse) {
          
          FB.api('/me', function(response) {

            // console.log('login ok');
            // console.log(response);

            self.renderData(response, function(data){
              // console.log('data renderData2');
              // console.log(data);
              self.settings.dataLogged = data;
              self.getUser({
                userNotRegistered: function(){ self.showForm(); },
              });
            });
          });

        } else {
          // console.log('User cancelled login or did not fully authorize.');
          // data.dayDashboardInit = 1;
          // self.isDashboard(function(){self.dashboardRender();});

          self.setUserState(false, function(){
        
            self.clear();
            self.show();
            self.hideMessage();
          }, function(){
            self.show();
          });
        }
        self.hideLoader();
      }, {
        scope: 'publish_stream,email'
      });
    },
    logout: function(){

      // console.log('logout()');
      var self = this, s = self.settings,
          data = self.settings.dataLogged;
      
      FB.logout(function(response) {});

      self.hide();

      self.setUserState(false, function(){
        
        self.clear();
        self.show();
        self.hideMessage();
      }, function(){
        self.show();
      });
    },
    setUserState: function(state, onDone, onFail) {

      // console.log('----------------');
      // console.log('setUserState()', state);
      var s = this.settings,
        onDone = onDone || function(){},
        onFail = onFail || function(){};

      $.ajax({
        type: "POST",
        url : s.setUserState + '/' + state,
      }).done(function (dt, textStatus, jqXHR) {
        onDone();
      }).fail(function () {
        onFail();
      });
    },
    getUserState: function(onTrue, onFalse){
      // console.log('----------------');
      // console.log('getUserState()');

      var onTrue = onTrue || function(){};
      var onFalse = onFalse || function(){};
      
      var s = this.settings;

      $.ajax({
        type: "GET",
        url : s.getUserState,
      }).done(function (dt) {
        // console.log(dt);
        // console.log('getUserState() done');
        if(dt + '' === 'True'){

          // console.log('getUserState() true');
          onTrue();
        }else{
          // console.log('getUserState() false');
          onFalse();
        }
      }).fail(function () {
        // console.log('getUserState() fail');
        onFalse()
      });
    },
    delete: function(){
      // console.log('---------');
      // console.log('delete()');

      var self = this, 
        s = self.settings, 
        data = self.settings.dataLogged;

      self.hide();
      self.showLoader();
      if( s.postDeleteUser !== false){
        // console.log('delete() ajax!');
        $.ajax({
          type: "POST",
          url : s.postDeleteUser,
          dataType: 'json',
          data: { facebookid: data.userId },
        }).done(function (data, textStatus, jqXHR) {
          self.hide();
          // console.log('delete() done! 1-2');
          self.setUserState(false, function(){
            // console.log('delete() done! 2-2');
            FB.logout(function(response) {});
            self.clear();
            self.show();
            self.hideMessage();
            self.hideLoader();
          });

        }).fail(function () {

          // console.log('delete() fail!');
          self.show();
          self.hideLoader();
        });
      }
    },
    planInit: function(init){

      var self = this, 
        s = self.settings,
        data = s.dataLogged;

      self.hide();
      self.showLoader();

      // console.log('--------------------')
      // console.log('planInit()');

      if( !s.dataLogged.isRegistered ){
        self.showForm();
        return false;
      };

      if( s.postInitPlan !== false){
        
        $.ajax({
          type: "POST",
          url : s.postInitPlan,
          dataType: 'json',
          data: { day: init, userId: data.userId, idPlan: data.dashboardPlan },
        }).done(function () {
          window.location.reload();
        }).fail(function  () {
          
          self.show();
          self.hideLoader();
        });
      }

      self.hideMessage();
    },
    planCancel: function(){
      // console.log('cancel plan');
      var self = this, 
        s = self.settings,
        data = s.dataLogged;

      self.hide();
      self.showLoader();

      if( s.postCancelPlan !== false){

        $.ajax({
          type: "POST",
          url : s.postCancelPlan,
          data: { day: 1, userId: data.userId, idPlan: data.dashboardPlan },
          // data: { day: 1, userId: '10153022243708320', idPlan: 'power-fitness' },
        }).done(function () {
          
          //datos del plan del usuario
          self.settings.dataLogged.planDiscipline = false;
          self.settings.dataLogged.planUser = false;
          self.settings.dataLogged.planDay = false;
          self.settings.dataLogged.planUrl = false;
          self.settings.dataLogged.currentStatus = 'inactive';

          self.show();
          self.hideLoader();

        }).fail(function () {

          self.show();
          self.hideLoader();
        });
      }

      self.hideMessage();
    },
    hide: function(){ animations( { action:'hide-login'}); },
    show: function(){
      // console.log('---------')
      // console.log('show()')
      var self = this,
        toShow = 'enter', 
        toHide = 'logged';
      
      // console.log('is Logged?:', self.settings.dataLogged.isLogged)
      
      if( self.settings.dataLogged.isLogged === true ){ 
        toHide = 'enter';
        toShow = 'logged';
        self.hideBtLogin();
      };
      TweenLite.set( '#login-' + toHide, { css: { display:'none'} } );
      TweenLite.set( '#login-' + toShow, { css: { display:'block'} } );
      animations( { action:'show-login'});
    },
    registerUser: function(){
      // console.log('-----------')
      // console.log('registerUser()')

      var self = this,
        s = self.settings,
        data = self.settings.dataLogged;

      if( s.postRegisterUser !== false){
        
        var data = {
          nombre: self.inputFirstName.val(),
          apellidos: self.inputLastName.val(),
          correo: self.inputEmail.val(),
          facebookid: data.userId
        }
        // console.log(data)
        $.ajax({
          type: "POST",
          url : s.postRegisterUser,
          dataType: 'json',
          data: data,
        }).done(function (dt, textStatus, jqXHR) {

          data.isRegistered = dt.isRegistered;

          if(data.isRegistered){

            //datos del usuario
            data.userName = dt.userName;
            data.userLastName = dt.userLastName;
            data.userEmail = dt.userEmail;
            
            self.render();
            self.hideForm();
          }else{

            TweenLite.set( '#login-form-email', { borderColor: '#ff884c', color: '#fff' } );
            TweenLite.set( '#login-form-email', { backgroundColor: '#ff884c'} );
            TweenLite.from( '#login-form-email', 0.35, { x: -20, ease: Elastic.easeOut, yoyo: false, repeat: true, onComplete:
              function(){

                TweenLite.to('#login-form-email', 0.75, { x: 0, color: '#0c0c0c', borderColor: 'transparent',backgroundColor: '#fff', delay: 0.25 } );
              } } );
            '#login-form-email'.focus();
            this.inputEmail.val('Este correo ya existe.');
          }
            
        }).fail(function () {

        });
      }
    },
    showForm: function(){
      
      this.inputFirstName.val(this.settings.dataLogged.userName);
      this.inputLastName.val(this.settings.dataLogged.userLastName);
      this.inputEmail.val(this.settings.dataLogged.userEmail);
      
      animations( { action:'show-form'} );
      this.inputFirstName.focus();
    },
    hideForm: function(){

      animations( {
        action: 'hide-form'
      });
      this.hideLoader();

      if(this.settings.dataLogged.isLogged === true){ 
        this.show();
      };

      this.inputFirstName.val('');
      this.inputLastName.val('');
      this.inputEmail.val('');
    },
    showMessage: function( messageID ){
      
      this.hidePanelUser( 0 );
      
      animations( { action: 'show-message', id: messageID || '#message-account-close' } );
    },
    hideMessage: function(){

      animations( { action: 'hide-message' } ) 
    },
    hidePanelUser: function( time ){

      var self = this, data = self.settings.dataLogged;
      self.settings.timer = setTimeout(function() {
        animations( { action: 'hide-user', data: data } );
        self.hideBtLogOut();
      }, time || 150);
    },
    showBtLogOut: function(){

      clearTimeout(this.settings.timer);
      animations( { action: 'show-bt', id: '#login-logout' } ); 
      animations( { action: 'show-user', data: this.settings.dataLogged } );
    },
    hideBtLogOut: function(){ animations( { action: 'hide-bt', id: '#login-logout' } ); },
    showBtLogIn: function(){ animations( { action: 'show-bt', id: '#login-login' } ); },
    hideBtLogin: function(){ animations( { action: 'hide-bt', id: '#login-login' } ); },
    showLoader: function(){ animations({action:'show-loader'}); },
    hideLoader: function(){ animations({action:'hide-loader'}); },
  
    bindBtInitPlan: function(e){

      e.preventDefault();
      var self = this,
        data = self.settings.dataLogged;
      if(data.isDashboard !== true){ return; }

      //no está logeado
      if (data.isLogged === false){

        self.btLogin.click();

      //Estoy logeado
      }else{
        console.log(data.planUser !== false, data.dashboardPlan !== data.planUser)

        switch(true){
          //Es mi plan
          case  data.dashboardPlan === data.planUser :
            console.log(1)
            $('#bt-prev-anatomy').click();
            break;
          //Tengo plan y no es mi plan
          case data.planUser !== "" && data.planUser !== false && (data.dashboardPlan !== data.planUser):
            console.log(2)
            self.btNew.click();
            break;
          //No tengo plan
          case data.planUser === false || data.planUser === "":
            console.log(3)
            self.btInit.click();
            break;
        }
      }
    }
  }

  return User;
}());