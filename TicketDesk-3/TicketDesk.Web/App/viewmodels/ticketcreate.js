define(['durandal/app', 'services/datacontext', 'durandal/plugins/router'],
    function (app, datacontext, router) {
        var isSaving = ko.observable(false);
        var isDeleting = ko.observable(false);
        var ticket = ko.observable();

        var goBack = function () {
            router.navigateBack();
        };

        var hasChanges = ko.computed(function () {
            return datacontext.hasChanges();
        });

        var cancel = function () {
            datacontext.cancelChanges();
        };

        var canSave = ko.computed(function () {
            return hasChanges() && !isSaving();
        });



        var canDeactivate = function () {
            if (isDeleting()) { return false; }

            if (hasChanges()) {
                var title = $.i18n.t('appuitext:navNavigateAwayConfirmDialogTitle', { pagetitle: vm.title });
                var msg = $.i18n.t('appuitext:navNavigateAwayConfirmDialogMessage');
                var checkAnswer = function (selectedOption) {
                    if (selectedOption === $.i18n.t('appuitext:generalYes')) {
                        cancel();
                    }
                    return selectedOption;
                };
                return app.showMessage(title, msg, [$.i18n.t('appuitext:generalYes'), $.i18n.t('appuitext:generalNo')])
                    .then(checkAnswer);
            };
            return true;
        };

        var save = function () {
            isSaving(true);
            return datacontext.saveChanges().fin(complete);

            function complete() {
                isSaving(false);
            }
        };

        var vm = {
            title: $.i18n.t('appuitext:viewTicketCreateTitle'),
            cancel: cancel,
            canDeactivate: canDeactivate,
            canSave: canSave,
            goBack: goBack,
            hasChanges: hasChanges,
            save: save,
            ticket: ticket
        };

        return vm;
    });