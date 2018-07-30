FilterTest = function(params) {

    this.settings = {

        // Botones del header
        btToPlanes: $Train.find("header .bt-scroll-next-slide"),
        btToTest: $Train.find("header .bt-more"),

        // Header de filtros y test
        headerList: $Train.find("#train-filter header"),

        btFilters: $Train.find("#train-filter ul.filters a"),
        btTests: $Train.find("#train-filter ul.tests a").eq(0),

        containerTest: $Train.find("#train-filter section.test"),
        containerList: $Train.find("#train-filter section.list"),

        containerOptionTest: $Train.find(".test-step"),
        btOptionTest: $Train.find("ul.test-step-options a"),
        breadcrumbsTest: $Train.find("a.breadcrumb"),
        abrstractTest: $Train.find("#filtro-resumen ul.test-step-options li"),

        btCloseTest: $Train.find("#btCerrar"),
        btRefreshTest: $Train.find("#btReiniciar"),

        listTop: $Train.find(".list-top"),
        list: $Train.find("ul.list"),
        listItems: "",
    };

    this.isTestComplete = false;
    this.optionsTestSelected = {
        // "genero" 		: "",
        // "complexion" 	: "",
        // "nivel" 			: "",
        // "objetivo" 		: "",
    };
    this.data = params.data || {}
};

FilterTest.prototype.init = function() {

    this.render();

    this.bind("header");
    this.bind("filters");
    this.bind("tests");
};

/**
 *
 * Trae todos los planes del plan
 */
FilterTest.prototype.render = function() {

    // Agrega loader y limpia lista
    this.settings.containerList.loader();
    this.settings.list.html("");

    var self = this;
    $.each(this.data, function(key, val) {

        var list = '<li class="todos ' + val.tags.gender + ' ' + val.tags.level + ' ' + val.tags.complexion + ' ' + val.tags.target + '"> <a href="#" title="Ver ' + val.title + '"> <div class="hover"> <div class="triangle"></div> <div class="blocks"> <div class="block"> <span class="top"> <span class="' + val.tags.gender + '"></span> </span> <span class="bottom">SEXO</span> </div> <div id="circle-' + key + '-1" class="block level"></div> <div id="circle-' + key + '-2" class="block days"></div> </div> </div> <div class="content"> <div class="left"> <strong>' + val.title + '</strong> <span>' + val.subtitle + '</span> </div> <div class="right"> <span class="circle"></span> <p>' + val.description + '</p> </div> </div> </a> </li>';

        // Agrega a la lista
        self.settings.list
            .append(list);

        // Genera circulos
        _generateCircle('circle-' + key + '-1', val.daysPercent, val.days, 'DÍAS')
        _generateCircle('circle-' + key + '-2', val.levelPercent, val.level, 'NIVEL')
    });

    this.settings.listItems = this.settings.list.find("li");

    // Quita loader
    this.settings.containerList.removeLoader();

    this.bind("list");
};

/**
 *
 * @param {string}						- "header", "filters", "list", "tests"
 */
