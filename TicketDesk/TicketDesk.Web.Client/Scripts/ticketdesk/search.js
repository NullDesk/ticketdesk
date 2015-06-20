(function (window) {
    window.search = function () {

        var activate = function () {
            makeClicky();
           
        };
        var makeClicky = function () {
            $(".clickable").clickable();
        };

        
        return {
            activate: activate
        };
    }();
})(window);
