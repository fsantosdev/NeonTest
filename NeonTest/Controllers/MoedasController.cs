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
        public async Task<Response> ListagemMoedas()
        {
            Response response = null;
            try
            {
                dynamic responseContent = await MoedaDomain.ListagemMoedas();
                response = new Response(true, 200, responseContent);
            }
            catch (Exception e) {
                response = new Response(false, 400, e);
            }

            return response;
        }

        [HttpGet]
        public async Task<Response> ListagemCotacoes()
        {
            Response response = null;
            try
            {
                dynamic responseContent = await MoedaDomain.ListagemCotacoes();
                response = new Response(true, 200, responseContent);
            }
            catch (Exception e)
            {
                response = new Response(false, 400, e);
            }

            return response;
        }

        [HttpPost]
        public Response ConverterMoeda(string siglaOrigem, string siglaDestino, double valor)
        {
            Response response = null;
            try
            {
                dynamic responseContent = MoedaDomain.Converte(siglaOrigem, siglaDestino, valor);
                response = new Response(true, 200, responseContent);
            }
            catch (Exception e)
            {
                response = new Response(false, 400, e);
            }

            return response;
        }
    }
}