(function (window) {
    var editTicket = (function () {

        var activate = function () {

            initDetails();
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
            activate: activate
        };

        
        

    })();
    window.editTicket = editTicket;
})(window);
