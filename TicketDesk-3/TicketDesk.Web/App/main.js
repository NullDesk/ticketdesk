requirejs.config({
	baseUrl : "/App",
	paths: {
		'text': '../Scripts/text',
		'durandal': '../Scripts/durandal',
		'plugins': '../Scripts/durandal/plugins',
		'transitions': '../Scripts/durandal/transitions',
		'i18next': '../Scripts/i18next.amd.withJQuery-1.7.1'
	}
});

define('jquery', function () { return jQuery; });
define('knockout', ko);

define(['durandal/app', 'durandal/viewLocator', 'durandal/system', 'services/appsecurity', 'durandal/binder', 'i18next'],
	function (app, viewLocator, system, appsecurity, binder, i18n) {

		//>>excludeStart("build", true);
		system.debug(true);
		//>>excludeEnd("build");

		app.title = 'TicketDesk 3';

		//specify which plugins to install and their configuration
		app.configurePlugins({
			router: true,
			dialog: true,
			widget: {
				kinds: ['expander']
			}
		});

		app.start().then(function () {

			//Replace 'viewmodels' in the moduleId with 'views' to locate the view.
			//Look for partial views in a 'views' folder in the root.
			viewLocator.useConvention();

			var i18nOptions = {
			    //fallbackLang: 'en',
			    useCookie: false,
			    ns: { namespaces: ['appuitext', 'appmodeltext'], defaultNs: 'appuitext' },
			    resGetPath: window.applicationPath + 'api/text/__lng__/__ns__'
			    //resGetPath: 'api/text/__lng__/__ns__'
			};
		    
			if (!window.location.search.indexOf("setLng=")) {
			    $.extend(i18nOptions, { lng: window.ticketDeskUserLanguage() });
			}


			i18n.init(i18nOptions, function () {
			    app.title = i18n.t('appuitext:appName');//"TicketDesk"
			    binder.binding = function (obj, view) {
			        $(view).i18n();
			    };

			});

			

			//Loading indicator

			var loader = new Stashy.Loader("body");

			$(document).ajaxStart(function () {
				loader.on("fixed", "-5px", "#FFF", "prepend");
			}).ajaxComplete(function () {
				loader.off();
			});

			// Configure ko validation
			ko.validation.init({
				decorateElement: true,
				errorElementClass: "has-error",
				registerExtenders: true,
				messagesOnModified: true,
				insertMessages: true,
				parseInputAttributes: true,
				messageTemplate: null
			});

			//hightlight.js configuration
			marked.setOptions({
				highlight: function (code) {
					return hljs.highlightAuto(code).value;
				},
				sanitize: true,
				breaks : true
			});

			// Automatic resizing for textareas
			// auto adjust the height of
			$(document).on('keyup', '.auto-height-textarea', function (e) {
				$(this).css('height', 'auto');
				$(this).height(this.scrollHeight);
			});	

		    //Show the app by setting the root view model for our application with a transition.
			appsecurity.initializeAuth()
                .then(function (data) {
                   
                    app.setRoot('viewmodels/shell');
                });
		});
	});