using OnionApiUpgradeBogus.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnionApiUpgradeBogus.Application.Interfaces.Repositories
{
    public interface IGenericDocumentAsync<T> where T : class
    {
        Task<T> Add(T entity);
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(Guid id);
        Task Update(T entity);
        Task Delete(Guid id);

    }
}
