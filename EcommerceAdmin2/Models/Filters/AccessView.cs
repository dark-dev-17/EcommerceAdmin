using EcommerceAdmin2.Models.Empleado;
using EcommerceAdmin2.Models.Sistema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAdmin2.Models.Filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AccessView : ActionFilterAttribute
    {
        public int IdAction { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session.IsAvailable && filterContext.HttpContext.Session.GetInt32("USR_IdSplinnet") != null)
            {
                int USR_IdSplinnet = (int)filterContext.HttpContext.Session.GetInt32("USR_IdSplinnet");
                if(IdAction == 0 )
                {
                    if(USR_IdSplinnet != 48)
                    {
                        filterContext.Result = new RedirectResult("~/ErrorPages/NoAccess");
                        return;
                    }
                }
                else
                {
                    DBMysql dBMysql = new DBMysql("Splinet");
                    dBMysql.OpenConnection();
                    using (Usuario Usuario_ = new Usuario(dBMysql))
                    {
                        bool PermissAction = Usuario_.AccessToAction(USR_IdSplinnet, IdAction);
                        if (!PermissAction)
                        {
                            filterContext.Result = new RedirectResult("~/ErrorPages/NoAccess");
                            return;
                        }
                    }
                    dBMysql.CloseConnection();
                }
            }
            else
            {
                filterContext.Result = new RedirectResult("~/");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }
    }
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AccessViewSession : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session.IsAvailable && filterContext.HttpContext.Session.GetInt32("USR_IdSplinnet") != null)
            {
                
            }
            else
            {
                filterContext.Result = new RedirectResult("~/");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }
    }
}
