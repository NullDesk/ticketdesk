(function (window) {
    window.i18n = {
        unwatch : 'Unwatch',
        watch : 'Watch',
        formatWatch: function (isSubscribed) { return isSubscribed ? 'Unwatch' : 'Watch'; },
        formatWatchTitle: function (isSubscribed) { return (isSubscribed ? 'Unwatch' : 'Watch') + ' Ticket'; }
    };
})(window);