using System;
using System.Collections.Generic;

namespace NeonTest.Models
{
    public class Moeda
    {
        private Moeda()
        {
        }

        public string Nome { get; set; }
        public string Sigla { get; set; }
        public double Valor { get; set; }

        public static Moeda Create(string sigla, string nome)
        {
            Moeda MoedaInstance = new Moeda();
            MoedaInstance.Sigla = sigla;
            MoedaInstance.Nome = nome;

            return MoedaInstance;
        }

        public static Moeda Create(string sigla, string nome, double valor)
        {
            Moeda MoedaInstance = new Moeda();
            MoedaInstance.Sigla = sigla;
            MoedaInstance.Nome = nome;
            MoedaInstance.Valor = (double)valor;

            return MoedaInstance;
        }
    }
}
