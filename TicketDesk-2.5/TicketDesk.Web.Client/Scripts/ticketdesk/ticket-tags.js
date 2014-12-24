(function (window) {
    var ticketTags = (function () {

        var config = {};
        var acivate = function (tdConfig) {
            config = tdConfig;
            cofigureTags();
        };


        var cofigureTags = function () {
            $('#ticketTags').select2({
                tags: [],
                multiple: true,
                minimumInputLength: 2,
                tokenSeparators: [",", " "],
                width: 'resolve',
                initSelection: function (element, callback) {
                    var data = [];
                    $(element.val().split(",")).each(function () {
                        data.push({ id: this, text: this });
                    });
                    callback(data);
                },
                ajax: {
                    url: config.tagAutoCompleteUrl,
                    type: 'POST',
                    dataType: 'json',
                    data: function (term) {
                        return {
                            term: term,
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
        };

        return {
            activate: acivate
        }
    })();

    window.ticketTags = ticketTags;

})(window);