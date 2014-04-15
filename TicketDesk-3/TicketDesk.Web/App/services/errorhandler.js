/** 
 * @module Manage errors in the app
 * @requires logger
 * @requires system
 * @requires utils
 */

define(['services/logger', 'durandal/system', 'services/utils'],
    function (logger, system, util) {

        var ErrorHandler = (function () {
            /**
             * @constructor
             */
            var ctor = function (targetObject) {

                /** 
                 * Handle Breeze validation errors
                 * @method
                 * @param {object} error - The error object
                 */
                this.handleError = function (error) {
                    if (error.entityErrors) {
                        error.message = util.getSaveValidationErrorMessage(error);
                    }

                    logger.logError(error.message, null, system.getModuleId(targetObject), true);
                    throw error;
                };

                /**
                 * Log the error
                 * @method
                 * @param {string} message
                 * @param {bool} showToast - Show a toast using toastr.js
                 */
                this.log = function (message, showToast) {
                    logger.log(message, null, system.getModuleId(targetObject), showToast);                    
                };

                /**
                 * Handle validation errors without Breeze. 
				 * It´s mandatory to return a ModelState object from server.
                 * @method
                 * @param {jQueryXMLHttpRequest} jqXHR
				 * @param {string} textStatus
				 * @param {string} error
                 */
                this.handlevalidationerrors = function (jqXHR, textStatus, error) {
					var data,
						items;

					try {
						data = $.parseJSON(jqXHR.responseText);
					}
					catch (e) {
						data = null;
					}					

					if (!data || !data.message) {
						return;
					}

					if (data.modelState) {
						for (var key in data.modelState) {
							items = data.modelState[key];

							if (items.length) {
								for (var i = 0; i < items.length; i++) {
									logger.logError(items[i], null, items, true);
								}
							}
						}
					}

					if (items.length === 0) {
						logger.logError(data.message, null, data, true);
					}
                };

                /**
                 * Handle authentication errors                 
                 * @method
                 * @param {object} errors
				 * @returns {object} error object
                 */
                this.handleauthenticationerrors = function (errors) {
                    if (errors.responseText != "") {
                        var data = $.parseJSON(errors.responseText);
                        if (data && data.error_description) {
                            logger.logError(data.error_description, null, errors, true);
                        } else {
                            if (data.message) {
                                logger.logError(data.message, null, errors, true);
                            }
                        }
                    }
                };
            };

            return ctor;
        })();

        return {
            includeIn: includeIn
        };

        /**
         * Include the error handler class in any viewmodel
         * @method
         * @param {object} targetObject
         * @return {object} - The extended object
         */
        function includeIn(targetObject) {
            return $.extend(targetObject, new ErrorHandler(targetObject));
        }
    });