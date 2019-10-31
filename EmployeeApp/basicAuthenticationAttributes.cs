using EmployeeApp.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EmployeeApp
{
    public class basicAuthenticationAttributes:AuthorizationFilterAttribute
    {
       public override void OnAuthorization(HttpActionContext actionContext)
       {
           if(actionContext.Request.Headers.Authorization==null)
           {
               //actionContext.Response=actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);

           }
           else
           {
               string authoticationToken = actionContext.Request.Headers.Authorization.Parameter;
               string decodedAuthoticationToken= Encoding.UTF8.GetString(Convert.FromBase64String(authoticationToken));
               string[] UsernamePasswordArray = decodedAuthoticationToken.Split(':');
               string Username = UsernamePasswordArray[0];
               string Password = UsernamePasswordArray[1];
               if(EmployeeSecurity.Login(Username,Password))
               {
                   Thread.CurrentPrincipal=new GenericPrincipal(new GenericIdentity(Username),null);
               }
               else
               {
                 //  actionContext.Response = actionContext.Request
                 //.CreateResponse(HttpStatusCode.Unauthorized);  
               }
           }
       }

       
    }
}