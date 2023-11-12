using OnionApiUpgradeBogus.Domain.Entities;
using System.Collections.Generic;

namespace OnionApiUpgradeBogus.Application.Interfaces
{
    public interface IMockService
    {
        List<Position> GetPositions(int rowCount);

        List<Customer> GetCustomers(int rowCount);

        List<Employee> GetEmployees(int rowCount);

        List<Position> SeedPositions(int rowCount);
    }
}