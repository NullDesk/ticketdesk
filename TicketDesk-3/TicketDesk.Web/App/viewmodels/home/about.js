define(function () {

    return {
        activate: function () {
            ga('send', 'pageview', { 'page': window.location.href, 'title': document.title });
        }
    };

});