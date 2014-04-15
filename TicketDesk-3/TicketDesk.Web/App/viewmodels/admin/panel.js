define(['services/appsecurity'], function (appsecurity) {
    var viewmodel = {
        userprofiles: ko.observableArray(),

        activate: function () {
            ga('send', 'pageview', { 'page': window.location.href, 'title': document.title });
        },

        attached: function () {
            var self = this;

            appsecurity.getUsers().then(function (data) {
                self.userprofiles(data);
            });
        }
    }

    return viewmodel;
});