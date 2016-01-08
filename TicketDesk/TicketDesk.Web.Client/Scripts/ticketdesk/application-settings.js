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
    window.applicationSettings = (function () {
        var config;

        var cofigureSettings = function (elementId) {
            $('#' + elementId).select2({
                tags: [],
                multiple: true,
                minimumInputLength: 2,
                tokenSeparators: [',', ' '],
                width: 'resolve',
                initSelection: function (element, callback) {
                    var data = [];
                    $(element.val().split(',')).each(function () {
                        data.push({ id: this, text: this });
                    });
                    callback(data);
                },
                //ajax: {
                //    url: config.tagAutoCompleteUrl,
                //    type: 'GET',
                //    dataType: 'json',
                //    data: function (term) {
                //        return {
                //            term: term
                //        };
                //    },
                //    results: function (data, page) {
                //        return { results: data };
                //    }
                //},
                createSearchChoice: function (term, data) {
                    if ($(data).filter(function () { return this.text.localeCompare(term) === 0; }).length === 0) {
                        return { id: term, text: term };
                    }
                    return null;
                }
            });
            $('#' + elementId).select2('container').find('ul.select2-choices').sortable({
                containment: 'parent',
                start: function () { $('#' + elementId).select2('onSortStart'); },
                update: function () { $('#' + elementId).select2('onSortEnd'); }
            });
        };

        var activate = function (tdConfig) {
            config = tdConfig;
            cofigureSettings('categories');
            cofigureSettings('priorities');
            cofigureSettings('tickettypes');
            $("#defaultroles > option").each(function () {
                if (this.value === 'TdPendingUsers') {
                    $(this).attr('locked', 'locked');
                }
            });

            $('#defaultroles').select2();
        };

        return {
            activate: activate
        }
    })();
})(window);