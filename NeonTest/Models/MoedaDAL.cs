using System;
using System.Collections.Generic;

namespace NeonTest.Models
{
    public class MoedaDAL
    {
        private static MoedaDAL _instance { get; set; }

        private MoedaDAL()
        {
            this.ListagemMoeda = new Dictionary<string, string>();
            this.ListagemCotacoes = new Dictionary<string, double>();
        }

        public bool Success { get; set; }
        public string Source { get; set; }
        public Dictionary<string, string> ListagemMoeda { get; private set; }
        public Dictionary<string, double> ListagemCotacoes { get; private set; }
        
        public Dictionary<string, string> Currencies {
            set
            {
                ListagemMoeda = value;
            }
        }

        public Dictionary<string, double> Quotes {
            set
            {
                ListagemCotacoes = value;
            }
        }

        public static MoedaDAL GetInstance()
        {
            if (_instance == null) {
                _instance = new MoedaDAL();
            }

            return _instance;
        }
    }
}
