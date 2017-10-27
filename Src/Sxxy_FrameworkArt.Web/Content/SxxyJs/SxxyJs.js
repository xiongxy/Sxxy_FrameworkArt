//Define JsName
var
    version = "0.0.1",
    SxxyJs = function (selector, context) {
        return new SxxyJs.fn.init(selector, context);
    }


SxxyJs.fn = SxxyJs.prototype = {
    // 实例化化方法，这个方法可以称作 SxxyJs 对象构造器
    init: function (selector, context, rootjQuery) {
        // ...
    }
}
SxxyJs.fn.init.prototype = SxxyJs.fn;


SxxyJs.Create = function (c) {
    alert(c);
};
SxxyJs.Create.Form = function (c) {
    alert(c);
};



SxxyJs.StopFormConventAjax = function () {
    $("form").submit(function () {
        var data = $(this).serializeArray();
        var url = $(this).attr("action");
        debugger;
        $.ajax({
            url: url,
            type: "POST",
            data: data,
            success: function (result) {
                $(".modal").html();
                $(".modal").html(result);
            }
        });
        return false;
    });
}

