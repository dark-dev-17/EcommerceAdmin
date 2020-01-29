using EcommerceAdmin2.Models.Sistema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAdmin2.Models
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class FilterSessionValid : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session.IsAvailable && filterContext.HttpContext.Session.GetInt32("USR_IdSplinnet") != null)
            {

                int USR_IdArea = (int)filterContext.HttpContext.Session.GetInt32("USR_IdArea");
                int USR_IdSplinnet = (int)filterContext.HttpContext.Session.GetInt32("USR_IdSplinnet");
                string USR_Sociedad = (string)filterContext.HttpContext.Session.GetString("USR_Sociedad");
                //if ((string)filterContext.RouteData.Values["Controller"] == "Login")
                //{
                //    filterContext.Result = new RedirectResult("~/Home/");
                //    return;
                //}
            }
            else
            {
                filterContext.Result = new RedirectResult("~/Login/");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }
        protected bool ValidUserLoged(int USR_IdArea, int USR_IdSplinnet, string USR_Sociedad)
        {
            return true;
        }
    }
}