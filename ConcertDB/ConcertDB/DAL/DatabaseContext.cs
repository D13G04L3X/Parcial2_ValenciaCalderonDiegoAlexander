using ConcertDB.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConcertDB.DAL
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<Ticket> Ticket { get; set; }
        public object Database { get; internal set; }

        internal Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}