using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Loja_N2.Controllers
{
    public class HelperControllers
    {
        public static Boolean VerificaUserLogado(ISession session)
        {
            string logado = session.GetString("Client");
            string admLogado = session.GetString("Staff");

            if (logado == null && admLogado == null)
                return false;
            else
                return true;
        }
    }
}
