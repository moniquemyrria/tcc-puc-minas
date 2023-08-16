using Kodigos.Dominio.ModelsData.Help;
using Kodigos.Infra.Core;
using Kodigos.Infra.Data.Contexto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kodigos.API.Controllers.Help
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelpController : ControllerBase
    {
        private readonly KodigosContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<HelpController> _logger;

        public HelpController(KodigosContext context, IConfiguration configuration, ILogger<HelpController> logger)
        {
            _configuration = configuration;
            _context = context;
            _logger = logger;

        }

        /// <summary>
        /// Serviço - Salvar formulário Como ajuda-lo?
        /// </summary>

        [HttpPost]
        public async Task<KRetorno<HelpDTO>> PostHelpl(HelpDTO help)
        {

            var result = new KRetorno<HelpDTO>();

            try
            {
                _context.Help.Add(help);
                _context.SaveChanges();

                result.Sucesso = true;
                result.Message = "Informações enviadas com sucesso.";

            }
            catch (Exception e)
            {
                result.Sucesso = false;
                result.Message = "Não foi possível salvar as informações do formulário de ajuda. Erro: " + e.Message;
            }

            return result;
        }
    }
}
