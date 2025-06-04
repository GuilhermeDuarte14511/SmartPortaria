using SmartPortaria.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartPortaria.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<bool> CadastrarUsuarioAsync(UsuarioCadastroModalRequest request);
        Task<UsuarioDto?> ReconhecerUsuarioAsync(IEnumerable<float> vetorFacial);
    }
}
