﻿require.config({
    paths: { "text": "durandal/amd/text" }
});

define([
        'durandal/app',
        'durandal/viewLocator',
        'durandal/viewModelBinder',
        'durandal/system',
        'durandal/plugins/router',
        'services/logger'],
    function (
        app,
        viewLocator,
        viewModelBinder,
        system,
        router,
        logger) {

        // Enable debug message to show in the console 
        system.debug(true);

        app.start().then(function () {
            toastr.options.positionClass = 'toast-bottom-right';
            toastr.options.backgroundpositionClass = 'toast-bottom-right';

            router.handleInvalidRoute = function (route, params) {
                logger.logError('No Route Found', route, 'main', true);
            };

            // When finding a viewmodel module, replace the viewmodel string 
            // with view to find it partner view.
            router.useConvention();
            viewLocator.useConvention();
            
            var option = {
                //fallbackLang: 'en',
                useCookie: false,
                ns: { namespaces: ['appuitext', 'appmodeltext'], defaultNs: 'appuitext' },
                resGetPath: 'api/text/__lng__/__ns__'
            };

            if (!window.location.search.indexOf("setLng=")) {
                $.extend(option, { lng: window.ticketDeskUserLanguage() });
            }
            viewModelBinder.beforeBind = function (obj, view) {
                $(view).i18n();
            };
            
            
            $.i18n.init(option, function () {
                
                //// smr - This breaks scrolling on touch enabled devices; I am not sure 
                ////   what they thought preventing default on touch events
                ////   was supposed to do, but it isn't what I'd call adapting
                ////   to the device.
                // Adapt to touch devices
                // app.adaptToDevice();
                //Show the app by setting the root view model for our application.
                app.setRoot('viewmodels/shell', 'entrance');
            });
        });
    });