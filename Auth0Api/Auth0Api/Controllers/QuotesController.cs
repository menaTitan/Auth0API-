using Auth0Api.Data;
using Auth0Api.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Auth0Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotesController : ControllerBase
    {
        private readonly QuotesDbContext _quotesDbContext;


        public QuotesController(QuotesDbContext quotesDbContext)
        {
            _quotesDbContext = quotesDbContext;
        }

        // GET: api/<QuotesController>
        [HttpGet]
        public IActionResult Get()
        {
            return StatusCode(StatusCodes.Status200OK, _quotesDbContext.Quotes);
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
