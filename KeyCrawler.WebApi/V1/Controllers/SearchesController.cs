using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KeyCrawler.Domain;
using KeyCrawler.WebApi.V1.Requests;
using KeyCrawler.Service.Services;

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
        public IActionResult Search(SearchRequest searchRequest)
        {
            _searchService.Search(searchRequest.Uris, searchRequest.Keywords);
            return Ok(); //TODO later return 201 and job id
        }
    }
}
