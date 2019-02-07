using NeonTest.Domain;
using NeonTest.DTO;
using NeonTest.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Tests
{
    public class MoedaDomainTest
    {
        [SetUp]
        public void Setup()
        {
            Moeda moedaOrigem = Moeda.Create("BRL", "Real Brasileiro", 3.7302);
            Moeda moedaDestino = Moeda.Create("EUR", "Euro", 0.85721);
            Moeda moedaDolar = Moeda.Create("USD", "Dólar Americano", 1);
        }

        public IEnumerable<TestCaseData> Converte_ConversaoFuncionaComoEsperadoData
        {
            get
            {
                Setup();
                Moeda moedaOrigem = new Moeda("BRL", "Real Brasileiro", 3.7302);
                Moeda moedaDestino = new Moeda("EUR", "Euro", 0.85721);
                Moeda moedaDolar = new Moeda("USD", "Dólar Americano", 1);
                yield return new TestCaseData(moedaOrigem, moedaDestino, 1000);
                yield return new TestCaseData(moedaOrigem, moedaDolar, 1000);
                yield return new TestCaseData(moedaDestino, moedaDolar, 1000);
            }
        }
        
        [TestCase]
        public void Converte_ConversaoFuncionaComoEsperado(double valor = 1000)
        {
            Moeda moedaOrigem = new Moeda("BRL", "Real Brasileiro", 3.7302);
            Moeda moedaDestino = new Moeda("EUR", "Euro", 0.85721);
            Moeda moedaDolar = new Moeda("USD", "Dólar Americano", 1);

            MoedaDomain moeda = new MoedaDomain();
            ResponseConversao response = moeda.Converte(moedaOrigem, moedaDestino, valor);
            Debug.WriteLine($"====> {valor}");



            Assert.AreEqual(response.Resultado, Is.GreaterThan(0));
        }

    }
}