(function (window) {
    var editTicket = (function () {
        var config;
        var activate = function (tdConfig) {
            config = tdConfig;
            initDetails();
            loadActivityButtons();
        };


        var loadActivityButtons = function() {
            $.get(config.activityButtonsUrl, function(data) {
                $('#activityButtonsPanel').empty().append(data);
            });
        }

        var loadActivity = function (activityName) {
            $('#activityPanel').parent().animate({ opacity: 0.5 }, 200);
            $.get(config.loadActivityUrl, {"activity" : activityName }, function (data) {
                $('#activityPanel').empty().addClass('panel-body').append(data).parent().animate({ opacity: 1 }, 200);
                configureEditor();
            });
        };

        var beginActivity = function () {
            
            $('#activityPanel').parent().animate({ opacity: 0.5 }, 200);
        };

        var completeActivity = function () {
            $('#activityPanel').empty().removeClass('panel-body').parent().animate({ opacity: 1 }, 200);
        };


        var configureEditor = function () {
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
            beginActivity: beginActivity,
            completeActivity: completeActivity
        };

        
        

    })();
    window.editTicket = editTicket;
})(window);
