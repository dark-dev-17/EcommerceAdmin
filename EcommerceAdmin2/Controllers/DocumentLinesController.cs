using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAdmin2.Models.Documents;
using EcommerceAdmin2.Models.Sistema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace EcommerceAdmin2.Controllers
{
    public class DocumentLinesController : Controller
    {
        private ResponseList<DocumentLinesGeneral> responseList;
        private Response response;
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Cotizaciones(int id)
        {
            int USR_IdArea = (int)HttpContext.Session.GetInt32("USR_IdArea");
            int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
            string USR_Sociedad = (string)HttpContext.Session.GetString("USR_Sociedad");
            if (USR_IdArea == 3 && USR_Sociedad == "Fibremex" || USR_IdArea == 14 && USR_Sociedad == "Fibremex")
            {
                ViewData["DocEntry"] = id;
                return View();
            }
            else
            {
                return new ContentResult()
                {
                    Content = "<h1>Sin acceso a esta sección</h1>",
                    ContentType = "text/html",
                };
            }
        }
        public IActionResult List(int DocEntry, string DocType)
        {
            int USR_IdArea = (int)HttpContext.Session.GetInt32("USR_IdArea");
            int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
            string USR_Sociedad = (string)HttpContext.Session.GetString("USR_Sociedad");
            if (USR_IdArea == 3 && USR_Sociedad == "Fibremex" || USR_IdArea == 14 && USR_Sociedad == "Fibremex")
            {
                ViewData["DocEntry"] = DocEntry;
                ViewData["DocType"] = DocType;
                return View();
            }
            else
            {
                return new ContentResult()
                {
                    Content = "<h1>Sin acceso a esta sección</h1>",
                    ContentType = "text/html",
                };
            }
        }
        [HttpPost]
        public IActionResult DocumentLinesByCotizacion(int DocEntry)
        {
            try
            {
                int USR_IdArea = (int)HttpContext.Session.GetInt32("USR_IdArea");
                int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
                string USR_Sociedad = (string)HttpContext.Session.GetString("USR_Sociedad");
                if (USR_IdArea == 3 && USR_Sociedad == "Fibremex" || USR_IdArea == 14 && USR_Sociedad == "Fibremex")
                {
                    // conectar a base de datos sap 
                    DBMysql dBMysql = new DBMysql("Ecommerce");
                    dBMysql.OpenConnection();
                    responseList = new ResponseList<DocumentLinesGeneral>
                    {
                        Code = 0,
                        Description = "Autorization to access",
                        Type = "Suscess",
                        Records = new DocumentLinesGeneral(dBMysql).GetDocumentLinesEcomerce((DocEntry + ""))
                    };
                    dBMysql.CloseConnection();
                    dBMysql.Dispose();
                    return Ok(responseList);
                }
                else
                {
                    response = new Response { Code = 100, Description = "No access", Type = "Danger" };
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
        [HttpPost]
        public IActionResult DocumentLinesByDocument(int DocEntry, string DocType)
        {
            try
            {
                int USR_IdArea = (int)HttpContext.Session.GetInt32("USR_IdArea");
                int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
                string USR_Sociedad = (string)HttpContext.Session.GetString("USR_Sociedad");
                if (USR_IdArea == 3 && USR_Sociedad == "Fibremex" || USR_IdArea == 14 && USR_Sociedad == "Fibremex")
                {
                    // conectar a base de datos sap 
                    DBSqlServer DBSqlServer = new DBSqlServer();
                    bool IsConnectionDB = DBSqlServer.OpenDataBaseAccess();
                    responseList = new ResponseList<DocumentLinesGeneral> {
                        Code = 0,
                        Description = "Autorization to access",
                        Type = "Suscess",
                        Records = new DocumentLinesGeneral(DBSqlServer).GetDocumentLines((DocEntry + ""), DocType)
                    };
                    DBSqlServer.CloseDataBaseAccess();
                    DBSqlServer.Dispose();
                    return Ok(responseList);
                }
                else
                {
                    response = new Response { Code = 100, Description = "No access", Type = "Danger" };
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