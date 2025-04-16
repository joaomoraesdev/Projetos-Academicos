document.addEventListener('DOMContentLoaded', function () {
    const page = document.body.getAttribute('data-page'); // Obtém a identificação da página
    if(page == 'anuncios') {
        const btnCriacaoConteudo = $('#btnCriacaoConteudo');
        btnCriacaoConteudo.text("Anunciar");
        btnCriacaoConteudo.on('click', anuncios().abrirModalCadastrarAnuncio);
        anuncios().pesquisarTodosAnuncios();
    }   
});

function anuncios() {
    var dto = {
        textboxPesquisaAnuncios: $('#textboxPesquisaAnuncios').val(),
        modalCadastroAnuncio: $('#modalCadastroAnuncio'),
        modalPerfilInvalido: $('#modalPerfilInvalido'),
        btnCriacaoConteudo: $('#btnCriacaoConteudo').text(),

        //Campos do modal
        divImagemPerfil: $('#divImagemPerfil'),
        imgPreview: $('#imgPreview'),
        textboxTitulo: $("#textboxTitulo").val(),
        textboxProfissao: $("#textboxProfissao").val(),
        comboEstado: $("#comboEstado :selected").val(),
        textboxCidade: $("#textboxCidade").val(),
        textareaDescricao: $("#textareaDescricao").val(),
        btnCadastrarAnuncio: $('#btnCadastrarAnuncio')
    }

    function abrirModalCadastrarAnuncio(anuncio) {
       
        $(document).ready(function () {
            $.ajax({
                url: base_path + "/Anuncios/ValidacaoPerfil",
                type: 'POST',
                dataType: 'json',
                success: function (data) {
                    if (data.success) {
                        $("#containerCadastroAnuncio").show();
                        dto.modalCadastroAnuncio.css('display', 'flex');

                        if (data.imagemPerfil != null) {
                            dto.imgPreview.attr('src', data.imagemPerfil);
                            dto.divImagemPerfil.show();
                        }
                        else
                            dto.divImagemPerfil.hide();
                    }
                    else {
                        dto.modalPerfilInvalido.css('display', 'flex');
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error('Erro na requisição:', textStatus, errorThrown);
                }
            });
            setTimeout(() => {
            if(anuncio){
            passarValoresModal(anuncio);
            }
            },50);
        
        });

        

    }

    function passarValoresModal(anuncio){
        $("#textboxTitulo").val(anuncio.titulo);
        $("#textboxProfissao").val(anuncio.profissao);
        $("#comboEstado").prop("selectedIndex", 0);
        $("#textboxCidade").val(anuncio.cidade);
        $("#textareaDescricao").val(anuncio.descricao);
        $("#idUsuarioModal").val(anuncio.idUsuario);
        $("#idAnuncioModal").val(anuncio.id);
    }


    function fecharModalCadastrarAnuncio() {
        dto.modalCadastroAnuncio.css('display', 'none');
        $("#textboxTitulo").val('');
        $("#textboxProfissao").val('');
        $("#comboEstado").prop("selectedIndex", 0);
        $("#textboxCidade").val('');
        $("#textareaDescricao").val('');
    }

    function fecharModalPerfilInvalido() {
        dto.modalPerfilInvalido.css('display', 'none');
    }

    function inserirImagem(inputImagem) {
        $('#divImagemPerfil').removeAttr('hidden');
        const file = inputImagem.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function (e) {
                $('#imgPreview').attr('src', e.target.result).show();
            };
            reader.readAsDataURL(file);
        }
    }

    function cadastrarAnuncio() {
        if (validaCampos()) {
            $(document).ready(function () {
                $.ajax({
                    url: base_path + "/Anuncios/CadastrarAnuncio",
                    data: {
                        dto: {
                            Id: $("#idAnuncioModal").val(),
                            IdUsuario: $("#idUsuarioModal").val(),
                            Titulo: dto.textboxTitulo,
                            Profissao: dto.textboxProfissao,
                            Estado: dto.comboEstado,
                            Cidade: dto.textboxCidade,
                            Descricao: dto.textareaDescricao,
                            ImagemAnunciante: dto.imgPreview.attr('src')
                        }
                    },
                    type: 'POST',
                    dataType: 'json',
                    success: function (data) {
                        alert(`${data.message}`);
                        if($("#idUsuarioModal").val()){
                            fecharModalCadastrarAnuncio();
                            pesquisarAnunciosPorUsuario();
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

    function pesquisarTodosAnuncios() {
        $(document).ready(function () {
            $.ajax({
                url: base_path + "/Anuncios/PesquisarTodosAnuncios",
                type: 'POST',
                dataType: 'json',
                success: function (data) {
                    criarAnunciosHtml(data.lista);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error('Erro na requisição:', textStatus, errorThrown);
                }
            });
        });
    }

    function pesquisarAnunciosPorUsuario() {
        $(document).ready(function () {
            $.ajax({
                url: base_path + "/Anuncios/PesquisarAnunciosPorUsuario",
                type: 'POST',
                dataType: 'json',
                success: function (data) {
                    criarAnunciosHtmlPerfil(data.lista);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error('Erro na requisição:', textStatus, errorThrown);
                }
            });
        });
    }

    function pesquisarAnuncio() {
        if (dto.textboxPesquisaAnuncios != '') {
            $(document).ready(function () {
                $.ajax({
                    url: base_path + "/Anuncios/PesquisarAnuncios",
                    data: {
                        pesquisa: dto.textboxPesquisaAnuncios
                    },
                    type: 'POST',
                    dataType: 'json',
                    success: function (data) {
                        criarAnunciosHtml(data.lista);
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        console.error('Erro na requisição:', textStatus, errorThrown);
                    }
                });
            });
        }
        else {
            $(document).ready(function () {
                $.ajax({
                    url: base_path + "/Anuncios/PesquisarTodosAnuncios",
                    type: 'POST',
                    dataType: 'json',
                    success: function (data) {
                        criarAnunciosHtml(data.lista);
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        console.error('Erro na requisição:', textStatus, errorThrown);
                    }
                });
            });
        }
    }

    function criarAnunciosHtml(listaAnuncios) {
        // Seleciona a div pai onde os anúncios serão inseridos
        const divAnuncios = document.getElementById('divAnuncios');

        // Limpa a div antes de adicionar novos elementos (opcional)
        divAnuncios.innerHTML = '';

        // Itera sobre cada anúncio na lista
        listaAnuncios.forEach(anuncio => {
            // Cria a estrutura do card
            const card = document.createElement('div');
            card.classList.add('card');

            // Cria o container da imagem
            const imgContainer = document.createElement('div');
            imgContainer.classList.add('img-container');

            const img = document.createElement('img');
            img.id = 'imagem_card';
            img.src = anuncio.imagemAnunciante || '../img/default_usuario.png'; // Usa a imagem do anúncio ou uma padrão
            img.alt = 'Imagem do anunciante';
            imgContainer.appendChild(img);

            // Cria o container do texto
            const textContainer = document.createElement('div');
            textContainer.classList.add('text-container');

            const h1 = document.createElement('h1');
            h1.id = 'anunciante_card';
            h1.textContent = anuncio.nomeAnunciante;

            const h2 = document.createElement('h2');
            h2.id = 'profissao_card';
            h2.textContent = anuncio.profissao;

            const h3 = document.createElement('h3');
            h3.id = 'localizacao_card';
            h3.textContent = anuncio.cidade + " - " + anuncio.estado;

            textContainer.append(h1, h2, h3);

            // Cria a descrição
            const description = document.createElement('div');
            description.classList.add('description');

            const p = document.createElement('p');
            p.id = 'descricao_card';
            p.textContent = anuncio.descricao;

            description.appendChild(p);

            // Adiciona os elementos ao card
            card.append(imgContainer, textContainer, description);

            // Adiciona o card à div pai
            divAnuncios.appendChild(card);
            divAnuncios.appendChild(document.createElement('br'));
        });
    }

    function criarAnunciosHtmlPerfil(listaAnuncios) {
         // Seleciona a div pai onde os anúncios serão inseridos
    const divAnuncios = document.getElementById('divAnuncios');

    // Limpa a div antes de adicionar novos elementos (opcional)
    divAnuncios.innerHTML = '';

    // Itera sobre cada anúncio na lista
    listaAnuncios.forEach(anuncio => {
        // Serializa o objeto para passar como parâmetro
        const serializedAnuncio = JSON.stringify(anuncio).replace(/"/g, '&quot;');

        // Cria a estrutura do card
        const card = document.createElement('div');
        card.classList.add('card');

        // Cria o container da imagem
        const imgContainer = document.createElement('div');
        imgContainer.classList.add('img-container');

        const img = document.createElement('img');
        img.id = 'imagem_card';
        img.src = anuncio.imagemAnunciante || '../img/default_usuario.png'; // Usa a imagem do anúncio ou uma padrão
        img.alt = 'Imagem do anunciante';
        imgContainer.appendChild(img);

        // Cria o container do texto
        const textContainer = document.createElement('div');
        textContainer.classList.add('text-container');

        const h1 = document.createElement('h1');
        h1.id = 'anunciante_card';
        h1.textContent = anuncio.nomeAnunciante;

        const h2 = document.createElement('h2');
        h2.id = 'profissao_card';
        h2.textContent = anuncio.profissao;

        const h3 = document.createElement('h3');
        h3.id = 'localizacao_card';
        h3.textContent = `${anuncio.cidade} - ${anuncio.estado}`;

        textContainer.append(h1, h2, h3);

        // Cria a descrição
        const description = document.createElement('div');
        description.classList.add('description');

        const p = document.createElement('p');
        p.id = 'descricao_card';
        p.textContent = anuncio.descricao;

        description.appendChild(p);

        // Cria o container de botões
        const buttonContainer = document.createElement('div');
        buttonContainer.classList.add('button-container');

        // Botão de editar
        const editButton = document.createElement('button');
        editButton.id = `editar-${anuncio.id}`;
        editButton.classList.add('botao-edicao');
        editButton.setAttribute('aria-label', 'Clique e edite este anúncio');
        editButton.onclick = () => abrirModalCadastrarAnuncio(anuncio);

        const editIcon = document.createElement('img');
        editIcon.src = '../img/editar.png';
        editIcon.alt = 'Editar anúncio';
        editButton.appendChild(editIcon);

        // Botão de excluir
        const deleteButton = document.createElement('button');
        deleteButton.id = `excluir-${anuncio.id}`;
        deleteButton.classList.add('botao-edicao');
        deleteButton.setAttribute('aria-label', 'Clique e exclua este anúncio');
        deleteButton.onclick = () => excluirAnuncio(anuncio);

        const deleteIcon = document.createElement('img');
        deleteIcon.src = '../img/excluir.png';
        deleteIcon.alt = 'Excluir anúncio';
        deleteButton.appendChild(deleteIcon);

        // Adiciona os botões ao container
        buttonContainer.append(editButton, deleteButton);

        // Adiciona os elementos ao card
        card.append(imgContainer, textContainer, description, buttonContainer);

        // Adiciona o card à div pai
            divAnuncios.appendChild(card);
            divAnuncios.appendChild(document.createElement('br'));
        });
    }


    function validaCampos() {
        if (dto.textboxTitulo == '') {
            mensagemErro = "Por favor, escolha uma imagem para o anúncio!";
            return false;
        }
        else if (dto.textboxProfissao == '') {
            mensagemErro = "Por favor, preencha o nome do item do anúncio";
            return false;
        }
        else if (dto.comboEstado == '') {
            mensagemErro = "Por favor, preencha um contato para o anúncio";
            return false;
        }
        else if (dto.textboxCidade == '') {
            mensagemErro = "Por favor, preencha o método de pagamento do anúncio";
            return false;
        }
        else if (dto.textareaDescricao == '') {
            mensagemErro = "Por favor, preencha o campo de descrição do anúncio";
            return false;
        }

        return true;
    }

    function excluirAnuncio(anuncio){
        // Exibe a confirmação para o usuário
        const confirmacao = confirm("Você tem certeza que deseja excluir este anúncio?");
    
        if (confirmacao) {
            // Se o usuário confirmar, prossegue com a exclusão
                   
            $.ajax({
                url: '/Anuncios/ExcluirAnuncio', // Certifique-se do caminho correto
                type: 'POST',
                data: {
                    anuncio: anuncio
                },
                dataType: 'json',
                success: function(data) {
                    alert('Anúncio excluído com sucesso!');
                    pesquisarAnunciosPorUsuario(anuncio.idUsuario);
                },
                error: function(jqXHR, textStatus, errorThrown) {
                    console.error('Erro na requisição:', textStatus, errorThrown);
                    alert('Erro ao excluir o anúncio. Tente novamente.');
                }
            });
        } else {
            // Se o usuário cancelar, exibe uma mensagem ou apenas retorna
            console.log('Exclusão cancelada pelo usuário.');
        }
    }

    return {
        inserirImagem: inserirImagem,
        validaCampos: validaCampos,
        abrirModalCadastrarAnuncio: abrirModalCadastrarAnuncio,
        fecharModalCadastrarAnuncio: fecharModalCadastrarAnuncio,
        fecharModalPerfilInvalido: fecharModalPerfilInvalido,
        cadastrarAnuncio: cadastrarAnuncio,
        pesquisarTodosAnuncios: pesquisarTodosAnuncios,
        pesquisarAnuncio: pesquisarAnuncio,
        pesquisarAnunciosPorUsuario:pesquisarAnunciosPorUsuario,
        criarAnunciosHtmlPerfil:criarAnunciosHtmlPerfil,
        excluirAnuncio:excluirAnuncio
    }
}
