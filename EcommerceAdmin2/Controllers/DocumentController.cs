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
using Microsoft.AspNetCore.Mvc.Filters;
using EcommerceAdmin2.Models;
using EcommerceAdmin2.Models.Filters;

namespace EcommerceAdmin2.Controllers
{
    
    public class DocumentController : Controller
    {
        private ResponseList<DocumentGeneral> responseList;
        private Response response;
        [AccessViewSession]
        public IActionResult Index()
        {
            int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
            using (Usuario usuario = new Usuario())
            {
                if (usuario.ValidPermisControlView(USR_IdSplinnet, 15))
                {
                    return View();
                }
                else if (usuario.ValidPermisControlView(USR_IdSplinnet, 16))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("NoAccess", "ErrorPages");
                }
            }
        }
        [AccessViewSession]
        public IActionResult List()
        {
            int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
            using (Usuario usuario = new Usuario())
            {
                if (usuario.ValidPermisControlView(USR_IdSplinnet, 15))
                {
                    return View();
                }
                else if (usuario.ValidPermisControlView(USR_IdSplinnet, 16))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("NoAccess", "ErrorPages");
                }
            }
        }
        [AccessViewSession]
        public IActionResult History()
        {
            int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
            using (Usuario usuario = new Usuario())
            {
                if (usuario.ValidPermisControlView(USR_IdSplinnet, 20))
                {
                    return View();
                }
                else if (usuario.ValidPermisControlView(USR_IdSplinnet, 21))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("NoAccess", "ErrorPages");
                }
            }
        }
        [AccessViewSession]
        public IActionResult BussinessPartner(string id)
        {
            int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
            using (Usuario usuario = new Usuario())
            {
                if (usuario.ValidPermisControlView(USR_IdSplinnet, 15))
                {
                    ViewData["CardCode"] = id;
                    return View();
                }
                else if (usuario.ValidPermisControlView(USR_IdSplinnet, 16))
                {
                    ViewData["CardCode"] = id;
                    return View();
                }
                else
                {
                    return RedirectToAction("NoAccess", "ErrorPages");
                }
            }
        }
        [AccessViewSession]
        public IActionResult Tracker(string id)
        {
            int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
            using (Usuario usuario = new Usuario())
            {
                if (usuario.ValidPermisControlView(USR_IdSplinnet, 22))
                {
                    ViewData["DocEcommerce"] = id;
                    return View();
                }
                else
                {
                    return RedirectToAction("NoAccess", "ErrorPages");
                }
            }
        }
        [AccessViewSession]
        public IActionResult Rejected()
        {
            int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
            using (Usuario usuario = new Usuario())
            {
                if (usuario.ValidPermisControlView(USR_IdSplinnet, 13))
                {
                    return View();
                }
                else if (usuario.ValidPermisControlView(USR_IdSplinnet, 14))
                {
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
        public IActionResult DataList()
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
                    // obtener informacion de todos los clientes sin importar el ejecutivo
                    if (!AccessBySalesEmp && AccessAll)
                    {
                        responseList = new ResponseList<DocumentGeneral> { Code = 0, Description = "Autorization to access", Type = "Suscess", Records = new List<DocumentGeneral>() };
                        List<BussinesPartner> bussinesPartners = null;
                        using (DBSqlServer DBSqlServer = new DBSqlServer())
                        {
                            bool IsConnectionDB = DBSqlServer.OpenDataBaseAccess();
                            bussinesPartners = new BussinesPartner(DBSqlServer).SelectAllactive();
                            DocumentGeneral DocumentGeneral = new DocumentGeneral(DBSqlServer);
                            responseList.Records = DocumentGeneral.GetOrderInProccess();
                            dBMysql.CloseConnection();
                            DBSqlServer.CloseDataBaseAccess();
                        }
                        return Ok(responseList);
                    }
                    // obtener informacion de clientes por ejecutivo
                    else if (AccessBySalesEmp && !AccessAll)
                    {
                        responseList = new ResponseList<DocumentGeneral> { Code = 0, Description = "Autorization to access", Type = "Suscess", Records = new List<DocumentGeneral>() };
                        using (Empleado empleado = new Empleado(dBMysql))
                        {
                            empleado.GetEmpleado(USR_IdSplinnet);
                            empleado.GetIdSapDB(USR_IdSplinnet);
                            List<BussinesPartner> bussinesPartners = new List<BussinesPartner>(); ;
                            using (DBSqlServer DBSqlServer = new DBSqlServer())
                            {
                                DBSqlServer.OpenDataBaseAccess();
                                BussinesPartner bussinesPartner = new BussinesPartner(DBSqlServer);
                                bussinesPartner.GetBussinesPartnersBySalesEmp(empleado.Id_sap, bussinesPartners);
                                DocumentGeneral DocumentGeneral = new DocumentGeneral(DBSqlServer);
                                bussinesPartners.Where(p => p.IsActiveEcomerce && p.IsActive).ToList().ForEach(p => DocumentGeneral.GetOrderInProccess(p.CardCode, p.CardName).ForEach(aux => responseList.Records.Add(aux)));
                                DBSqlServer.CloseDataBaseAccess();
                            }
                            return Ok(responseList);
                        }
                    }
                    else
                    {
                        return BadRequest("Sin acceso a ninguna seccion, configuración de permisos erronea");
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
        public IActionResult DataBussinessPartner(string CardCode)
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
                        AccessBySalesEmp = usuario.AccessToAction(USR_IdSplinnet, 13);
                        // acceso a todos los clientes
                        AccessAll = usuario.AccessToAction(USR_IdSplinnet, 14);
                    }
                    if (!AccessBySalesEmp && AccessAll || AccessBySalesEmp && !AccessAll)
                    {

                        DBSqlServer DBSqlServer = new DBSqlServer();
                        bool IsConnectionDB = DBSqlServer.OpenDataBaseAccess();
                        responseList = new ResponseList<DocumentGeneral> { Code = 0, Description = "Autorization to access", Type = "Suscess", Records = new List<DocumentGeneral>() };
                        //Get informacion bp
                        DocumentGeneral DocumentGeneral = new DocumentGeneral(DBSqlServer);
                        BussinesPartner BussinesPartner = new BussinesPartner(DBSqlServer);
                        BussinesPartner.GetBussinesPartner(CardCode);
                        responseList.Records = DocumentGeneral.GetOrderInProccess(BussinesPartner.CardCode, BussinesPartner.CardName);

                        DBSqlServer.CloseDataBaseAccess();
                        BussinesPartner.Dispose();
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
        /// <summary>
        /// Obtiene documentos rechazados por clientes asignados al ejecutivo logeado o obtiene todos los pedidos rechazados
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AccessDataSession]
        public IActionResult DataRejected()
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
                        AccessBySalesEmp = usuario.AccessToAction(USR_IdSplinnet, 13);
                        // acceso a todos los clientes
                        AccessAll = usuario.AccessToAction(USR_IdSplinnet, 14);
                    }
                    // obtener informacion de todos los clientes sin importar el ejecutivo
                    if (!AccessBySalesEmp && AccessAll)
                    {
                        responseList = new ResponseList<DocumentGeneral> { Code = 0, Description = "Autorization to access", Type = "Suscess", Records = new List<DocumentGeneral>() };
                        using (DBSqlServer DBSqlServer = new DBSqlServer())
                        {
                            bool IsConnectionDB = DBSqlServer.OpenDataBaseAccess();
                            DocumentGeneral DocumentGeneral = new DocumentGeneral(DBSqlServer);
                            responseList.Records = DocumentGeneral.GetOrderRejectedAll();
                            dBMysql.CloseConnection();
                            DBSqlServer.CloseDataBaseAccess();
                        }
                        return Ok(responseList);
                    }
                    // obtener informacion de clientes por ejecutivo
                    else if (AccessBySalesEmp && !AccessAll)
                    {
                        responseList = new ResponseList<DocumentGeneral> { Code = 0, Description = "Autorization to access", Type = "Suscess", Records = new List<DocumentGeneral>() };
                        using (Empleado empleado = new Empleado(dBMysql))
                        {
                            empleado.GetEmpleado(USR_IdSplinnet);
                            empleado.GetIdSapDB(USR_IdSplinnet);
                            List<BussinesPartner> bussinesPartners = new List<BussinesPartner>(); ;
                            using (DBSqlServer DBSqlServer = new DBSqlServer())
                            {
                                DBSqlServer.OpenDataBaseAccess();
                                BussinesPartner bussinesPartner = new BussinesPartner(DBSqlServer);
                                bussinesPartner.GetBussinesPartnersBySalesEmp(empleado.Id_sap, bussinesPartners);
                                DocumentGeneral DocumentGeneral = new DocumentGeneral(DBSqlServer);
                                bussinesPartners.Where(p => p.IsActiveEcomerce && p.IsActive).ToList().ForEach(p => DocumentGeneral.GetDocumentsRejectedByCustomer(responseList.Records, p.CardCode, p.CardName));


                                DBSqlServer.CloseDataBaseAccess();
                            }
                            return Ok(responseList);
                        }
                    }
                    else
                    {
                        return BadRequest("Sin acceso a ninguna seccion, configuración de permisos erronea");
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
        [ValidateAntiForgeryToken]
        [AccessData(IdAction = 19)]
        public IActionResult DataGetOrderRejectedByCustomer(string CardCode)
        {
            try
            {
                DBSqlServer DBSqlServer = new DBSqlServer();
                bool IsConnectionDB = DBSqlServer.OpenDataBaseAccess();
                DocumentGeneral DocumentGeneral = new DocumentGeneral(DBSqlServer);
                List<DocumentGeneral> documents = DocumentGeneral.GetOrderRejectedByCustomer(CardCode);
                responseList = new ResponseList<DocumentGeneral> { Code = 0, Description = "Autorization to access", Type = "Suscess", Records = documents };
                DBSqlServer.CloseDataBaseAccess();
                return Ok(responseList);
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
        [ValidateAntiForgeryToken]
        [AccessData(IdAction = 19)]
        public IActionResult DataGetOrderHistoryByCustomer(string CardCode)
        {
            try
            {
                DBSqlServer DBSqlServer = new DBSqlServer();
                bool IsConnectionDB = DBSqlServer.OpenDataBaseAccess();
                DocumentGeneral DocumentGeneral = new DocumentGeneral(DBSqlServer);
                List<DocumentGeneral> documents = DocumentGeneral.GetOrderHistoryByCustomer(CardCode);
                responseList = new ResponseList<DocumentGeneral> { Code = 0, Description = "Autorization to access", Type = "Suscess", Records = documents };
                DBSqlServer.CloseDataBaseAccess();
                return Ok(responseList);
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
        /// <summary>
        /// Obtiene historico documentos  por clientes asignados al ejecutivo logeado o obtiene todos los pedidos rechazados
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AccessDataSession]
        public IActionResult DataOrderHistoric()
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
                        AccessBySalesEmp = usuario.AccessToAction(USR_IdSplinnet, 20);
                        // acceso a todos los clientes
                        AccessAll = usuario.AccessToAction(USR_IdSplinnet, 21);
                    }
                    // obtener informacion de todos los clientes sin importar el ejecutivo
                    if (!AccessBySalesEmp && AccessAll)
                    {
                        responseList = new ResponseList<DocumentGeneral> { Code = 0, Description = "Autorization to access", Type = "Suscess", Records = new List<DocumentGeneral>() };
                        using (DBSqlServer DBSqlServer = new DBSqlServer())
                        {
                            bool IsConnectionDB = DBSqlServer.OpenDataBaseAccess();
                            DocumentGeneral DocumentGeneral = new DocumentGeneral(DBSqlServer);
                            responseList.Records = DocumentGeneral.GetOrderHistoricAll();
                            dBMysql.CloseConnection();
                            DBSqlServer.CloseDataBaseAccess();
                        }
                        return Ok(responseList);
                    }
                    // obtener informacion de clientes por ejecutivo
                    else if (AccessBySalesEmp && !AccessAll)
                    {
                        responseList = new ResponseList<DocumentGeneral> { Code = 0, Description = "Autorization to access", Type = "Suscess", Records = new List<DocumentGeneral>() };
                        using (Empleado empleado = new Empleado(dBMysql))
                        {
                            empleado.GetEmpleado(USR_IdSplinnet);
                            empleado.GetIdSapDB(USR_IdSplinnet);
                            List<BussinesPartner> bussinesPartners = new List<BussinesPartner>(); ;
                            using (DBSqlServer DBSqlServer = new DBSqlServer())
                            {
                                DBSqlServer.OpenDataBaseAccess();
                                BussinesPartner bussinesPartner = new BussinesPartner(DBSqlServer);
                                bussinesPartner.GetBussinesPartnersBySalesEmp(empleado.Id_sap, bussinesPartners);
                                DocumentGeneral DocumentGeneral = new DocumentGeneral(DBSqlServer);
                                bussinesPartners.Where(p => p.IsActiveEcomerce && p.IsActive).ToList().ForEach(p => DocumentGeneral.GetOrderHistoryByCustomer(p.CardCode).ForEach(aux => responseList.Records.Add(aux)));


                                DBSqlServer.CloseDataBaseAccess();
                            }
                            return Ok(responseList);
                        }
                    }
                    else
                    {
                        return BadRequest("Sin acceso a ninguna seccion, configuración de permisos erronea");
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
        [AccessDataSession]
        [ValidateAntiForgeryToken]
        public IActionResult DataGetOrderStatus(int DocEcommerce)
        {
            
            try
            {
                using (DBSqlServer DBSqlServer = new DBSqlServer())
                {
                    bool IsConnectionDB = DBSqlServer.OpenDataBaseAccess();
                    DocumentGeneral DocumentGeneral = new DocumentGeneral(DBSqlServer);
                    bool result = DocumentGeneral.DataGetOrderStatus(DocEcommerce);
                    DBSqlServer.CloseDataBaseAccess();
                    if (result)
                    {
                        return Ok(DocumentGeneral);
                    }
                    else
                    {
                        throw new DBException("No existe el pedido Ecommerce seleccionado");
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
        }
    }
}