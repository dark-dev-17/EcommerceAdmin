using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAdmin2.Models.Documents;
using EcommerceAdmin2.Models.Sistema;
using EcommerceAdmin2.Models.Empleado;
using EcommerceAdmin2.Models.BussinesPartner;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace EcommerceAdmin2.Controllers
{
    public class DocumentController : Controller
    {
        private ResponseList<DocumentGeneral> responseList;
        private Response response;
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            return View();
        }
        public IActionResult ListDocCustomerByEmpl()
        {
            try
            {
                int USR_IdArea = (int)HttpContext.Session.GetInt32("USR_IdArea");
                int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
                string USR_Sociedad = (string)HttpContext.Session.GetString("USR_Sociedad");
                if (USR_IdArea == 3 && USR_Sociedad == "Fibremex" || USR_IdArea == 14 && USR_Sociedad == "Fibremex")
                {
                    DBMysql dBMysql = new DBMysql("Splinet");
                    dBMysql.OpenConnection();
                    DBSqlServer DBSqlServer = new DBSqlServer();
                    bool IsConnectionDB = DBSqlServer.OpenDataBaseAccess();
                    using (Empleado Empleado = new Empleado(dBMysql))
                    {
                        Empleado.GetEmpleado(USR_IdSplinnet);
                        if (Empleado.Id_sap.Count > 0)
                        {
                            List<BussinesPartner> ListBussinesPartner = new List<BussinesPartner>();
                            using (BussinesPartner BussinesPartner = new BussinesPartner(DBSqlServer))
                            {
                                BussinesPartner.GetBussinesPartnersBySalesEmp(Empleado.Id_sap, ListBussinesPartner);
                            }
                            DocumentGeneral DocumentGeneral = new DocumentGeneral(DBSqlServer);
                            responseList = new ResponseList<DocumentGeneral> { Code = 0, Description = "Autorization to access", Type = "Suscess", Records = new List<DocumentGeneral>() };
                            foreach (BussinesPartner item in ListBussinesPartner)
                            {
                                if (item.IsActiveEcomerce)
                                {
                                    DocumentGeneral.GetDocumentsByCustomer(responseList.Records, item.CardCode, item.CardName);
                                }
                            }
                        }
                        else
                        {
                            responseList = new ResponseList<DocumentGeneral> { Code = 0, Description = "No tiene id de sap en splinnet", Type = "Suscess" };
                        }
                    }
                    
                    dBMysql.CloseConnection();
                    DBSqlServer.CloseDataBaseAccess();
                    return Ok(responseList);
                }
                else
                {
                    response = new Response { Code = 100, Description = "No autorization to access", Type = "Danger" };
                    return NotFound(response);
                }
            }
            catch (DBException ex)
            {
                response = new Response { Code = 200, Description = ex.Message, Type = "Danger" };
                return BadRequest(response);
            }
            catch (MySqlException ex)
            {
                response = new Response { Code = 200, Description = ex.Message, Type = "Danger" };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response = new Response { Code = 200, Description = ex.Message, Type = "Danger" };
                return BadRequest(response);
            }
        }
    }
}