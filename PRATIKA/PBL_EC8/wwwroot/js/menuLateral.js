
function menuLateral() {
    var dto = {
        textboxLogin: $("#textboxLogin").val(),
        textboxSenha: $("#textboxSenha").val(),
        loginPopup: $("#loginPopup")

        

    }

    function verificaPerfil(){
        $(document).ready(function () {
            $.ajax({
                url: base_path + "/Home/ValidacaoPerfil",
                type: 'POST',
                dataType: 'json',
                success: function (data) {
                    
                    if(data.success) {
                        window.location.href = "/Anuncios/AnunciosIndex";
                    }
                    else{
                        alert("Acesso apenas para usuários premium!");
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error('Erro na requisição:', textStatus, errorThrown);
                }
            });
        });
    }

    return {
        verificaPerfil:verificaPerfil
    };
}
