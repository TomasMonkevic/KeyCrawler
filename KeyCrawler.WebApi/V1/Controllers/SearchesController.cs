using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KeyCrawler.Domain;
using KeyCrawler.WebApi.V1.Requests;
using KeyCrawler.Service.Services;
using Hangfire;

namespace KeyCrawler.WebApi.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class SearchesController : ControllerBase
    {

        private readonly ILogger<SearchesController> _logger;
        private readonly ISearchService _searchService;

        public SearchesController(ISearchService searchService, ILogger<SearchesController> logger)
        {
            _searchService = searchService;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult CreateSearchJob(SearchRequest searchRequest)
        {
            var searchJobId = BackgroundJob.Enqueue<ISearchService>(s => s.Search(searchRequest.Uris, searchRequest.Keywords, CancellationToken.None));
            return CreatedAtAction("GetSearchJob", new { id = searchJobId }, searchJobId);
        }

        [HttpGet("{id}")]
        [ActionName("GetSearchJob")]
        public IActionResult GetSearchJob([FromRoute] string id)
        {
            //TODO not implemented
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult CancelSearchJob([FromRoute] string id)
        {
            var isSuccessful = BackgroundJob.Delete(id);
            return isSuccessful ? NoContent() : Ok();
        }
    }
}
