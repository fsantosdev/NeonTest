using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NeonTest.Models
{
    public class Response
    {
        private Response()
        {
        }

        public Response(bool status, int codigo, dynamic data)
        {
            this.Status = status;
            this.Codigo = codigo;
            this.Data = data;
        }

        public bool Status { get; private set; }
        public int Codigo { get; private set; }
        public dynamic Data { get; private set; }
    }
}
