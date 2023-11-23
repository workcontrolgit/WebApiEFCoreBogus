﻿using Nest;
using OnionApiUpgradeBogus.WebApi.Models;

namespace ElasticSearch
{
    public static class ElasticSearchExtension
    {
        public static void AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            var baseUrl = configuration["ElasticSettings:baseUrl"];
            var index = configuration["ElasticSettings:defaultIndex"];

            var settings = new ConnectionSettings(new Uri(baseUrl ?? ""))
                .PrettyJson()
                .CertificateFingerprint("6b6a8c2ad2bc7b291a7363f7bb96a120b8de326914980c868c1c0bc6b3dc41fd")
                .BasicAuthentication("elastic", "JbNb_unwrJy3W0OaZ07n")
                .DefaultIndex(index)
                // .ServerCertificateValidationCallback(CertificateValidations.AllowAll)
                ;
            settings.EnableApiVersioningHeader();
            AddDefaultMappings(settings);

            var client = new ElasticClient(settings);

            if (client == null)
                throw new Exception("Elastic Connectionstring not supplied");


            services.AddSingleton<IElasticClient>(client);

            CreateIndex(client, index);
        }

        private static void AddDefaultMappings(ConnectionSettings settings)
        {
            settings
                .DefaultMappingFor<ArticleModel>(m => m
                    .Ignore(p => p.Link)
                    .Ignore(p => p.AuthorLink)
                );
        }

        private static void CreateIndex(IElasticClient client, string indexName)
        {
            var createIndexResponse = client.Indices.Create(indexName,
                index => index.Map<ArticleModel>(x => x.AutoMap())
            );
        }
    }
}
