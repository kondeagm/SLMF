//rootAPI = 'http://slmfapi.grupow.net/api/';
//rootSite = 'http://slmfsite.grupow.net/home/';
//fbApp = '445098585637252';

root = "/";
rootAPI = 'http://api.sololosmasfuertes.com/api/';
rootSite = 'http://app.sololosmasfuertes.com/home/';
fbApp = '398343356979442';

$(document).ready(function () {
    $(".bt-back-arrow").btBack();
    $(".bt-close-left").btBack();
    $(".bt-next-arrow").btNext();

    $(".inactive").on("click", function (event) {
        event.preventDefault()
    });

    (function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) return;
        js = d.createElement(s); js.id = id;
        js.src = "//connect.facebook.net/en_US/sdk.js";
        fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));

    User = new User({
        appId: fbApp,
        postLogin: rootSite + "login",
        postLogout: rootSite + "logout",
        setUserState: rootSite + "setUserStatus",
        getUserState: rootSite + "getUserStatus",

        postRegisterUser: rootAPI + "user/register",
        postDeleteUser: rootAPI + "user/delete",
        postInitPlan: rootAPI + "plan/register",
        postCancelPlan: rootAPI + "plan/delete",
    });
});

//IE Detection for custom css
if (navigator.userAgent.indexOf('MSIE') !== -1 || navigator.appVersion.indexOf('Trident/') > 0) {
    $("html").addClass("ie");
    if (navigator.appVersion.indexOf("rv:11") !== -1) {
        $("html").addClass("ie-edge");
    } else {
        $("html").addClass("ie-lt11");
    }
}

// First, checks if it isn't implemented yet.
if (!String.prototype.format) {
    String.prototype.format = function () {
        var args = arguments;
        return this.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined' ? args[number] : match;
        });
    };
}