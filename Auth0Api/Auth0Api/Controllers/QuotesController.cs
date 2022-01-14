using Auth0Api.Data;
using Auth0Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Auth0Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuotesController : ControllerBase
    {
        private readonly QuotesDbContext _quotesDbContext;


        public QuotesController(QuotesDbContext quotesDbContext)
        {
            _quotesDbContext = quotesDbContext;
        }

        [HttpGet]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        public IActionResult Get(string? sort)
        {
            IQueryable<Quote> quotes;

            switch (sort)
            {
                case "desc":
                    quotes = _quotesDbContext.Quotes.OrderByDescending(q => q.CreatedAt);
                    break;
                case "asc":
                    quotes = _quotesDbContext.Quotes.OrderBy(q => q.CreatedAt);
                    break;
                default:
                    quotes = _quotesDbContext.Quotes;
                    break;

            }

            return StatusCode(StatusCodes.Status200OK, quotes);
        }

        [HttpGet("[action]")]
        //[Route("[action]")]
        public IActionResult PagingQuote(int? pageNumber, int? pageSize)
        {
            var quotes = _quotesDbContext.Quotes;

            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 5;

            return Ok(quotes.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));

        }
        [HttpGet]
        [Route("[action]")]
        public IActionResult SearchQuote(string type)
        {
            var quotes = _quotesDbContext.Quotes.Where(q => q.Type.StartsWith(type));
            return Ok(quotes);
        }

        // GET api/<QuotesController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var quote = _quotesDbContext.Quotes.FirstOrDefault(x => x.Id == id);
            if (quote == null)
                return StatusCode(StatusCodes.Status404NotFound, quote);
            else
                return StatusCode(StatusCodes.Status200OK, _quotesDbContext.Quotes.Find(id));
            
        }

        // POST api/<QuotesController>
        [HttpPost]
        public IActionResult Post([FromBody] Quote quote)
        {
            _quotesDbContext.Quotes.Add(quote);
             _quotesDbContext.SaveChanges();
             return StatusCode(StatusCodes.Status201Created, quote);
        }

        // PUT api/<QuotesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Quote quote)
        {
           ;
           var entity = _quotesDbContext.Quotes.Find(id);
           if (entity == null)
               return StatusCode(StatusCodes.Status404NotFound, $"No Record Found against this Id {id}.");

           entity.Title = quote.Title;
           entity.Description = quote.Description;
           entity.Author = quote.Author;
           entity.Author = quote.Type;
           entity.CreatedAt = quote.CreatedAt;
           _quotesDbContext.SaveChanges();
           return StatusCode(StatusCodes.Status201Created, entity);
        }

        // DELETE api/<QuotesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
             var entity = _quotesDbContext.Quotes.Find(id);
             if (entity == null)
                 return StatusCode(StatusCodes.Status404NotFound, $"No Record Found against this Id {id}.");

            _quotesDbContext.Quotes.Remove(entity);
            _quotesDbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created, "The Quote has been deleted.");
        }
    }
}
