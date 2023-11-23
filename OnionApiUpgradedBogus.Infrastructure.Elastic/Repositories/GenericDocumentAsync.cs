using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nest;
using OnionApiUpgradeBogus.Application.Interfaces.Repositories;

namespace OnionApiUpgradeBogus.Infrastructure.Persistence.Repositories
{
    public class GenericDocumentAsync<T> : IGenericDocumentAsync<T> where T : class
    {
        private readonly IElasticClient _elasticClient;

        public GenericDocumentAsync(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<T> Add(T entity)
        {
            await _elasticClient.IndexDocumentAsync(entity);
            return entity;
        }

        public async Task Delete(Guid id)
        {
            await _elasticClient.DeleteAsync<T>(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            var esResponse = await _elasticClient.SearchAsync<T>(s => s
                .Query(q => q
                .MatchAll()
                ));

            return esResponse.Documents;
        }

        public async Task<T> GetById(Guid id)
        {
            var esResponse = await _elasticClient.GetAsync<T>(id);

            return esResponse.Source;
        }

        public async Task Update(T entity)
        {
            await _elasticClient.UpdateAsync<T>(entity, u => u.Doc(entity));
        }
    }
}
