using MinimalApi.Domain.DTOs;
using MinimalApi.Domain.Entities;

namespace MinimalApi.Domain.Interfaces
{
    public interface IAdminService
    {
        Admin? Login(LoginDTO loginDTO);
        Admin AddAdmin(Admin admin);
        Admin? GetAdmin(int id);
        List<Admin> All(int? page);
    }
}
