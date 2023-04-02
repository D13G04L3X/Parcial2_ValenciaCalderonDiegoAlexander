using ConcertDB.DAL.Entities;
using ConcertDB.DAL;

namespace ConcertDB.DAL
{
    public class SeederDb
    {
        private readonly DatabaseContext _context;
        public SeederDb(DatabaseContext context)
        {
            _context = context;
        }

        public async Task SeederAsync()             //It´s a possible solution, but in the moment realize test for code to SeederAsync
        {
            await _context.Database.EnsureCreatedAsync();
            await PopulTicketsAsync();
            await _context.SaveChangesAsync();
        }

        private async Task PopulTicketsAsync(){       //For register 50000 files

            if (!_context.Tickets.Any()){

                for (int i = 1; i <= 50000; i++){

                    _context.Tickets.Add(new Ticket { IsUsed = false });
                }
            }
        }
    }
}