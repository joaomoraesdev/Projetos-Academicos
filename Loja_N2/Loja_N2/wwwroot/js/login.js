function switchClienteFunc() {
    var checkBox = document.getElementById("switch");

    var ClientCard = document.getElementById("clientCard");
    var FuncCard = document.getElementById("funcCard");

    if (checkBox.checked == true) {
        ClientCard.style.display = "none";
        FuncCard.style.display = "block";
        document.getElementById("lblClienteFunc").innerHTML = 'Funcionario'
    }

    if (checkBox.checked == false) {
        FuncCard.style.display = "none";
        ClientCard.style.display = "block";
        document.getElementById("lblClienteFunc").innerHTML = 'Cliente'
    }
}