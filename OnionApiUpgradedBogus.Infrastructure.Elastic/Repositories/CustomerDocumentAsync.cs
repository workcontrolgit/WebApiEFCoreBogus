using Nest;
using OnionApiUpgradeBogus.Application.Interfaces.Repositories;
using OnionApiUpgradeBogus.Domain.Entities;

namespace OnionApiUpgradeBogus.Infrastructure.Persistence.Repositories
{
    public class CustomerDocumentAsync : GenericDocumentAsync<Customer>, ICustomerDocumentAsync
    {
        public CustomerDocumentAsync(IElasticClient elasticClient) : base(elasticClient)
        {
        }
    }
}
