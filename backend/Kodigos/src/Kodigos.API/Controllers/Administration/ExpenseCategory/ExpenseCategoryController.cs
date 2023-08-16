
using Kodigos.Dominio.ModelsData.Administration.ExpenseCategory;
using Kodigos.Infra.Core;
using Kodigos.Infra.Data.Contexto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kodigos.API.Controllers.Administration.ExpenseCategory
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseCategoryController : ControllerBase
    {
        private readonly KodigosContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ExpenseCategoryController> _logger;

        public ExpenseCategoryController(KodigosContext context, IConfiguration configuration, ILogger<ExpenseCategoryController> logger)
        {
            _configuration = configuration;
            _context = context;
            _logger = logger;

        }

        [HttpGet("{idUser}")]
        public async Task<KRetorno<List<ExpenseCategoryDTO>>> GetExpenseCategory(string idUser)
        {

            List<ExpenseCategoryDTO> listExpenseCategory = new List<ExpenseCategoryDTO>();
            var result = new KRetorno<List<ExpenseCategoryDTO>>();

            try
            {
                listExpenseCategory = _context.ExpenseCategory.Where(t => t.user.Id == idUser).ToList();


                result.Sucesso = true;
                result.TRetorno = new List<ExpenseCategoryDTO>();
                result.TRetorno = listExpenseCategory;

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados da listagem das categorias. Erro: " + e.Message;
            }

            return result;
        }

        [HttpGet("Status/{status}/{idUser}/")]
        public async Task<KRetorno<List<ExpenseCategoryDTO>>> GetStatusAccountCategory(bool status, string idUser)
        {

            List<ExpenseCategoryDTO> listExpenseCategory = new List<ExpenseCategoryDTO>();
            var result = new KRetorno<List<ExpenseCategoryDTO>>();

            try
            {
                listExpenseCategory = _context.ExpenseCategory.Where(t => t.user.Id == idUser && t.isActive == status).ToList();


                result.Sucesso = true;
                result.TRetorno = new List<ExpenseCategoryDTO>();
                result.TRetorno = listExpenseCategory;

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados da listagem das categorias. Erro: " + e.Message;
            }

            return result;
        }

        [HttpGet("Edit/{id}")]
        public async Task<KRetorno<ExpenseCategoryDTO>> GetExpenseCategoryId(long id)
        {

            var result = new KRetorno<ExpenseCategoryDTO>();

            try
            {
                if (ExpenseCategoryIdExists(id))
                {
                    ExpenseCategoryDTO expenseCategoryDTO = _context.ExpenseCategory.FirstOrDefault(t => t.Id == id);

                    result.Sucesso = true;
                    result.TRetorno = expenseCategoryDTO;
                }
                else
                {
                    result.Sucesso = false;
                    result.Message = "Categoria  não localizada.";
                }


            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados da Categoria. Erro: " + e.Message;
            }

            return result;
        }


        [HttpPost("{idUser}")]
        public async Task<KRetorno<ExpenseCategoryDTO>> PostExpenseCategory(string idUser, ExpenseCategoryDTO expenseCategory)
        {

            var result = new KRetorno<ExpenseCategoryDTO>();

            try
            {
                if (!ExpenseCategoryDescExists(expenseCategory.description, idUser))
                {
                    var user = _context.Users.First(t => t.Id == expenseCategory.idUser);

                    expenseCategory.user = user;
                    _context.ExpenseCategory.Add(expenseCategory);
                    _context.SaveChanges();

                    result.Sucesso = true;
                    result.TRetorno = expenseCategory;
                    result.Message = "Nova categoria cadastrada com sucesso.";

                }
                else
                {
                    result.Sucesso = false;
                    result.Message = "Já existe uma Categoria cadastrada com esta Descrição.";
                }

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados das Categorias. Erro: " + e.Message;
            }

            return result;
        }

        [HttpPut("{id}")]
        public async Task<KRetorno<ExpenseCategoryDTO>> PutExpenseCategory(long id, ExpenseCategoryDTO expenseCategory)
        {

            var result = new KRetorno<ExpenseCategoryDTO>();

            try
            {
                if (ExpenseCategoryIdExists(id))
                {
                    _context.Update(expenseCategory);
                    _context.SaveChanges();

                    result.Sucesso = true;
                    result.TRetorno = expenseCategory;
                    result.Message = "Dados da Categoria alterada com sucesso.";

                }
                else
                {
                    result.Sucesso = false;
                    result.Message = "Categoria não localizada.";
                }

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados da Categoria. Erro: " + e.Message;
            }

            return result;
        }

        [HttpDelete("{id}")]
        public async Task<KRetorno<ExpenseCategoryDTO>> DeleteExpenseCategory(long id)
        {

            var result = new KRetorno<ExpenseCategoryDTO>();

            try
            {
                if (ExpenseCategoryIdExists(id))
                {
                    ExpenseCategoryDTO expenseCategory = _context.ExpenseCategory.FirstOrDefault(t => t.Id == id);
                    if (expenseCategory != null)
                    {
                        expenseCategory.isActive = !expenseCategory.isActive;
                        _context.Entry(expenseCategory).State = EntityState.Modified;
                        _context.SaveChanges();

                        result.Sucesso = true;
                        result.TRetorno = expenseCategory;
                        result.Message = expenseCategory.isActive ? "A categoria " + expenseCategory.description + " ativada com sucesso." : "A categoria " + expenseCategory.description + " desativada com sucesso.";
                    }
                    else
                    {
                        result.Sucesso = false;
                        result.Message = "Categoria não localizada.";
                    }

                }
                else
                {
                    result.Sucesso = false;
                    result.Message = "Categoria não localizada.";
                }

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível desativar/ativar a Categoria . Erro: " + e.Message;
            }

            return result;
        }


        private bool ExpenseCategoryIdExists(long id)
        {
            return _context.ExpenseCategory.Any(e => e.Id == id);
        }

        private bool ExpenseCategoryDescExists(string descricao, string idUser)
        {
            return _context.ExpenseCategory.Any(e => e.description == descricao && e.user.Id == idUser);
        }





    }
}
