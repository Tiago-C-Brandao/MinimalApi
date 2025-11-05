using Microsoft.EntityFrameworkCore;
using MinimalApi.Domain.DTOs;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Interfaces;
using MinimalApi.Infrastructure.Db;
using System.Xml.Linq;

namespace MinimalApi.Domain.Services
{
    public class AdminService : IAdminService
    {
        private readonly MinimalApiContext _minimalApiContext;
        public AdminService(MinimalApiContext minimalContext) 
        {
            _minimalApiContext = minimalContext;
        }

        public Admin AddAdmin(Admin admin)
        {
            _minimalApiContext.Admins.Add(admin);
            _minimalApiContext.SaveChanges();

            return admin;
        }

        public Admin? GetAdmin(int id)
        {
            return _minimalApiContext.Admins.Where(x => x.Id == id).FirstOrDefault();
        }

        public List<Admin> All(int? page)
        {
            var query = _minimalApiContext.Admins.AsQueryable();

            int itemsPerPage = 10;

            if (page != null)
                query = query.Skip(((int)page - 1) * itemsPerPage).Take(itemsPerPage);

            return query.ToList();
        }

        public Admin? Login(LoginDTO loginDTO)
        {
            var admin = _minimalApiContext.Admins.Where(a => a.Email == loginDTO.Email && a.Password == loginDTO.Password).FirstOrDefault();
            return admin;
        }
    }
}
