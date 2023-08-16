
using Kodigos.API.ModelView.Filters;
using Kodigos.Dominio.ModelsData.Expense;
using Kodigos.Infra.Core;
using Kodigos.Infra.Data.Contexto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kodigos.API.Controllers.Administration.Expense
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly KodigosContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ExpenseController> _logger;

        public ExpenseController(KodigosContext context, IConfiguration configuration, ILogger<ExpenseController> logger)
        {
            _configuration = configuration;
            _context = context;
            _logger = logger;

        }

        [HttpGet("{idUser}")]
        public async Task<KRetorno<ExpenseModelView>> GetExpense(string idUser)
        {

            var listExpense = new ExpenseModelView();
            var result = new KRetorno<ExpenseModelView>();

            try
            {
                listExpense.expenses = _context
                                .Expense
                                .Include(t => t.expenseCategory)
                                .Include(t => t.supplier)
                                .Include(t => t.account)
                                .Include(t => t.paymentMethods)
                                .Where(t => t.user.Id == idUser)
                                .ToList();
                listExpense.expenses.ForEach(t => t.dateCreatedFormat = t.dateCreated.ToString("dd/MM/yyyy"));
                listExpense.sumExpense = listExpense.expenses.Where(e => e.isActive).Sum(e => e.amount);
                listExpense.sumExpensePaid = listExpense.expenses.Where(e => e.isActive && e.expenseStatus == "Pago").Sum(e => e.amount);
                listExpense.sumExpenseUnpaid = listExpense.expenses.Where(e => e.isActive && e.expenseStatus == "A Pagar").Sum(e => e.amount);
                listExpense.sumExpenseVariable = listExpense.expenses.Where(e => e.isActive && e.expenseType == "V").Sum(e => e.amount);
                listExpense.sumExpenseFixed = listExpense.expenses.Where(e => e.isActive && e.expenseType == "F").Sum(e => e.amount);


                result.Sucesso = true;
                result.TRetorno = listExpense;

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados da listagem das Despesas. Erro: " + e.Message;
            }

            return result;
        }

        [HttpPost("Filters/{idUser}")]
        public async Task<KRetorno<ExpenseModelView>> PostExpenseFilters(string idUser, ExpenseFilterDTO filters)
        {

            var result = new KRetorno<ExpenseModelView>();
            var expenseModelView = new ExpenseModelView();

            try
            {
                List<ExpenseDTO> listExpense = new List<ExpenseDTO>();

                //apenas categoria
                listExpense = FilterExpenseCategory(idUser, filters, listExpense);

                //apenas tipo de pessoa
                listExpense = FilterTypePerson(idUser, filters, listExpense);

                //apenas conta
                listExpense = FilterAccount(idUser, filters, listExpense);

                //periodo de date de criação da despesa
                listExpense = FilterPeriodDate(idUser, filters, listExpense);


                foreach (var item in listExpense)
                {
                    if (item.expenseStatus == "Pago")
                    {
                        var itemPayment = _context.ExpensePaymentMethodsTypes.FirstOrDefault(t => t.expense.Id == item.Id);

                        if (itemPayment != null)
                        {
                            item.paymentDateFormat = itemPayment.paymentDate.ToString("dd/MM/yyyy");
                        }
                    }

                    item.dateCreatedFormat = item.dateCreated.ToString("dd/MM/yyyy");
                    item.dueDateFormat = item.dueDate.ToString("dd/MM/yyyy");
                }


                listExpense = filters.expenseStatus == "Todas" ? listExpense : listExpense.Where(t => t.expenseStatus == filters.expenseStatus).ToList();

                expenseModelView.expenses = listExpense.OrderBy(t => t.dateCreated).ToList();
                expenseModelView.sumExpense = listExpense.Where(e => e.isActive).Sum(e => e.valueInstallment);
                expenseModelView.sumExpensePaid = listExpense.Where(e => e.isActive && e.expenseStatus == "Pago").Sum(e => e.valueInstallment);
                expenseModelView.sumExpenseUnpaid = listExpense.Where(e => e.isActive && e.expenseStatus == "A Pagar").Sum(e => e.valueInstallment);
                expenseModelView.sumExpenseVariable = listExpense.Where(e => e.isActive && e.expenseType == "V").Sum(e => e.valueInstallment);
                expenseModelView.sumExpenseFixed = listExpense.Where(e => e.isActive && e.expenseType == "F").Sum(e => e.valueInstallment);
                result.Sucesso = true;
                result.TRetorno = expenseModelView;

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados da listagem de Despesas. Erro: " + e.Message;
            }

            return result;

            //Filters
            List<ExpenseDTO> FilterExpenseCategory(string idUser, ExpenseFilterDTO filters, List<ExpenseDTO> listExpense)
            {
                if (listExpense.Count() == 0 && filters.expenseCategory != null)
                {

                    var expenses = _context
                               .Expense
                               .Where(t => t.user.Id == idUser && t.expenseCategory.Id == filters.expenseCategory.Id)
                               .Include(t => t.expenseCategory)
                               .Include(t => t.supplier)
                               .Include(t => t.account)
                               .ToList();

                    ExpenseInstallments(idUser, listExpense, expenses);
                }

                return listExpense;
            }

            List<ExpenseDTO> FilterTypePerson(string idUser, ExpenseFilterDTO filters, List<ExpenseDTO> listExpense)
            {
                if (listExpense.Count() == 0 && filters.typePerson != null)
                {
                    var expenses = _context
                               .Expense
                               .Where(t => t.user.Id == idUser && t.supplier.Id == filters.typePerson.Id)
                               .Include(t => t.expenseCategory)
                               .Include(t => t.supplier)
                               .Include(t => t.account)
                               .ToList();

                    ExpenseInstallments(idUser, listExpense, expenses);

                }
                else if (listExpense.Count() > 0 && filters.typePerson != null)
                {
                    listExpense = listExpense.Where(t => t.supplier.Id == filters.typePerson.Id).ToList();
                }
                return listExpense;
            }

            List<ExpenseDTO> FilterAccount(string idUser, ExpenseFilterDTO filters, List<ExpenseDTO> listExpense)
            {
                if (listExpense.Count() == 0 && filters.account != null)
                {
                    var expenses = _context
                               .Expense
                               .Where(t => t.user.Id == idUser && t.account.Id == filters.account.Id)
                               .Include(t => t.expenseCategory)
                               .Include(t => t.supplier)
                               .Include(t => t.account)
                               .ToList();

                    ExpenseInstallments(idUser, listExpense, expenses);
                }
                else if (listExpense.Count() > 0 && filters.account != null)
                {
                    listExpense = listExpense.Where(t => t.account.Id == filters.account.Id).ToList();

                }
                return listExpense;
            }

            List<ExpenseDTO> FilterPeriodDate(string idUser, ExpenseFilterDTO filters, List<ExpenseDTO> listExpense)
            {

                if (listExpense.Count() == 0 && filters.dateInitial != null && filters.dateFinal != null)
                {

                    DateTime dtInicial = filters.dateInitial.Value.Date;
                    DateTime dtFinal = filters.dateFinal.Value.Date;

                    var expenses = _context
                        .Expense
                        .Where(t => t.user.Id == idUser)
                        .Include(t => t.expenseCategory)
                        .Include(t => t.supplier)
                        .Include(t => t.account)
                        .ToList();

                    ExpenseInstallmentsPeriod(idUser, listExpense, expenses, dtInicial, dtFinal);


                }
                else if (listExpense.Count() > 0 && filters.dateInitial != null && filters.dateFinal != null)
                {
                    string format = "dd/MM/yyyy";

                    DateTime dtInicial = filters.dateInitial.Value.Date;
                    DateTime dtFinal = filters.dateFinal.Value.Date;
                    //ExpenseInstallmentsPeriod(idUser, listExpense, listExpense, dtInicial, dtFinal);

                    var listExpenseAux = listExpense.Where(t => t.idUser == idUser && t.dueDate.Date >= dtInicial && t.dueDate.Date <= dtFinal).ToList();

//
                    listExpense = listExpenseAux;
                }

                return listExpense;
            }
        }

        private void ExpenseInstallments(string idUser, List<ExpenseDTO> listExpense, List<ExpenseDTO> expenses)
        {
            foreach (var expense in expenses)
            {
                var installments = _context.ExpenseInstallments.Where(t => t.expense.Id == expense.Id).ToList();

                if (installments.Count() > 0)
                {

                    foreach (var installment in installments)
                    {
                        AddExpenseInstallments(idUser, listExpense, expense, installment);
                    }
                }
            }
        }

        private void ExpenseInstallmentsPeriod(string idUser, List<ExpenseDTO> listExpense, List<ExpenseDTO> expenses, DateTime dtInicial, DateTime dtFinal)
        {
            foreach (var expense in expenses)
            {
                var installments = _context.ExpenseInstallments.Where(t => t.expense.Id == expense.Id && t.dueDate.Date >= dtInicial && t.dueDate.Date <= dtFinal).ToList();

                if (installments.Count() > 0)
                {

                    foreach (var installment in installments)
                    {
                        AddExpenseInstallments(idUser, listExpense, expense, installment);
                    }
                }
            }
        }

        private static void AddExpenseInstallments(string idUser, List<ExpenseDTO> listExpense, ExpenseDTO expense, ExpenseInstallmentsDTO installment)
        {
            listExpense.Add(new ExpenseDTO
            {
                Id                          = expense.Id,
                idUser                      = idUser,
                idAccount                   = expense.account.Id,
                idExpenseCategory           = expense.expenseCategory.Id,
                idTypePerson                = expense.supplier.Id,
                account                     = expense.account,
                expenseCategory             = expense.expenseCategory,
                supplier                    = expense.supplier,
                amount                      = expense.amount,
                dateCreated                 = expense.dateCreated,
                dateUpdate                  = expense.dateUpdate,
                daysBetweenInstallments     = expense.daysBetweenInstallments,
                description                 = expense.description,
                dueDate                     = installment.dueDate,
                expensePaymentMethodsTypes  = expense.expensePaymentMethodsTypes,
                expenseStatus               = installment.isPaid ? "Pago" : "A Pagar",
                expenseType                 = expense.expenseType,
                isActive                    = expense.isActive,
                numberInstallments          = expense.numberInstallments,
                obs                         = expense.obs,
                typeInstallment             = expense.typeInstallment,
                //installments
                idInstallment               = installment.Id,
                installmentDescription      = installment.installmentDescription,
                valueInstallment            = installment.value,
                isPaid                      = installment.isPaid,

            });
        }

        [HttpGet("Edit/{id}")]
        public async Task<KRetorno<ExpenseDTO>> GetExpenseId(long id)
        {

            var result = new KRetorno<ExpenseDTO>();

            try
            {
                if (ExpenseIdExists(id))
                {
                    ExpenseDTO expense = _context
                                            .Expense
                                            .Include(t => t.expenseCategory)
                                            .Include(t => t.supplier)
                                            .Include(t => t.paymentMethods)
                                            .ThenInclude(t => t.paymentMethodsTypes)
                                            .Include(t => t.account)
                                            .First(t => t.Id == id);

                    var expenseInstallments = _context.ExpenseInstallments.Where(t => t.expense.Id == expense.Id).ToList();

                    if (expenseInstallments.Any())
                    {
                        expense.expenseInstallments = new List<ExpenseInstallmentsDTO>();
                        expenseInstallments.ForEach(t => t.dueDateFormatter = t.dueDate.ToString("dd/MM/yyyy"));
                        expenseInstallments.ForEach(t => t.paymentDateFormatter = t.paymentDate?.ToString("dd/MM/yyyy"));
                        expenseInstallments.ForEach(t => t.valueFormatter = t.value.ToString("N2"));

                        expense.expenseInstallments = expenseInstallments;
                    }

                    if (expense.paymentMethods != null && expense.paymentMethods.paymentMethodsTypes.Count() > 0)
                    {
                        var expensePaymentMethodsTypes = _context
                                                            .ExpensePaymentMethodsTypes
                                                            .Where(t => t.expense.Id == t.expense.Id && t.paymentMethods.Id == expense.paymentMethods.Id)
                                                            .ToList();

                        if (expensePaymentMethodsTypes.Any())
                        {
                            foreach (var payMethodTypes in expense.paymentMethods.paymentMethodsTypes)
                            {
                                var item = expensePaymentMethodsTypes.FirstOrDefault(t => t.paymentMethodsTypes.Id == payMethodTypes.Id);

                                if (item != null)
                                {
                                    payMethodTypes.taxRate = item.taxRate;
                                    payMethodTypes.totalPayment = item.totalPayment;
                                    payMethodTypes.trafficTicket = item.trafficTicket;
                                    payMethodTypes.value = item.value;
                                }
                            }
                        }
                    }




                    result.Sucesso = true;
                    result.TRetorno = expense;
                }
                else
                {
                    result.Sucesso = false;
                    result.Message = "Despesa  não localizada.";
                }


            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados da Despesa. Erro: " + e.Message;
            }

            return result;
        }


        [HttpPost("{idUser}")]
        public async Task<KRetorno<ExpenseDTO>> PostExpense(string idUser, ExpenseDTO newExpense)
        {

            var result = new KRetorno<ExpenseDTO>();

            try
            {

                var expense = new ExpenseDTO();

                var user = _context.Users.First(t => t.Id == newExpense.idUser);
                var account = _context.Account.First(t => t.Id == newExpense.idAccount);
                var expenseCategory = _context.ExpenseCategory.First(t => t.Id == newExpense.idExpenseCategory);
                var paymentMethod = newExpense.idPaymentMethods != 0 ? _context.PaymentMethods.First(t => t.Id == newExpense.idPaymentMethods) : null;
                var typePerson = _context.Supplier.First(t => t.Id == newExpense.idTypePerson);

                expense.user = user;
                expense.account = account;
                expense.expenseCategory = expenseCategory;
                expense.supplier = typePerson;
                if (paymentMethod != null)
                {
                    expense.paymentMethods = paymentMethod;
                }

                expense.amount = newExpense.amount;
                expense.description = newExpense.description;
                expense.obs = newExpense.obs;
                expense.daysBetweenInstallments = newExpense.daysBetweenInstallments;
                expense.dueDate = newExpense.dueDate;
                expense.expenseStatus = newExpense.expenseStatus;
                expense.numberInstallments = newExpense.numberInstallments;
                expense.typeInstallment = newExpense.typeInstallment;
                expense.expenseType = newExpense.expenseType;
                expense.isActive = newExpense.isActive;
                expense.dateCreated = newExpense.dateCreated;

                _context.Expense.Add(expense);

                if (newExpense.expenseInstallments.Count() > 0)
                {

                    foreach (var inst in newExpense.expenseInstallments)
                    {
                        var newInstallment = new ExpenseInstallmentsDTO
                        {
                            dueDate = inst.dueDate,
                            installment = inst.installment,
                            installmentDescription = inst.installmentDescription,
                            value = inst.value,
                            isPaid = inst.isPaid,
                            paymentDate = inst.paymentDate,
                            expense = expense
                        };

                        _context.ExpenseInstallments.Add(newInstallment);


                        if (newExpense.expenseStatus == "Pago" && newExpense.expenseInstallments.Count() == 1)
                        {
                            var expensePaymentMethodsTypes = new List<ExpensePaymentMethodsTypesDTO>();

                            foreach (var item in newExpense.paymentMethods.paymentMethodsTypes)
                            {
                                var paymentMethodsTypes = _context.PaymentMethodsTypes.First(t => t.Id == item.Id);

                                var itemExpensePaymentMethodsTypes = new ExpensePaymentMethodsTypesDTO
                                {
                                    taxRate = item.taxRate != null ? item.taxRate : 0,
                                    totalPayment = item.totalPayment != null ? item.totalPayment : 0,
                                    trafficTicket = item.trafficTicket != null ? item.trafficTicket : 0,
                                    value = item.value != null ? item.value : 0,
                                    paymentDate = DateTime.Now.Date,
                                    expense = expense,
                                    paymentMethods = paymentMethod,
                                    paymentMethodsTypes = paymentMethodsTypes,
                                    expenseInstallments = newInstallment
                                };

                                _context.ExpensePaymentMethodsTypes.Add(itemExpensePaymentMethodsTypes);

                            };

                        }
                    }

                }

                _context.SaveChanges();

                result.Sucesso = true;
                result.TRetorno = expense;
                result.Message = "Nova Despesa cadastrada com sucesso.";



            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados das Despesas. Erro: " + e.Message;
            }

            return result;
        }

        [HttpPost("Payment")]
        public async Task<KRetorno> PostExpensePayment(ExpensePaymentDTO expensePayment)
        {
            var result = new KRetorno();

            try
            {
                var installment = _context.ExpenseInstallments.FirstOrDefault(t => t.Id == expensePayment.idInstallment);

                if (installment != null)
                {
                    installment.isPaid = true;
                    installment.paymentDate = expensePayment.paymentDate;

                    _context.Update(installment);

                    var expense = _context.Expense.FirstOrDefault(t => t.Id == expensePayment.idExpense);
                    var paymentMethods = _context.PaymentMethods.FirstOrDefault(t => t.Id == expensePayment.paymentMethods.Id);

                    if (expense != null && paymentMethods != null)
                    {
                        foreach (var item in expensePayment.paymentMethods.paymentMethodsTypes)
                        {
                            var paymentMethodsTypes = _context.PaymentMethodsTypes.First(t => t.Id == item.Id);

                            var itemExpensePaymentMethodsTypes = new ExpensePaymentMethodsTypesDTO
                            {
                                taxRate = item.taxRate != null ? item.taxRate : 0,
                                totalPayment = item.totalPayment != null ? item.totalPayment : 0,
                                trafficTicket = item.trafficTicket != null ? item.trafficTicket : 0,
                                value = item.value != null ? item.value : 0,
                                paymentDate = DateTime.Now.Date,
                                expense = expense,
                                paymentMethods = paymentMethods,
                                paymentMethodsTypes = paymentMethodsTypes,
                                expenseInstallments = installment
                            };

                            _context.ExpensePaymentMethodsTypes.Add(itemExpensePaymentMethodsTypes);

                        };
                    }

                    _context.SaveChanges();
                    result.Sucesso = true;
                    result.Message = "Despesa paga com sucesso.";

                }
                else
                {
                    result.Sucesso = false;
                    result.Message = "Não foi possível localizar a Parcela.";
                }

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível localizar a Parcela. Erro: " + e.Message;
            }


            return result;
        }

        [HttpPut("{id}")]
        public async Task<KRetorno<ExpenseDTO>> PutExpense(long id, ExpenseDTO updateExpense)
        {

            var result = new KRetorno<ExpenseDTO>();

            try
            {
                if (ExpenseIdExists(id))
                {
                    var expense = _context.Expense.First(t => t.Id == id);

                    var user = _context.Users.First(t => t.Id == updateExpense.idUser);
                    var account = _context.Account.First(t => t.Id == updateExpense.idAccount);
                    var expenseCategory = _context.ExpenseCategory.First(t => t.Id == updateExpense.idExpenseCategory);
                    var typePerson = _context.Supplier.First(t => t.Id == updateExpense.idTypePerson);

                    expense.user = user;
                    expense.account = account;
                    expense.expenseCategory = expenseCategory;
                    expense.supplier = typePerson;
                    expense.description = updateExpense.description;
                    expense.obs = updateExpense.obs;
                    expense.dateUpdate = updateExpense.dateUpdate;

                    _context.Update(expense);
                    _context.SaveChanges();

                    result.Sucesso = true;
                    result.TRetorno = expense;
                    result.Message = "Dados da Despesa alterada com sucesso.";

                }
                else
                {
                    result.Sucesso = false;
                    result.Message = "Despesa não localizada.";
                }

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados da Despesa. Erro: " + e.Message;
            }

            return result;
        }

        [HttpDelete("{id}")]
        public async Task<KRetorno<ExpenseDTO>> DeleteExpense(long id)
        {

            var result = new KRetorno<ExpenseDTO>();

            try
            {
                if (ExpenseIdExists(id))
                {
                    ExpenseDTO expense = _context.Expense.FirstOrDefault(t => t.Id == id);
                    if (expense != null)
                    {
                        expense.isActive = !expense.isActive;
                        _context.Entry(expense).State = EntityState.Modified;
                        _context.SaveChanges();

                        result.Sucesso = true;
                        result.TRetorno = expense;
                        result.Message = expense.isActive ? "A Despesa " + expense.description + " ativada com sucesso." : "A Despesa " + expense.description + " desativada com sucesso.";
                    }
                    else
                    {
                        result.Sucesso = false;
                        result.Message = "Despesa não localizada.";
                    }

                }
                else
                {
                    result.Sucesso = false;
                    result.Message = "Despesa não localizada.";
                }

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível desativar/ativar a Despesa . Erro: " + e.Message;
            }

            return result;
        }


        private bool ExpenseIdExists(long id)
        {
            return _context.Expense.Any(e => e.Id == id);
        }

        private bool ExpenseDescExists(string descricao, string idUser)
        {
            return _context.Expense.Any(e => e.description == descricao && e.user.Id == idUser);
        }





    }
}
