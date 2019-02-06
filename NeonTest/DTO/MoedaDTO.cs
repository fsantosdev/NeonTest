using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NeonTest.Models;

namespace NeonTest.DTO
{
    public class MoedaDTO
    {
        private static MoedaDTO _instance { get; set; }

        private MoedaDTO()
        {
            this.DictionaryMoedas = new Dictionary<string, string>();
            this.DictionaryCotacoes = new Dictionary<string, double>();
        }
        
        public Dictionary<string, string> DictionaryMoedas { get; set; }
        public Dictionary<string, double> DictionaryCotacoes { get; set; }
        public bool success { get; set; }

        public Dictionary<string, string> currencies
        {
            set
            {
                DictionaryMoedas = value;
            }
        }

        public Dictionary<string, double> quotes
        {
            set
            {
                DictionaryCotacoes = value;
            }
        }

        public static MoedaDTO GetInstance()
        {
            if (_instance == null)
            {
                _instance = new MoedaDTO();
            }

            return _instance;
        }

        public static Moeda Create(string sigla, string nome)
        {
            Moeda MoedaInstance = Moeda.Create(sigla, nome);
            return MoedaInstance;
        }

        public static Moeda Create(string sigla, string nome, double valor)
        {
            Moeda MoedaInstance = Moeda.Create(sigla, nome, valor);
            return MoedaInstance;
        }

        internal void setResponse(MoedaDTO moedaParam)
        {
            this.success = moedaParam.success;

            if (this.DictionaryCotacoes.Count == 0)
            {
                this.DictionaryCotacoes = moedaParam.DictionaryCotacoes;
            }

            if (this.DictionaryMoedas.Count == 0)
            {
                this.DictionaryMoedas = moedaParam.DictionaryMoedas;
            }
        }
    }
}
