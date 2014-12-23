(function (window) {
    var newTicket = (function () {

        var config = {};
        var acivate = function (tdConfig) {
            config = tdConfig;
            cofigureTags();
            configureEditor();
            window.ticketDeskUploader.activate(config);
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

            var editor1 = new Markdown.Editor(converter1, "-ticketDetails");

            editor1.run();
        };


        return {
            activate: acivate
        }
    })();

    window.newTicket = newTicket;

})(window);