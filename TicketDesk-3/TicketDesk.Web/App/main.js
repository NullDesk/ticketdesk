requirejs.config({
    paths: {
        'text': '../Scripts/text',
        'durandal': '../Scripts/durandal',
        'plugins': '../Scripts/durandal/plugins',
        'transitions': '../Scripts/durandal/transitions'
    }
});

define('jquery', [], function () { return jQuery; });
define('knockout', [], function () { return ko; });


define(['durandal/system', 'durandal/app', 'durandal/viewLocator', 'durandal/binder','services/logger'],
    function (system, app, viewLocator, binder, logger) {

        app.title = $.i18n.t('appuitext:appName');//"TicketDesk"

        // Enable debug message to show in the console 
        system.debug(true);


        //specify which plugins to install and their configuration
        app.configurePlugins({
            router: true,
            dialog: true,
            //widget: {
            //    kinds: ['expander']
            //}
        });

        app.start().then(function () {
            toastr.options.positionClass = 'toast-bottom-right';
            toastr.options.backgroundpositionClass = 'toast-bottom-right';

            viewLocator.useConvention();


            var i18nOptions = {
                //fallbackLang: 'en',
                useCookie: false,
                ns: { namespaces: ['appuitext', 'appmodeltext'], defaultNs: 'appuitext' },
                resGetPath: 'api/text/__lng__/__ns__'
            };

            if (!window.location.search.indexOf("setLng=")) {
                $.extend(i18nOptions, { lng: window.ticketDeskUserLanguage() });
            }
            binder.binding = function (obj, view) {
                $(view).i18n();
            };


            $.i18n.init(i18nOptions, function () {

                //Show the app by setting the root view model for our application.
                app.setRoot('viewmodels/shell', 'entrance');
            });
        });
    });