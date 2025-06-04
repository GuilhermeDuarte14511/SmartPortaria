using SmartPortaria.Application.DTOs;
using System.Threading.Tasks;

namespace SmartPortaria.Application.Interfaces
{
    public interface IAdminService
    {
        Task<AdminDto?> ObterPorEmailAsync(string email);
        Task<bool> VerificarLoginAsync(string email, string senha);
    }
}
