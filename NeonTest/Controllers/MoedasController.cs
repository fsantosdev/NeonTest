using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NeonTest.Domain;
using NeonTest.Models;

namespace NeonTest.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class MoedasController : Controller
    {
        [HttpGet]
        public async Task<Response> Listagem()
        {
            Response response = null;
            try
            {
                dynamic responseConteudo = await MoedaDomain.ListagemMoedas();
                response = new Response(true, 200, responseConteudo);
            }
            catch (Exception e) {
                response = new Response(false, 400, e);
            }

            return response;
        }

        [HttpPost]
        public Response ConverterMoeda([FromBody] RequestConvesao request)
        {
            Response response = null;
            try
            {
                MoedaDomain moeda = new MoedaDomain();
                dynamic responseConteudo = moeda.Converte(request.moedaOrigem, request.moedaDestino, request.valor);
                response = new Response(true, 200, responseConteudo);
            }
            catch (Exception e)
            {
                response = new Response(false, 400, e);
            }

            return response;
        }
    }
}