define(['durandal/app', 'services/datacontext', 'durandal/plugins/router', 'services/logger'],
    function (app, datacontext, router, logger) {

        var ticket = ko.observable();
        var priorityList = ko.observableArray();
        var categoryList = ko.observableArray();
        var statusList = ko.observableArray();
        var ticketTypeList = ko.observableArray();
        var isSaving = ko.observable(false);
        var isDeleting = ko.observable(false);


        var activate = function (routeData) {
            var id = parseInt(routeData.id);
            return Q.all([
                datacontext.getTicketById(id, ticket),
                datacontext.getPriorityList(priorityList),
                datacontext.getTicketTypeList(ticketTypeList),
                datacontext.getCategoryList(categoryList),
                datacontext.getStatusList(statusList)
            ]);
        };

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

        var save = function () {
            isSaving(true);
            return datacontext.saveChanges().fin(complete);

            function complete() {
                isSaving(false);
            }
        };

        var canDeactivate = function () {
            if (isDeleting()) { return false; }

            if (hasChanges()) {
                var title = $.i18n.t('appuitext:navNavigateAwayConfirmDialogTitle', { pagetitle: ticket().title() });
                var msg = $.i18n.t('appuitext:navNavigateAwayConfirmDialogMessage');
                var checkAnswer = function (selectedOption) {
                    if (selectedOption === $.i18n.t('appuitext:generalYes')) {
                        cancel();
                    }
                    return selectedOption;
                }

                return app.showMessage(title, msg, [$.i18n.t('appuitext:generalYes'), $.i18n.t('appuitext:generalNo')])
                    .then(checkAnswer);
            };
            return true;
        };

        var vm = {
            activate: activate,
            title: $.i18n.t('appuitext:viewTicketDetailsTitle'),
            cancel: cancel,
            canDeactivate: canDeactivate,
            canSave: canSave,
            goBack: goBack,
            hasChanges: hasChanges,
            save: save,
            ticket: ticket,
            categoryList: categoryList,
            ticketTypeList: ticketTypeList,
            priorityList: priorityList,
            statusList: statusList
        };
        return vm;
    });