//const base_path = window.location.origin;
var mensagemErro = "";

// Pegar a data atual e formatá-la como 'YYYY-MM-DD'
const dataAtual = new Date();
const ano = dataAtual.getFullYear();
const mes = String(dataAtual.getMonth() + 1).padStart(2, '0');
const dia = String(dataAtual.getDate()).padStart(2, '0');

// Formatar a data atual no formato 'YYYY-MM-DD'
const dataAtualFormatada = `${ano}-${mes}-${dia}`;

function cadastroUsuario() {
    var dto = {
        textboxNomeUsuario: $("#textboxNomeUsuario").val(),
        textboxNome: $('#textboxNome').val(),
        textboxSobrenome: $("#textboxSobrenome").val(),
        textboxEmail: $('#textboxEmail').val(),
        textboxSenha: $('#textboxSenha').val(),
        textboxConfirmacaoSenha: $('#textboxConfirmacaoSenha').val(),
        textboxDataNascimento: $('#textboxDataNascimento').val(),
        textareaDescricao: $('#textareaDescricao').val(),
        comboGenero: $('#comboGenero').find(":selected").text(),
        imagemInput: $('#imagemInput'),
        comboPerfil: $('#comboPerfil').find(":selected").text(),
        btnLogin: $('#btnLogin')
    }

    function inserirImagem(inputImagem) {
        $('#divImagemPerfil').removeAttr('hidden');
        const file = inputImagem.files[0]; // Pega o primeiro arquivo do input
        if (file) {
            const reader = new FileReader(); // Cria um FileReader

            reader.onload = function (e) {
                $('#imagePreview').attr('src', e.target.result).show(); // Define a fonte da imagem para o resultado do FileReader e mostra a imagem
            };

            reader.readAsDataURL(file); // Lê o arquivo como URL de dados
        }
    }

    function converterImagem(inputFiles, callback) {
        const reader = new FileReader();

        reader.onload = function (e) {
            callback(e.target.result); // Chama o callback com o resultado Base64
        };

        reader.onerror = function (error) {
            console.error("Erro ao ler o arquivo:", error);
        };

        reader.readAsDataURL(inputFiles);
    }

    function cadastrarUsuario() {
        if (validaCampos() && validarEmail(dto.textboxEmail) && validarSenha(dto.textboxSenha)) {
            $(document).ready(function () {
                converterImagem(dto.imagemInput[0].files[0], function(imagemProcessada) {
                    $.ajax({
                        url: base_path + "/Home/CadastrarUsuario", // Verifique se o base_path está correto
                        data: {
                            dto: {
                                ImagemPerfil: imagemProcessada,
                                NomeUsuario: dto.textboxNomeUsuario,
                                Nome: dto.textboxNome,
                                Sobrenome: dto.textboxSobrenome,
                                Email: dto.textboxEmail,
                                Senha: dto.textboxSenha,
                                DataNascimento: dto.textboxDataNascimento,
                                Descricao: dto.textareaDescricao,
                                Genero: dto.comboGenero,
                                Perfil: dto.comboPerfil
                            }
                        },
                        type: 'POST', // Certifique-se de que o método do controlador está aceitando GET
                        dataType: 'json',
                        success: function (data) {
                             
                            alert(`${data.message}`);
                            if (data.success) {
                                window.location.href = data.redirectUrl;
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            console.error('Erro na requisição:', textStatus, errorThrown);
                        }
                    });
                });               
            });
        }
        else {
            alert(mensagemErro);
        }
    }

    function voltar() {
        window.location.href = "/";
    }

    function validarSenha(senha) {
        const regexSenhaForte = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\W)(?=.{8,})/;
        if (regexSenhaForte.test(senha)) {
            mensagemErro = "Senha válida!";
            return true;
        } else {
            mensagemErro = "Senha inválida! A senha deve ter pelo menos 8 caracteres, com 1 letra maiúscula, 1 letra minúscula e 1 caracter especial.";
            return false;
        }
    }

    function validarEmail(email) {
        const regexEmail = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (regexEmail.test(email)) {
            mensagemErro = "Email válido!";
            return true;
        } else {
            mensagemErro = "Email inválido! Verifique se está no formato correto (exemplo: usuario@dominio.com).";
            return false;
        }
    }

    function validaCampos() {
        if (dto.imagemInput[0].files.length == 0) {
            mensagemErro = "Por favor, escolha uma imagem para seu perfil!";
            return false;
        }
        else if (dto.textboxNomeUsuario == "") {
            mensagemErro = "Por favor, preencha o campo de Login";
            return false;
        }
        else if (dto.textboxNome == "") {
            mensagemErro = "Por favor, preencha o campo de Nome";
            return false;
        }
        else if (dto.textboxSobrenome == "") {
            mensagemErro = "Por favor, preencha o campo de Sobrenome";
            return false;
        }
        else if (dto.textboxEmail == "") {
            mensagemErro = "Por favor, preencha o campo de Email";
            return false;
        }
        else if (dto.textboxSenha == "") {
            mensagemErro = "Por favor, preencha o campo de Senha";
            return false;
        }
        else if (dto.textboxConfirmacaoSenha == "") {
            mensagemErro = "Por favor, preencha o campo de Confirmação de Senha";
            return false;
        }
        else if (dto.textboxSenha != dto.textboxConfirmacaoSenha) {
            mensagemErro = "Os campos de senhas não são compatíveis";
            return false;
        }
        else if (dto.textboxDataNascimento == "") {
            mensagemErro = "Por favor, preencha o campo de Data de Nascimento";
            return false;
        }
        else if (new Date(dto.textboxDataNascimento) > new Date(dataAtualFormatada) || new Date(dto.textboxDataNascimento).getFullYear() < 1908) {
            mensagemErro = "Data de Nascimento inválida";
            return false;
        }
        else if (dto.textareaDescricao == "") {
            mensagemErro = "Por favor, preencha o campo de Descrição";
            return false;
        }
        else if (dto.comboGenero != "Masculino" && dto.comboGenero != "Feminino" && dto.comboGenero != "Outro") {
            mensagemErro = "Por favor, preencha o campo de Gênero corretamente";
            return false;
        }
        else if (dto.comboPerfil != "Básico" && dto.comboPerfil != "Premium") {
            mensagemErro = "Por favor, preencha o campo de Perfil corretamente";
            return false;
        }
        else {
            return true;
        }
    }

    return {
        cadastrarUsuario: cadastrarUsuario,
        validaCampos: validaCampos,
        inserirImagem: inserirImagem,
        voltar: voltar
    };
}
