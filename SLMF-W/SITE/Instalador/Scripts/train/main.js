// @codekit-prepend "../vendor/raphael.min.js"
// @codekit-prepend "../vendor/waypoints.min.js"
// @codekit-prepend "../vendor/waypoints-sticky.min.js"

// @codekit-prepend "filter-test.js"
Train = {

    init: function(data) {

        this.cachedElements();

        filterTest = new FilterTest({});
        filterTest.data = data;

        filterTest.init();
    },
    cachedElements: function() {

        $Train = $("#train");
        $Window = $(window);
    },
};