namespace Quete_Entity_CodeFirst.Models
{
    public class SavingAccount
    {
        public Guid SavingAccountId { get; set; }
        public decimal InitialAmount { get; set; }
        public double MonthlyRate { get; set; } // Taux mensuel
        public double AnnualRate { get; set; } // Taux annuel
        public bool IsMonthly { get; set; }
    }
}
