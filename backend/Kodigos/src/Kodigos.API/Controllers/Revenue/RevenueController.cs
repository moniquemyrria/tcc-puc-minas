

using Kodigos.API.ModelView.Filters;
using Kodigos.Dominio.ModelsData.Revenue;
using Kodigos.Infra.Core;
using Kodigos.Infra.Data.Contexto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Kodigos.API.Controllers.Revenue
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevenueController : ControllerBase
    {
        private readonly KodigosContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RevenueController> _logger;

        public RevenueController(KodigosContext context, IConfiguration configuration, ILogger<RevenueController> logger)
        {
            _configuration = configuration;
            _context = context;
            _logger = logger;

        }

        [HttpGet("{idUser}")]
        public async Task<KRetorno<List<RevenueDTO>>> GetRevenue(string idUser)
        {

            List<RevenueDTO> listRevenue = new List<RevenueDTO>();
            var result = new KRetorno<List<RevenueDTO>>();

            try
            {
                listRevenue = _context
                                .Revenue
                                .Where(t => t.user.Id == idUser)
                                .Include(t => t.revenueCategory)
                                .Include(t => t.supplier)
                                .Include(t => t.account)
                                .ToList();

                listRevenue.ForEach(t => t.colorRevenueCategory = t.revenueCategory.color);
                listRevenue.ForEach(t => t.dateCreatedFormat = t.dateCreated.ToString("dd/MM/yyyy"));
                listRevenue.ForEach(t => t.totalSumRevenue = listRevenue.Where(e => e.isActive).Sum(e => e.value));


                result.Sucesso = true;
                result.TRetorno = new List<RevenueDTO>();
                result.TRetorno = listRevenue.OrderByDescending(t => t.dateCreated).ToList();

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados da listagem de Receitas. Erro: " + e.Message;
            }

            return result;
        }

        [HttpGet("Edit/{id}")]
        public async Task<KRetorno<RevenueDTO>> GetRevenueId(long id)
        {

            var result = new KRetorno<RevenueDTO>();

            try
            {
                if (RevenueIdExists(id))
                {
                    var revenueDTO = _context
                                                 .Revenue
                                                 .Include(t => t.revenueCategory)
                                                 .Include(t => t.account)
                                                 .Include(t => t.supplier)
                                                 .FirstOrDefault(t => t.Id == id);

                    result.Sucesso = true;
                    result.TRetorno = revenueDTO;
                }
                else
                {
                    result.Sucesso = false;
                    result.Message = "Receita não localizada.";
                }


            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados da Receita. Erro: " + e.Message;
            }

            return result;
        }

        [HttpPost("Filters/{idUser}")]
        public async Task<KRetorno<List<RevenueDTO>>> PostRevenueFilters(string idUser, RevenueFilterDTO filters)
        {

            List<RevenueDTO> listRevenue = new List<RevenueDTO>();
            var result = new KRetorno<List<RevenueDTO>>();

            try
            {
                //apenas categoria
                listRevenue = FilterRevenueCategory(idUser, filters, listRevenue);

                //apenas tipo de pessoa
                listRevenue = FilterTypePerson(idUser, filters, listRevenue);

                //apenas conta
                listRevenue = FilterAccount(idUser, filters, listRevenue);

                //apenas forma de recebimento
                listRevenue = FilterRevenueReceipt(idUser, filters, listRevenue);

                //categoria e tipo de pessoa
                listRevenue = FilterRevenueCategoryAndTypePerson(idUser, filters, listRevenue);

                //categoria e conta
                listRevenue = FilterRevenueCategoryAndAccount(idUser, filters, listRevenue);

                //tipo de pessoa e conta
                listRevenue = FilterTypePersonAndAccount(idUser, filters, listRevenue);

                //categoria, tipo de pessoa e conta
                listRevenue = FilterRevenueCategoryAndTypePersonAndAccount(idUser, filters, listRevenue);

                //forma de recebimento e categoria
                listRevenue = FilterRevenueReceiptAndRevenueCategory(idUser, filters, listRevenue);

                //forma de recebimento e tipo de pessoa
                listRevenue = FilterRevenueReceiptAndTypePerson(idUser, filters, listRevenue);

                //forma de recebimento e conta
                listRevenue = FilterRevenueReceiptAndAccount(idUser, filters, listRevenue);

                //forma de recebimento, categoria, tipo de pessoa e conta
                listRevenue = FilterRevenueReceiptRevenueCategoryTypePersonAndAccount(idUser, filters, listRevenue);


                //periodo
                if (listRevenue.Count > 0)
                {
                    listRevenue = FilterPeriodWithParamters(filters, listRevenue);
                }
                else
                {
                    listRevenue = FilterPeriodNoParamters(idUser, filters, listRevenue);
                }


                listRevenue.ForEach(t => t.colorRevenueCategory = t.revenueCategory.color);
                listRevenue.ForEach(t => t.dateCreatedFormat = t.dateCreated.ToString("dd/MM/yyyy"));
                listRevenue.ForEach(t => t.totalSumRevenue = listRevenue.Where(e => e.isActive).Sum(e => e.value));


                result.Sucesso = true;
                result.TRetorno = new List<RevenueDTO>();
                result.TRetorno = listRevenue.OrderBy(t => t.dateCreated).ToList();

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados da listagem de Receitas. Erro: " + e.Message;
            }

            return result;


            //Filters
            List<RevenueDTO> FilterRevenueCategory(string idUser, RevenueFilterDTO filters, List<RevenueDTO> listRevenue)
            {
                if (filters.revenueCategory != null && filters.supplier == null && filters.account == null && filters.revenueReceipt == "")
                {
                    listRevenue = _context
                               .Revenue
                               .Where(t => t.user.Id == idUser && t.revenueCategory.Id == filters.revenueCategory.Id)
                               .Include(t => t.revenueCategory)
                               .Include(t => t.supplier)
                               .Include(t => t.account)
                               .ToList();
                }

                return filters.revenueStatus == "Todas" ? listRevenue : listRevenue.Where(t => t.revenueStatus == filters.revenueStatus).ToList();

            }

            List<RevenueDTO> FilterTypePerson(string idUser, RevenueFilterDTO filters, List<RevenueDTO> listRevenue)
            {
                if (filters.revenueCategory == null && filters.supplier != null && filters.account == null && filters.revenueReceipt == "")
                {
                    listRevenue = _context
                               .Revenue
                               .Where(t => t.user.Id == idUser && t.supplier.Id == filters.supplier.Id)
                               .Include(t => t.revenueCategory)
                               .Include(t => t.supplier)
                               .Include(t => t.account)
                               .ToList();
                }

                return filters.revenueStatus == "Todas" ? listRevenue : listRevenue.Where(t => t.revenueStatus == filters.revenueStatus).ToList(); ;
            }

            List<RevenueDTO> FilterAccount(string idUser, RevenueFilterDTO filters, List<RevenueDTO> listRevenue)
            {
                if (filters.revenueCategory == null && filters.supplier == null && filters.account != null && filters.revenueReceipt == "")
                {
                    listRevenue = _context
                               .Revenue
                               .Where(t => t.user.Id == idUser && t.account.Id == filters.account.Id)
                               .Include(t => t.revenueCategory)
                               .Include(t => t.supplier)
                               .Include(t => t.account)
                               .ToList();
                }

                return filters.revenueStatus == "Todas" ? listRevenue : listRevenue.Where(t => t.revenueStatus == filters.revenueStatus).ToList();;
            }

            List<RevenueDTO> FilterRevenueReceipt(string idUser, RevenueFilterDTO filters, List<RevenueDTO> listRevenue)
            {
                if (filters.revenueCategory == null && filters.supplier == null && filters.account == null && filters.revenueReceipt != "")
                {
                    listRevenue = _context
                               .Revenue
                               .Where(t => t.user.Id == idUser && t.revenueReceipt == filters.revenueReceipt)
                               .Include(t => t.revenueCategory)
                               .Include(t => t.supplier)
                               .Include(t => t.account)
                               .ToList();
                }

                return filters.revenueStatus == "Todas" ? listRevenue : listRevenue.Where(t => t.revenueStatus == filters.revenueStatus).ToList();;
            }

            List<RevenueDTO> FilterRevenueCategoryAndTypePerson(string idUser, RevenueFilterDTO filters, List<RevenueDTO> listRevenue)
            {

                if (filters.revenueCategory != null && filters.supplier != null && filters.account == null && filters.revenueReceipt == "" )
                {
                    listRevenue = _context
                               .Revenue
                               .Where(t =>
                                        t.user.Id == idUser &&
                                        t.revenueCategory.Id == filters.revenueCategory.Id &&
                                        t.supplier.Id == filters.supplier.Id
                                )
                               .Include(t => t.revenueCategory)
                               .Include(t => t.supplier)
                               .Include(t => t.account)
                               .ToList();
                }

                return filters.revenueStatus == "Todas" ? listRevenue : listRevenue.Where(t => t.revenueStatus == filters.revenueStatus).ToList();;
            }

            List<RevenueDTO> FilterRevenueCategoryAndAccount(string idUser, RevenueFilterDTO filters, List<RevenueDTO> listRevenue)
            {
                if (filters.revenueCategory != null && filters.supplier == null && filters.account != null && filters.revenueReceipt == "")
                {
                    listRevenue = _context
                               .Revenue
                               .Where(t =>
                                        t.user.Id == idUser &&
                                        t.revenueCategory.Id == filters.revenueCategory.Id &&
                                        t.account.Id == filters.account.Id
                                )
                               .Include(t => t.revenueCategory)
                               .Include(t => t.supplier)
                               .Include(t => t.account)
                               .ToList();
                }

                return filters.revenueStatus == "Todas" ? listRevenue : listRevenue.Where(t => t.revenueStatus == filters.revenueStatus).ToList();;
            }

            List<RevenueDTO> FilterTypePersonAndAccount(string idUser, RevenueFilterDTO filters, List<RevenueDTO> listRevenue)
            {
                if (filters.revenueCategory == null && filters.supplier != null && filters.account != null && filters.revenueReceipt == "")
                {
                    listRevenue = _context
                               .Revenue
                               .Where(t =>
                                        t.user.Id == idUser &&
                                        t.supplier.Id == filters.supplier.Id &&
                                        t.account.Id == filters.account.Id
                                )
                               .Include(t => t.revenueCategory)
                               .Include(t => t.supplier)
                               .Include(t => t.account)
                               .ToList();
                }

                return filters.revenueStatus == "Todas" ? listRevenue : listRevenue.Where(t => t.revenueStatus == filters.revenueStatus).ToList();;
            }

            List<RevenueDTO> FilterRevenueCategoryAndTypePersonAndAccount(string idUser, RevenueFilterDTO filters, List<RevenueDTO> listRevenue)
            {
                if (filters.revenueCategory != null && filters.supplier != null && filters.account != null)
                {
                    listRevenue = _context
                               .Revenue
                               .Where(t =>
                                        t.user.Id == idUser &&
                                        t.revenueCategory.Id == filters.revenueCategory.Id &&
                                        t.supplier.Id == filters.supplier.Id &&
                                        t.account.Id == filters.account.Id
                                )
                               .Include(t => t.revenueCategory)
                               .Include(t => t.supplier)
                               .Include(t => t.account)
                               .ToList();
                }

                return filters.revenueStatus == "Todas" ? listRevenue : listRevenue.Where(t => t.revenueStatus == filters.revenueStatus).ToList();;
            }

            List<RevenueDTO> FilterRevenueReceiptAndRevenueCategory(string idUser, RevenueFilterDTO filters, List<RevenueDTO> listRevenue)
            {
                if (filters.revenueCategory != null && filters.supplier == null && filters.account == null && filters.revenueReceipt != "")
                {
                    listRevenue = _context
                               .Revenue
                               .Where(t => t.user.Id == idUser && t.revenueCategory.Id == filters.revenueCategory.Id && t.revenueReceipt == filters.revenueReceipt)
                               .Include(t => t.revenueCategory)
                               .Include(t => t.supplier)
                               .Include(t => t.account)
                               .ToList();
                }

                return filters.revenueStatus == "Todas" ? listRevenue : listRevenue.Where(t => t.revenueStatus == filters.revenueStatus).ToList();;
            }

            List<RevenueDTO> FilterRevenueReceiptAndTypePerson(string idUser, RevenueFilterDTO filters, List<RevenueDTO> listRevenue)
            {
                if (filters.revenueCategory == null && filters.supplier != null && filters.account == null && filters.revenueReceipt != "")
                {
                    listRevenue = _context
                               .Revenue
                               .Where(t => t.user.Id == idUser && t.supplier.Id == filters.supplier.Id && t.revenueReceipt == filters.revenueReceipt)
                               .Include(t => t.revenueCategory)
                               .Include(t => t.supplier)
                               .Include(t => t.account)
                               .ToList();
                }

                return filters.revenueStatus == "Todas" ? listRevenue : listRevenue.Where(t => t.revenueStatus == filters.revenueStatus).ToList();;
            }

            List<RevenueDTO> FilterRevenueReceiptAndAccount(string idUser, RevenueFilterDTO filters, List<RevenueDTO> listRevenue)
            {
                if (filters.revenueCategory == null && filters.supplier == null && filters.account != null && filters.revenueReceipt != "")
                {
                    listRevenue = _context
                               .Revenue
                               .Where(t => t.user.Id == idUser && t.account.Id == filters.account.Id && t.revenueReceipt == filters.revenueReceipt)
                               .Include(t => t.revenueCategory)
                               .Include(t => t.supplier)
                               .Include(t => t.account)
                               .ToList();
                }

                return filters.revenueStatus == "Todas" ? listRevenue : listRevenue.Where(t => t.revenueStatus == filters.revenueStatus).ToList();;
            }

            List<RevenueDTO> FilterRevenueReceiptRevenueCategoryTypePersonAndAccount(string idUser, RevenueFilterDTO filters, List<RevenueDTO> listRevenue)
            {
                if (filters.revenueCategory != null && filters.supplier != null && filters.account != null && filters.revenueReceipt != "")
                {
                    listRevenue = _context
                               .Revenue
                               .Where(t =>
                                        t.user.Id == idUser &&
                                        t.revenueCategory.Id == filters.revenueCategory.Id &&
                                        t.supplier.Id == filters.supplier.Id &&
                                        t.account.Id == filters.account.Id &&
                                        t.revenueReceipt == filters.revenueReceipt)
                               .Include(t => t.revenueCategory)
                               .Include(t => t.supplier)
                               .Include(t => t.account)
                               .ToList();
                }

                return filters.revenueStatus == "Todas" ? listRevenue : listRevenue.Where(t => t.revenueStatus == filters.revenueStatus).ToList();;
            }

            static List<RevenueDTO> FilterPeriodWithParamters(RevenueFilterDTO filters, List<RevenueDTO> listRevenue)
            {
                if (filters.dateInitial != null && filters.dateFinal != null)
                {
                    DateTime dtInicial = filters.dateInitial.Value.Date;
                    DateTime dtFinal = filters.dateFinal.Value.Date;
                    listRevenue = listRevenue.Where(t => t.dateCreated.Date >= dtInicial && t.dateCreated.Date <= dtFinal).ToList();
                }

                return filters.revenueStatus == "Todas" ? listRevenue : listRevenue.Where(t => t.revenueStatus == filters.revenueStatus).ToList();;
            }

            List<RevenueDTO> FilterPeriodNoParamters(string idUser, RevenueFilterDTO filters, List<RevenueDTO> listRevenue)
            {
                if (filters.dateInitial != null && filters.dateFinal != null)
                {

                    DateTime dtInicial = filters.dateInitial.Value.Date;
                    DateTime dtFinal = filters.dateFinal.Value.Date;
                    listRevenue = _context
                            .Revenue
                            .Where(t => t.user.Id == idUser && t.dateCreated.Date >= dtInicial && t.dateCreated.Date <= dtFinal)
                            .Include(t => t.revenueCategory)
                            .Include(t => t.supplier)
                            .Include(t => t.account)
                            .ToList();
                }

                return filters.revenueStatus == "Todas" ? listRevenue : listRevenue.Where(t => t.revenueStatus == filters.revenueStatus).ToList(); ;
            }
        }


        [HttpPost("{idUser}")]
        public async Task<KRetorno<RevenueDTO>> PostRevenue(string idUser, RevenueDTO revenue)
        {

            var result = new KRetorno<RevenueDTO>();

            try
            {
                var user = _context.Users.First(t => t.Id == revenue.idUser);
                var revenueCategory = _context.RevenueCategory.First(t => t.Id == revenue.idRevenueCategory);
                var account = _context.Account.First(t => t.Id == revenue.idAccount);
                var typePerson = _context.Supplier.First(t => t.Id == revenue.idTypePerson);

                revenue.user = user;
                revenue.revenueCategory = revenueCategory;
                revenue.account = account;
                revenue.supplier = typePerson;
                _context.Revenue.Add(revenue);
                _context.SaveChanges();

                result.Sucesso = true;
                result.TRetorno = revenue;
                result.Message = "Nova receita cadastrada com sucesso.";



            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados das Categorias. Erro: " + e.Message;
            }

            return result;
        }

        [HttpPut("{id}")]
        public async Task<KRetorno<RevenueDTO>> PutRevenue(long id, RevenueDTO revenue)
        {

            var result = new KRetorno<RevenueDTO>();

            try
            {
                if (RevenueIdExists(id))
                {
                    var user = _context.Users.First(t => t.Id == revenue.idUser);
                    var revenueCategory = _context.RevenueCategory.First(t => t.Id == revenue.idRevenueCategory);
                    var account = _context.Account.First(t => t.Id == revenue.idAccount);
                    var typePerson = _context.Supplier.First(t => t.Id == revenue.idTypePerson);

                    revenue.user = user;
                    revenue.revenueCategory = revenueCategory;
                    revenue.account = account;
                    revenue.supplier = typePerson;
                    _context.Update(revenue);
                    _context.SaveChanges();

                    result.Sucesso = true;
                    result.TRetorno = revenue;
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
        public async Task<KRetorno<RevenueDTO>> DeleteRevenue(long id)
        {

            var result = new KRetorno<RevenueDTO>();

            try
            {
                if (RevenueIdExists(id))
                {
                    RevenueDTO revenue = _context.Revenue.FirstOrDefault(t => t.Id == id);
                    if (revenue != null)
                    {
                        revenue.isActive = !revenue.isActive;
                        _context.Entry(revenue).State = EntityState.Modified;
                        _context.SaveChanges();

                        result.Sucesso = true;
                        result.TRetorno = revenue;
                        result.Message = revenue.isActive ? "A categoria " + revenue.description + " ativada com sucesso." : "A categoria " + revenue.description + " desativada com sucesso.";
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


        private bool RevenueIdExists(long id)
        {
            return _context.Revenue.Any(e => e.Id == id);
        }

        private bool RevenueDescExists(string descricao, string idUser)
        {
            return _context.Revenue.Any(e => e.description == descricao && e.user.Id == idUser);
        }





    }
}
