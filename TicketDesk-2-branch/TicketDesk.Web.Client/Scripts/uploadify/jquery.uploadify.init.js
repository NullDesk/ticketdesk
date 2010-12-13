// code based on a post by matt at www.trycatchfail.com

//This is used in conjunction with the HtmlHelper.Uploadify extension method.
function initUploadify(control, uploadUrl, baseUrl, fileExtensions, fileDescription, authenticationToken, errorFunction, completeFunction, buttonText) {
    var options = {};

    options.script = uploadUrl;
    options.uploader = baseUrl + 'uploadify.swf';
    options.cancelImg = baseUrl + 'cancel.png';
    options.auto = true;
    options.multi = true;
    options.buttonText = "Select Files";
    if (authenticationToken != null) {
        options.scriptData = { token: authenticationToken };
    }
    options.fileExt = fileExtensions;
    options.fileDesc = fileDescription;
    options.buttonText = buttonText;
    options.buttonImg = baseUrl + 'uploadifySelect.png';
    options.rollover = true;
    options.height = 25;
    options.width = 100;
    if (errorFunction != null) {
        options.onError = errorFunction;
    }

    if (completeFunction != null) {
        options.onComplete = completeFunction;
    }

    control.uploadify(options);
}