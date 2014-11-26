(function (window) {
    var newTicketEditor = (function () {

        var tdConfig = {};
        var acivate = function(config) {
            tdConfig = config;
            cofigureTags();
            configureEditor();
            configureDropzone();
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
                    url: tdConfig.tagAutoCompleteUrl,
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

        var configureDropzone = function () {
            Dropzone.autoDiscover = false;

            $("div#attachmentsDropZone").dropzone({
                url: tdConfig.dropzoneUploadUrl,
                //prevents Dropzone from uploading dropped files immediately
                autoProcessQueue: true,
                addRemoveLinks: true,
                createImageThumbnails: true,
                init: function () {
                    var self = this;
                    $.get(tdConfig.pendingAttachmentsUrl, { tempId: $('#tempId').val() }, function (data) {
                        $.each(data, function (index, file) {

                            var existingFile = { name: file.name, size: file.size };

                            self.emit("addedfile", existingFile);
                            //TODO: Not sure about all this thumbnail business, need a custom template instead, and proably just use a stock image.
                            if (!file.type.match(/image.*/)) {
                                // This is not an image, so Dropzone doesn't create a thumbnail.
                                // Set a default thumbnail:
                                self.emit("thumbnail", existingFile, tdConfig.defaultThumbnailUrl);
                            } else {
                                self.emit("thumbnail", existingFile, file.url);
                            }
                        });

                    });
                },
                removed: function (file) { },
                sending: function (file, xhr, formData) {
                    formData.append('tempId', $('#tempId').val());
                },
                removedfile: function (file) {
                    $.ajax({
                        type: 'POST',
                        url: '@Url.Action("Delete", "File")',
                        data: {
                            "tempId": $('#tempId').val(),
                            "fileName": file.name
                        },
                        dataType: 'json'
                    });
                    var _ref;
                    return (_ref = file.previewElement) != null ? _ref.parentNode.removeChild(file.previewElement) : void 0;
                }
            });
        };
        return {
            activate: acivate
        }
    })();

    window.newTicketEditor = newTicketEditor;

})(window);