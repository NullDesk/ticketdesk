/** 
  * @module Several utils for the app
*/

define(function () {
    return {
        getUrlParameter : getUrlParameter,
        addItemAnimation : addItemAnimation,
        removeItemAnimation : removeItemAnimation,
        getCurrentDate : getCurrentDate,
        getSaveValidationErrorMessage : getSaveValidationErrorMessage,
        getEntityValidationErrorMessage : getEntityValidationErrorMessage
    };

    /**
     * Get parameters from url
     * @param {string} name - The name of the parameter
     * @return {string}
    */
    function getUrlParameter(name) {
        return decodeURI(
            (RegExp(name + '=' + '(.+?)(&|$)').exec(location.href) || [, null])[1]
        );
    };

    /**
     * Add items to knockout foreach bindings with an animation
     * @param {string} elem - The element to add
    */
    function addItemAnimation(elem) {
        if (elem.nodeType === 1) {
            $(elem).addClass("animated fadeInLeft");
        }
    };

    /**
     * Remove items to knockout foreach bindings with an animation
     * @param {string} elem - The element to remove
    */
    function removeItemAnimation(elem) {
        if (elem.nodeType === 1) {
            $(elem).addClass("animated fadeOutLeft");
            setTimeout(function () {
                $(elem).remove();
            }, 500);
        } 
    };

    /** 
     * Get current date
     * @return {Date} - Current date
    */ 
    function getCurrentDate() {
        return moment.utc().format();
    };

    /**
     * Provisional version returns validation error messages 
     * of first entity that failed to save
     * @param {object} saveError - The save error object
     * @return {string} - Error validation message
    */ 
    function getSaveValidationErrorMessage(saveError) {
        try { // return the first entity's error message(s)
            var firstError = saveError.entityErrors[0];
            return 'Validation Error: ' + firstError.errorMessage;
        } catch (e) { // ignore problem extracting error message 
            return "Save validation error";
        }
    };

    /**
     * Return string of an entity's validation error messages 
     * @param {entity} entity
     * @return {string} - Error messages
    */
    function getEntityValidationErrorMessage(entity) {
        try {
            var errs = entity.entityAspect.getValidationErrors();
            var errmsgs = errs.map(function (ve) { return ve.errorMessage; });
            return errmsgs.length ? errmsgs.join("; ") : "no validation errors";
        } catch (e) {
            return "not an entity";
        }
    };
});