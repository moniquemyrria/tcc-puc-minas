
using Kodigos.Dominio.ModelsData.Administration.RevenueCategory;
using Kodigos.Infra.Core;
using Kodigos.Infra.Data.Contexto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kodigos.API.Controllers.Administration.RevenueCategory
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevenueCategoryController : ControllerBase
    {
        private readonly KodigosContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RevenueCategoryController> _logger;

        public RevenueCategoryController(KodigosContext context, IConfiguration configuration, ILogger<RevenueCategoryController> logger)
        {
            _configuration = configuration;
            _context = context;
            _logger = logger;

        }

        [HttpGet("{idUser}")]
        public async Task<KRetorno<List<RevenueCategoryDTO>>> GetRevenueCategory(string idUser)
        {

            List<RevenueCategoryDTO> listRevenueCategory = new List<RevenueCategoryDTO>();
            var result = new KRetorno<List<RevenueCategoryDTO>>();

            try
            {
                listRevenueCategory = _context.RevenueCategory.Where(t => t.user.Id == idUser).ToList();


                result.Sucesso = true;
                result.TRetorno = new List<RevenueCategoryDTO>();
                result.TRetorno = listRevenueCategory;

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados da listagem das Categorias. Erro: " + e.Message;
            }

            return result;
        }

        [HttpGet("Edit/{id}")]
        public async Task<KRetorno<RevenueCategoryDTO>> GetRevenueCategoryId(long id)
        {

            var result = new KRetorno<RevenueCategoryDTO>();

            try
            {
                if (RevenueCategoryIdExists(id))
                {
                    RevenueCategoryDTO revenueCategoryDTO = _context.RevenueCategory.FirstOrDefault(t => t.Id == id);

                    result.Sucesso = true;
                    result.TRetorno = revenueCategoryDTO;
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

        [HttpGet("Status/{status}/{idUser}/")]
        public async Task<KRetorno<List<RevenueCategoryDTO>>> GetStatusAccountCategory(bool status, string idUser)
        {

            List<RevenueCategoryDTO> listRevenueCategory = new List<RevenueCategoryDTO>();
            var result = new KRetorno<List<RevenueCategoryDTO>>();

            try
            {
                listRevenueCategory = _context.RevenueCategory.Where(t => t.user.Id == idUser && t.isActive == status).ToList();


                result.Sucesso = true;
                result.TRetorno = new List<RevenueCategoryDTO>();
                result.TRetorno = listRevenueCategory;

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível retornar os dados da listagem das categorias. Erro: " + e.Message;
            }

            return result;
        }


        [HttpPost("{idUser}")]
        public async Task<KRetorno<RevenueCategoryDTO>> PostRevenueCategory(string idUser, RevenueCategoryDTO revenueCategory)
        {

            var result = new KRetorno<RevenueCategoryDTO>();

            try
            {
                if (!RevenueCategoryDescExists(revenueCategory.description, idUser))
                {
                    var user = _context.Users.First(t => t.Id == revenueCategory.idUser);

                    revenueCategory.user = user;
                    _context.RevenueCategory.Add(revenueCategory);
                    _context.SaveChanges();

                    result.Sucesso = true;
                    result.TRetorno = revenueCategory;
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
        public async Task<KRetorno<RevenueCategoryDTO>> PutRevenueCategory(long id, RevenueCategoryDTO revenueCategory)
        {

            var result = new KRetorno<RevenueCategoryDTO>();

            try
            {
                if (RevenueCategoryIdExists(id))
                {
                    _context.Update(revenueCategory);
                    _context.SaveChanges();

                    result.Sucesso = true;
                    result.TRetorno = revenueCategory;
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
        public async Task<KRetorno<RevenueCategoryDTO>> DeleteRevenueCategory(long id)
        {

            var result = new KRetorno<RevenueCategoryDTO>();

            try
            {
                if (RevenueCategoryIdExists(id))
                {
                    RevenueCategoryDTO revenueCategory = _context.RevenueCategory.FirstOrDefault(t => t.Id == id);
                    if (revenueCategory != null)
                    {
                        revenueCategory.isActive = !revenueCategory.isActive;
                        _context.Entry(revenueCategory).State = EntityState.Modified;
                        _context.SaveChanges();

                        result.Sucesso = true;
                        result.TRetorno = revenueCategory;
                        result.Message = revenueCategory.isActive ? "A categoria " + revenueCategory.description + " ativada com sucesso." : "A categoria " + revenueCategory.description + " desativada com sucesso.";
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


        private bool RevenueCategoryIdExists(long id)
        {
            return _context.RevenueCategory.Any(e => e.Id == id);
        }

        private bool RevenueCategoryDescExists(string descricao, string idUser)
        {
            return _context.RevenueCategory.Any(e => e.description == descricao && e.user.Id == idUser);
        }





    }
}
