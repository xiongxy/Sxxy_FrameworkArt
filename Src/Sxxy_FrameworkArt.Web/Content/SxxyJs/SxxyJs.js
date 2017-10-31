//后缀 titlebar 标题栏  操作栏 actionbar
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
SxxyJs.ModalFormSubmit = function () {
    //改写Form表单提交事件
    $("form").submit(function () {
        var data = $(this).serializeArray();
        var url = $(this).attr("action");
        $.ajax({
            url: url,
            type: "POST",
            data: data,
            success: function (result) {
                $(".modal").html(result);
            }
        });
        return false;
    });
}
SxxyJs.BootStrapSearcherPanel = function (selector, tableJsName) {
    var action = selector.find(".box-header.with-border.actionbar").find("a");
    $(action).each(function (index, value) {
        var actionType = $(value).attr("action-type");
        var actionUrl = $(value).attr("action-url");
        var modalId = $(value).attr("modalId");
        $(value).on("click", function () {
            var tableJsNameObj;
            var selectNum;
            switch (actionType) {
                case "Create":
                    $("" + modalId + "").modal('show');
                    $.ajax({
                        url: actionUrl,
                        type: "Get",
                        success: function (result) {
                            $(".modal").html(result);
                        }
                    });
                    break;
                case "Edit":
                    tableJsNameObj = eval(tableJsName);
                    selectNum = tableJsNameObj.rows('.selected').data().length;
                    if (selectNum !== 1) {
                        layer.alert("请选择一行进行操作！");
                        return;
                    }
                    $("" + modalId + "").modal("show");
                    $.ajax({
                        url: actionUrl,
                        type: "Get",
                        data: { "Id": tableJsNameObj.rows(".selected").data()[0].Id },
                        success: function (result) {
                            $(".modal").html(result);
                        }
                    });
                    break;
                case "Delete":
                    //layer.alert("暂时无法使用！");
                    //return;
                    tableJsNameObj = eval(tableJsName);
                    selectNum = tableJsNameObj.rows(".selected").data().length;
                    if (selectNum === 0) {
                        layer.alert("请选择一行进行操作！");
                        return;
                    }
                    $("" + modalId + "").modal("show");
                    var str = "";
                    for (var i = 0; i < tableJsNameObj.rows(".selected").data().length; i++) {
                        str += tableJsNameObj.rows(".selected").data()[i].Id + ",";
                    }
                    var array = str.split(",");
                    $.ajax({
                        url: actionUrl,
                        type: "Get",
                        traditional: true,
                        data: { "Ids": array },
                        success: function (result) {
                            $(".modal").html(result);
                        }
                    });
                    break;
                default:
            }
        });
    });
}
SxxyJs.FormErrorMessageCheck = function (vmguid) {
    var allInput = $("#modaldialog_" + vmguid).find("input");
    $(allInput).each(function (index, value) {
        var id = "#" + $(value).attr("id");
        var errorMessage = $(value).attr("errorMessage");
        if (errorMessage !== "") {
            layer.tips(errorMessage, id, {
                tips: [2, '#CC0033']
            });
        }
    });
}