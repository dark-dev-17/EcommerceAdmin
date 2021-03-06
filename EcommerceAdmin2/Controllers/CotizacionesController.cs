﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAdmin2.Models;
using EcommerceAdmin2.Models.BussinesPartner;
using EcommerceAdmin2.Models.Cotizaciones;
using EcommerceAdmin2.Models.Documents;
using EcommerceAdmin2.Models.Empleado;
using EcommerceAdmin2.Models.Filters;
using EcommerceAdmin2.Models.Sistema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace EcommerceAdmin2.Controllers
{

    public class CotizacionesController : Controller
    {
        private ResponseList<DocumentGeneral> responseList;
        private Response response;
        [AccessViewSession]
        public IActionResult Index()
        {
            int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
            using (Usuario usuario = new Usuario())
            {
                if (usuario.ValidPermisControlView(USR_IdSplinnet, 11))
                {
                    return View();
                }
                else if (usuario.ValidPermisControlView(USR_IdSplinnet, 12))
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
                if (usuario.ValidPermisControlView(USR_IdSplinnet, 11))
                {
                    return View();
                }
                else if (usuario.ValidPermisControlView(USR_IdSplinnet, 12))
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
                if (usuario.ValidPermisControlView(USR_IdSplinnet, 11))
                {
                    ViewData["CardCode"] = id;
                    return View();
                }
                else if (usuario.ValidPermisControlView(USR_IdSplinnet, 12))
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
                        AccessBySalesEmp = usuario.AccessToAction(USR_IdSplinnet, 11);
                        // acceso a todos los clientes
                        AccessAll = usuario.AccessToAction(USR_IdSplinnet, 12);
                    }
                    
                    // obtener informacion de todos los clientes sin importar el ejecutivo
                    if (!AccessBySalesEmp && AccessAll)
                    {
                        ResponseList<Cotizacion>  responseLis = new ResponseList<Cotizacion> { Code = 0, Description = "Autorization to access", Type = "Suscess", Records = new List<Cotizacion>() };
                        using (DBMysql dBMysql1 = new DBMysql("Ecommerce"))
                        {
                            dBMysql1.OpenConnection();
                            Cotizacion cotizacion = new Cotizacion(dBMysql1);
                            responseLis.Records = cotizacion.GetQuoatations();
                            dBMysql1.CloseConnection();
                        }
                        dBMysql.CloseConnection();
                        return Ok(responseLis);
                    }
                    // obtener informacion de clientes por ejecutivo
                    else if (AccessBySalesEmp && !AccessAll)
                    {
                        ResponseList<Cotizacion> responseLis = new ResponseList<Cotizacion> { Code = 0, Description = "Autorization to access", Type = "Suscess", Records = new List<Cotizacion>() };
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
                                using (DBMysql dBMysql1 = new DBMysql("Ecommerce"))
                                {
                                    dBMysql1.OpenConnection();
                                    Cotizacion cotizacion = new Cotizacion(dBMysql1);
                                    bussinesPartners.Where(bp => bp.IsActive && bp.IsActiveEcomerce).ToList().ForEach(bp =>
                                    {
                                        cotizacion.GetQuoatations(bp.CardCode).ForEach(aux => responseLis.Records.Add(aux));
                                    });
                                    dBMysql1.CloseConnection();
                                }
                                DBSqlServer.CloseDataBaseAccess();
                            }
                            dBMysql.CloseConnection();
                            return Ok(responseLis);
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
                        AccessBySalesEmp = usuario.AccessToAction(USR_IdSplinnet, 11);
                        // acceso a todos los clientes
                        AccessAll = usuario.AccessToAction(USR_IdSplinnet, 12);
                    }
                    dBMysql.CloseConnection();
                    if (!AccessBySalesEmp && AccessAll || AccessBySalesEmp && !AccessAll)
                    {

                        DBSqlServer DBSqlServer = new DBSqlServer();
                        bool IsConnectionDB = DBSqlServer.OpenDataBaseAccess();
                        responseList = new ResponseList<DocumentGeneral> { Code = 0, Description = "Autorization to access", Type = "Suscess", Records = new List<DocumentGeneral>() };
                        //Get informacion bp
                        BussinesPartner BussinesPartner = new BussinesPartner(DBSqlServer);
                        BussinesPartner.GetBussinesPartner(CardCode);
                        using (DBMysql dBMysql1 = new DBMysql("Ecommerce"))
                        {
                            dBMysql1.OpenConnection();
                            DocumentGeneral DocumentGeneral = new DocumentGeneral(dBMysql1);
                            
                            DocumentGeneral.GetSalesQuotationEcomerce(responseList.Records, BussinesPartner.CardCode, BussinesPartner.CardName);
                            
                            dBMysql1.CloseConnection();
                        }

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
        [HttpPost]
        [AccessData(IdAction = 18)]
        public IActionResult DataGetNoCotizaciones(DateTime start, DateTime end, string ModeBussiness)
        {
            try
            {
                using (DBMysql dBMysql1 = new DBMysql("Ecommerce"))
                {
                    //open connection ton database ecommerce
                    dBMysql1.OpenConnection();
                    //start object cotizaciones with the connection starts 
                    Cotizacion DocumentGeneral = new Cotizacion(dBMysql1);
                    int total = DocumentGeneral.GetNoCotizaciones(start, end, ModeBussiness);
                    dBMysql1.CloseConnection();
                    ResponseInt responseInt = new ResponseInt { Code = 0, Description = "Informacion obtenida", Type = "success", Value = total };
                    return Ok(responseInt);
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
        [AccessData(IdAction = 18)]
        public IActionResult DataGetNoPedidos(DateTime start, DateTime end, string ModeBussiness)
        {
            try
            {
                using (DBMysql dBMysql1 = new DBMysql("Ecommerce"))
                {
                    //open connection ton database ecommerce
                    dBMysql1.OpenConnection();
                    //start object cotizaciones with the connection starts 
                    Cotizacion DocumentGeneral = new Cotizacion(dBMysql1);
                    int total = DocumentGeneral.GetNoPedidos(start, end, ModeBussiness);
                    dBMysql1.CloseConnection();
                    ResponseInt responseInt = new ResponseInt { Code = 0, Description = "Informacion obtenida", Type = "success", Value = total };
                    return Ok(responseInt);
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
        [AccessData(IdAction = 18)]
        public IActionResult DataGetTotalPedidos(DateTime start, DateTime end, string Currency, string ModeBussiness)
        {
            try
            {
                using (DBMysql dBMysql1 = new DBMysql("Ecommerce"))
                {
                    //open connection ton database ecommerce
                    dBMysql1.OpenConnection();
                    //start object cotizaciones with the connection starts 
                    Cotizacion DocumentGeneral = new Cotizacion(dBMysql1);
                    double total = DocumentGeneral.GetTotalPedidos(start, end, Currency, ModeBussiness);
                    dBMysql1.CloseConnection();
                    ResponseDouble responseInt = new ResponseDouble { Code = 0, Description = "Informacion obtenida", Type = "success", Value = total };
                    return Ok(responseInt);
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
        [AccessData(IdAction = 18)]
        public IActionResult DataGetOrdersInPendingByBP(string CardCode)
        {
            try
            {
                using (DBMysql dBMysql1 = new DBMysql("Ecommerce"))
                {
                    //open connection ton database ecommerce
                    dBMysql1.OpenConnection();
                    //start object cotizaciones with the connection starts 
                    Cotizacion cotizacion = new Cotizacion(dBMysql1);
                    List<Cotizacion> responses = cotizacion.GetOrdersInPendingByBP(CardCode);
                    dBMysql1.CloseConnection();
                    ResponseList<Cotizacion> responseCot = new ResponseList<Cotizacion> { Code = 0, Description = "Informacion obtenida", Type = "success", Records = responses };
                    return Ok(responseCot);
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
        [AccessData(IdAction = 18)]
        public IActionResult DataGetOrdersInPending()
        {
            try
            {
                using (DBMysql dBMysql1 = new DBMysql("Ecommerce"))
                {
                    //open connection ton database ecommerce
                    dBMysql1.OpenConnection();
                    //start object cotizaciones with the connection starts 
                    Cotizacion cotizacion = new Cotizacion(dBMysql1);
                    List<Cotizacion> responses = cotizacion.GetOrdersInPending();
                    dBMysql1.CloseConnection();
                    ResponseList<Cotizacion> responseCot = new ResponseList<Cotizacion> { Code = 0, Description = "Informacion obtenida", Type = "success", Records = responses };
                    return Ok(responseCot);
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
        [AccessDataSession]
        public IActionResult DataGetOrdersById(int DocNumEcommerce)
        {
            try
            {
                using (DBMysql dBMysql1 = new DBMysql("Ecommerce"))
                {
                    //open connection ton database ecommerce
                    dBMysql1.OpenConnection();
                    //start object cotizaciones with the connection starts 
                    Cotizacion cotizacion = new Cotizacion(dBMysql1);
                    bool result = cotizacion.GetById(DocNumEcommerce);
                    dBMysql1.CloseConnection();
                    if (result)
                    {
                        Response<Cotizacion> responseCot = new Response<Cotizacion> { Code = 0, Description = "Informacion obtenida", Type = "success", Objeto = cotizacion };
                        return Ok(responseCot);
                    }
                    else
                    {
                        throw new Exception("No existe el pedido");
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