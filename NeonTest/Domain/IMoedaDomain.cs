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
    interface IMoedaDomain
    {
        ResponseConversao Converte(Moeda moedaOrigem, Moeda moedaDestino, double valor);
    }
}
