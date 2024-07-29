using Microsoft.EntityFrameworkCore;
using Quete_Entity_CodeFirst.Data;
using Quete_Entity_CodeFirst.Models;
using System.Collections.Generic;
using System.Text;

namespace Quete_Entity_CodeFirst
{
    public class Program
    {

        public class SavingCalculator
        {
            public void CalculateSavings(Person person)
            {
                Console.WriteLine($"Savings for {person.Name}:");
                foreach (var account in person.SavingAccounts)
                {
                    decimal totalAmount = account.InitialAmount;
                    if (account.IsMonthly)
                    {
                        for (int i = 0; i < 36; i++)
                        {
                            totalAmount += totalAmount * (decimal)(account.MonthlyRate / 100.0);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            totalAmount += totalAmount * (decimal)(account.AnnualRate / 100.0);
                        }
                    }
                    Console.WriteLine($"Account with initial amount {account.InitialAmount:C} and " +
                                      $"{(account.IsMonthly ? "monthly rate" : "annual rate")} of " +
                                      $"{(account.IsMonthly ? account.MonthlyRate : account.AnnualRate)}%: " +
                                      $"{totalAmount:C}");
                }
            }
        }


        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Setup DI
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection, configuration);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Run database operations
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Ensure database is created
                context.Database.EnsureCreated();

                // Add a person with saving accounts
                var person = new Person
                {
                    PersonId = Guid.NewGuid(),
                    Name = "John Doe",
                    SavingAccounts = new List<SavingAccount>
                    {
                        new SavingAccount
                        {
                            SavingAccountId = Guid.NewGuid(),
                            InitialAmount = 1000m,
                            MonthlyRate = 1.0,
                            AnnualRate = 0.0,   
                            IsMonthly = true
                        },
                        new SavingAccount
                        {
                            SavingAccountId = Guid.NewGuid(),
                            InitialAmount = 5000m,
                            MonthlyRate = 0.0,
                            AnnualRate = 5.0,
                            IsMonthly = false
                        }
                    }
                };
                context.Persons.Add(person);
                context.SaveChanges();

                // Retrieve and display data
                var persons = context.Persons.Include(p => p.SavingAccounts).ToList();
                var calculator = new SavingCalculator();
                foreach (var p in persons)
                {
                    Console.WriteLine($"ID: {p.PersonId}, Name: {p.Name}");
                    calculator.CalculateSavings(p);
                }
            }
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