FilterTest.prototype.bind = function(action) {

    switch (action) {

        case "header":

            this.settings.btToPlanes
                .on("click", this, function(e) {

                    e.preventDefault();

                    e.data.settings
                        .btFilters
                        .eq(0)
                        .click();
                });

            this.settings.btToTest
                .on("click", this, function(e) {

                    e.preventDefault();

                    e.data.test({
                        action: "open"
                    });
                });
            break;

        case "filters":

            this.settings.headerList
                .waypoint('sticky', {
                    stuckClass: 'stuck'
                });

            this.settings.containerTest
                .waypoint('sticky', {
                    offset: 96,
                    stuckClass: 'stuck'
                });

            this.settings.containerTest
                .waypoint('sticky', {
                    stuckClass: 'small',
                    wrapper: '',
                    offset: 80,
                });

            this.settings.btFilters
                .on("click", this, function(e) {

                    e.preventDefault();

                    // Si el test no estaba terminado, limplia las opciones seleccionadas del test
                    if ($.isEmptyObject(e.data.optionsTestSelected["#objetivo"])) {
                        // console.log("el test no se habia completado!!!!!!!!!")
                        e.data.optionsTestSelected = {};
                    };
                    e.data.settings.listTop.html('');
                    e.data.isTestComplete = false;

                    // console.log("+++++++++++++++remove class open")
                    e.data.settings.btTests
                        .removeClass("open")
                        .removeClass("active");

                    e.data.settings.btFilters
                        .removeClass("active");

                    $(this)
                        .addClass("active");

                    var toDisplay = $(this).get(0).hash;
                    toDisplay = toDisplay.replace("#", "");

                    e.data.filter({
                        action: "show",
                        items: [toDisplay]
                    });
                });
            break;

        case "tests":

            // Muestra el panel del test
            this.settings.btTests
                .on("click", this, function(e) {

                    e.preventDefault();

                    action = "open";
                    if ($(this).hasClass("open")) { // Valida que esté abierto o cerrado
                        action = "close";
                    } else {

                        if ($.isEmptyObject(e.data.optionsTestSelected["#objetivo"])) { // Si el test no se habia completado se reinicia

                            // console.log("El test no se habia completado, se reinicia: ");
                            // console.log(e.data.optionsTestSelected)

                            e.data.settings.breadcrumbsTest
                                .eq(0)
                                .click();
                        } else { // Carga la lista de planes del test
                            // console.log("Carga la lista de planes del test: ")
                            // console.log(e.data.optionsTestSelected)

                            var total = e.data.filter({
                                action: "show",
                                items: e.data.optionsTestSelected
                            });

                            e.data.settings.listTop.html('<p>"RESULTADO DEL TEST: ' + total + ' coincidencias."</p>');
                        };
                    };
                    // console.log("aaaaaa action on btTests: " + action)
                    e.data.test({
                        action: action
                    });
                });

            // Selecciona una opcion del test
            this.settings.btOptionTest
                .on("click", this, function(e) {

                    e.preventDefault();
                    e.stopPropagation();

                    e.data.test({
                        action: "go-to-step",
                        nexStep: $(this).data("next-step"), // paso siguiente
                        resume: {
                            id: $(this).get(0).hash, // índice
                            content: $(this).data("content"), // contenido
                            text: $(this).find("p").text() // contenido html
                        }
                    });
                });

            // BreadcrumbsTest
            this.settings.breadcrumbsTest
                .on("click", this, function(e) {

                    e.preventDefault();
                    e.stopPropagation();

                    // Se reinicia el test
                    e.data.isTestComplete = false;
                    // console.log("Se reinicia el test: a");
                    // console.log(e.data.isTestComplete)

                    var nexStep = $(this).data("next-step");

                    e.data.test({
                        action: "go-to-step",
                        nexStep: nexStep
                    });
                });

            // Cierra Test
            this.settings.btCloseTest
                .on("click", this, function(e) {

                    e.preventDefault();

                    // Si no se termino el test, se quita el copy "RESULTADO DEL TEST"
                    if ($.isEmptyObject(e.data.optionsTestSelected["#objetivo"])) {

                        e.data.settings.listTop.html('');
                    };

                    e.data.settings.btTests
                        .click();
                });

            // Reinicia Test
            this.settings.btRefreshTest
                .on("click", this, function(e) {

                    e.data.isTestComplete = false;
                    // console.log("reinicia test: ");
                    // console.log(e.data.isTestComplete)

                    e.preventDefault();
                    e.data.settings.breadcrumbsTest
                        .eq(0)
                        .click();
                });

            break;

        case "list":

            this.settings.planes = $Train.find("#train-filter ul.list li a");

            this.settings.planes.each(function(index, val) {

                // console.log($(this))
                var hover = $(this).find(".hover");

                $(this).mouseenter(function() {

                    TweenMax.to(hover, 0.15, {
                        left: "43.7%"
                    })
                })
                    .mouseleave(function() {

                        TweenMax.to(hover, 0.15, {
                            left: "100%"
                        })
                    });
            });
            break;
    };
};

/**
 *
 * @param {object} 	params				-
 * @param {string} 	params.action		- "hidden", "visible", "show"
 * @param {string} 	params.items		- Nombre de clase que contienen los planes que se mostrarán
 */
