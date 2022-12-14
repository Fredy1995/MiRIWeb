function openNav() {
    tamanioIni = document.getElementById("mySidepanel").style;
    if (tamanioIni.width > "0px") {
        document.getElementById("mySidepanel").style.width = "0";
    } else {
        document.getElementById("mySidepanel").style.width = "280px";
    }

}

function closeNav() {
    document.getElementById("mySidepanel").style.width = "0";
}