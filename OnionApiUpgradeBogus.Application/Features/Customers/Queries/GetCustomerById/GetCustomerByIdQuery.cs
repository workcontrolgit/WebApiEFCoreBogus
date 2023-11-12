using MediatR;
using OnionApiUpgradeBogus.Application.Exceptions;
using OnionApiUpgradeBogus.Application.Interfaces.Repositories;
using OnionApiUpgradeBogus.Application.Wrappers;
using OnionApiUpgradeBogus.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnionApiUpgradeBogus.Application.Features.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQuery : IRequest<Response<Customer>>
    {
        public Guid Id { get; set; }

        public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Response<Customer>>
        {
            private readonly ICustomerRepositoryAsync _positionRepository;

            public GetCustomerByIdQueryHandler(ICustomerRepositoryAsync positionRepository)
            {
                _positionRepository = positionRepository;
            }

            public async Task<Response<Customer>> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken)
            {
                var position = await _positionRepository.GetByIdAsync(query.Id);
                if (position == null) throw new ApiException($"Customer Not Found.");
                return new Response<Customer>(position);
            }
        }
    }
}