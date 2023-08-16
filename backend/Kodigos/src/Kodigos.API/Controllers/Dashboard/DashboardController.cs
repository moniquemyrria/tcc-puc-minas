using System;
using System.Linq;
using Kodigos.API.ModelView.Filters;
using Kodigos.Infra.Core;
using Kodigos.Infra.Data.Contexto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;
using Kodigos.Dominio.ModelsData.Expense;
using System.Data.SqlClient;
using Dapper;

namespace Kodigos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly KodigosContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(KodigosContext context, IConfiguration configuration, ILogger<DashboardController> logger)
        {
            _configuration = configuration;
            _context = context;
            _logger = logger;
        }

        [HttpPost("TotalizersCards")]
        public async Task<KRetorno<DashboardTotalizersCardsModelView>> GetExpense(FilterDashboardModelView filters)
        {


            var result = new KRetorno<DashboardTotalizersCardsModelView>();
            var dashboard = new DashboardTotalizersCardsModelView();

            try
            {

                DateTime dtInicial = filters.dateInitial.Date;
                DateTime dtFinal = filters.dateFinal.Date;

                double sumTotalExpense = GetSumExpense(filters.idUser, dtInicial, dtFinal);
                double sumTotalRevenue = GetSumRevenue(filters.idUser, dtInicial, dtFinal);


                double sumTotalBalance = 0;

                if (dtInicial.Month > 1)
                {
                    var balanceTotal = await GetBalanceTotal(dtInicial.Year);

                    if (balanceTotal.Sucesso && balanceTotal.TRetorno != null)
                    {

                        sumTotalBalance = balanceTotal.TRetorno.Sum(t => t.balance);
                    }
                }
                else
                {
                    var balanceTotal = await GetBalanceTotal(dtInicial.Year - 1);

                    if (balanceTotal.Sucesso && balanceTotal.TRetorno != null)
                    {

                        sumTotalBalance = balanceTotal.TRetorno.Sum(t => t.balance);
                    }
                }

                
                

                dashboard.sumTotalExpense = sumTotalExpense.ToString("N2");
                dashboard.sumTotalRevenue = sumTotalRevenue.ToString("N2");
                dashboard.sumTotalBalance = sumTotalBalance.ToString("N2");

                result.Sucesso = true;
                result.TRetorno = dashboard;


            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados. Erro: " + e.Message;
            }

            return result;
        }

        private double GetSumRevenue(string idUser, DateTime dtInicial, DateTime dtFinal)
        {
            double sumTotalRevenue = 0;

            var listRevenue = _context
                                .Revenue
                                .Where(t =>
                                    t.user.Id == idUser &&
                                    t.revenueStatus == "Recebido" &&
                                    t.isActive &&
                                    t.dateCreated.Date >= dtInicial && t.dateCreated.Date <= dtFinal
                                )
                                .ToList();
            listRevenue.ForEach(t => sumTotalRevenue += t.value);
            return sumTotalRevenue;
        }

        private double GetSumExpense(string idUser, DateTime dtInicial, DateTime dtFinal)
        {
            var listExpense = new List<ExpenseDTO>();
            double sumTotalExpense = 0;
            var expenses = _context
                            .Expense
                            .Where(t => t.isActive && t.user.Id == idUser)
                            .Include(t => t.expenseCategory)
                            .Include(t => t.supplier)
                            .Include(t => t.account)
                            .ToList();

            foreach (var expense in expenses)
            {
                var installments = _context.ExpenseInstallments.Where(t => t.isPaid && t.expense.Id == expense.Id && t.dueDate.Date >= dtInicial && t.dueDate.Date <= dtFinal).ToList();

                installments.ForEach(t => sumTotalExpense += t.value);

            }

            return sumTotalExpense;
        }

        [HttpPost("TotalizersChartExpenseCategory")]
        public async Task<KRetorno<DashboardChartExpenseModelView>> GetChatExpense(FilterDashboardModelView filters)
        {


            var result = new KRetorno<DashboardChartExpenseModelView>();
            var dashboard = new DashboardChartExpenseModelView();

            try
            {

                DateTime dtInicial = filters.dateInitial.Date;
                DateTime dtFinal = filters.dateFinal.Date;

                var totalizersExpenseCategory = GetListTotalizersExpenseCategory(filters.idUser, dtInicial, dtFinal);
                dashboard.categories = totalizersExpenseCategory.Item1;
                dashboard.series = totalizersExpenseCategory.Item2;
                dashboard.colors = totalizersExpenseCategory.Item3;


                result.Sucesso = true;
                result.TRetorno = dashboard;


            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados. Erro: " + e.Message;
            }

            return result;
        }

        private (List<string[]>, List<double>, List<string>) GetListTotalizersExpenseCategory(string idUser, DateTime dtInicial, DateTime dtFinal)
        {
            var listExpenseCategoryAndExpenseId = new List<(string, long)>();
            var listExpenseCategory = new List<string[]>();
            var listExpenseCategoryTemp = new List<string>();
            var listExpenseCategoryColors = new List<string>();
            var listExpenseCategoryValue = new List<double>();
            var expenses = _context
                            .Expense
                            .Where(t => t.isActive && t.user.Id == idUser)
                            .Include(t => t.expenseCategory)
                            .Include(t => t.supplier)
                            .Include(t => t.account)
                            .ToList();

            foreach (var expense in expenses)
            {
                var installments = _context.ExpenseInstallments.Where(t => t.isPaid && t.expense.Id == expense.Id && t.dueDate.Date >= dtInicial && t.dueDate.Date <= dtFinal).ToList();

                installments.ForEach(t => listExpenseCategoryAndExpenseId.Add(new(expense.expenseCategory.description, expense.Id)));


                foreach (var item in installments)
                {
                    if (listExpenseCategoryTemp.FirstOrDefault(t => t == item.expense.expenseCategory.description) == null)
                    {
                        listExpenseCategoryTemp.Add(item.expense.expenseCategory.description);
                        string[] expenseCategory = item.expense.expenseCategory.description.Split(' ');

                        listExpenseCategory.Add(expenseCategory);
                        listExpenseCategoryColors.Add(item.expense.expenseCategory.color);
                    }

                };

            }

            foreach (var category in listExpenseCategoryTemp)
            {
                var listItemsExpenseCategory = listExpenseCategoryAndExpenseId.Where(t => t.Item1 == category);

                double sumCategory = 0;
                foreach (var item in listItemsExpenseCategory)
                {

                    var installment = _context.ExpenseInstallments.FirstOrDefault(t => t.isPaid && t.expense.Id == item.Item2 && t.dueDate.Date >= dtInicial && t.dueDate.Date <= dtFinal);

                    if (installment != null)
                    {
                        sumCategory = sumCategory + installment.value;
                    }

                }

                double sumValue = Math.Round(sumCategory, 2);

                listExpenseCategoryValue.Add(sumValue);

            }

            return (listExpenseCategory, listExpenseCategoryValue, listExpenseCategoryColors);

        }



        [HttpPost("TotalizersChartRevenueCategory")]
        public async Task<KRetorno<DashboardChartRevenueModelView>> GetChatRevenue(FilterDashboardModelView filters)
        {


            var result = new KRetorno<DashboardChartRevenueModelView>();
            var dashboard = new DashboardChartRevenueModelView();

            try
            {
                DateTime dtInicial = filters.dateInitial.Date;
                DateTime dtFinal = filters.dateFinal.Date;

                var totalizersExpenseCategory = GetListTotalizersRevenueCategory(filters.idUser, dtInicial, dtFinal);
                dashboard.labels = totalizersExpenseCategory.Item1;
                dashboard.series = totalizersExpenseCategory.Item2;
                dashboard.colors = totalizersExpenseCategory.Item3;

                result.Sucesso = true;
                result.TRetorno = dashboard;


            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados. Erro: " + e.Message;
            }

            return result;
        }

        private (List<string>, List<double>, List<string>) GetListTotalizersRevenueCategory(string idUser, DateTime dtInicial, DateTime dtFinal)
        {
            var listRevenueCategoryAndRevenueId = new List<(string, long)>();
            var listRevenueCategory = new List<string>();
            var listRevenueCategoryColors = new List<string>();
            var listRevenueCategoryValue = new List<double>();

            var listRevenue = _context
                                .Revenue
                                .Where(t =>
                                    t.user.Id == idUser &&
                                    t.revenueStatus == "Recebido" &&
                                    t.isActive &&
                                    t.dateCreated.Date >= dtInicial && t.dateCreated.Date <= dtFinal
                                )
                                .Include(t => t.revenueCategory)
                                .ToList();

            listRevenue.ForEach(t => listRevenueCategoryAndRevenueId.Add(new(t.revenueCategory.description, t.Id)));

            foreach (var revenue in listRevenue)
            {

                if (listRevenueCategory.FirstOrDefault(t => t == revenue.revenueCategory.description) == null)
                {
                    listRevenueCategory.Add(revenue.revenueCategory.description);

                    listRevenueCategoryColors.Add(revenue.revenueCategory.color);
                }

            }

            foreach (var category in listRevenueCategory)
            {
                var listItemsRevenueCategory = listRevenueCategoryAndRevenueId.Where(t => t.Item1 == category).ToList();

                double sumCategory = 0;
                foreach (var item in listItemsRevenueCategory)
                {

                    var revenue = listRevenue.FirstOrDefault(t => t.isActive && t.Id == item.Item2);

                    if (revenue != null)
                    {
                        sumCategory = sumCategory + revenue.value;
                    }

                }

                double sumValue = Math.Round(sumCategory, 2);

                listRevenueCategoryValue.Add(sumCategory);

            }


            /*foreach (var category in listRevenueCategoryId)
            {
                double sumCategory = 0;

                var revenueCategory = listRevenue.FirstOrDefault(t => t.Id == category.Item2);
                if (revenueCategory.revenueCategory.description == category.Item1)
                {
                    sumCategory = sumCategory + revenueCategory.value;
                }

                listRevenueCategoryValue.Add(sumCategory);
            }*/

            return (listRevenueCategory, listRevenueCategoryValue, listRevenueCategoryColors);
        }

        [HttpGet("FinanceBalance/year")]
        public async Task<KRetorno<List<DashboardFinanceBalanceModelView>>> GetBalanceTotal(int year)
        {

            var result = new KRetorno<List<DashboardFinanceBalanceModelView>>();


            string sql = $@"
                SELECT 
	                CONVERT(datetime, CONVERT(varchar(4),SUBSTRING(periodo, 4,9))+Right(SUBSTRING(periodo, 0,3),2)+'01', 102) [dateInicial], 
	                DATEADD(DD, - DAY (DATEADD(M, 1, CONVERT(datetime, CONVERT(varchar(4),SUBSTRING(periodo, 4,9))+Right(SUBSTRING(periodo, 0,3),2)+'01', 102))), DATEADD(M, 1, CONVERT(datetime, CONVERT(varchar(4),SUBSTRING(periodo, 4,9))+Right(SUBSTRING(periodo, 0,3),2)+'01', 102))) [dateFinal],
	                periodo [period], isNull(col1,0) [expense], isNull(col2,0) [revenue], isNull((col2 - col1),0) [balance]
                FROM (
	                SELECT periodo, valor, 
	                CASE 
		                WHEN tipo = 'Receita'THEN 'Col2' -- + CAST(ROW_NUMBER() OVER(PARTITION BY periodo ORDER BY tipo) AS VARCHAR(10))
		                WHEN tipo = 'Despesa'THEN 'Col1' -- + CAST(ROW_NUMBER() OVER(PARTITION BY periodo ORDER BY tipo) AS VARCHAR(10))
	                END AS Coluna
	                FROM (
		                SELECT 
			                (CASE WHEN DATEPART(MONTH, G.data) < 10 THEN '0' + CAST(DATEPART(MONTH, G.data) AS VARCHAR(2)) ELSE CAST(DATEPART(MONTH, G.data) AS VARCHAR(2)) END) + '-' + CAST(DATEPART(YEAR, G.data) AS VARCHAR(4)) 
				                AS [periodo],
			                SUM(G.value) 
				                AS [valor],
			                tipo
		                FROM (	
			                SELECT dateCreated [data], value, 'Receita' [tipo] FROM Revenue WITH(NOLOCK)
			                WHERE revenueStatus = 'Recebido' AND isActive = 1
	
			                UNION ALL
			                SELECT paymentDate [data], value, 'Despesa' [tipo] from [Expense.Installments]
			                INNER JOIN Expense ON (Expense.Id = [Expense.Installments].expenseId)
			                WHERE paymentDate IS NOT NULL AND isActive = 1

		                ) AS G
		                GROUP BY (CASE WHEN DATEPART(MONTH, G.data) < 10 THEN '0' + CAST(DATEPART(MONTH, G.data) AS VARCHAR(2)) ELSE CAST(DATEPART(MONTH, G.data) AS VARCHAR(2)) END) + '-' + CAST(DATEPART(YEAR, G.data) AS VARCHAR(4)), G.tipo
	                ) as T
                ) AS SourceTable
                PIVOT (
                    MAX(valor)
                    FOR  coluna in (col1, col2)
                ) as P
                WHERE SUBSTRING(P.periodo, 4,9) = '{ year }' 
            ";

            try
            {

                List<DashboardFinanceBalanceModelView> listBalances = new List<DashboardFinanceBalanceModelView>();
                using SqlConnection conexao = new SqlConnection(
                    _configuration.GetConnectionString("DefaultConnection"));
                listBalances = conexao.Query<DashboardFinanceBalanceModelView>(sql, commandTimeout: 600).ToList();

                result.Sucesso = true;
                result.TRetorno = listBalances;

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados da consulta. Erro: " + e.Message;
            }

            return result;
        }
    }
}
