using Microsoft.EntityFrameworkCore;
using MinimalApi.Domain.DTOs;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Interfaces;
using MinimalApi.Infrastructure.Db;

namespace MinimalApi.Domain.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly MinimalApiContext _minimalApiContext;
        public VehicleService(MinimalApiContext minimalApiContext) 
        {
            _minimalApiContext = minimalApiContext;
        }

        public void AddVehicle(Vehicle vehicle)
        {
            _minimalApiContext.Vehicles.Add(vehicle);
            _minimalApiContext.SaveChanges();
        }
        public Vehicle? GetVehicle(int id)
        {
            return _minimalApiContext.Vehicles.Where(x => x.Id == id).FirstOrDefault();
        }
        public void UpdateVehicle(Vehicle vehicle)
        {
            _minimalApiContext.Vehicles.Update(vehicle);
            _minimalApiContext.SaveChanges();
        }

        public void DeleteVehicle(Vehicle vehicle)
        {
            _minimalApiContext.Vehicles.Remove(vehicle);
            _minimalApiContext.SaveChanges();
        }

        public List<Vehicle> All(int? page = 1, string? name = null, string? brand = null ) 
        {
            var query = _minimalApiContext.Vehicles.AsQueryable();
            if(!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => EF.Functions.Like(x.Name.ToLower(), $"%{name}%"));
            }

            int itemsPerPage = 10;

            if(page != null)
                query = query.Skip(((int)page - 1) * itemsPerPage).Take(itemsPerPage);

            return query.ToList();
        }
        
    }
}