FilterTest.prototype.filter = function(params) {

    var toReturn = true;
    var _filter = function(self, items) {
        var results = 0;
        $.each(self.settings.listItems, function(keyElement, valElement) {

            var el = $(this);
            var isMatch = true;
            $.each(items, function(keyTest, valTest) {

                if (!el.hasClass(valTest)) {
                    isMatch = false;
                };
            });

            if (isMatch) {

                results = results + 1;
                el.css({
                    display: "block"
                });
            };
        });

        return results;
    };
    switch (params.action) {

        //agrega clase "deactive" a filtros y a cada plan de la lista
        //agrega un margen top a la lista de planes para que de espacio a que se muestre el test
        case "hidden":

            // quita filtro seleccionado
            this.settings.btFilters
                .addClass("deactive");

            // oculta todo los planes si el test no se ha terminado
            if ($.isEmptyObject(this.optionsTestSelected["#objetivo"])) {
                // console.log("oculta todo los planes, el test no se ha terminado");
                // console.log($.isEmptyObject(this.optionsTestSelected["#objetivo"]))
                this.settings.listItems
                    .addClass("deactive");
            };

            //agrega un margen top a la lista para que de espacio a que se muestre el test
            this.animations({
                action: "open-margin-test"
            });

            break;

            //quita clase "deactive" a filtros y a cada plan de la lista
            //quita margen top a la lista de planes, se oculta el test
        case "visible":
            // console.log(this.isTestComplete)
            // Si no se ha completado el test muestra el filtro previamente seleccionado
            if (this.isTestComplete === false) {
                // console.log("no se ha completado el test, se muestra el filtro previamente seleccionado");
                // console.log(this.isTestComplete)
                this.settings.btFilters
                    .removeClass("deactive");
            };

            // si no se ha completado el test muestra los planes que estaban activos
            if (this.isTestComplete === false) {
                // console.log("no se ha completado el test, muestra los planes que estaban activos");
                // console.log(this.isTestComplete)
                this.settings.listItems
                    .removeClass("deactive");
            };

            this.animations({
                action: "close-margin-test"
            });
            break;

            //muestra planes de la lista
        case "show":

            this.filter({
                action: "visible"
            });
            this.test({
                action: "close"
            });

            // Oculta todos los elementos de la lista
            this.settings.listItems
                .css({
                    display: "none"
                });

            // Muestra los seleccionados
            // this.filter( { action: "filter-option", items: params.items } );

            toReturn = _filter(this, params.items);

            // console.log("to return: " + toReturn)
            break;

            // Muestra los seleccionados
        case "filter-option":

            toReturn = _filter(this, params.items);
            // var self = this;
            // toReturn = 0;
            // $.each( self.settings.listItems, function( keyElement, valElement ){

            // 	var el = $( this );
            // 	var isMatch = true;
            // 	$.each( params.items, function( keyTest, valTest ){

            // 		if( !el.hasClass( valTest ) ){
            // 			isMatch = false;
            // 		};
            // 	} );

            // 	if( isMatch ){

            // 		toReturn = toReturn + 1;
            // 		el.css( { display: "block" } );
            // 	};
            // } );
            break;
    };

    return toReturn;
};

/**
 *
 * @param {object} 	params				-
 * @param {string} 	params.action		- "open", "close", "show"
 * @param {integer}	params.nexStep		- 0-3
 */
