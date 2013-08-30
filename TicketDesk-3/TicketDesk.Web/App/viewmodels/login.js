define(['services/logger'], function (logger) {
    var vm = {
        activate: activate,
        title: $.i18n.t('appuitext:viewLoginTitle'),
    };

    //#region Internal Methods

    function activate() {
        logger.log('Login View Activated', null, 'login', true);
        return true;
    }

    //#endregion

    return vm;
});