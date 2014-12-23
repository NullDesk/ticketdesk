

(function (window) {
    var ticketDeskUploader = (function () {
        var activate = function (config) {
            Dropzone.autoDiscover = false;


            $("div#attachmentsDropZone").dropzone({
                url: config.dropzoneUploadUrl,
                //prevents Dropzone from uploading dropped files immediately
                autoProcessQueue: true,
                addRemoveLinks: true,
                createImageThumbnails: true,
                previewsContainer: "#dz-preview",
                init: function () {
                    var self = this;
                    $.get(config.getAttachmentsUrl, { tempId: $('#tempId').val(), id: config.ticketId }, function (data) {
                        $.each(data, function (index, file) {

                            var existingFile = { name: file.name, size: file.size, isAttached: file.isAttached, };

                            self.emit("addedfile", existingFile);
                            ////TODO: Not sure about all this thumbnail business, need a custom template instead, and proably just use a stock image.
                            if (!file.type.match(/image.*/)) {
                                // This is not an image, so Dropzone doesn't create a thumbnail.
                                // Set a default thumbnail:
                                self.emit("thumbnail", existingFile, config.defaultThumbnailUrl);
                            } else {
                                self.emit("thumbnail", existingFile, file.url);
                            }
                        });
                        //wait to register the custom event here, otherwise the above emit ends up calling this and things go sideways
                        self.on("addedfile", function (file) {
                            if (!file.type.match(/image.*/)) {
                                self.emit("thumbnail", file, config.defaultThumbnailUrl);
                            }
                            else {
                                self.emit("thumbnail", file, file.url);
                            }
                        });
                    });

                },
                sending: function (file, xhr, formData) {
                    formData.append('tempId', $('#tempId').val());
                },
                removedfile: function (file) {
                    if (file.isAttached) {
                        //file is attached to a saved ticket, collect files to delete in hidden input instead of deleting from server
                        var elem = $('input[name="deleteFiles"]');
                        var val = elem.val()? elem.val().split(',') : [];
                        val.push(file.name);
                        elem.val(val.join(','));
                        killPreview();
                    } else {
                        //file is pending for either a new or existing ticket, delete from server immediately
                        $.ajax({
                            type: 'POST',
                            url: config.deleteFileUrl,
                            data: {
                                "id": $('#tempId').val(),
                                "fileName": file.name
                            },
                            success: killPreview,
                            dataType: 'json'
                        });
                    }
                    function killPreview() {
                        var ref;
                        return (ref = file.previewElement) != null ? ref.parentNode.removeChild(file.previewElement) : void 0;
                    }
                }
            });
        };
        return {
            activate: activate
        }
    })();

    window.ticketDeskUploader = ticketDeskUploader;

})(window);