FilterTest.prototype.test = function(params) {

    switch (params.action) {

        case "open":

            this.settings.btTests
                .addClass("active");

            this.settings.btTests
                .addClass("open");

            this.filter({
                action: "hidden"
            });

            this.animations({
                action: "open-test"
            });

            break;

        case "close":

            // console.log("<<<<<<<<<<< close, is empty " + $.isEmptyObject(this.optionsTestSelected["#objetivo"]))

            if (!$.isEmptyObject(this.optionsTestSelected["#objetivo"])) {

                this.isTestComplete = true;
            };

            this.filter({
                action: "visible"
            });

            this.settings.btTests
                .removeClass("open");


            // Si el test no se habia terminado, queda seleccionado el boton del test previamente activado
            if (this.isTestComplete === false) {

                // console.log("el test no se habia terminado, queda seleccionado el boton del test previamente activado");
                // console.log(this.isTestComplete)
                this.settings.listTop.html('');

                this.settings.btTests
                    .removeClass("active");

                this.settings.btFilters
                    .find("active")
                    .click();
            };

            this.animations({
                action: "close-test"
            });
            break;

        case "go-to-step":

            this.settings.containerOptionTest
                .css({
                    display: "none"
                });

            this.settings.containerOptionTest
                .eq(params.nexStep)
                .css({
                    display: "block"
                });

            // Breadcrumbs, regresa al paso seleccionado
            this.settings.breadcrumbsTest
                .css({
                    display: "none"
                });
            for (var i = params.nexStep; i >= 0; i--) {

                this.settings.breadcrumbsTest
                    .eq(i)
                    .css({
                        display: "block"
                    })
                    .removeClass("last");
                this.optionsTestSelected["#objetivo"] = {};
            };
            this.settings.breadcrumbsTest
                .eq(params.nexStep)
                .addClass("last");

            // Agrega opcion seleccionada del test a su resumen
            if (params.resume) {

                this.settings.abrstractTest
                    .eq(params.nexStep - 1)
                    .find("p")
                    .html(params.resume.text);

                this.optionsTestSelected[params.resume.id] = params.resume.content
            };

            // Si ya es el último paso del test, muestra los resultados
            if (params.nexStep > 3) {

                this.isTestComplete = true;
                // console.log("se completa el test, a: ");
                // console.log(this.isTestComplete)

                // Muestra los items del filtro
                this.settings.listItems
                    .removeClass("deactive")
                    .css({
                        display: "none"
                    });

                var total = this.filter({
                    action: "filter-option",
                    items: this.optionsTestSelected
                });

                this.settings.listTop.html('<p>"RESULTADO DEL TEST: ' + total + ' coincidencias."</p>');
            } else {

                this.filter({
                    action: "hidden"
                });

                this.optionsTestSelected["#objetivo"] = {};
                this.settings.listTop.html('<h2>"REALIZA EL TEST PARA VER LOS RESULTADOS"</h2>');

                this.isTestComplete = false;
                // console.log("no esta completo, a: ");
                // console.log(this.isTestComplete)
            };
            break;
    };
};

/**
 *
 * @param {object} 	params				-
 * @param {string} 	params.action		- "scroll-test"
 */
FilterTest.prototype.animations = function(params) {

    switch (params.action || "error") {

        case "scroll-test": // Recorre scroll al header de los filtros

            var offset = -90;
            if (this.settings.btTests.hasClass("open")) {
                offset = -370;
            };
            var top = this.settings.listTop.offset().top + offset;
            TweenMax.to(window, 0.5, {
                scrollTo: {
                    y: top,
                    x: 0
                }
            });
            break;

        case "open-margin-test": // agrega margen top a la lista de planes

            var self = this;
            TweenMax.to(
                self.settings.listTop,
                .15, {
                    marginTop: 280,
                    onComplete: function() {
                        self.animations({
                            action: "scroll-test"
                        });
                    }
                }
            );
            break;

        case "close-margin-test": // quita margen top a la lista de planes

            var self = this;
            TweenMax.to(
                self.settings.listTop,
                .15, {
                    marginTop: 0,
                    onComplete: function() {
                        self.animations({
                            action: "scroll-test"
                        });
                    }
                }
            );
            break;

        case "open-test": // abre las opciones para hacer el test

            var self = this;

            self.settings.containerTest
                .addClass("active");

            TweenMax.to(
                self.settings.containerTest,
                0.15, {
                    height: 275,
                    onComplete: function() {
                        self.animations({
                            action: "scroll-test"
                        });
                    }
                }
            );
            break;

        case "close-test": // colapsa las opciones para hacer el test

            var self = this;

            self.settings.containerTest
                .removeClass("active");

            TweenMax.to(
                self.settings.containerTest,
                0.15, {
                    height: 0,
                    onComplete: function() {
                        self.animations({
                            action: "scroll-test"
                        });
                    }
                }
            );
            break;
    };
};