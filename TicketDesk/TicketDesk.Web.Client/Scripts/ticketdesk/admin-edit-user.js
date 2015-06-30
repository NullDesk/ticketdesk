(function (window) {
    window.adminEditUser = function () {
        var config;
        var activate = function (tdConfig) {
            config = tdConfig;
            $('#userroles').select2();
            $('#userroles').on('change', function(e) {
                var options = $(this).find('option:selected');
                var pOption = false;
                var numOptions = 0;
                options.each(function (idx, opt) {
                    opt = $(opt);
                    numOptions++;
                    if (opt.val() === config.pendingOptionId) {
                        pOption = true;
                    }
                });
                
                if (numOptions > 1 && pOption) {
                    $("#userroles option[value='" + config.pendingOptionId + "']").attr('selected', false);
                    $('#userroles').select2();
                }
            });
        };

        return {
            activate: activate
        };
    }();
})(window);
