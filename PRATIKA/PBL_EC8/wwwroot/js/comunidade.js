document.addEventListener('DOMContentLoaded', function () {
    const page = document.body.getAttribute('data-page'); // Obtém a identificação da página
   
    if(page == 'comunidade') {
        const btnCriacaoConteudo = $('#btnCriacaoConteudo');
        btnCriacaoConteudo.text("Postar");
        // btnCriacaoConteudo.on('click', anuncios().abrirModalCadastrarAnuncio);
        // anuncios().pesquisarTodosAnuncios();
    }   
});

var usuario;
var curtidas =[];
var relevancias =[];

function comunidade() {
    function abrirModalNovaPostagem(dto) {
        var post;
        if(dto)
        {
            post = JSON.parse(decodeURIComponent(dto));
        }
        $.ajax({
            url: '/Comunidade/ShowModalContent',
            type: 'POST',
            success: function(response) {
                const modalContainer = document.getElementById("modalContainer");
                modalContainer.innerHTML = response;
                modalContainer.style.display = "flex";
                modalContainer.style.position = "fixed";
                modalContainer.style.top = "0";
                modalContainer.style.left = "0";
                modalContainer.style.width = "100%";
                modalContainer.style.height = "100%";
                modalContainer.style.backgroundColor = "rgba(0, 0, 0, 0.5)";
                modalContainer.style.justifyContent = "center";
                modalContainer.style.alignItems = "center";
            },
            error: function() {
                console.log("Erro ao carregar o conteúdo da modal.");
            }
        });
      
        setTimeout(() => {
            if(post){
                atribuiValoresDoPost(post);
            } 
        },150)
        
    }

    function atribuiValoresDoPost(post){
      
        $('#idPostModal').val(post.id);
        $('#idUsuarioModal').val(post.idUsuario);
        $('#qtdCurtidasModal').val(post.qtdCurtidas);
        $('#qtdImpulsionamentoModal').val(post.qtdImpulsionamentos);
        $('#postContent').val(post.descricao);

    }

    function closeModal() {
        document.getElementById("modalContainer").style.display = "none";
    }

    window.onclick = function(event) {
        const modalContainer = document.getElementById("modalContainer");
        if (event.target === modalContainer) {
            closeModal();
        }
    };

    async function cadastrarPost() {
        var idPost = $('#idPostModal').val();
        if(idPost)
        {
            const dto = {
                id: $('#idPostModal').val(),
                descricao: $('#postContent').val(),
                idUsuario: $('#idUsuarioModal').val(),
                qtdCurtidas: $('#qtdCurtidasModal').val(),
                qtdImpulsionamentos: $('#qtdImpulsionamentoModal').val(),
               
            };

            const fileInput = $('#fileInput')[0];
            const file = fileInput.files[0];
        
            if (file) {
                try {
                    // Converte o arquivo para base64 de forma assíncrona
                    const base64String = await convertFileToBase64(file);
                    dto.fotoAnexo = base64String; // Agora a foto estará em base64
                } catch (error) {
                    console.error('Erro ao converter a imagem:', error);
                    alert('Erro ao converter a imagem. Tente novamente.');
                    return;
                }
            } else {
                dto.fotoAnexo = null; // Caso nenhum arquivo tenha sido selecionado
            }

            $.ajax({
                url: '/Comunidade/EditarPost', // Certifique-se do caminho correto
                type: 'POST',
                data:dto, // Serializa o objeto como JSON
                dataType: 'json',
                success: function(data) {
                    alert(data.message);
                    if (data.success) {
                        closeModal(); // Fecha a modal após sucesso
                        carregarPostsPorUsuario(dto.idUsuario);
                    }
                },
                error: function(jqXHR, textStatus, errorThrown) {
                    console.error('Erro na requisição:', textStatus, errorThrown);
                    alert('Erro ao cadastrar post. Tente novamente.');
                }
            });
        }
        else{
            const dto = {
                descricao: $('#postContent').val(),
            };
        
            // Obtém o arquivo selecionado
            const fileInput = $('#fileInput')[0];
            const file = fileInput.files[0];
        
            if (file) {
                try {
                    // Converte o arquivo para base64 de forma assíncrona
                    const base64String = await convertFileToBase64(file);
                    dto.fotoAnexo = base64String; // Agora a foto estará em base64
                } catch (error) {
                    console.error('Erro ao converter a imagem:', error);
                    alert('Erro ao converter a imagem. Tente novamente.');
                    return;
                }
            } else {
                dto.fotoAnexo = null; // Caso nenhum arquivo tenha sido selecionado
            }
        
            // Envia a requisição AJAX
            $.ajax({
                url: '/Comunidade/CadastrarPost', // Certifique-se do caminho correto
                type: 'POST',
                data:dto, // Serializa o objeto como JSON
                dataType: 'json',
                success: function(data) {
                    alert(data.message);
                    if (data.success) {
                        closeModal(); // Fecha a modal após sucesso
                        window.location.reload(); // Recarrega a página
                    }
                },
                error: function(jqXHR, textStatus, errorThrown) {
                    console.error('Erro na requisição:', textStatus, errorThrown);
                    alert('Erro ao cadastrar post. Tente novamente.');
                }
            });
        }
    }
    
    function convertFileToBase64(file) {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.onload = () => resolve(reader.result.split(',')[1]); // Remove o prefixo 'data:*/*;base64,'
            reader.onerror = (error) => reject(error);
            reader.readAsDataURL(file);
        });
    }
    

    async function carregarPosts(pesquisa) {
        buscarCurtidasGeral();
        buscarRelevanciasGeral();

        $.ajax({
            url: '/Comunidade/ListarPosts', // Certifique-se do caminho correto
            type: 'POST',
            dataType: 'json',
            success: async function(data) {
                if (!data || data.length === 0) {
                    document.getElementById('posts-container').innerHTML = '<p>Nenhum post encontrado.</p>';
                    return;
                }
               
                if(pesquisa){
                    data = pesquisa;
                }            
                
                // Limpe o contêiner
                const postsContainer = document.getElementById('posts-container');
                postsContainer.innerHTML = ' ';
               // Ordena os posts por quantidade de impulsionamentos (qtdImpulsionamentos) em ordem decrescente
                data.sort((a, b) => b.qtdImpulsionamentos - a.qtdImpulsionamentos);

                data.forEach(post => {
                    let profilePicHtml = '';
                
                    // Verifique se a imagem Base64 está presente (foto de perfil)
                    if (post.fotoAnexo) {
                        // Não adicione a foto no perfil, mas adicione abaixo da descrição
                        profilePicHtml = `<img src="data:image/png;base64,${post.fotoAnexo}" alt="Foto do post" class="post-image">`;
                    }
                
                    const max = 1000000000000000000000;
                    post.idBotaoPumpCheio = post.id + "_pumpPreenchido";
                    post.idBotaoPumpVazio = post.id + "_pumpVazio";
                    post.idBotaoImpulsionarCheio = post.id + "_impulsionarVazio";
                    post.idBotaoImpulsionarVazio = post.id + "_impulsionarPreenchido";
                
                    // Serializa o post para JSON e o escapa para uso no HTML
                    const serializedPost = encodeURIComponent(JSON.stringify(post));
                
                    const postHtml = `
                    <div class="tweet" id="post-${post.id}">
                        <div class="tweet-content">
                            <div class = "perfil-post">
                                <img id="fotoPerfil-${post.id}" alt="Foto do post" class="profile-pic">
                                <span class="username" id="username-${post.id}">Carregando...</span>
                                <span class="handle" id="handle-${post.id}">@carregando</span>
                            </div>
                            <div class="content">${post.descricao}</div>
                        </div>
                        <div class="post-image-container">
                            ${profilePicHtml}
                        </div>
                        <div class="icons">
                            <button id="${post.idBotaoPumpVazio}" class="pump-button" onclick="comunidade().darPumpPost('${serializedPost}')" aria-label="Clique e dê um PUMP!">
                                <img src="../img/pump_vazio.png" alt="" class="pump">
                            </button>
                            <button id="${post.idBotaoPumpCheio}" class="pump-button" onclick="comunidade().retirarPumpPost('${serializedPost}')" aria-label="Clique e dê um PUMP!" hidden>
                                <img src="../img/pump_preenchido.png" alt="" class="pump">
                            </button>
                            <label>${post.qtdCurtidas}</label>
                            <button id="${post.idBotaoImpulsionarVazio}" class="relevancia-button" onclick="comunidade().impulsionarPost('${serializedPost}')" aria-label="Aumente a relevância desse post!">
                                <img src="../img/relevancia_vazia.png" alt="" class="relevancia">
                            </button>
                            <button id="${post.idBotaoImpulsionarCheio}" class="relevancia-button" onclick="comunidade().retiraImpulsionarPost('${serializedPost}')" aria-label="Aumente a relevância desse post!" hidden>
                                <img src="../img/relevancia_preenchido.png" alt="" class="relevancia">
                            </button>
                            <label>${post.qtdImpulsionamentos}</label>
                        </div>
                    </div>`;
                    postsContainer.innerHTML += postHtml;
                
                    setTimeout(() => {
                        curtidas.forEach(curtida => {
                            if (curtida.idUsuario == usuario.id && curtida.idPost == post.id) {
                                tiraPumpVazioColocaPreenchido(post);
                            }
                        });
                    }, 150);
                
                    setTimeout(() => {
                        relevancias.forEach(relevancia => {
                            if (relevancia.idUsuario == usuario.id && relevancia.idPost == post.id) {
                                tiraImpulsionarVazioColocaPreenchido(post);
                            }
                        });
                    }, 150);
                
                    setTimeout(async () => {
                        await pesquisarUsuarioPost(post.idUsuario, post.id);
                    }, 150);
                });

                 
               
               
            },                        
            error: function(jqXHR, textStatus, errorThrown) {
                console.error('Erro na requisição:', textStatus, errorThrown);
                alert('Erro ao cadastrar post. Tente novamente.'); console.error('Erro ao carregar posts:', error);
                document.getElementById('posts-container').innerHTML = '<p>Erro ao carregar os posts. Tente novamente mais tarde.</p>';
            }
        });
    }

    function pesquisarPostsSeachbar(){
      var pesquisa = $('#textboxPesquisaAnuncios').val();
  
        $(document).ready(function () {
            $.ajax({
                url: base_path + "/Comunidade/PesquisarAnunciosSeachbar",
                data: {
                    pesquisa: pesquisa
                },
                type: 'POST',
                dataType: 'json',
                success: function (data) {
                    carregarPosts(data);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error('Erro na requisição:', textStatus, errorThrown);
                }
            });
        });
    
    }

    async function darPumpPost(dto) {
        const post = JSON.parse(decodeURIComponent(dto));
        
        try {
            // Realiza a requisição AJAX
            const data = await $.ajax({
                url: '/Comunidade/DarPumpPost', // Certifique-se do caminho correto
                type: 'POST',
                data: post, // Serializa o objeto como JSON
                dataType: 'json'
            });

            // Aguarda o carregamento dos posts após o sucesso
            await carregarPosts();
            
            // Adiciona um pequeno atraso para garantir que os elementos estejam prontos
            
                tiraPumpVazioColocaPreenchido(post);
           // 100 milissegundos de atraso
            
        } catch (error) {
            console.error('Erro na requisição:', error);
            alert('Erro ao cadastrar post. Tente novamente.');
        }
    }
    
    async function retirarPumpPost(dto) {
        const post = JSON.parse(decodeURIComponent(dto));
        
        
        try {
            // Realiza a requisição AJAX
            const data = await $.ajax({
                url: '/Comunidade/RetirarPumpPost', // Certifique-se do caminho correto
                type: 'POST',
                data: post, // Serializa o objeto como JSON
                dataType: 'json'
            });
            curtidas =[];
            setTimeout(() => {
                 buscarCurtidasGeral();

           }, 100); 
            // Aguarda o carregamento dos posts após o sucesso
            setTimeout(() => {
                 carregarPosts();

            }, 100); 
            // Adiciona um pequeno atraso para garantir que os elementos estejam prontos
            setTimeout(() => {
                tiraPumpPreenchidoColocaVazio(post);
            }, 150); // 100 milissegundos de atraso
            
        } catch (error) {
            console.error('Erro na requisição:', error);
            alert('Erro ao cadastrar post. Tente novamente.');
        }
    }
    

    async function tiraPumpVazioColocaPreenchido(post){
        
        // Após a execução, esconde o botão do "pump vazio"
        var vazio = post.id + "_pumpVazio";
        document.getElementById(vazio).hidden = true;
        var preenchido = post.id + "_pumpPreenchido";
        document.getElementById(preenchido).hidden = false;

        //$("#" + elementId).prop("hidden", true);
    }
    async function tiraPumpPreenchidoColocaVazio(post){
        
        // Após a execução, esconde o botão do "pump vazio"
        var vazio = post.id + "_pumpVazio";
        document.getElementById(vazio).hidden = false;
        var preenchido = post.id + "_pumpPreenchido";
        document.getElementById(preenchido).hidden = true;

        //$("#" + elementId).prop("hidden", true);
    }

    async function impulsionarPost(dto) {
        const post = JSON.parse(decodeURIComponent(dto));
        
        try {
            // Realiza a requisição AJAX
            const data = await $.ajax({
                url: '/Comunidade/ImpulsionarPost', // Certifique-se do caminho correto
                type: 'POST',
                data: post, // Serializa o objeto como JSON
                dataType: 'json'
            });
            
            // Aguarda o carregamento dos posts após o sucesso
            await carregarPosts();
            
            // Adiciona um pequeno atraso para garantir que os elementos estejam prontos
            setTimeout(() => {
                tiraImpulsionarVazioColocaPreenchido(post);
            }, 100); // 100 milissegundos de atraso
            
        } catch (error) {
            console.error('Erro na requisição:', error);
            alert('Erro ao cadastrar post. Tente novamente.');
        }
    }

    async function retiraImpulsionarPost(dto) {
        const post = JSON.parse(decodeURIComponent(dto));
     
        
        try {
            // Realiza a requisição AJAX
            const data = await $.ajax({
                url: '/Comunidade/RetiraImpulsionarPost', // Certifique-se do caminho correto
                type: 'POST',
                data: post, // Serializa o objeto como JSON
                dataType: 'json'
            });
            relevancias =[];
            setTimeout(() => {
                buscarRelevanciasGeral();
           }, 100); 
           
            setTimeout(() => {
                carregarPosts();
            }, 100); 
         
            setTimeout(() => {
                colocaImpulsionarVazioTiraPreenchido(post);
            }, 150);
           
        } catch (error) {
            console.error('Erro na requisição:', error);
            alert('Erro ao cadastrar post. Tente novamente.');
        }
    }

    function tiraImpulsionarVazioColocaPreenchido(post){
        
        var vazio = post.id + "_impulsionarVazio";
        document.getElementById(vazio).hidden = false;
        var preenchido = post.id + "_impulsionarPreenchido";
        document.getElementById(preenchido).hidden = true;
    }

    function colocaImpulsionarVazioTiraPreenchido(post){
        var vazio = post.id + "_impulsionarVazio";
        document.getElementById(vazio).hidden = true;
        var preenchido = post.id + "_impulsionarPreenchido";
        document.getElementById(preenchido).hidden = false;
    }

    async function  buscarCurtidasGeral(){
        $.ajax({
            url: '/Comunidade/BuscarCurtidas', // Certifique-se do caminho correto
            type: 'POST', // Serializa o objeto como JSON
            dataType: 'json',
            success: function(data) {
                if(data){
                curtidas = data;
                }
            },
            error: function(jqXHR, textStatus, errorThrown) {
                console.error('Erro na requisição:', textStatus, errorThrown);
                alert('Erro ao buscar curtidas. Tente novamente.');
            }
        });
    }

    async function  buscarRelevanciasGeral(){
        $.ajax({
            url: '/Comunidade/BuscarRelevancia', // Certifique-se do caminho correto
            type: 'POST', // Serializa o objeto como JSON
            dataType: 'json',
            success: function(data) {
                if(data){
                relevancias = data;
                }
            },
            error: function(jqXHR, textStatus, errorThrown) {
                console.error('Erro na requisição:', textStatus, errorThrown);
                alert('Erro ao buscar relevancias. Tente novamente.');
            }
        });
    }

    function getUsuarioLogado(isPerfil){
        $.ajax({
            url: '/Comunidade/GetUsuarioLogado', // Certifique-se do caminho correto
            type: 'POST', // Serializa o objeto como JSON
            dataType: 'json',
            success: function(data) {
                if(isPerfil){
                    document.getElementById("nome").textContent = data.nome;
                    document.getElementById("usuario").textContent = "@" + `${data.nomeUsuario}`;
                    document.getElementById("fotoPerfil").src = data.imagemPerfil;
                    document.getElementById("email").textContent = data.email;
                    document.getElementById("genero").textContent = data.genero;
                    document.getElementById("descricao").textContent = data.descricao;
                    $('#idUsuario').val(data.id);
                }
                usuario = data;
            
                return data;
            },
            error: function(jqXHR, textStatus, errorThrown) {
                console.error('Erro na requisição:', textStatus, errorThrown);
                alert('Erro ao buscar curtidas. Tente novamente.');
            }
        });
    }

    function pesquisarUsuarioPost(idUsuario, postId){
    
        $.ajax({
            url: '/Comunidade/PesquisarUsuarioPost', // Certifique-se do caminho correto
            type: 'POST', // Serializa o objeto como JSON
            data: { idUsuario: idUsuario },
            dataType: 'json',
            success: function(data) {
             
                document.getElementById(`username-${postId}`).textContent = data.nome;
                document.getElementById(`handle-${postId}`).textContent = "@" + data.nomeUsuario;
                document.getElementById(`fotoPerfil-${postId}`).src  = data.imagemPerfil;
                return data;
            },
            error: function(jqXHR, textStatus, errorThrown) {
                console.error('Erro na requisição:', textStatus, errorThrown);
                alert('Erro ao buscar curtidas. Tente novamente.');
            }
        });
    }

    function limparSearchbar(){
        $('#textboxPesquisaAnuncios').val('');
        carregarPosts();
    }

    function onchangeInputFile(event) {

        const file = event.target.files[0];
        const previewContainer = document.getElementById('previewContainer');

        previewContainer.innerHTML = ''; // Limpa o contêiner anterior

        if (file && file.type.startsWith('image/')) {
            const reader = new FileReader();
            reader.onload = function (e) {
                const img = document.createElement('img');
                img.src = e.target.result;
                img.style.width = '100%';
                img.style.height = '100%';
                img.style.objectFit = 'cover';
                previewContainer.appendChild(img);
            };
            reader.readAsDataURL(file);
        }
    }

    async function carregarPostsPorUsuario(idUsuario) {
       
        buscarCurtidasGeral();
        buscarRelevanciasGeral();

        $.ajax({
            url: '/Comunidade/ListarPostsPorUsuario', // Certifique-se do caminho correto
            type: 'POST',
            data:{
                idUsuario: idUsuario
            },
            dataType: 'json',
            success: async function(data) {
                if (!data || data.length === 0) {
                    document.getElementById('posts-container').innerHTML = '<p>Nenhum post encontrado.</p>';
                    return;
                }
               
                      
                
                // Limpe o contêiner
                const postsContainer = document.getElementById('tab1-content');
                postsContainer.innerHTML = ' ';
               // Ordena os posts por quantidade de impulsionamentos (qtdImpulsionamentos) em ordem decrescente
                data.sort((a, b) => b.qtdImpulsionamentos - a.qtdImpulsionamentos);

                data.forEach(post => {
                    let profilePicHtml = '';
                
                    // Verifique se a imagem Base64 está presente (foto de perfil)
                    if (post.fotoAnexo) {
                        // Não adicione a foto no perfil, mas adicione abaixo da descrição
                        profilePicHtml = `<img src="data:image/png;base64,${post.fotoAnexo}" alt="Foto do post" class="post-image">`;
                    }
                
                    const max = 1000000000000000000000;
                    post.idBotaoPumpCheio = post.id + "_pumpPreenchido";
                    post.idBotaoPumpVazio = post.id + "_pumpVazio";
                    post.idBotaoImpulsionarCheio = post.id + "_impulsionarVazio";
                    post.idBotaoImpulsionarVazio = post.id + "_impulsionarPreenchido";
                
                    // Serializa o post para JSON e o escapa para uso no HTML
                    const serializedPost = encodeURIComponent(JSON.stringify(post));
                
                    const postHtml = `
                    <div id="modalContainer" style="display: none;"></div>
                    <div class="tweet" id="post-${post.id}">
                        <div class="tweet-content">
                            <div class = "perfil-post">
                                <img id="fotoPerfil-${post.id}" alt="Foto do post" class="profile-pic">
                                <span class="username" id="username-${post.id}">Carregando...</span>
                                <span class="handle" id="handle-${post.id}">@carregando</span>
                            </div>
                            <div class="icons-edicao">
                            <button id="editar-${post.id}" class="botao-edicao" onclick="comunidade().abrirModalNovaPostagem('${serializedPost}')" aria-label="Clique e dê um PUMP!">
                                <img  src="../img/editar.png" alt="editar post">
                            </button>
                            <button id="excluir-${post.id}" class="botao-edicao" onclick="comunidade().excluirPost('${serializedPost}')" aria-label="Clique e dê um PUMP!">        
                                <img  src="../img/excluir.png" alt="excluir post">
                            </button>        
                            </div>
                            <div class="content">${post.descricao}</div>
                        </div>
                        <div class="post-image-container">
                            ${profilePicHtml}
                        </div>
                        <div class="icons">
                            <button id="${post.idBotaoPumpVazio}" class="pump-button" onclick="comunidade().darPumpPost('${serializedPost}')" aria-label="Clique e dê um PUMP!">
                                <img src="../img/pump_vazio.png" alt="" class="pump">
                            </button>
                            <button id="${post.idBotaoPumpCheio}" class="pump-button" onclick="comunidade().retirarPumpPost('${serializedPost}')" aria-label="Clique e dê um PUMP!" hidden>
                                <img src="../img/pump_preenchido.png" alt="" class="pump">
                            </button>
                            <label>${post.qtdCurtidas}</label>
                            <button id="${post.idBotaoImpulsionarVazio}" class="relevancia-button" onclick="comunidade().impulsionarPost('${serializedPost}')" aria-label="Aumente a relevância desse post!">
                                <img src="../img/relevancia_vazia.png" alt="" class="relevancia">
                            </button>
                            <button id="${post.idBotaoImpulsionarCheio}" class="relevancia-button" onclick="comunidade().retiraImpulsionarPost('${serializedPost}')" aria-label="Aumente a relevância desse post!" hidden>
                                <img src="../img/relevancia_preenchido.png" alt="" class="relevancia">
                            </button>
                            <label>${post.qtdImpulsionamentos}</label>
                        </div>
                    </div>`;
                    postsContainer.innerHTML += postHtml;
                
                    setTimeout(() => {
                        curtidas.forEach(curtida => {
                            if (curtida.idUsuario == usuario.id && curtida.idPost == post.id) {
                                tiraPumpVazioColocaPreenchido(post);
                            }
                        });
                    }, 150);
                
                    setTimeout(() => {
                        relevancias.forEach(relevancia => {
                            if (relevancia.idUsuario == usuario.id && relevancia.idPost == post.id) {
                                tiraImpulsionarVazioColocaPreenchido(post);
                            }
                        });
                    }, 150);
                
                    setTimeout(async () => {
                        await pesquisarUsuarioPost(post.idUsuario, post.id);
                    }, 150);
                });

                 
               
               
            },                        
            error: function(jqXHR, textStatus, errorThrown) {
                console.error('Erro na requisição:', textStatus, errorThrown);
                alert('Erro ao cadastrar post. Tente novamente.'); console.error('Erro ao carregar posts:', error);
                document.getElementById('posts-container').innerHTML = '<p>Erro ao carregar os posts. Tente novamente mais tarde.</p>';
            }
        });
    }

    function excluirPost(dto) {
        // Exibe a confirmação para o usuário
        const confirmacao = confirm("Você tem certeza que deseja excluir este post?");
    
        if (confirmacao) {
            // Se o usuário confirmar, prossegue com a exclusão
            const post = JSON.parse(decodeURIComponent(dto));
        
            $.ajax({
                url: '/Comunidade/ExcluirPost', // Certifique-se do caminho correto
                type: 'POST',
                data: {
                    dto: post
                },
                dataType: 'json',
                success: function(data) {
                    alert('Post excluído com sucesso!');
                    carregarPostsPorUsuario(post.idUsuario);
                },
                error: function(jqXHR, textStatus, errorThrown) {
                    console.error('Erro na requisição:', textStatus, errorThrown);
                    alert('Erro ao excluir o post. Tente novamente.');
                }
            });
        } else {
            // Se o usuário cancelar, exibe uma mensagem ou apenas retorna
            console.log('Exclusão cancelada pelo usuário.');
        }
    }
    
    

    return {
        abrirModalNovaPostagem: abrirModalNovaPostagem,
        closeModal: closeModal,
        cadastrarPost: cadastrarPost,
        carregarPosts: carregarPosts,
        darPumpPost:darPumpPost,
        retirarPumpPost: retirarPumpPost,
        retiraImpulsionarPost: retiraImpulsionarPost,
        impulsionarPost: impulsionarPost,
        buscarCurtidasGeral:buscarCurtidasGeral,
        getUsuarioLogado:getUsuarioLogado,
        buscarRelevanciasGeral:buscarRelevanciasGeral,
        pesquisarUsuarioPost:pesquisarUsuarioPost,
        pesquisarPostsSeachbar:pesquisarPostsSeachbar,
        limparSearchbar:limparSearchbar,
        onchangeInputFile:onchangeInputFile,
        carregarPostsPorUsuario:carregarPostsPorUsuario,
        excluirPost:excluirPost

    };
}
