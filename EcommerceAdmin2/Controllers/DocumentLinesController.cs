using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAdmin2.Models.Documents;
using EcommerceAdmin2.Models;
using EcommerceAdmin2.Models.Sistema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using EcommerceAdmin2.Models.Filters;
using EcommerceAdmin2.Models.Empleado;

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
        [AccessViewSession]
        public IActionResult Cotizaciones(int id)
        {
            int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
            using (Usuario usuario = new Usuario())
            {
                if (usuario.ValidPermisControlView(USR_IdSplinnet, 11))
                {
                    ViewData["DocEntry"] = id;
                    return View();
                }
                else if (usuario.ValidPermisControlView(USR_IdSplinnet, 12))
                {
                    ViewData["DocEntry"] = id;
                    return View();
                }
                else
                {
                    return RedirectToAction("NoAccess", "ErrorPages");
                }
            }
        }
        [AccessViewSession]
        public IActionResult List(int DocEntry, string DocType)
        {
            int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
            using (Usuario usuario = new Usuario())
            {
                if (usuario.ValidPermisControlView(USR_IdSplinnet, 15))
                {
                    ViewData["DocEntry"] = DocEntry;
                    ViewData["DocType"] = DocType;
                    return View();
                }
                else if (usuario.ValidPermisControlView(USR_IdSplinnet, 16))
                {
                    ViewData["DocEntry"] = DocEntry;
                    ViewData["DocType"] = DocType;
                    return View();
                }
                else
                {
                    return RedirectToAction("NoAccess", "ErrorPages");
                }
            }
        }
        [HttpPost]
        [AccessDataSession]
        public IActionResult DocumentLinesByCotizacion(int DocEntry)
        {
            try
            {
                bool AccessBySalesEmp = false;
                bool AccessAll = false;
                int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
                using (DBMysql dBMysql = new DBMysql("Splinet"))
                {
                    dBMysql.OpenConnection();
                    using (Usuario usuario = new Usuario(dBMysql))
                    {
                        // acceso a solo clientes por ejecutivo
                        AccessBySalesEmp = usuario.AccessToAction(USR_IdSplinnet, 11);
                        // acceso a todos los clientes
                        AccessAll = usuario.AccessToAction(USR_IdSplinnet, 12);
                    }
                    dBMysql.CloseConnection();
                    if (!AccessBySalesEmp && AccessAll || AccessBySalesEmp && !AccessAll)
                    {
                        using (DBMysql dBMysql1 = new DBMysql("Ecommerce"))
                        {
                            dBMysql1.OpenConnection();
                            responseList = new ResponseList<DocumentLinesGeneral>
                            {
                                Code = 0,
                                Description = "Autorization to access",
                                Type = "Suscess",
                                Records = new DocumentLinesGeneral(dBMysql1).GetDocumentLinesEcomerce((DocEntry + ""))
                            };
                            dBMysql1.CloseConnection();
                            return Ok(responseList);
                        }
                    }
                    else
                    {
                        return BadRequest("Sin acceso a ninguna sección, configuración de permisos erronea");
                    }
                }
            }
            catch (DBException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (MySqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult DocumentLinesByDocument(int DocEntry, string DocType)
        {
            try
            {
                bool AccessBySalesEmp = false;
                bool AccessAll = false;
                int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
                using (DBMysql dBMysql = new DBMysql("Splinet"))
                {
                    dBMysql.OpenConnection();
                    using (Usuario usuario = new Usuario(dBMysql))
                    {
                        // acceso a solo clientes por ejecutivo
                        AccessBySalesEmp = usuario.AccessToAction(USR_IdSplinnet, 15);
                        // acceso a todos los clientes
                        AccessAll = usuario.AccessToAction(USR_IdSplinnet, 16);
                    }
                    dBMysql.CloseConnection();
                    if (!AccessBySalesEmp && AccessAll || AccessBySalesEmp && !AccessAll)
                    {
                        DBSqlServer DBSqlServer = new DBSqlServer();
                        bool IsConnectionDB = DBSqlServer.OpenDataBaseAccess();
                        responseList = new ResponseList<DocumentLinesGeneral>
                        {
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
                        return BadRequest("Sin acceso a ninguna sección, configuración de permisos erronea");
                    }
                }
            }
            catch (DBException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (MySqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}