using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Mocks
{
    internal class VehicleServiceMock : IVehicleService
    {
        private static List<Vehicle> vehicles = new List<Vehicle>()
        {
            new Vehicle
            {
                Id = 1,
                Name = "NameTest1",
                Brand = "BrandTest1",
                Year = 2021
            },
            new Vehicle
            {
                Id = 2,
                Name = "NameTest2",
                Brand = "BrandTest2",
                Year = 2022
            },
        };
        public void AddVehicle(Vehicle vehicle)
        {
            vehicle.Id = vehicles.Count() + 1;
            vehicles.Add(vehicle);
        }

        public List<Vehicle> All(int? page = 1, string? name = null, string? brand = null)
        {
            return vehicles;
        }

        public void DeleteVehicle(Vehicle vehicle)
        {
            var vehicleFind = vehicles.Find(x => x.Id == vehicle.Id);
            if (vehicleFind != null)
            {
                vehicles.Remove(vehicleFind);
            }
            else
            {
                Console.WriteLine("Not found");
            }
        }

        public Vehicle? GetVehicle(int id)
        {
            return vehicles.Find(x => x.Id == id);
        }

        public void UpdateVehicle(Vehicle vehicle)
        {
            var vehicleFind = vehicles.Find(x => x.Id == vehicle.Id);
            if (vehicleFind != null)
            {
                vehicles.Remove(vehicleFind);
                vehicles.Add(vehicle);
            }
            else
            {
                Console.WriteLine("Not found");
            }
        }
    }
}
