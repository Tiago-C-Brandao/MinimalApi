using MinimalApi.Domain.DTOs;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Interfaces;

namespace Test.Mocks
{
    internal class AdminServiceMock : IAdminService
    {
        private static List<Admin> admins = new List<Admin>()
        {
            new Admin
            {
                Id = 1,
                Email = "admin@test.com",
                Password = "123456",
                Role = "Adm"
            },
            new Admin
            {
                Id = 2,
                Email = "editor@test.com",
                Password = "123456",
                Role = "Editor"
            }
        };

        public Admin AddAdmin(Admin admin)
        {
            admin.Id = admins.Count() + 1;
            admins.Add(admin);
            return admin;
        }

        public List<Admin> All(int? page)
        {
            return admins;
        }

        public Admin? GetAdmin(int id)
        {
           return admins.Find(x => x.Id == id);
        }

        public Admin? Login(LoginDTO loginDTO)
        {
            return admins.Find(x => x.Email == loginDTO.Email && x.Password == loginDTO.Password);
        }
    }
}
