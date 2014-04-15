define(['plugins/router'], function (router) {

    return {

        convertRouteToHash: router.convertRouteToHash,
        attached: function (view, parent) {
            //$(view).i18n();
        },
        activate: function () {
            ga('send', 'pageview', { 'page': window.location.href, 'title': document.title });                      
        }
    };
});