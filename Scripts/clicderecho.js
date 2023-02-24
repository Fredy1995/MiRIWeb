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
                document.getElementById('hiddenIDTemaC').value = idSelec;
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

//***************************************Script para agregar o crear una lista de usuarios para compartir tema
// Click on a close button to hide the current list item
var close = document.getElementsByClassName("close");
var i;
for (i = 0; i < close.length; i++) {
    close[i].onclick = function () {
        var div = this.parentElement;
        div.style.display = "none";
    }
}



// Create a new list item when clicking on the "Add" button
function newElement() {
    var dataElement = [...document.querySelectorAll('#myUL li')].map(element => element.innerText);
    for (i = 0; i < dataElement.length; i++) {
        dataElement[i] = dataElement[i].slice(0, -2);
    }
    var li = document.createElement("li");

    var inputValue = document.getElementById("myInput").value;
    var combo = document.getElementById("myInput");
    var inputText = combo.options[combo.selectedIndex].text;
    var t = document.createTextNode(inputText);
    li.appendChild(t);
    if (inputValue === '') {
        alert("¡DEBE SELECCIONAR UN ELEMENTO!");
    } else {
        if (dataElement.includes(inputText)) {
            alert("EL ELEMENTO YA SE AGREGÓ A LA LISTA");
        } else {
            document.getElementById("myUL").appendChild(li);
        }

    }
    document.getElementById("myInput").value = "";

    var span = document.createElement("SPAN");
    var txt = document.createTextNode("\u00D7");
    span.className = "close";
    span.appendChild(txt);
    li.appendChild(span);

    for (i = 0; i < close.length; i++) {
        close[i].onclick = function () {
            var div = this.parentElement;
            div.style.display = "none";
        }
    }
}

///Metodo encargado de eliminar todos los elementos de la lista UL
function deleteElement(){
    var dataElement = [...document.querySelectorAll('#myUL li')].map(element => element.innerText);
    for (i = 0; i < dataElement.length; i++) {

        //Buscamos por id
        var lista = document.getElementById("myUL");
        //Asignamos a variable item el elemento con id ='item2'de la lista
        var item = lista.querySelector('li');
        lista.removeChild(item);
    }
}

function enviarList() {
    var dataElement = [...document.querySelectorAll('#myUL li')].map(element => element.innerText);
    for (i = 0; i < dataElement.length; i++) {
        dataElement[i] = dataElement[i].slice(0, -2);
    }
    document.getElementById("listUsers").value = dataElement;
}
