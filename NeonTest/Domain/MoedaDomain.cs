using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using NeonTest.Models;
using System.Diagnostics;

namespace NeonTest.Domain
{
    public class MoedaDomain
    {
        private const string BASE_URL = "http://www.apilayer.net/api/";
        private const string ACCESS_KEY = "access_key=f2a4deb9fa7acfe8e57bc92e48ccaa77";

        //private static Dictionary<string, string> ListaMoedas { get; set; }
        //private static Dictionary<string, double> ListaValores { get; set; }

        public static async Task<Dictionary<string, string>> ListagemMoedas()
        {
            MoedaDAL MoedaInstance = MoedaDAL.GetInstance();
            if (ExisteListagem(MoedaInstance))
            {
                return MoedaInstance.ListagemMoeda;
            }

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(BASE_URL + "list?" + ACCESS_KEY);
                string responseString = String.Empty;

                if (response.IsSuccessStatusCode)
                {
                    responseString = await response.Content.ReadAsStringAsync();
                    try
                    {
                        MoedaInstance = Newtonsoft.Json.JsonConvert.DeserializeObject<MoedaDAL>(responseString);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                }
            }
            
            return MoedaInstance.ListagemMoeda;
        }

        public static async Task<Dictionary<string, double>> ListagemValores()
        {
            MoedaDAL MoedaInstance = MoedaDAL.GetInstance();
            if (ExisteCotacao(MoedaInstance))
            {
                return MoedaInstance.ListagemCotacoes;
            }

            MoedaDAL responseContent = MoedaDAL.GetInstance();
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(BASE_URL + "live?" + ACCESS_KEY);
                string responseString = String.Empty;

                if (response.IsSuccessStatusCode)
                {
                    responseString = await response.Content.ReadAsStringAsync();
                    try
                    {
                        responseContent = Newtonsoft.Json.JsonConvert.DeserializeObject<MoedaDAL>(responseString);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                }
            }
            
            return MoedaInstance.ListagemCotacoes;
        }

        private static bool ExisteListagem(MoedaDAL moedaInstance)
        {
            bool ExisteListagem = moedaInstance.ListagemMoeda.Count > 0;
            return ExisteListagem;
        }

        private static bool ExisteCotacao(MoedaDAL moedaInstance)
        {
            bool ExisteCotacao = moedaInstance.ListagemCotacoes.Count > 0;
            return ExisteCotacao;
        }

        public static double Converte(string siglaOrigem, string siglaDestino, double valor)
        {
            MoedaDAL MoedaInstance = MoedaDAL.GetInstance();
            double origemValor = MoedaInstance.ListagemCotacoes.GetValueOrDefault<string, double>("USD" + siglaOrigem);
            double destinoValor = MoedaInstance.ListagemCotacoes.GetValueOrDefault<string, double>("USD" + siglaDestino);

            double origemConversao = origemValor < 1 ? valor * origemValor : valor / origemValor;
            double destinoConversao = destinoValor < 1 ? valor * origemConversao : valor / origemConversao;

            return destinoConversao;
        }
    }
}
