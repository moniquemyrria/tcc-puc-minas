
using Kodigos.Dominio.ModelsData.Administration.Account;
using Kodigos.Infra.Core;
using Kodigos.Infra.Data.Contexto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kodigos.API.Controllers.Administration.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly KodigosContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountController> _logger;

        public AccountController(KodigosContext context, IConfiguration configuration, ILogger<AccountController> logger)
        {
            _configuration = configuration;
            _context = context;
            _logger = logger;

        }

        [HttpGet("{idUser}")]
        public async Task<KRetorno<List<AccountDTO>>> GetAccount(string idUser)
        {

            List<AccountDTO> listAccount = new List<AccountDTO>();
            var result = new KRetorno<List<AccountDTO>>();

            try
            {
                listAccount = _context
                                .Account
                                .Where(t => t.user.Id == idUser)
                                .Include(T => T.accountCategory)
                                .Include(t => t.typePerson)
                                .ToList();


                result.Sucesso = true;
                result.TRetorno = new List<AccountDTO>();
                result.TRetorno = listAccount;

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados da listagem das contas. Erro: " + e.Message;
            }

            return result;
        }

        [HttpGet("Status/{status}/{idUser}")]
        public async Task<KRetorno<List<AccountDTO>>> GetAccountStatus(bool status, string idUser)
        {

            List<AccountDTO> listAccount = new List<AccountDTO>();
            var result = new KRetorno<List<AccountDTO>>();

            try
            {
                listAccount = _context
                                .Account
                                .Where(t => t.user.Id == idUser && t.isActive == status)
                                .Include(T => T.accountCategory)
                                .Include(t => t.typePerson)
                                .ToList();


                result.Sucesso = true;
                result.TRetorno = new List<AccountDTO>();
                result.TRetorno = listAccount;

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados da listagem das contas. Erro: " + e.Message;
            }

            return result;
        }

        [HttpGet("Edit/{id}")]
        public async Task<KRetorno<AccountDTO>> GetAccountId(long id)
        {

            var result = new KRetorno<AccountDTO>();

            try
            {
                if (AccountIdExists(id))
                {
                    AccountDTO accountDTO = _context
                                                .Account
                                                .Include(t => t.accountCategory)
                                                .Include(t => t.typePerson)
                                                .Where(t => t.Id == id)
                                                .First();

                    result.Sucesso = true;
                    result.TRetorno = accountDTO;
                }
                else
                {
                    result.Sucesso = false;
                    result.Message = "Conta  não localizada.";
                }


            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados da Conta. Erro: " + e.Message;
            }

            return result;
        }


        [HttpPost("{idUser}")]
        public async Task<KRetorno<AccountDTO>> PostAccount(string idUser, AccountDTO account)
        {

            var result = new KRetorno<AccountDTO>();

            try
            {
                if (!AccountNameExists(account.name, idUser))
                {
                    var user = _context.Users.First(t => t.Id == account.idUser);
                    var accountCategory = _context.AccountCategory.First(t => t.Id == account.idAccountCategory);
                    var typePerson = _context.Supplier.First(t => t.Id == account.idTypePerson);

                    account.user = user;
                    account.accountCategory = accountCategory;
                    account.typePerson = typePerson;
                    _context.Account.Add(account);
                    _context.SaveChanges();

                    result.Sucesso = true;
                    result.TRetorno = account;
                    result.Message = "Nova conta cadastrada com sucesso.";

                }
                else
                {
                    result.Sucesso = false;
                    result.Message = "Já existe uma Conta cadastrada com este Nome.";
                }

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados das Conta. Erro: " + e.Message;
            }

            return result;
        }

        [HttpPut("{id}")]
        public async Task<KRetorno<AccountDTO>> PutAccount(long id, AccountDTO account)
        {

            var result = new KRetorno<AccountDTO>();

            try
            {
                if (AccountIdExists(id))
                {
                    var user = _context.Users.First(t => t.Id == account.idUser);
                    var accountCategory = _context.AccountCategory.First(t => t.Id == account.idAccountCategory);
                    var typePerson = _context.Supplier.First(t => t.Id == account.idTypePerson);

                    account.user = user;
                    account.accountCategory = accountCategory;
                    account.typePerson = typePerson;

                    _context.Update(account);
                    _context.SaveChanges();

                    result.Sucesso = true;
                    result.TRetorno = account;
                    result.Message = "Dados da Conta alterada com sucesso.";

                }
                else
                {
                    result.Sucesso = false;
                    result.Message = "Conta não localizada.";
                }

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados da Conta. Erro: " + e.Message;
            }

            return result;
        }

        [HttpDelete("{id}")]
        public async Task<KRetorno<AccountDTO>> DeleteAccount(long id)
        {

            var result = new KRetorno<AccountDTO>();

            try
            {
                if (AccountIdExists(id))
                {
                    AccountDTO account = _context
                                            .Account
                                            .FirstOrDefault(t => t.Id == id);
                    if (account != null)
                    {
                        account.isActive = !account.isActive;
                        _context.Entry(account).State = EntityState.Modified;
                        _context.SaveChanges();

                        result.Sucesso = true;
                        result.TRetorno = account;
                        result.Message = account.isActive ? "Conta " + account.name + " ativada com sucesso." : "Conta " + account.name + " desativada com sucesso.";
                    }
                    else
                    {
                        result.Sucesso = false;
                        result.Message = "Conta não localizada.";
                    }

                }
                else
                {
                    result.Sucesso = false;
                    result.Message = "Conta não localizada.";
                }

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível desativar/ativar a Conta . Erro: " + e.Message;
            }

            return result;
        }


        private bool AccountIdExists(long id)
        {
            return _context.Account.Any(e => e.Id == id);
        }

        private bool AccountNameExists(string name, string idUser)
        {
            return _context.Account.Any(e => e.name == name && e.user.Id == idUser);
        }





    }
}
