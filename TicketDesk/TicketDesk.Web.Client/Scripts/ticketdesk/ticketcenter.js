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
    window.ticketCenter = function() {
        var makeClicky = function() {
            $(".clickable").clickable();
        };

        var completeChangeList = function() {
            $('#ticketList').fadeIn(100);
            makeClicky();
            filters.setupFilterForm();
        };

        var paging = function() {
            var beginChangePage = function(args) {

                $('#ticketList').fadeOut(100);
            };

            return { beginChangePage: beginChangePage }
        }();

        var filters = function() {
            var beginChangeFilter = function(args) {

                $('#ticketList').fadeOut(100);
            };

            var setupFilterForm = function() {
                $('.filterBar select').addClass('form-control');
                $('.pagination').addClass('pagination-sm');
                $("#filterSubmitButton").hide();
                $('select.postback').change(function() {
                    $('#filterSubmitButton').trigger('click');
                });
            };

            return {
                beginChangeFilter: beginChangeFilter,
                setupFilterForm: setupFilterForm
            }
        }();

        var sorts = function() {
            var shiftstatus = false;

            var setShiftStatus = function(e) {
                if (e) {
                    shiftstatus = e.shiftKey;
                }
            };

            var beginChangeSort = function(event, args) {

                if (shiftstatus) {
                    args.url = args.url + "&isMultiSort=true";
                }
                $('#ticketList').fadeOut(100);
            };

            return {
                setShiftStatus: setShiftStatus,
                beginChangeSort: beginChangeSort,
            }
        }();


        return {
            makeClicky: makeClicky,
            completeChangeList: completeChangeList,
            paging: paging,
            sorts: sorts,
            filters: filters
        };
    }();
})(window);













