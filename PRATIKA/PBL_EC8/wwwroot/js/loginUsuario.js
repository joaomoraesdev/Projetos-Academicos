const base_path = window.location.origin;
var mensagemErro = "";

function loginUsuario() {
    var dto = {
        textboxLogin: $("#textboxLogin").val(),
        textboxSenha: $('#textboxSenha').val(),
        lblEsqueciSenha: $("#lblEsqueciSenha").val(),
        lblCadastrar: $('#lblCadastrar').val(),
        btnLogin: $('#btnLogin')
    }  

    function validacaoLogin() {
        if (validaCampos()) {
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
                        alert(data.message);
                        if(data.success) {
                            expiracaoToken(data.token);
                            window.location.href = data.redirectUrl;
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        console.error('Erro na requisição:', textStatus, errorThrown);
                    }
                });
            });
        }
        else {
            alert(mensagemErro);
        }
    }

    function validaCampos() {
        if (dto.textboxLogin == "") {
            mensagemErro = "Por favor, para acessar, utilize seu email ou nome de usuário";
            return false;
        }
        else if (dto.textboxSenha == "") {
            mensagemErro = "Por favor, preencha o campo de Senha";
            return false;
        }
        
        return true;
    }

    function expiracaoToken(token) {
        const expiration = JSON.parse(atob(token.split('.')[1])).exp * 1000; // Converte o 'exp' para milissegundos
        sessionStorage.setItem('token', token);
        sessionStorage.setItem('tokenExpiration', expiration);
    }    

    return {
        validacaoLogin: validacaoLogin,
        validaCampos: validaCampos,
        expiracaoToken: expiracaoToken
    };
}
