(function (window) {
    var editTicket = (function () {
        var config;
        var activate = function (tdConfig) {
            config = tdConfig;
            initDetails();
            loadActivityButtons();
        };


        var loadActivityButtons = function () {
            $.get(config.activityButtonsUrl, function (data) {
                $('#activityButtonsPanel').empty().append(data);
            });
        }
        var renderActivityPanel = function (data) {
            $('#activityPanel').empty().addClass('panel-body').append(data).parent().animate({ opacity: 1 }, 200);
            configureEditor();
        };
        var renderEventPanel = function(data) {
            $('#eventPanel').empty().addClass('panel-body').append(data).parent().animate({ opacity: 1 }, 200);
        }

        var loadActivity = function (activityName) {
            $('#activityPanel').parent().animate({ opacity: 0.5 }, 200);
            $.get(config.loadActivityUrl, { "activity": activityName }, renderActivityPanel);
        };

        var loadEventPanel = function () {
            $('#eventPanel').parent().animate({ opacity: 0.5 }, 200);
            $.get(config.eventPanelUrl, renderEventPanel);
        }

        var cancelActivity = function() {
            $('#activityPanel').empty().removeClass('panel-body').parent().animate({ opacity: 1 }, 200);
        }

        var beginActivity = function () {

            $('#activityPanel').parent().animate({ opacity: 0.5 }, 200);
        };

        var completeActivity = function (data) {
            if (data.length > 0) {
                renderActivityPanel(data);
            } else {
                loadEventPanel();
                loadActivityButtons();
                $('#activityPanel').empty().removeClass('panel-body').parent().animate({ opacity: 1 }, 200);
            }
        };

        var failActivity = function (data) {
            $('#activityPanel').animate({ opacity: 1 }, 200);
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

        var initDetails = function () {

            var detailsLastHeight = 0;
            var detailsMinHeight = 200;

            setupDetails();

            //#region internal details area functions
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

        return {
            activate: activate,
            loadActivity: loadActivity,
            cancelActivity: cancelActivity,
            beginActivity: beginActivity,
            completeActivity: completeActivity,
            failActivity: failActivity
        };




    })();
    window.editTicket = editTicket;
})(window);
