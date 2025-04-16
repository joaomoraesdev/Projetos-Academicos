using System.Security;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace PBL_EC8.Bll;

public class UsuarioBll : IUsuarioBll
{
    private readonly IMongoCollection<Usuario> _usuariosCollection;

    // Construtor que injeta a dependência de IMongoClient
    public UsuarioBll(IMongoClient mongoClient, string databaseName, string collectionName)
    {
        var database = mongoClient.GetDatabase(databaseName);
        _usuariosCollection = database.GetCollection<Usuario>(collectionName);
    }

    // Método de criação de usuário, que tenta criar e retorna uma mensagem
    public async Task<RetornoAcaoDto> CriarUsuario(UsuarioDto usuarioDto)
    {
        RetornoAcaoDto retorno = new RetornoAcaoDto();
        try
        {
            Usuario usuario = ConverterUsuarioDto(usuarioDto); // Conversão do DTO
            RetornoAcaoDto usuarioPesquisado = await ValidaCriacaoUsuario(usuarioDto);
            if (!usuarioPesquisado.Sucesso)
            {
                retorno.Mensagem = usuarioPesquisado.Mensagem;
                retorno.Sucesso = false;
                return retorno;
            }
            else
            {
                await _usuariosCollection.InsertOneAsync(usuario); // Aguarda o insert no MongoDB
                retorno.Mensagem = "Usuário criado com sucesso!";
                retorno.Sucesso = true;
            }
        }
        catch (Exception ex)
        {
            retorno.Mensagem = "Falha ao criar o usuário...";
            retorno.Sucesso = false;
        }
        return retorno;
    }

    public async Task<UsuarioDto> PesquisarUsuario(UsuarioDto usuarioDto)
    {
        try
        {
            FilterDefinition<Usuario> filtro = Builders<Usuario>.Filter.Or
                    (
                        Builders<Usuario>.Filter.Eq(u => u.Id, usuarioDto.Id),
                        Builders<Usuario>.Filter.Eq(u => u.Email, usuarioDto.Email),
                        Builders<Usuario>.Filter.Eq(u => u.NomeUsuario, usuarioDto.NomeUsuario)
                    );
            Usuario usuario = await _usuariosCollection.Find(filtro).FirstOrDefaultAsync();
            usuarioDto = ConverterUsuario(usuario);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Falha ao pesquisar o usuário");
        }
        return usuarioDto;
    }

    public async Task<RetornoAcaoDto> ValidaCriacaoUsuario(UsuarioDto usuarioDto)
    {
        RetornoAcaoDto retorno = new RetornoAcaoDto();
        try
        {
            //Valida apenas Nome de Usuário
            FilterDefinition<Usuario> filtro = Builders<Usuario>.Filter.Eq(u => u.NomeUsuario, usuarioDto.NomeUsuario);
            Usuario usuario = await _usuariosCollection.Find(filtro).FirstOrDefaultAsync();
            if (usuario != null)
            {
                retorno.Sucesso = false;
                retorno.Mensagem = "Nome de Usuário já utilizado!";
                return retorno;
            }

            //Valida apenas email
            filtro = Builders<Usuario>.Filter.Eq(u => u.Email, usuarioDto.Email);
            usuario = await _usuariosCollection.Find(filtro).FirstOrDefaultAsync();
            if (usuario != null)
            {
                retorno.Sucesso = false;
                retorno.Mensagem = "Email já utilizado!";
                return retorno;
            }
        }
        catch (Exception ex)
        {
            retorno.Sucesso = false;
            retorno.Mensagem = ex.Message;
        }
        retorno.Sucesso = true;
        retorno.Mensagem = "Usuário pronto para ser utilizado";
        return retorno;
    }

    public async Task<RetornoAcaoDto> ValidacaoLogin(string login, string senha)
    {
        RetornoAcaoDto retorno = new RetornoAcaoDto();
        try
        {
            FilterDefinition<Usuario> filtro = Builders<Usuario>.Filter.And(
                Builders<Usuario>.Filter.Or
                    (
                        Builders<Usuario>.Filter.Eq(u => u.Email, login),
                        Builders<Usuario>.Filter.Eq(u => u.NomeUsuario, login)
                    ),
                Builders<Usuario>.Filter.Eq(u => u.Senha, senha)
            );
            Usuario usuario = await _usuariosCollection.Find(filtro).FirstOrDefaultAsync();
            if (usuario != null)
            {
                retorno.Sucesso = true;
                retorno.Mensagem = $"Bem Vindo à Pratika, {usuario.NomeUsuario}";
            }
            else
            {
                retorno.Sucesso = false;
                retorno.Mensagem = "Falha no acesso, verifique seu login e senha";
            }
        }
        catch (Exception ex)
        {
            retorno.Sucesso = false;
            retorno.Mensagem = ex.Message;
        }
        return retorno;
    }

    public async Task<List<Usuario>> PesquisarTodosUsuario()
    {
        return await _usuariosCollection.Find(_ => true).ToListAsync();
    }

    public async Task<UsuarioDto> PesquisarUsuarioPorId(string IdUsuario)
    {
        UsuarioDto usuarioDto = new UsuarioDto();
        try
        {
            FilterDefinition<Usuario> filtro = Builders<Usuario>.Filter.Or
                    (
                       
                        Builders<Usuario>.Filter.Eq(u => u.Id, IdUsuario)
                    );
            Usuario usuario = await _usuariosCollection.Find(filtro).FirstOrDefaultAsync();
            usuarioDto = ConverterUsuario(usuario);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Falha ao pesquisar o usuário" + ex);
        }
        return usuarioDto;
    }

    public void DeletarUsuario()
    {

    }

    public void AtualizarUsuario()
    {

    }

    private Usuario ConverterUsuarioDto(UsuarioDto usuarioDto)
    {
        Usuario usuario = new Usuario();

        usuario.ImagemPerfil = usuarioDto.ImagemPerfil;
        usuario.NomeUsuario = usuarioDto.NomeUsuario;
        usuario.Nome = usuarioDto.Nome;
        usuario.Sobrenome = usuarioDto.Sobrenome;
        usuario.Email = usuarioDto.Email;
        usuario.Senha = usuarioDto.Senha;
        usuario.DataNascimento = Convert.ToDateTime(usuarioDto.DataNascimento);
        usuario.Descricao = usuarioDto.Descricao;
        usuario.Genero = usuarioDto.Genero;
        usuario.Perfil = usuarioDto.Perfil;
        return usuario;
    }

    private UsuarioDto ConverterUsuario(Usuario usuario)
    {
        UsuarioDto usuarioDto = new UsuarioDto();

        usuarioDto.Id = usuario.Id;
        usuarioDto.ImagemPerfil = usuario.ImagemPerfil;
        usuarioDto.NomeUsuario = usuario.NomeUsuario;
        usuarioDto.Nome = usuario.Nome;
        usuarioDto.Sobrenome = usuario.Sobrenome;
        usuarioDto.Email = usuario.Email;
        usuarioDto.Senha = usuario.Senha;
        usuarioDto.DataNascimento = usuario.DataNascimento.ToString();
        usuarioDto.Descricao = usuario.Descricao;
        usuarioDto.Genero = usuario.Genero;
        usuarioDto.Perfil = usuario.Perfil;
        return usuarioDto;
    }
}
