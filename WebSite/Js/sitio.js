
$(document).ready(function () {
    //Cada vez que se hace clic en un botón, se recupera el valor del atributo "data-id" asociado con ese botón
    $(".btnDelete").click(function () {
        var id = $(this).data("id");
        if (confirm("¿Está seguro de que desea eliminar este elemento?")) {
            $.ajax({
                type: "GET",
                url: urlEliminarDetalle + '?id=' + id,
               
                dataType: 'json',
                success: function (data) {
                    if (data.message == 1) {
                        alert('Acción realizada con éxito!');
                        window.location.href = urlConsultarDetalle + '?id=' + data.idDetalle;
                    } else {
                        alert("Ocurrio un error al realizar la acción");
                    }
                    
                },
                error: function (xhr, status, error) {
                    alert("La solicitud no se pudo completar");
                }
            });
        }
    });
});
