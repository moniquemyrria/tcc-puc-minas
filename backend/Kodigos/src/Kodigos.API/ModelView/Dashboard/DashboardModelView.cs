namespace Kodigos.API.ModelView.Filters
{
    //Totalizers Cards
    public class DashboardTotalizersCardsModelView
    {
        public string sumTotalExpense { get; set; }
        public string sumTotalRevenue { get; set; }
        public string sumTotalBalance { get; set; }
       

    }

    //Expense
    public class DashboardChartExpenseModelView
    {
        public List<string[]> categories { get; set; }
        public List<double> series { get; set; }
        public List<string> colors { get; set; }

    }

    //Expense
    public class DashboardChartRevenueModelView
    {
        public List<string> labels { get; set; }
        public List<double> series { get; set; }
        public List<string> colors { get; set; }

    }

    //Finance Balance
    public class DashboardFinanceBalanceModelView
    {
        public DateTime dateInicial { get; set; }
        public DateTime dateFinal { get; set; }
        public string period { get; set; }
        public double expense { get; set; }
        public double revenue { get; set; }
        public double balance { get; set; }

    }
}
