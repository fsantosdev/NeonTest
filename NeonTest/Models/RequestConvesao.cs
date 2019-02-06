using NeonTest.Models;

namespace NeonTest.Controllers
{
    public class RequestConvesao
    {
        public Moeda moedaOrigem { get; set; }
        public Moeda moedaDestino { get; set; }
        public double valor { get; set; }
    }
}