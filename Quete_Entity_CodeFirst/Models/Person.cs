namespace Quete_Entity_CodeFirst.Models
{
    public class Person
    {
        public Guid PersonId { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<SavingAccount> SavingAccounts { get; set; } = new List<SavingAccount>();
    }
}
