using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAdmin2.Models.Sistema;
using EcommerceAdmin2.Models.Empleado;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
using EcommerceAdmin2.Models.BussinesPartner;

namespace EcommerceAdmin2.Controllers
{
    public class BussinesPartnerController : Controller
    {
        private ResponseList<BussinesPartner> responseList;
        private Response response;
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            return View();
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ListUserLoged()
        {
            
            int USR_IdArea = (int)HttpContext.Session.GetInt32("USR_IdArea");
            int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
            string USR_Sociedad =(string)HttpContext.Session.GetString("USR_Sociedad");
            
            if (USR_IdArea == 3 && USR_Sociedad == "Fibremex" || USR_IdArea == 14 && USR_Sociedad == "Fibremex")
            {
                using (DBSqlServer DBSqlServer = new DBSqlServer())
                {
                    bool IsConnectionDB = DBSqlServer.OpenDataBaseAccess();
                    if (IsConnectionDB)
                    {
                        using (DBMysql dBMysql = new DBMysql())
                        {
                            dBMysql.OpenConnection();
                            if (dBMysql.Connection != null)
                            {
                                using (Empleado Empleado = new Empleado(dBMysql))
                                {
                                    Empleado.GetEmpleado(USR_IdSplinnet);
                                    if(Empleado.Id_sap.Count > 0)
                                    {
                                        responseList = new ResponseList<BussinesPartner> { Code = 0, Description = "Autorization to access", Type = "Suscess", Records = new List<BussinesPartner>()  };
                                        using (BussinesPartner BussinesPartner = new BussinesPartner(DBSqlServer))
                                        {
                                            foreach (int idSap in Empleado.Id_sap)
                                            {
                                                BussinesPartner.GetBussinesPartnersBySalesEmp(idSap, responseList.Records);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        responseList = new ResponseList<BussinesPartner> { Code = 0, Description = "No tiene id de sap en splinnet", Type = "Suscess" };
                                    }
                                    dBMysql.CloseConnection();
                                    return Ok(responseList);
                                }
                                
                            }
                            else
                            {
                                response = new Response { Code = 200, Description = "Somthing happened, please try again!!", Type = "Danger" };
                                dBMysql.CloseConnection();
                                return NotFound(response);
                            }
                        }
                    }
                    else
                    {
                        response = new Response { Code = 200, Description = "Somthing happened, please try again!!", Type = "Danger" };
                        return NotFound(response);
                    }
                }
                
            }
            else
            {
                response = new Response { Code = 100, Description = "No autorization to access", Type = "Danger" };
                return NotFound(response);
            }
        }
    }
}