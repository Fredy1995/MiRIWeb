

$(document).ready(function () {
     //Para seleccionar con clic izquierdo

    $('.nom_fic').dblclick(function () {
        location.href = ("Clasificacion?idT="+$(this).attr("id") + "&tema=" + $(this).attr("value"));
    });
   
});

