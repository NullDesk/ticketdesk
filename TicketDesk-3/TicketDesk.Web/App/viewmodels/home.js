define(['services/logger'], function (logger) {
    var vm = {
        activate: activate,
        title: $.i18n.t('appuitext:viewHomeTitle'),
    };

    //#region Internal Methods
    
    function activate() {
        logger.log('Home View Activated', null, 'home', true);
        return true;
    }
    
    //#endregion
    
    return vm;
});