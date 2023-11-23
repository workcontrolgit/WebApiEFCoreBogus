﻿using Microsoft.AspNetCore.Mvc;
using Nest;
using OnionApiUpgradeBogus.WebApi.Models;
using OnionApiUpgradeBogus.WebApi.ViewModels;

namespace OnionApiUpgradeBogus.WebApi.Controllers.v1
{
    public class SearchController : BaseApiController
    {
        private readonly IElasticClient _elasticClient;
        private readonly ILogger<SearchController> _logger;

        public SearchController(
            IElasticClient elasticClient,
            ILogger<SearchController> logger)
        {
            _elasticClient = elasticClient;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string q)
        {
            if (string.IsNullOrEmpty(q))
            {
                var noResultsVM = new SearchViewModel { Term = "[No Search]" };
                //return View(noResultsVM);
                return Ok(await Mediator.Send(noResultsVM));
            }

            // Good article if you want to read an overview of the types of
            // queries available: https://qbox.io/blog/elasticsearch-queries-match-phrase-match
            // Specific usage for MultiMatch: https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/multi-match-usage.html
            var response = await _elasticClient.SearchAsync<Book>(s =>
                s.Query(sq =>
                    sq.MultiMatch(mm => mm
                        .Query(q)
                        .Fuzziness(Fuzziness.Auto)
                    )
                )
            );

            var vm = new SearchViewModel
            {
                Term = q
            };

            if (response.IsValid)
                vm.Results = response.Documents?.ToList();
            else
                _logger.LogError(response.OriginalException, "Problem searching Elasticsearch for term {0}", q);

            // return View(vm);

            return Ok(await Mediator.Send(vm));

        }
    }
}
