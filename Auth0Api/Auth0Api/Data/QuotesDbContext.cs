using Auth0Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth0Api.Data
{
    public class QuotesDbContext : DbContext
    {
        public QuotesDbContext(DbContextOptions<QuotesDbContext>? options) : base(options) { }

        public DbSet<Quote> Quotes { get; set; }
    }
}
