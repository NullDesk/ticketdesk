define(['durandal/app', 'services/datacontext', 'plugins/router', 'services/logger'],
    function (app, datacontext, router, logger) {
        var isSaving = ko.observable(false);
        var isDeleting = ko.observable(false);
        var ticket = ko.observable();
        var priorityList = ko.observableArray();
        var categoryList = ko.observableArray();
        var ticketTypeList = ko.observableArray();

        var activate = function () {
            ticket(datacontext.ticketEntityManager.createTicket());
            return Q.all([

                            datacontext.getPriorityList(priorityList),
                            datacontext.getTicketTypeList(ticketTypeList),
                            datacontext.getCategoryList(categoryList)])
                .then(function () {
                    logger.log('Ticket Create View Activated', null, 'TicketCreate', true);
                });
        };



        var goBack = function () {
            router.navigateBack();
        };

        var hasChanges = ko.computed(function () {
            return datacontext.hasChanges();
        });

        var cancel = function () {
            resetModel(false);
        };

        var resetModel = function(isDeactivating) {
            datacontext.cancelChanges();
            if (!isDeactivating) {
                ticket(datacontext.ticketEntityManager.createTicket());
            }
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
                    var isYes = selectedOption === $.i18n.t('appuitext:generalYes');
                    if (isYes) {
                        resetModel(true);
                    }
                    return isYes;
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


        var initEditors = function () {
            (function () {

                //var help = function () { alert("Do you need help?"); };
                //var options = {
                //    helpButton: { handler: help },
                //    strings: { quoteexample: "whatever you're quoting, put it right here" }
                //};

                var converter1 = Markdown.getSanitizingConverter();
                converter1.hooks.chain("preBlockGamut", function (text, rbg) {
                    return text.replace(/^ {0,3}""" *\n((?:.*?\n)+?) {0,3}""" *$/gm, function (whole, inner) {
                        return "<blockquote>" + rbg(inner) + "</blockquote>\n";
                    });
                });

                // smarter newline handling
                converter1.hooks.chain("preConversion", function (text) {
                    return text.replace(/(\w+)\n/g, '$1  \n');
                });


                converter1.hooks.chain("plainLinkText", function (url) {
                    return url; //.replace(/^https?:\/\//, "");
                });

                var editor1 = new Markdown.Editor(converter1, "-ticketDetails", null);
                editor1.run();
               
                logger.log('Ticket Create View Editor Initialized', null, 'TicketCreate', false);

            })();


        };

        var vm = {
            attached: initEditors,
            activate: activate,
            title: $.i18n.t('appuitext:viewTicketCreateTitle'),
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
        };

        return vm;
    });