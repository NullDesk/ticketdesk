(function (window) {
    window.i18n = {
        unwatch: 'Esquecer',
        watch: 'Acompanhar Atualizações',
        formatWatch: function (isSubscribed) { return isSubscribed ? 'Esquecer' : 'Acompanhar Atualizações'; },
        formatWatchTitle: function (isSubscribed) { return (isSubscribed ? 'Esquecer' : 'Acompanhar Atualizações') + ' do Chamado'; }
    };
})(window);