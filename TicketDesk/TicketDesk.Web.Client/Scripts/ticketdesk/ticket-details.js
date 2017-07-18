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
    window.ticketDetails = (function () {

        var config = {};
        var acivate = function (tdConfig) {
            config = tdConfig;
            $.validator.setDefaults({
                ignore: ""
            });
            if (config.isHtml) {
                $('#wmd-input-ticketDetails').summernote({
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

            } else {
                configureMarkdownEditor();
            }
        };

        var configureMarkdownEditor = function () {

            var jelem = $('#wmd-input-ticketDetails');
            if (jelem.length > 0) {

                jelem.attr('data-val', "true").attr('data-val-required', "");
                $.validator.unobtrusive.parseElement(jelem.get(0));
            }
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
})(window);