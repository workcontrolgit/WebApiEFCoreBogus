using AutoMapper;
using MediatR;
using OnionApiUpgradeBogus.Application.Interfaces.Repositories;
using OnionApiUpgradeBogus.Application.Wrappers;
using OnionApiUpgradeBogus.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnionApiUpgradeBogus.Application.Features.Customers.Commands.CreateCustomer
{

    public partial class CreateCustomerCommand: Customer, IRequest<Response<Customer>>
    {
    }

    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Response<Customer>>
    {
        private readonly ICustomerRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public CreateCustomerCommandHandler(ICustomerRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<Customer>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = _mapper.Map<Customer>(request);
            await _repository.AddAsync(customer);
            return new Response<Customer>(customer);
        }
    }


}