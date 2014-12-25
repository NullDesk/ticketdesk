(function (window) {
    var ticketDetails = (function () {

        var config = {};
        var acivate = function (tdConfig) {
            config = tdConfig;
            configureEditor();
        };
       
        var configureEditor = function () {
            //TODO: move to general editor js, or rename this whole script for use in both new and edit ticket views
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

    window.ticketDetails = ticketDetails;

})(window);