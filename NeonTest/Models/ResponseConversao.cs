using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NeonTest.Models;

namespace NeonTest.Models
{
    public class ResponseConversao
    {
        private ResponseConversao()
        {
        }

        public ResponseConversao(Moeda moedaOrigem, Moeda moedaDestino, double valor, double resultado)
        {
            this.MoedaOrigem = moedaOrigem;
            this.MoedaDestino = moedaDestino;
            this.Valor = valor;
            this.Resultado = resultado;
        }

        public Moeda MoedaOrigem { get; set; }
        public Moeda MoedaDestino { get; set; }
        public double Valor { get; set; }
        public double Resultado { get; set; }

        public static Moeda Create(string sigla, string nome)
        {
            Moeda MoedaInstance = new Moeda();
            MoedaInstance.Sigla = sigla;
            MoedaInstance.Nome = nome;

            return MoedaInstance;
        }

        public static ResponseConversao Create(Moeda moedaOrigem, Moeda moedaDestino, double valor, double resultado)
        {
            ResponseConversao responseConversao = new ResponseConversao(moedaOrigem, moedaDestino, valor, resultado);

            return responseConversao;
        }
    }
}
