

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

    //Para seleccionar con clic derecho
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
                document.getElementById('hiddenIDDirectorio').value = idSelec;
            } else if (part == 'Compartir') {
                $('#myModalCompartir').modal('show');
                document.getElementById('hiddenIDDirectorioC').value = idSelec;
                //***********************Limpiar el tag selec y agregar una primera opcion antes de consumir la api
                document.getElementById("users-select").innerHTML = "";
                const select = document.getElementById('users-select');
                const option = document.createElement('option');
                option.value = "";
                option.text = "--Selecciona un usuario--";
                select.appendChild(option);
                //**********************Fin de agregar opcion al selec

                fetch('https://localhost:7241/temaController/esTema/' + document.getElementById(idSelec).value)
                    .then((response) => response.json())
                    .then((data) => {
                        if (data == true) {
                            //:::::::::::::::::::::::::::::::::::::::::::::::::::::::API MIRI TEMAS POR USUARIO 
                            // Hacer la solicitud HTTP GET a la API REST
                            fetch('https://localhost:7241/temaController/readUsuariosSinTema/' + idSelec)
                                .then(response => response.json())
                                .then(data => {
                                    // Función para llenar la select con las opciones
                                    function fillSelect(data) {
                                        const select = document.getElementById('users-select');

                                        data.forEach(user => {
                                            const option = document.createElement('option');
                                            option.value = user.idUsuario;
                                            option.text = user.usuario;
                                            select.appendChild(option);
                                        });
                                    }

                                    // Llamar a la función para llenar la select
                                    fillSelect(data);
                                })
                                .catch(error => alert('Error: ' + error));
                             //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::FIN API
                        } else {
                            fetch('https://localhost:7241/clasificacionController/esClasificacion/' + document.getElementById(idSelec).value)
                                .then((response) => response.json())
                                .then((data) => {
                                    if (data == true) {
                                        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::API MIRI TEMAS POR USUARIO 
                                        // Hacer la solicitud HTTP GET a la API REST
                                        fetch('https://localhost:7241/clasificacionController/readUsuariosSinClasif/' + idSelec)
                                            .then(response => response.json())
                                            .then(data => {
                                                // Función para llenar la select con las opciones
                                                function fillSelect(data) {
                                                    const select = document.getElementById('users-select');

                                                    data.forEach(user => {
                                                        const option = document.createElement('option');
                                                        option.value = user.idUsuario;
                                                        option.text = user.usuario;
                                                        select.appendChild(option);
                                                    });
                                                }

                                                // Llamar a la función para llenar la select
                                                fillSelect(data);
                                            })
                                            .catch(error => alert('Error: '+error));
                                        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::FIN API
                                    } else {
                                        alert('HAY UN PROBLEMA CON API-MIRI');
                                    }
                                });
                        }

                    });
                
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

    var inputValue = document.getElementById("users-select").value;
    var combo = document.getElementById("users-select");
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
    document.getElementById("users-select").value = "";

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
