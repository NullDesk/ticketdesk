(function (window) {
    var editTicket = (function () {
        var config;
        var activate = function (tdConfig) {
            config = tdConfig;
            configureDetails();
            loadActivityButtons();
        };
      
        var beginActivity = function () {

            $('#activityPanel').parent().animate({ opacity: 0.5 }, 200);
        };

        var cancelActivity = function (e) {
            e.preventDefault();
            $('#activityPanel').empty().parent().animate({ opacity: 1 }, 200);
        }

        var completeActivity = function (data) {
            if (data.length > 0) {
                renderActivityPanel(data);
            } else {
                loadEventPanel();
                loadActivityButtons();
                loadDetails();
                $('#activityPanel').empty().parent().animate({ opacity: 1 }, 200);
            }
        };
        var configureDetails = function () {

            var detailsLastHeight = 0;
            var detailsMinHeight = 200;

            setupDetails();

            //#region internal details configuration functions
            function setupDetails() {
                $("#detailTextExpander").each(function (idx, elem) {
                    $(elem).hide();
                    $(elem).data('expanded', false);
                });

                $("#detailsText").each(function () {
                    if ((this.scrollHeight > this.clientHeight)) {
                        detailsLastHeight = this.scrollHeight;
                        $("#detailTextExpander")
                            .show()
                            .on("click", expandDetails)
                            .children("#expandersymbol")
                            .on("click", function (event) {
                                $(event.target).parent().trigger('click');
                            });
                    }
                });
            }

            function toggleExpanderState(jqElem, newHeight, overflow, addClass, remClass) {
                jqElem.siblings("#detailsText")
                    .animate({ height: newHeight }, 300, function () {
                        $(this).css('overflow', overflow)
                            .siblings("#detailTextExpander")
                            .children("#expandersymbol")
                            .addClass(addClass)
                            .removeClass(remClass);
                    });
            }

            function expandDetails(event) {
                var jqElem = $(event.target);
                var expState = jqElem.data("expanded"); //jqElem.attr("data-expanded");
                if (expState) {
                    toggleExpanderState(jqElem, detailsMinHeight, "auto", "fa-chevron-down", "fa-chevron-up");
                } else {
                    toggleExpanderState(jqElem, detailsLastHeight, "visible", "fa-chevron-up", "fa-chevron-down");
                }
                jqElem.data("expanded", !expState);
            }
            //#endregion

        }
        var configureEditor = function () {
            var jelem = $('#wmd-input-activity');
            if (jelem.length > 0) {


                if (jelem.data('is-required')) {
                    jelem.attr('data-val', "true").attr('data-val-required', "");
                    $.validator.unobtrusive.parseElement(jelem.get(0));
                }


                var converter1 = Markdown.getSanitizingConverter();

                converter1.hooks.chain("preBlockGamut", function (text, rbg) {
                    return text.replace(/^ {0,3}""" *\n((?:.*?\n)+?) {0,3}""" *$/gm, function (whole, inner) {
                        return "<blockquote>" + rbg(inner) + "</blockquote>\n";
                    });
                });

                converter1.hooks.chain("postSpanGamut", function (text) {
                    return text.replace(/\n/g, " <br>\n");
                });

                var editor1 = new Markdown.Editor(converter1, "-activity");

                editor1.run();
            }
        };

        // ReSharper disable once UnusedParameter
        var failActivity = function (data) {
            $('#activityPanel').animate({ opacity: 1 }, 200);
        }

        var loadActivityButtons = function () {
            $.get(config.activityButtonsUrl, function (data) {
                $('#activityButtonsPanel').empty().append(data);
            });
        };

        var loadActivity = function (activityName) {
            $('#activityPanel').parent().animate({ opacity: 0.5 }, 200);
            $.get(config.loadActivityUrl, { "activity": activityName }, renderActivityPanel);
        };

        var loadDetails = function () {
            $('#ticketDetailPanel').parent().animate({ opacity: 0.5 }, 200);
            $.get(config.loadDetailsUrl, renderDetailsPanel);
        };

        var loadEventPanel = function () {
            $('#eventPanel').parent().animate({ opacity: 0.5 }, 200);
            $.get(config.eventPanelUrl, renderEventPanel);
        }

        var renderActivityPanel = function (data) {
            $('#activityPanel').empty().append(data).parent().animate({ opacity: 1 }, 200);;
            $('#activityPanel #activityBody').addClass('panel-body');
            
            configureEditor();
        };

        var renderDetailsPanel = function (data) {
            $('#ticketDetailPanel').empty().append(data).parent().animate({ opacity: 1 }, 200);
            configureDetails();
        };

        var renderEventPanel = function (data) {
            $('#eventPanel').empty().addClass('panel-body').append(data).parent().animate({ opacity: 1 }, 200);
        }

        return {
            activate: activate,
            loadDetails: loadDetails,
            loadActivity: loadActivity,
            cancelActivity: cancelActivity,
            beginActivity: beginActivity,
            completeActivity: completeActivity,
            failActivity: failActivity
        };




    })();
    window.editTicket = editTicket;
})(window);
