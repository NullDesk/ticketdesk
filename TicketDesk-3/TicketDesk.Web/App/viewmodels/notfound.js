define(['plugins/router'], function (router) {

    return {
        convertRouteToHash: router.convertRouteToHash,

        activate: function () {
            ga('send', 'pageview', { 'page': window.location.href, 'title': document.title });
        }
    };

});