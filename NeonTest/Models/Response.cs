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

        public Response(bool success, int code, dynamic data)
        {
            this.Success = success;
            this.Code = code;
            this.Data = data;
        }

        public bool Success { get; private set; }
        public int Code { get; private set; }
        public dynamic Data { get; private set; }
    }
}
