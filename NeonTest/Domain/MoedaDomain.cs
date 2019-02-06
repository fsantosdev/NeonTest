using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using NeonTest.Models;
using NeonTest.DTO;
using System.Diagnostics;
using System.Linq;

namespace NeonTest.Domain
{
    public class MoedaDomain
    {
        private const string BASE_URL = "http://www.apilayer.net/api/";
        private const string ACCESS_KEY = "access_key=f2a4deb9fa7acfe8e57bc92e48ccaa77";

        public static object ResponseConversao { get; private set; }

        public static async Task<IList<Moeda>> ListagemMoedas()
        {
            MoedaListagemDTO MoedaListagemInstance = MoedaListagemDTO.GetInstance();
            MoedaDTO MoedaDTOInstance = MoedaDTO.GetInstance();

            if (ExisteListagem(MoedaListagemInstance))
            {
                return MoedaListagemInstance.ListagemMoeda;
            }

            MoedaDTOInstance = await RequestAPILayer(MoedaDTOInstance, "list");
            MoedaDTOInstance = await RequestAPILayer(MoedaDTOInstance, "live");

            AdicionaMoedaNaListagem(MoedaListagemInstance, MoedaDTOInstance);
            AtualizaValoresDeCotacao(MoedaListagemInstance, MoedaDTOInstance);

            return MoedaListagemInstance.ListagemMoeda;
        }

        private static void AtualizaValoresDeCotacao(MoedaListagemDTO MoedaListagemInstance, MoedaDTO MoedaDTOInstance)
        {
            foreach (KeyValuePair<string, double> cotacao in MoedaDTOInstance.DictionaryCotacoes)
            {
                MoedaListagemInstance.ListagemMoeda
                    .Where(moeda => moeda.Sigla == cotacao.Key.Substring(3))
                    .Select(moeda =>
                    {
                        moeda.Valor = cotacao.Value;
                        return moeda;
                    })
                    .ToList();
            }
        }

        private static async Task<MoedaDTO> RequestAPILayer(MoedaDTO MoedaDTOInstance, string endpoint)
        {
            using (HttpClient client = new HttpClient())
            {
                FormattableString requestURL = $"{BASE_URL}{endpoint}?{ACCESS_KEY}";
                HttpResponseMessage response = await client.GetAsync(requestURL.ToString());
                string responseString = String.Empty;

                if (response.IsSuccessStatusCode)
                {
                    responseString = await response.Content.ReadAsStringAsync();
                    try
                    {
                        MoedaDTOInstance.setResponse(Newtonsoft.Json.JsonConvert.DeserializeObject<MoedaDTO>(responseString));
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                }
            }

            return MoedaDTOInstance;
        }

        private static void AdicionaMoedaNaListagem(MoedaListagemDTO MoedaListagemInstance, MoedaDTO MoedaDTOInstance)
        {
            foreach (KeyValuePair<string, string> cotacao in MoedaDTOInstance.DictionaryMoedas)
            {
                Moeda Moeda = MoedaDTO.Create(cotacao.Key, cotacao.Value);
                MoedaListagemInstance.ListagemMoeda.Add(Moeda);
            }
        }

        private static bool ExisteListagem(MoedaListagemDTO moedaInstance)
        {
            bool ExisteListagem = moedaInstance.ListagemMoeda.Count > 0;
            return ExisteListagem;
        }


        public static ResponseConversao Converte(Moeda moedaOrigem, Moeda moedaDestino, double valor)
        {
            double origemConversao = CalculaConversaoParaDolar(moedaOrigem, valor);
            double destinoConversao = CalculaConversaoParaMoedaDestino(moedaDestino, valor, origemConversao);
            ResponseConversao response = new ResponseConversao(moedaOrigem, moedaDestino, valor, destinoConversao);

            return response;
        }

        private static bool DecideTipoOperacao(double valor)
        {
            return valor < 1;
        }

        private static double CalculaConversaoParaDolar(Moeda moedaOrigem, double valor)
        {
            double Resultado = DecideTipoOperacao(moedaOrigem.Valor) ? valor * moedaOrigem.Valor : valor / moedaOrigem.Valor;
            return Resultado;
        }

        private static double CalculaConversaoParaMoedaDestino(Moeda moedaDestino, double valor, double origemConversao)
        {
            double Resultado = DecideTipoOperacao(moedaDestino.Valor) ? moedaDestino.Valor * origemConversao : moedaDestino.Valor / origemConversao;
            return Resultado;
        }
    }
}
