using MediatR;
using OnionApiUpgradeBogus.Application.Interfaces.Repositories;
using OnionApiUpgradeBogus.Application.Wrappers;
using System.Threading;
using System.Threading.Tasks;

namespace OnionApiUpgradeBogus.Application.Features.Customers.Commands.CreateCustomer
{
    public partial class InsertMockCustomerCommand : IRequest<Response<int>>
    {
        public int RowCount { get; set; }
    }

    public class SeedCustomerCommandHandler : IRequestHandler<InsertMockCustomerCommand, Response<int>>
    {
        private readonly ICustomerRepositoryAsync _repository;

        public SeedCustomerCommandHandler(ICustomerRepositoryAsync repository)
        {
            _repository = repository;
        }

        public async Task<Response<int>> Handle(InsertMockCustomerCommand request, CancellationToken cancellationToken)
        {
            await _repository.SeedDataAsync(request.RowCount);
            return new Response<int>(request.RowCount);
        }
    }
}