var time;

$(document).ready(function () {
    time = setTimeout(Test, 5000);
});

function Test() {
    $('#cont-loading').html('<div class="loading"><img src="img/ajax-loader.gif" alt="loading" style="width: 25px;"/>Procesando...</div>');
    $.ajax({
        type: "POST",
        url: 'LecturaCorreo.aspx/Process',
        data: null,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            setTimeout(function () {
                $('#cont-loading').html(data.d);
            }, 3000);
            time = setTimeout(Test, 300000);
        },
        error: function () {
            window.location.href = "LecturaCorreo.aspx";
        }//,
        //timeout: 10000,
        //complete: function () {
        //    window.location.href = "Sonda.aspx";
        //}
    });
}