using System.Collections.Generic;
using System.Threading.Tasks;

public interface IUsuarioBll
{
    public Task<RetornoAcaoDto> CriarUsuario(UsuarioDto usuario);
    public Task<UsuarioDto> PesquisarUsuario(UsuarioDto usuarioDto);
    public Task<List<Usuario>> PesquisarTodosUsuario();
    public void DeletarUsuario();
    public void AtualizarUsuario();
    public Task<RetornoAcaoDto> ValidacaoLogin(string login, string senha);
}
