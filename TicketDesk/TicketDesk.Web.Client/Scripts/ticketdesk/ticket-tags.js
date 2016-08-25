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
    window.ticketTags = (function () {

        var config = {};
       


        var cofigureTags = function () {
            $('#ticketTags').select2({
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
                ajax: {
                    url: config.tagAutoCompleteUrl,
                    type: 'GET',
                    dataType: 'json',
                    data: function (term) {
                        return {
                            term: term
                        };
                    },
                    results: function (data, page) {
                        return { results: data };
                    }
                },
                createSearchChoice: function (term, data) {
                    if ($(data).filter(function () { return this.text.localeCompare(term) === 0; }).length === 0) {
                        return { id: term, text: term };
                    }
                    return null;
                }
            });
            $('#ticketTags').select2('container').find('ul.select2-choices').sortable({
                containment: 'parent',
                start: function () { $('#ticketTags').select2('onSortStart'); },
                update: function () { $('#ticketTags').select2('onSortEnd'); }
            });
        };

        var acivate = function (tdConfig) {
            config = tdConfig;
            cofigureTags();
        };

        return {
            activate: acivate
        }
    })();
})(window);