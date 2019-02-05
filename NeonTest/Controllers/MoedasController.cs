using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NeonTest.Domain;

namespace NeonTest.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class MoedasController : Controller
    {
        [HttpGet]
        public async Task<string> Listagem()
        {
            object responseContent = await MoedaDomain.ListagemMoedas();
            return responseContent.ToString();
        }

        [HttpPost]
        public double ConverterMoeda(string siglaOrigem, string siglaDestino, double valor)
        {
            return MoedaDomain.Converte(siglaOrigem, siglaDestino, valor);
        }
    }
}