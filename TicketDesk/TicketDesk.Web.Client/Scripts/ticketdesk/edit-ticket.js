// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (https://github.com/stephenredd)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

(function (window) {
    window.editTicket = (function () {
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
        };

        var completeActivity = function (data) {
            if (data.length > 0) {
                renderActivityPanel(data);
            } else {
                loadEventPanel();
                loadAttachmentsPanel();
                loadActivityButtons();
                loadDetails();
                $('#activityPanel').empty().parent().animate({ opacity: 1 }, 200);
            }
        };
        var configureDetails = function () {

            var detailsLastHeight = 0;
            var detailsMinHeight = 200;

            setupDetails();
            configureWatchButton();
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

        };
        var configureCommentEditor = function () {

            $.validator.setDefaults({
                ignore: ""
            });
            if (config.isEditorDefaultHtml) {
                jelem = $('#wmd-input-activity');
                jelem.summernote({
                    height: 200,
                    lang: window.currentCulture,
                    toolbar: [
                        ['style', ['style']],
                        ['font', ['bold', 'italic']],
                        ['fontname', ['fontname']],
                        ['fontsize', ['fontsize']],
                        ['color', ['color']],
                        ['para', ['ul', 'ol', 'paragraph']],
                        ['insert', ['link', 'hr']],
                        ['view', ['fullscreen']],
                        ['help', ['help']]
                    ]
                });
                if (jelem.length > 0) {

                    if (jelem.data('is-required')) {
                        jelem.attr('data-val', "true").attr('data-val-required', "");
                       

                       
                        $.validator.unobtrusive.parseElement(jelem.get(0));
                    }
                }

            } else {

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
            }
        };

        var configureWatchButton = function () {
            setWatchButton(config.isSubscribed);
            $('#watch').on('click', function (e) {
                $.ajax({
                    type: 'POST',
                    url: config.changeTicketSubscription,
                    dataType: 'json'
                }).done(function (data) {
                    config.isSubscribed = data.isSubscribed;
                    loadDetails();
                });
            });
        }



        // ReSharper disable once UnusedParameter
        var failActivity = function (data) {
            $('#activityPanel').animate({ opacity: 1 }, 200);
        };

        var loadActivityButtons = function () {
            $.get(config.activityButtonsUrl, function (data) {
                $('#activityButtonsPanel').empty().append(data);
            });
        };

        var loadActivity = function (activityName) {
            $('#activityPanel').parent().animate({ opacity: 0.5 }, 200);
            $.get(config.loadActivityUrl, { "activity": activityName, "tempId": $('#tempId').val() }, renderActivityPanel);
        };

        var loadAttachmentsPanel = function () {
            $('#attachmentsPanel').animate({ opacity: 0.5 }, 200);
            $.get(config.attachmentsPanelUrl, renderAttachmentsPanel);
        }
        var loadDetails = function () {
            $('#ticketDetailPanel').parent().animate({ opacity: 0.5 }, 200);
            $.get(config.loadDetailsUrl, renderDetailsPanel);            
        };

        var loadEventPanel = function () {
            $('#eventPanel').parent().animate({ opacity: 0.5 }, 200);
            $.get(config.eventPanelUrl, renderEventPanel);
        };

        var renderActivityPanel = function (data) {
            $('#activityPanel').empty().append(data).parent().animate({ opacity: 1 }, 200);
            $('#activityPanel #activityBody').addClass('panel-body');


            if ($('div#attachmentsDropZone').length) {
                window.ticketFileUploader.activate(config.uploaderConfig);
            }
            if ($('#ticketTags').length) {
                window.ticketTags.activate(config.tagsConfig);
            }
            if ($('#wmd-input-ticketDetails').length) {
                window.ticketDetails.activate(config.detailsConfig);
            }
            if ($('#wmd-input-activity').length) {
                configureCommentEditor();
            }
            $("#DueDateAsString").datepicker({ autoclose: true, clearBtn: true, todayBtn: "linked" });
            $("#TargetDateAsString").datepicker({ autoclose: true, clearBtn: true, todayBtn: "linked" });
            $("#ResolutionDateAsString").datepicker({ autoclose: true, clearBtn: true, todayBtn: "linked" });
        };

        var renderDetailsPanel = function (data) {
            $('#ticketDetailPanel').empty().append(data).parent().animate({ opacity: 1 }, 200);
            configureDetails();
        };

        var renderEventPanel = function (data) {
            $('#eventPanel').empty().addClass('panel-body').append(data).parent().animate({ opacity: 1 }, 200);
        };

        var renderAttachmentsPanel = function (data) {
            $('#attachmentsPanel').empty().append(data).animate({ opacity: 1 }, 200);
        }

        var setWatchButton = function (isSubscribed) {
            var icon = $('#watch > i');
            var iconClass = isSubscribed ? 'fa-eye-slash' : 'fa-eye';
            icon.removeClass('fa-eye fa-eye-slash').addClass(iconClass);
            $('#watch>span').text(window.i18n.formatWatch(isSubscribed));
            $('#watch').attr('title', window.i18n.formatWatchTitle(isSubscribed));
            $('#watch[data-toggle="tooltip"]').tooltip();
            config.isSubscribed = isSubscribed;
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
})(window);
