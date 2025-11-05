using MinimalApi.Domain.DTOs;
using MinimalApi.Domain.Entities;

namespace MinimalApi.Domain.Interfaces
{
    public interface IVehicleService
    {
        List<Vehicle> All(int? page = 1, string? name = null, string? brand = null);
        Vehicle? GetVehicle(int id);
        void AddVehicle(Vehicle vehicle);
        void UpdateVehicle(Vehicle vehicle);
        void DeleteVehicle(Vehicle vehicle);
    }
}
