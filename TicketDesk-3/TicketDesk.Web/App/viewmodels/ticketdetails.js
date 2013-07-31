define(['services/datacontext', 'durandal/plugins/router'],
    function (datacontext, router) {

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
                var title = 'Do you want to leave "' +
                    ticket().title() + '" ?';
                var msg = 'Navigate away and cancel your changes?';
                var checkAnswer = function (selectedOption) {
                    if (selectedOption === 'Yes') {
                        cancel();
                    }
                    return selectedOption;
                };
                return app.showMessage(title, msg, ['Yes', 'No'])
                    .then(checkAnswer);
            }
            return true;
        };

        var vm = {
            activate: activate,
            title: 'Ticket Details',
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