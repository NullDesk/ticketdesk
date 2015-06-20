(function (window) {
    window.adminUsers = function () {

        var activate = function () {
            makeClicky();
            $('.pagination').addClass('pagination-sm');
        };

        var completeChangeList = function () {
            $('#userListBody').fadeIn(100);
            $('.pagination').addClass('pagination-sm');
            makeClicky();
        };

        var makeClicky = function () {
            $(".clickable").clickable();
        };

        var paging = function () {
            var beginChangePage = function (args) {
                $('#userListBody').fadeOut(100);
                
            };
            return { beginChangePage: beginChangePage }
        }();

        return {
            activate: activate,
            makeClicky: makeClicky,
            paging: paging,
            completeChangeList: completeChangeList
        };
    }();
})(window);
