using OnionApiUpgradeBogus.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnionApiUpgradeBogus.Application.Interfaces.Repositories
{
    public interface IProductService
    {
        Task<Product> Add(Product product);
        Task<IEnumerable<Product>> GetAll();
        Task<Product> GetById(Guid productId);
        Task Update(Product product);
        Task Delete(Guid productId);

    }
}
