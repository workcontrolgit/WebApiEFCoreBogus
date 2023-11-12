using AutoMapper;
using OnionApiUpgradeBogus.Application.Features.Customers.Commands.CreateCustomer;
using OnionApiUpgradeBogus.Application.Features.Customers.Queries.GetCustomers;
using OnionApiUpgradeBogus.Application.Features.Employees.Queries.GetEmployees;
using OnionApiUpgradeBogus.Application.Features.Positions.Commands.CreatePosition;
using OnionApiUpgradeBogus.Application.Features.Positions.Queries.GetPositions;
using OnionApiUpgradeBogus.Domain.Entities;

namespace OnionApiUpgradeBogus.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Employee, GetEmployeesViewModel>().ReverseMap();
            CreateMap<Position, GetPositionsViewModel>().ReverseMap();
            CreateMap<CreatePositionCommand, Position>();

            CreateMap<Customer, GetCustomersViewModel>().ReverseMap();
            CreateMap<CreateCustomerCommand, Customer>();
        }
    }
}