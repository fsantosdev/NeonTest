﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using NeonTest.Domain;
using NeonTest.Models;
using NeonTest.DTO;
using System.Diagnostics;
using System.Linq;

namespace NeonTest.Domain
{
    public class MoedaDomain : IMoedaDomain
    {
        private const string BASE_URL = "http://www.apilayer.net/api/";
        private const string ACCESS_KEY = "access_key=f2a4deb9fa7acfe8e57bc92e48ccaa77";

        public static object ResponseConversao { get; private set; }

        public static async Task<List<Moeda>> ListagemMoedas()
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


        public ResponseConversao Converte(Moeda moedaOrigem, Moeda moedaDestino, double valor)
        {
            double resultado = double.MinValue;
            if (ValidaMoedasIguals(moedaOrigem, moedaDestino))
            {
                resultado = valor;
            }
            else
            {
                double origemConversao = CalculaConversaoParaDolar(moedaOrigem, valor);
                resultado = CalculaConversaoParaMoedaDestino(moedaDestino, origemConversao);
            }

            ResponseConversao response = new ResponseConversao(moedaOrigem, moedaDestino, valor, resultado);
            return response;
        }

        private static bool ValidaMoedasIguals(Moeda moedaOrigem, Moeda moedaDestino)
        {
            return moedaOrigem.Sigla.Equals(moedaDestino.Sigla);
        }

        private static bool DecideTipoOperacao(double valor)
        {
            return valor > 1;
        }

        private static double CalculaConversaoParaDolar(Moeda moedaOrigem, double valorCotado)
        {
            double resultadoOperacao = ArrumaCotacaoCasoMenorQueDolar(moedaOrigem.Valor);
            double Resultado = DecideTipoOperacao(moedaOrigem.Valor) ? valorCotado / resultadoOperacao : valorCotado * resultadoOperacao;
            return Resultado;
        }

        private static double CalculaConversaoParaMoedaDestino(Moeda moedaDestino, double origemConversao)
        {
            double resultadoOperacao = ArrumaCotacaoCasoMenorQueDolar(moedaDestino.Valor);
            double Resultado = DecideTipoOperacao(moedaDestino.Valor) ? origemConversao * resultadoOperacao : origemConversao / resultadoOperacao;
            return Resultado;
        }

        private static double ArrumaCotacaoCasoMenorQueDolar(double valorMoeda)
        {
            double resultadoOperacao = valorMoeda;
            if (valorMoeda < 1)
            {
                double USD = 1;
                double USDPorcentagem = 1;
                double moedaValor = valorMoeda;
                resultadoOperacao = (USD * USDPorcentagem) / moedaValor;
            }

            return resultadoOperacao;
        }
    }
}
