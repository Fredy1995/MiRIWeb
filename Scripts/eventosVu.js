$(document).ready(function () {

    $('#btnAddUser').click(function () {
        document.getElementById("nombre").value = "";
        document.getElementById("aPaterno").value = "";
        document.getElementById("aMaterno").value = "";
        document.getElementById("usuario").value = "";
        document.getElementById("psw").value = "";
        //***********************Limpiar el tag selec y agregar una primera opcion antes de consumir la api
        document.getElementById("selectPerfil").innerHTML = "";
        const select = document.getElementById('selectPerfil');
        const option = document.createElement('option');
        option.value = "";
        option.text = "--Seleccione perfil--";
        select.appendChild(option);
        //**********************Fin de agregar opcion al selec
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::API MIRI PERFILES
        // Hacer la solicitud HTTP GET a la API REST
        fetch('https://localhost:7241/loginController/readPerfiles')
            .then(response => response.json())
            .then(data => {
                // Función para llenar la select con las opciones
                function fillSelect(data) {
                    const select = document.getElementById('selectPerfil');

                    data.forEach(perfil => {
                        const option = document.createElement('option');
                        option.value = perfil.idPerfil;
                        option.text = perfil.perfil;
                        select.appendChild(option);
                    });
                }

                // Llamar a la función para llenar la select
                fillSelect(data);
            })
            .catch(error => alert('Error: ' + error));
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::FIN API
    });

    $('#mySwitch').click(function () {
        if ($('#mySwitch').is(':checked')) {
            // Acá dentro pones tu código para cuando esté seleccionado
            $('#lblswitch').html('Habilitado')
        } else {
            // Acá dentro pones tu código para cuando NO esté seleccionado
            $('#lblswitch').html('Deshabilitado')
        }
    });
});