jQuery.ajax({
    url: '@Url.Action("ListarUsuarios", "Home")',
    type: "GET",
    dataType: "json",
    contentType: "application/json; charset=utf-8",
    success: function (data) {
        debugger;

        var texto1 = "Hola soy el texto 1"


        console.log(data)
    }
})