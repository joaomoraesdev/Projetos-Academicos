const base_path = window.location.origin;
var mensagemErro = "";
document.addEventListener("DOMContentLoaded", function () {
    tokenCheck().checkToken(); // Inicializa a verificação do token após o carregamento do DOM
});

function tokenCheck() {
    var dto = {
        textboxLogin: $("#textboxLogin").val(),
        textboxSenha: $("#textboxSenha").val(),
        loginPopup: $("#loginPopup"),
        divJwt: $("#divJwt"),
    }

    setInterval(() => {
        checkToken();
    }, 5000); // Verifica a cada 5 segundos
    

    function checkToken() {
        const token = sessionStorage.getItem('token');
        if (!token || isTokenExpired(token)) {
            showLoginPopup();
        }
    }

    function isTokenExpired(token) {
        try {
            // Extrair a parte payload do token (segunda parte)
            const payload = JSON.parse(atob(token.split('.')[1]));

            // Obter o tempo atual em segundos
            const currentTime = Math.floor(Date.now() / 1000);
            
            // Verificar se o token expirou
            console.log("Atual: " + currentTime);
            console.log("Token: " + payload.exp);
            return currentTime > payload.exp;
        } catch (error) {
            console.error("Token inválido:", error);
            return true; // Considera expirado se houver um erro ao processar
        }
    }

    function showLoginPopup() {
        //PROBLEMA A SOLUCIONAR: IMPEDINDO DE USAR A TELA TALVEZ MUDAR O Z-INDEX DE 0 PRA 9999
        if (dto.loginPopup) {
            dto.loginPopup.removeAttr('hidden');
            dto.divJwt.css('background-color', 'rgba(0, 0, 0, 0.8)');
            dto.divJwt.css('z-index', '9999');
            dto.divJwt.css('width', '100%');
            dto.divJwt.css('height', '100%');
        } else {
            console.error("Elemento 'loginPopup' não encontrado.");
        }
    }

    function hideLoginPopup() {
        if (dto.loginPopup) {
            dto.loginPopup.attr('hidden', true);
            dto.divJwt.css('background-color', '');
            dto.divJwt.css('z-index', '-9999');
            dto.divJwt.css('width', '');
            dto.divJwt.css('height', '');
        } else {
            console.error("Elemento 'loginPopup' não encontrado.");
        }
    }

    function validacaoLogin() {
        $(document).ready(function () {
            $.ajax({
                url: base_path + "/Home/ValidacaoLogin",
                data: {
                    login: dto.textboxLogin,
                    senha: dto.textboxSenha
                },
                type: 'POST',
                dataType: 'json',
                success: function (data) {
                    if (data.success) {

                        alert("Usuário validado com sucesso");
                        expiracaoToken(data.token);
                        hideLoginPopup();
                    }
                    else {
                        alert('Falha ao identificar as credenciais do usuário');
                        window.location.href = '/';
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error('Erro na requisição:', textStatus, errorThrown);
                }
            });
        });
    }

    function expiracaoToken(token) {
        const expiration = JSON.parse(atob(token.split('.')[1])).exp * 1000; // Converte o 'exp' para milissegundos
        sessionStorage.setItem('token', token);
        sessionStorage.setItem('tokenExpiration', expiration);
    }

    return {
        checkToken: checkToken,
        isTokenExpired: isTokenExpired,
        showLoginPopup: showLoginPopup,
        hideLoginPopup: hideLoginPopup,
        validacaoLogin: validacaoLogin,
        expiracaoToken: expiracaoToken
    };
}
