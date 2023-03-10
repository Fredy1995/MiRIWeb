

$(document).ready(function () {
     //Para seleccionar con clic izquierdo
    
    $('.nom_fic').dblclick(function () {
        fetch('https://localhost:7241/temaController/esTema/' + $(this).attr("value"))
            .then((response) => response.json())
            .then((data) => {
                if (data == true) {
                    
                    location.href = ("Clasificacion?idT=" + $(this).attr("id") + "&tema=" + $(this).attr("value"));
                } else {
                    fetch('https://localhost:7241/clasificacionController/esClasificacion/' + $(this).attr("value"))
                        .then((response) => response.json())
                        .then((data) => {
                            if (data == true) {
                                location.href = ("Grupo?idC=" + $(this).attr("id") + " &clasif=" + $(this).attr("value"));
                            } else {
                                alert('HAY UN PROBLEMA CON LA RUTA ESPECIFICADA');
                            }
                        });
                }

             });
       
    });
   
});

