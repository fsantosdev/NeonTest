using NeonTest.Models;
using System.Collections.Generic;

namespace NeonTest.DTO
{
    public class MoedaListagemDTO
    {
        private MoedaListagemDTO()
        {
            this.ListagemMoeda = new List<Moeda>();
        }

        private static MoedaListagemDTO _instance = null;
        public List<Moeda> ListagemMoeda { get; set; }

        public static MoedaListagemDTO GetInstance()
        {
            if (_instance == null)
            {
                _instance = new MoedaListagemDTO();
            }

            return _instance;
        }

        public void AdicionarMoeda(string sigla, string nome)
        {
            Moeda MoedaInstance = Moeda.Create(sigla, nome);
            _instance.ListagemMoeda.Add(MoedaInstance);
        }
    }
}
