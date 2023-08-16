
using Kodigos.Dominio.ModelsData.Administration.AccountCategory;
using Kodigos.Infra.Core;
using Kodigos.Infra.Data.Contexto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kodigos.API.Controllers.Administration.AccountCategory
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountCategoryController : ControllerBase
    {
        private readonly KodigosContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountCategoryController> _logger;

        public AccountCategoryController(KodigosContext context, IConfiguration configuration, ILogger<AccountCategoryController> logger)
        {
            _configuration = configuration;
            _context = context;
            _logger = logger;

        }

        [HttpGet("{idUser}")]
        public async Task<KRetorno<List<AccountCategoryDTO>>> GetAccountCategory(string idUser)
        {

            List<AccountCategoryDTO> listAccountCategory = new List<AccountCategoryDTO>();
            var result = new KRetorno<List<AccountCategoryDTO>>();

            try
            {
                listAccountCategory = _context.AccountCategory.Where(t => t.user.Id == idUser).ToList();


                result.Sucesso = true;
                result.TRetorno = new List<AccountCategoryDTO>();
                result.TRetorno = listAccountCategory;

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados da listagem das categorias. Erro: " + e.Message;
            }

            return result;
        }

        [HttpGet("Status/{status}/{idUser}/")]
        public async Task<KRetorno<List<AccountCategoryDTO>>> GetStatusAccountCategory(bool status, string idUser)
        {

            List<AccountCategoryDTO> listAccountCategory = new List<AccountCategoryDTO>();
            var result = new KRetorno<List<AccountCategoryDTO>>();

            try
            {
                listAccountCategory = _context.AccountCategory.Where(t => t.user.Id == idUser && t.isActive == status).ToList();


                result.Sucesso = true;
                result.TRetorno = new List<AccountCategoryDTO>();
                result.TRetorno = listAccountCategory;

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados da listagem das categorias. Erro: " + e.Message;
            }

            return result;
        }

        [HttpGet("Edit/{id}")]
        public async Task<KRetorno<AccountCategoryDTO>> GetAccountCategoryId(long id)
        {

            var result = new KRetorno<AccountCategoryDTO>();

            try
            {
                if (AccountCategoryIdExists(id))
                {
                    AccountCategoryDTO accountCategoryDTO = _context.AccountCategory.FirstOrDefault(t => t.Id == id);

                    result.Sucesso = true;
                    result.TRetorno = accountCategoryDTO;
                }
                else
                {
                    result.Sucesso = false;
                    result.Message = "Catergoria  não localizada.";
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
        public async Task<KRetorno<AccountCategoryDTO>> PostAccountCategory(string idUser, AccountCategoryDTO accountCategory)
        {

            var result = new KRetorno<AccountCategoryDTO>();

            try
            {
                if (!AccountCategoryDescExists(accountCategory.description, idUser))
                {
                    var user = _context.Users.First(t => t.Id == accountCategory.idUser);

                    accountCategory.user = user;
                    _context.AccountCategory.Add(accountCategory);
                    _context.SaveChanges();

                    result.Sucesso = true;
                    result.TRetorno = accountCategory;
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
        public async Task<KRetorno<AccountCategoryDTO>> PutAccountCategory(long id, AccountCategoryDTO accountCategory)
        {

            var result = new KRetorno<AccountCategoryDTO>();

            try
            {
                if (AccountCategoryIdExists(id))
                {
                    _context.Update(accountCategory);
                    _context.SaveChanges();

                    result.Sucesso = true;
                    result.TRetorno = accountCategory;
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
        public async Task<KRetorno<AccountCategoryDTO>> DeleteAccountCategory(long id)
        {

            var result = new KRetorno<AccountCategoryDTO>();

            try
            {
                if (AccountCategoryIdExists(id))
                {
                    AccountCategoryDTO accountCategory = _context.AccountCategory.FirstOrDefault(t => t.Id == id);
                    if (accountCategory != null)
                    {
                        accountCategory.isActive = !accountCategory.isActive;
                        _context.Entry(accountCategory).State = EntityState.Modified;
                        _context.SaveChanges();

                        result.Sucesso = true;
                        result.TRetorno = accountCategory;
                        result.Message = accountCategory.isActive ? "A categoria " + accountCategory.description + " ativada com sucesso." : "A categoria " + accountCategory.description + " desativada com sucesso.";
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


        private bool AccountCategoryIdExists(long id)
        {
            return _context.AccountCategory.Any(e => e.Id == id);
        }

        private bool AccountCategoryDescExists(string descricao, string idUser)
        {
            return _context.AccountCategory.Any(e => e.description == descricao && e.user.Id == idUser);
        }





    }
}
