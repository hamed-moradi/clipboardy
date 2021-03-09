
var serverUrl = 'http://localhost:2020/';
var bearerToke = 'Bearer ';

function callApi(method, type, data, success, error = null, complete = null) {
    if (error == null)
        error = function (jqXHR, textStatus, errorThrown) {
            alert(textStatus + ' ' + errorThrown);
        };

    if (complete == null)
        complete = function (jqXHR, textStatus) { };

    $.ajax({
        url: serverUrl + 'api/clipboard/' + method,
        method: type,
        contentType: 'application/json; charset=UTF-8',
        dataType: 'json',
        timeout: 30,
        headers: { Authorization: bearerToke },
        data: data,
        success: success,
        error: error,
        complete: complete
        //beforeSend: function (jqXHR, settings) { },
        //error: function (jqXHR, textStatus, errorThrown) { },
        //success: function (data, textStatus, jqXHR) { },
        //complete: function (jqXHR, textStatus) { }
    });
};

var clipboardGetBindingModel = function (typeId, content, createdAt) {
    return { typeId, content, createdAt };
}

function get() {
    callApi('get', 'GET', clipboardGetBindingModel(null, null, null),
        function (data, textStatus, jqXHR) {
            if (data != null && data.result != null && data.result.length > 0) {
                var rows = '';
                for (var i = 0; i < data.result.length; i++) {
                    rows += '<div class="icon-box wow fadeInUp">' +
                        '<div class="col-2">' +
                        data.result[i].createdAt +
                        '</div>' +
                        '<div class="col-8">' +
                        '<div class="icon"><i class="fa fa-@item.Icon"></i></div>' +
                        '<h4 class="title"><a href="">' + data.result[i].typeName + '</a></h4>' +
                        '<p class="description">' + data.result[i].content + '</p>' +
                        '</div>' +
                        '<div class="col-2">' +
                        '<div class="icon"><a onclick="edit(' + data.result[i].id + ')"><i class="fa fa-edit"></i></a></div>' +
                        '<div class="icon"><a onclick="remove(' + data.result[i].id + ')"><i class="fa fa-remove"></i></a></div>' +
                        '</div>' +
                        '</div>';
                }

                $('#clipboards').html(rows);
            }

        });

}

function add(typeId, content) {

}

function remove(id) {

}

$(document).ready(function () {
    get();
});