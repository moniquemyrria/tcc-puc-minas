
using Kodigos.Dominio.ModelsData.Administration.TypePerson;
using Kodigos.Infra.Core;
using Kodigos.Infra.Data.Contexto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kodigos.API.Controllers.Administration.TypePerson
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypePersonController : ControllerBase
    {
        private readonly KodigosContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TypePersonController> _logger;

        public TypePersonController(KodigosContext context, IConfiguration configuration, ILogger<TypePersonController> logger)
        {
            _configuration = configuration;
            _context = context;
            _logger = logger;

        }

        [HttpGet("{idUser}")]
        public async Task<KRetorno<List<SupplierDTO>>> GetTypePerson(string idUser)
        {

            List<SupplierDTO> listTypePerson = new List<SupplierDTO>();
            var result = new KRetorno<List<SupplierDTO>>();

            try
            {
                listTypePerson = _context.Supplier.Where(t => t.user.Id == idUser).ToList();


                result.Sucesso = true;
                result.TRetorno = new List<SupplierDTO>();
                result.TRetorno = listTypePerson;

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados da listagem. Erro: " + e.Message;
            }

            return result;
        }

        [HttpGet("Status/{status}/{idUser}")]
        public async Task<KRetorno<List<SupplierDTO>>> GetTypePersonStatus(bool status,string idUser)
        {

            List<SupplierDTO> listTypePerson = new List<SupplierDTO>();
            var result = new KRetorno<List<SupplierDTO>>();

            try
            {
                listTypePerson = _context.Supplier.Where(t => t.user.Id == idUser && t.isActive == status).ToList();


                result.Sucesso = true;
                result.TRetorno = new List<SupplierDTO>();
                result.TRetorno = listTypePerson;

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados da listagem. Erro: " + e.Message;
            }

            return result;
        }

        [HttpGet("Person/J/{idUser}")]
        public async Task<KRetorno<List<SupplierDTO>>> GetTypePersonJ(string idUser)
        {

            List<SupplierDTO> listTypePerson = new List<SupplierDTO>();
            var result = new KRetorno<List<SupplierDTO>>();

            try
            {
                listTypePerson = _context.Supplier.Where(t => t.typePerson == "Jurídico" && t.user.Id == idUser && t.isActive).ToList();


                result.Sucesso = true;
                result.TRetorno = new List<SupplierDTO>();
                result.TRetorno = listTypePerson;

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados da listagem. Erro: " + e.Message;
            }

            return result;
        }

        [HttpGet("Person/F/{idUser}")]
        public async Task<KRetorno<List<SupplierDTO>>> GetTypePersonF(string idUser)
        {

            List<SupplierDTO> listTypePerson = new List<SupplierDTO>();
            var result = new KRetorno<List<SupplierDTO>>();

            try
            {
                listTypePerson = _context.Supplier.Where(t => t.typePerson == "Físico" && t.user.Id == idUser).ToList();


                result.Sucesso = true;
                result.TRetorno = new List<SupplierDTO>();
                result.TRetorno = listTypePerson;

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados da listagem. Erro: " + e.Message;
            }

            return result;
        }

        [HttpGet("Edit/{id}")]
        public async Task<KRetorno<SupplierDTO>> GetTypePersonId(long id)
        {

            var result = new KRetorno<SupplierDTO>();

            try
            {
                if (TypePersonIdExists(id))
                {
                    SupplierDTO typePersonDTO = _context.Supplier.FirstOrDefault(t => t.Id == id);

                    result.Sucesso = true;
                    result.TRetorno = typePersonDTO;
                }
                else
                {
                    result.Sucesso = false;
                    result.Message = "Registro  não localizada.";
                }


            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados. Erro: " + e.Message;
            }

            return result;
        }


        [HttpPost("{idUser}")]
        public async Task<KRetorno<SupplierDTO>> PostTypePerson(string idUser, SupplierDTO expenseCategory)
        {

            var result = new KRetorno<SupplierDTO>();

            try
            {
                if (!TypePersonNameExists(expenseCategory.name, idUser))
                {
                    var user = _context.Users.First(t => t.Id == expenseCategory.idUser);

                    expenseCategory.user = user;
                    _context.Supplier.Add(expenseCategory);
                    _context.SaveChanges();

                    result.Sucesso = true;
                    result.TRetorno = expenseCategory;
                    result.Message = "Novo registro cadastrado com sucesso.";

                }
                else
                {
                    result.Sucesso = false;
                    result.Message = "Já existe um registro cadastrado com este Nome.";
                }

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados dos registros. Erro: " + e.Message;
            }

            return result;
        }

        [HttpPut("{id}")]
        public async Task<KRetorno<SupplierDTO>> PutTypePerson(long id, SupplierDTO expenseCategory)
        {

            var result = new KRetorno<SupplierDTO>();

            try
            {
                if (TypePersonIdExists(id))
                {
                    _context.Update(expenseCategory);
                    _context.SaveChanges();

                    result.Sucesso = true;
                    result.TRetorno = expenseCategory;
                    result.Message = "Dados alterado com sucesso.";

                }
                else
                {
                    result.Sucesso = false;
                    result.Message = "Registro não localizado.";
                }

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados. Erro: " + e.Message;
            }

            return result;
        }

        [HttpDelete("{id}")]
        public async Task<KRetorno<SupplierDTO>> DeleteTypePerson(long id)
        {

            var result = new KRetorno<SupplierDTO>();

            try
            {
                if (TypePersonIdExists(id))
                {
                    SupplierDTO expenseCategory = _context.Supplier.FirstOrDefault(t => t.Id == id);
                    if (expenseCategory != null)
                    {
                        expenseCategory.isActive = !expenseCategory.isActive;
                        _context.Entry(expenseCategory).State = EntityState.Modified;
                        _context.SaveChanges();

                        result.Sucesso = true;
                        result.TRetorno = expenseCategory;
                        result.Message = expenseCategory.isActive ? "O registro " + expenseCategory.name + " ativado com sucesso." : "O registro " + expenseCategory.name + " desativado com sucesso.";
                    }
                    else
                    {
                        result.Sucesso = false;
                        result.Message = "Registro não localizado.";
                    }

                }
                else
                {
                    result.Sucesso = false;
                    result.Message = "Registro não localizado.";
                }

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível desativar/ativar o registro . Erro: " + e.Message;
            }

            return result;
        }


        private bool TypePersonIdExists(long id)
        {
            return _context.Supplier.Any(e => e.Id == id);
        }

        private bool TypePersonNameExists(string name, string idUser)
        {
            return _context.Supplier.Any(e => e.name == name && e.user.Id == idUser);
        }





    }
}
