using NeonTest.Models;
using System;

namespace NeonTest.Domain
{
    public class ResponseDomain
    {
        public static dynamic BuildResponse(dynamic _data) {
            Response response = null;
            try
            {
                dynamic responseConteudo = null;
                response = new Response(true, 200, responseConteudo);
            }
            catch (Exception e)
            {
                response = new Response(false, 400, e);
            }

            return response;
        }
    }
}
