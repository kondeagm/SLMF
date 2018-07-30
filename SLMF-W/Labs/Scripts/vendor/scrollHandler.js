;
(ScrollHandler = function() {
    var callBackScrollDown = null;
    var callBackScrollUp = null;
    var isActive = true;
    var self = this;

    function handle(delta) {
        if (isActive) {
            if (delta < 0 && typeof(callBackScrollDown) == 'function') {
                callBackScrollDown();
            } else if (typeof(callBackScrollUp) == 'function') {
                callBackScrollUp();
            }
        }
    }

    function wheel(event) {
        var delta = 0;
        if (!event) event = window.event;
        if (event.wheelDelta) {
            delta = event.wheelDelta / 120;
        } else if (event.detail) {
            delta = -event.detail / 3;
        }
        if (delta)
            handle(delta);
        if (event.preventDefault)
            event.preventDefault();
        event.returnValue = false;
    }

    /* Initialization code. */
    if (window.addEventListener)
        window.addEventListener('DOMMouseScroll', wheel, false);
    window.onmousewheel = document.onmousewheel = wheel;

    return {
        onScrollDown: function(callback) {
            callBackScrollDown = callback;
        },
        onScrollUp: function(callback) {
            callBackScrollUp = callback;
        },
        setActive: function(active) {
            isActive = active;
        }
    }
})();