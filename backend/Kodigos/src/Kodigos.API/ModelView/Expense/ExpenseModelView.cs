using Kodigos.Dominio.ModelsData.Expense;

namespace Kodigos.API.ModelView.Filters
{
    public class ExpenseModelView
    {
       public List<ExpenseDTO> expenses { get; set; }

        public double sumExpense { get; set; } = 0;
        public double sumExpensePaid { get; set; } = 0;
        public double sumExpenseUnpaid { get; set; } = 0;
        public double sumExpenseVariable { get; set; } = 0;
        public double sumExpenseFixed { get; set; } = 0;


    }
}
