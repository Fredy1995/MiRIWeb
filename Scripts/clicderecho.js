$(document).ready(function () {

    //Ocultamos el menú al cargar la página
    $("#menu_derecho").hide();
    document.oncontextmenu = function () { return false }
    /* mostramos el menú si hacemos click derecho con el ratón */
    var div = $("#cuerpo").first();
    div.contextmenu(function (e) {
        $("#menu_derecho").css({ 'display': 'block', 'left': e.pageX, 'top': e.pageY });
       
    });

    //$(document).bind("contextmenu", function (e) {
    //    $("#menu_derecho").css({ 'display': 'block', 'left': e.pageX, 'top': e.pageY });
    //    return false;
    //});

    var idSelec;
    $('.nom_fic').mousedown(function (event) {
        switch (event.which) {
            case 3:
                idSelec = $(this).attr("id");
                break;
        }
    });

    $('.nom_menu').click(function () {
        /*alert("Has seleccionado " + $(this).html() + " sobre el fichero con id " + idSelec);*/
        let text = $(this).html();
        let length = text.length;
        if (length == 61) {
            let pos = text.indexOf("C");
            let part = text.slice(pos);
            if (part == 'Cambiar nombre') {
                $('#myModalCambiarNombre').modal('show');
                $('#updatedirectory').val(document.getElementById(idSelec).value);
                document.getElementById('hiddenIDTema').value = idSelec;
            } else if (part == 'Compartir') {
                $('#myModalCompartir').modal('show');
            }
        } else if (length == 66) {
            let pos = text.indexOf("Ver");
            let part = text.slice(pos);
            alert('Seleccionaste: ' + part);
        }
    });


    //cuando hagamos click, el menú desaparecerá
    $(document).click(function (e) {
        if (e.button == 0) {
            $("#menu_derecho").css("display", "none");
        }
    });

    //si pulsamos escape, el menú desaparecerá
    $(document).keydown(function (e) {
        if (e.keyCode == 27) {
            $("#menu_derecho").css("display", "none");
        }
    });
});

