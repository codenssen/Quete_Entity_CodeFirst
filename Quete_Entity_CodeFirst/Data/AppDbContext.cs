using Microsoft.EntityFrameworkCore;
using Quete_Entity_CodeFirst.Models;

namespace Quete_Entity_CodeFirst.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }
        public DbSet<Person> Persons { get; set; }
        public DbSet<SavingAccount> SavingAccounts { get; set; }
    }
}
