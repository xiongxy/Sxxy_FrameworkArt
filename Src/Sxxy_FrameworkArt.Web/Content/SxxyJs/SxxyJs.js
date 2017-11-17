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
        throw new Error("Not implemented");
    },

}
SxxyJs.fn.init.prototype = SxxyJs.fn;
SxxyJs.Create = function () { };
SxxyJs.Close = function () { };
SxxyJs.Create.Form = function () { };
SxxyJs.Create.SimpleTable = function (selector, options) {
    debugger;
    if (typeof options == "string")
        return;
    options = $.extend({}, SxxyJs.Create.SimpleTable.default, options || {});
    if (typeof options.dataType != "string") return;
    var tableData;
    if (options.dataType.toUpperCase() === "JSON") {
        tableData = JSON.parse(options.data);
    }
    else if (options.dataType.toUpperCase() === "OBJ") {
        tableData = options.data;
    }
    //开始获取Title
    var tableTitle = "<tr>";
    for (i in tableData[0]) {
        tableTitle += "<td>" + i + "</td>";
    }
    tableTitle += "</tr>";
    //开始获取Content
    var tableContent = "";
    for (var i = 0; i < tableData.length; i++) {
        tableContent = "<tr>";
        for (x in tableData[i]) {
            tableContent += "<td>" + tableData[i][x] + "</td>";
        }
        tableContent += "</tr>";
    }
    var jqueryObj = $(selector);
    jqueryObj.removeClass().addClass("box");
    var dataTableHtml = "";
    dataTableHtml += "<div class=\"box-header\">";
    dataTableHtml += "<h3 class=\"box-title\">Responsive Hover Table</h3>";
    dataTableHtml += "</div";
    dataTableHtml += "<div class=\"box-body table-responsive no-padding\">";
    dataTableHtml += "<table class=\"table table-hover\">";
    dataTableHtml += tableTitle;
    dataTableHtml += tableContent;
    dataTableHtml += "</table>";
    dataTableHtml += "</div></div>";
    jqueryObj.append(dataTableHtml);
};
SxxyJs.Create.SimpleTable.defaults = { data: null, dataType: "JSON", title: "" };
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
/**
 * @method  BootStrapSearcherPanel
 * @param {} selector
 * @param {} tableJsName
 * @returns {}
        */
SxxyJs.BootStrapSearcherPanel = function (selector, tableJsName) {
    var jqueryObj = $(selector);
    var action = jqueryObj.find(".box-header.with-border.actionbar").find("a");
    $(action).each(function (index, value) {
        var actionType = $(value).attr("action-type");
        var actionUrl = $(value).attr("action-url");
        var modalId = $(value).attr("modalId");
        $(value).on("click", function () {
            var tableJsNameObj = eval(tableJsName);
            var selectNum;
            var loading;
            switch (actionType) {
                case "Create":
                    loading = SxxyJs.Create.Loading();
                    $.ajax({
                        url: actionUrl,
                        type: "Get",
                        success: function (result) {
                            SxxyJs.Close.Loading(loading);
                            $(".modal").html(result);
                            $("" + modalId + "").modal('show');
                        }
                    });
                    break;
                case "Edit":
                    selectNum = tableJsNameObj.rows('.selected').data().length;
                    if (selectNum !== 1) {
                        layer.alert("请选择一行进行操作！");
                        return;
                    }
                    loading = SxxyJs.Create.Loading();
                    $.ajax({
                        url: actionUrl,
                        type: "Get",
                        data: { "Id": tableJsNameObj.rows(".selected").data()[0].Id },
                        success: function (result) {
                            SxxyJs.Close.Loading(loading);
                            $(".modal").html(result);
                            $("" + modalId + "").modal("show");
                        }
                    });
                    break;
                case "Delete":
                    loading = SxxyJs.Create.Loading();
                    selectNum = tableJsNameObj.rows(".selected").data().length;
                    if (selectNum === 0) {
                        layer.alert("请选择一行进行操作！");
                        return;
                    }
                    var id = tableJsNameObj.rows(".selected").data()[0].Id;
                    $.ajax({
                        url: actionUrl,
                        type: "Get",
                        traditional: true,
                        data: { "id": id },
                        success: function (result) {
                            SxxyJs.Close.Loading(loading);
                            $(".modal").html(result);
                            $("" + modalId + "").modal("show");
                        }
                    });
                    break;
                case "BatchDelete":
                    layer.alert("暂时无法使用！");
                    return;
                    loading = SxxyJs.Create.Loading();
                    selectNum = tableJsNameObj.rows(".selected").data().length;
                    if (selectNum === 0) {
                        layer.alert("请选择一行进行操作！");
                        return;
                    }
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
                            SxxyJs.Close.Loading(loading);
                            $(".modal").html(result);
                            $("" + modalId + "").modal("show");
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
SxxyJs.Create.Loading = function () {
    var loading = layer.load(1, {
        shade: [0.2, '#fff']
    });
    return loading;
}
SxxyJs.Close.Loading = function (obj) {
    layer.close(obj);
}
SxxyJs.Convert = function () { };
SxxyJs.Convert.HTML = {
    /*1.用浏览器内部转换器实现html转码*/
    htmlEncode: function (html) {
        //1.首先动态创建一个容器标签元素，如DIV
        var temp = document.createElement("div");
        //2.然后将要转换的字符串设置为这个元素的innerText(ie支持)或者textContent(火狐，google支持)
        (temp.textContent != undefined) ? (temp.textContent = html) : (temp.innerText = html);
        //3.最后返回这个元素的innerHTML，即得到经过HTML编码转换的字符串了
        var output = temp.innerHTML;
        temp = null;
        return output;
    },
    /*2.用浏览器内部转换器实现html解码*/
    htmlDecode: function (text) {
        //1.首先动态创建一个容器标签元素，如DIV
        var temp = document.createElement("div");
        //2.然后将要转换的字符串设置为这个元素的innerHTML(ie，火狐，google都支持)
        temp.innerHTML = text;
        //3.最后返回这个元素的innerText(ie支持)或者textContent(火狐，google支持)，即得到经过HTML解码的字符串了。
        var output = temp.innerText || temp.textContent;
        temp = null;
        return output;
    },
    /*3.用正则表达式实现html转码*/
    htmlEncodeByRegExp: function (str) {
        var s;
        if (str.length === 0) return "";
        s = str.replace(/&/g, "&amp;");
        s = s.replace(/</g, "&lt;");
        s = s.replace(/>/g, "&gt;");
        s = s.replace(/ /g, "&nbsp;");
        s = s.replace(/\'/g, "&#39;");
        s = s.replace(/\"/g, "&quot;");
        return s;
    },
    /*4.用正则表达式实现html解码*/
    htmlDecodeByRegExp: function (str) {
        var s;
        if (str.length === 0) return "";
        s = str.replace(/&amp;/g, "&");
        s = s.replace(/&lt;/g, "<");
        s = s.replace(/&gt;/g, ">");
        s = s.replace(/&nbsp;/g, " ");
        s = s.replace(/&#39;/g, "\'");
        s = s.replace(/&quot;/g, "\"");
        return s;
    }
}