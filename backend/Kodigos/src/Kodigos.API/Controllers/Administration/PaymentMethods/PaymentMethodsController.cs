
using Kodigos.Dominio.ModelsData.Administration.PaymentMethods;
using Kodigos.Dominio.ModelsData.Administration.PaymentMethodsTypes;
using Kodigos.Infra.Core;
using Kodigos.Infra.Data.Contexto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kodigos.API.Controllers.Administration.PaymentMethods
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodsController : ControllerBase
    {
        private readonly KodigosContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PaymentMethodsController> _logger;

        public PaymentMethodsController(KodigosContext context, IConfiguration configuration, ILogger<PaymentMethodsController> logger)
        {
            _configuration = configuration;
            _context = context;
            _logger = logger;

        }

        [HttpGet("{idUser}")]
        public async Task<KRetorno<List<PaymentMethodsDTO>>> GetPaymentMethods(string idUser)
        {

            List<PaymentMethodsDTO> listPaymentMethods = new List<PaymentMethodsDTO>();
            var result = new KRetorno<List<PaymentMethodsDTO>>();

            try
            {
                listPaymentMethods = _context
                                        .PaymentMethods
                                        .Where(t => t.user.Id == idUser)
                                        .Include(t => t.account)
                                        .ToList();


                result.Sucesso = true;
                result.TRetorno = new List<PaymentMethodsDTO>();
                result.TRetorno = listPaymentMethods;

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados da listagem. Erro: " + e.Message;
            }

            return result;
        }

        [HttpGet("Status/{status}/{idUser}")]
        public async Task<KRetorno<List<PaymentMethodsDTO>>> GetPaymentMethodsStatus(bool status,string idUser)
        {

            List<PaymentMethodsDTO> listPaymentMethods = new List<PaymentMethodsDTO>();
            var result = new KRetorno<List<PaymentMethodsDTO>>();

            try
            {
                listPaymentMethods = _context
                                        .PaymentMethods
                                        .Include(t => t.paymentMethodsTypes)
                                        .Where(t => t.user.Id == idUser && t.isActive == status)
                                        .ToList();


                result.Sucesso = true;
                result.TRetorno = new List<PaymentMethodsDTO>();
                result.TRetorno = listPaymentMethods;

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados da listagem. Erro: " + e.Message;
            }

            return result;
        }

        [HttpGet("Edit/{id}")]
        public async Task<KRetorno<PaymentMethodsDTO>> GetPaymentMethodsId(long id)
        {

            var result = new KRetorno<PaymentMethodsDTO>();

            try
            {
                if (PaymentMethodsIdExists(id))
                {
                    var paymentMethods = _context
                                            .PaymentMethods
                                            .Where(t => t.Id == id)
                                            .Include(t => t.account)
                                            .FirstOrDefault();

                    var listPaymentMethodsTypes = _context.PaymentMethodsTypes.Where(t => t.paymentMethods.Id == paymentMethods.Id && t.isActive).ToList();

                    if (listPaymentMethodsTypes.Count() > 0)
                    {
                        paymentMethods.paymentMethodsTypes = new List<PaymentMethodsTypesDTO>();

                        foreach(var item in listPaymentMethodsTypes)
                        {

                            item.idPaymentMethods = paymentMethods.Id;
                            item.paymentMethods = null;
                            paymentMethods.paymentMethodsTypes.Add(item);
                            
                            
                        }
                    }

                    result.Sucesso = true;
                    result.TRetorno = paymentMethods;
                }
                else
                {
                    result.Sucesso = false;
                    result.Message = "Forma de Pagamento não localizada.";
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
        public async Task<KRetorno<PaymentMethodsDTO>> PostPaymentMethods(string idUser, PaymentMethodsDTO paymentMethods)
        {

            var result = new KRetorno<PaymentMethodsDTO>();

            try
            {
                if (!PaymentMethodsNameExists(paymentMethods.name, idUser))
                {
                    var user = _context.Users.First(t => t.Id == paymentMethods.idUser);
                    var acoount = _context.Account.First(t => t.Id == paymentMethods.idAccount);

                    paymentMethods.account = acoount;
                    paymentMethods.user = user;
                    _context.PaymentMethods.Add(paymentMethods);

                    foreach(var item in paymentMethods.paymentMethodsTypes)
                    {
                        item.paymentMethods = paymentMethods;
                        _context.PaymentMethodsTypes.Add(item);
                    }
                    _context.SaveChanges();

                    result.Sucesso = true;
                    result.Message = "Nova Forma de Pagamento cadastrada com sucesso.";

                }
                else
                {
                    result.Sucesso = false;
                    result.Message = "Já existe uma Forma de Pagamento cadastrada com este Nome.";
                }

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados da Forma de Pagamento. Erro: " + e.Message;
            }

            return result;
        }

        [HttpPut("{id}")]
        public async Task<KRetorno<PaymentMethodsDTO>> PutPaymentMethods(long id, PaymentMethodsDTO paymentMethods)
        {

            var result = new KRetorno<PaymentMethodsDTO>();

            try
            {
                if (PaymentMethodsIdExists(id))
                {
                    _context.Update(paymentMethods);

                    var listpaymentMethodsTypes = _context.PaymentMethodsTypes.Where(t => t.paymentMethods.Id == paymentMethods.Id).ToList();

                    foreach (var item in paymentMethods.paymentMethodsTypes)
                    {
                        var itemTypeUpdate = listpaymentMethodsTypes.Where(t => t.description.Trim() == item.description.Trim()).FirstOrDefault();

                        if (itemTypeUpdate != null)
                        {
                            
                            
                            itemTypeUpdate.paymentMethods = paymentMethods;
                            itemTypeUpdate.isActive = item.isActive;
                            _context.Update(itemTypeUpdate);
                        }
                        else
                        {

                            item.paymentMethods = paymentMethods;
                            _context.PaymentMethodsTypes.Add(item);
                        }

                        
                    }

                    _context.SaveChanges();

                    result.Sucesso = true;
                    result.Message = "Dados da Forma de Pagamento alterada com sucesso.";

                }
                else
                {
                    result.Sucesso = false;
                    result.Message = "Forma de Pagamento não localizada.";
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
        public async Task<KRetorno<PaymentMethodsDTO>> DeletePaymentMethods(long id)
        {

            var result = new KRetorno<PaymentMethodsDTO>();

            try
            {
                if (PaymentMethodsIdExists(id))
                {
                    PaymentMethodsDTO paymentMethods = _context.PaymentMethods.FirstOrDefault(t => t.Id == id);
                    if (paymentMethods != null)
                    {
                        paymentMethods.isActive = !paymentMethods.isActive;
                        _context.Entry(paymentMethods).State = EntityState.Modified;
                        _context.SaveChanges();

                        result.Sucesso = true;
                        result.TRetorno = paymentMethods;
                        result.Message = paymentMethods.isActive ? "A Forma de Pagamento " + paymentMethods.name + " ativada com sucesso." : "A Forma de Pagamento " + paymentMethods.name + " desativada com sucesso.";
                    }
                    else
                    {
                        result.Sucesso = false;
                        result.Message = "Forma de Pagamento não localizada.";
                    }

                }
                else
                {
                    result.Sucesso = false;
                    result.Message = "Forma de Pagamento não localizada.";
                }

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível desativar/ativar a Forma de Pagamento . Erro: " + e.Message;
            }

            return result;
        }


        private bool PaymentMethodsIdExists(long id)
        {
            return _context.PaymentMethods.Any(e => e.Id == id);
        }

        private bool PaymentMethodsNameExists(string name, string idUser)
        {
            return _context.PaymentMethods.Any(e => e.name == name && e.user.Id == idUser);
        }





    }
}
