using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace LOF.Models
{
    public class AuthenticationSampleFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        private bool auth;

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            auth = (filterContext.ActionDescriptor.GetCustomAttributes(typeof(OverrideAuthenticationAttribute),true).Length==0);
           // throw new NotImplementedException( );
       
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            var user = filterContext.HttpContext.User;
            if(user == null || !user.Identity.IsAuthenticated)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
           // throw new NotImplementedException();
        }
    }
}