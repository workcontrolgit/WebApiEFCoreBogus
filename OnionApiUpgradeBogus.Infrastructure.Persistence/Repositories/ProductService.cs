using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Nest;
using OnionApiUpgradeBogus.Application.Interfaces.Repositories;
using OnionApiUpgradeBogus.Domain.Entities;

namespace OnionApiUpgradeBogus.Infrastructure.Persistence.Repositories
{
    public class ProductService : IProductService
    {
        public readonly IElasticClient _elasticClient;

        public ProductService(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<Product> Add(Product product)
        {
            var response = await _elasticClient.IndexDocumentAsync<Product>(product);

            if (response.IsValid)
            {
                return product;
            }
            else
            {
                return new Product();
            }
        }

        public async Task Delete(Guid productId)
        {
            await _elasticClient.DeleteByQueryAsync<Product>(p => p.Query(q1 => q1
                             .Match(m => m
                                 .Field(f => f.Id)
                                 .Query(productId.ToString()
                                 )
                         )));
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            var esResponse = await _elasticClient.SearchAsync<Product>(s => s
                .Query(q => q
                .MatchAll()
                ));

            return esResponse.Documents;
        }

        public async Task<Product> GetById(Guid productId)
        {
            var esResponse = await _elasticClient.SearchAsync<Product>(x => x.
                             Query(q1 => q1.Bool(b => b.Must(m =>
                             m.Terms(t => t.Field(f => f.Id)
                             .Terms<Guid>(productId))))));

            return esResponse.Documents.FirstOrDefault();
        }

        public async Task Update(Product product)
        {
            if (product != null)
            {
                var updateResponse = await _elasticClient.UpdateByQueryAsync<Product>(q =>
                                     q.Query(q1 => q1.Bool(b => b.Must(m =>
                                     m.Match(x => x.Field(f =>
                                     f.Id == product.Id)))))
                                     .Script(s => s.Source(
                                    "ctx._source.Name = params.Name;" +
                                    "ctx._source.Description = params.Description;" +
                                    "ctx._source.CreationDate = params.CreationDate;")
                                    .Lang("painless")
                                    .Params(p => p.Add("Name", product.Name)
                                    .Add("Description", product.Description)
                                    .Add("CreationDate", product.CreationDate)
                                    )).Conflicts(Conflicts.Proceed));
            }
        }
    }
}
