using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using EcommerceAdmin2.Models;
using EcommerceAdmin2.Models.Empleado;
using EcommerceAdmin2.Models.Filters;
using EcommerceAdmin2.Models.Produto;
using EcommerceAdmin2.Models.Sistema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace EcommerceAdmin2.Controllers
{
    public class ProductoController : Controller
    {
        private ResponseList<Articulos> responseList;
        private Response response;
        /// <summary>
        /// Ver pagina index
        /// </summary>
        /// <returns>view</returns>
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// ver vista para lista de articulos
        /// </summary>
        /// <returns>view</returns>
        [AccessView(IdAction = 1)]
        public IActionResult List()
        {
            return View();
        }
        /// <summary>
        /// ver vista para detalle de articulos
        /// </summary>
        /// <param name="id">ItemCode del articulo a ver detalles</param>
        /// <returns>view</returns>
        [AccessView(IdAction = 1)]
        public IActionResult Show(string id)
        {
            ViewData["ItemCode"] = id;
            return View();
        }
        /// <summary>
        /// vista para el manejo de la informacion del producto seleccionado
        /// </summary>
        /// <returns>view</returns>
        [HttpPost]
        [AccessData(IdAction = 1)]
        public IActionResult DataList()
        {
            try
            {
                responseList = new ResponseList<Articulos> { Code = 0, Description = "Autorization to access", Type = "Suscess" };
                // conectar abase de  datos splittel
                DBMysql dBMysql = new DBMysql("Ecommerce");
                dBMysql.OpenConnection();
                Articulos articulos = new Articulos(dBMysql);
                // obtener articulos dados de alta en ecommercce
                responseList.Records = articulos.SelectAll();
                dBMysql.CloseConnection();
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
        /// activa o desactiva un producto
        /// </summary>
        /// <param name="Active">status item</param>
        /// <param name="ItemCode">Codigo del producto a cambiar estatus</param>
        /// <returns>json</returns>
        [HttpPost]
        [AccessData(IdAction = 2)]
        public IActionResult DataactiveItem(bool Active, string ItemCode)
        {
            try
            {
                // conectar abase de  datos splittel
                DBMysql dBMysql = new DBMysql("Ecommerce");
                dBMysql.OpenConnection();
                Articulos articulos = new Articulos(dBMysql);
                // obtener articulos dados de alta en ecommercce
                bool result = articulos.ActiveEcomerce(Active, ItemCode);
                if (result)
                {
                    response = new Response { Code = 0, Description = "Autorization to access", Type = "Suscess" };
                }
                else
                {
                    response = new Response { Code = 100, Description = "Sin cambio de estatus", Type = "Suscess" };
                }
                dBMysql.CloseConnection();
                return Ok(response);
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
        /// Obtener lista de articulos por medio de solicitudes post
        /// </summary>
        /// <param name="ItemCode"></param>
        /// <returns>json</returns>
        [HttpPost]
        [AccessData(IdAction = 1)]
        public IActionResult DataShow(string ItemCode)
        {
            Response<Articulos> Respons = null;
            try
            {
                // conectar abase de  datos splittel
                DBMysql dBMysql = new DBMysql("Ecommerce");
                dBMysql.OpenConnection();
                Articulos articulos = new Articulos(dBMysql);
                // obtener articulos dados de alta en ecommercce
                if (articulos.FindByID(ItemCode))
                {
                    Respons = new Response<Articulos> { Code = 0, Description = "Autorization to access", Type = "Suscess", Objeto = articulos };
                }
                else
                {
                    Respons = new Response<Articulos> { Code = 0, Description = "Producto no encontrado", Type = "Suscess" };
                }
                dBMysql.CloseConnection();
                return Ok(Respons);
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
        /// obtener listado de imagenes del preview del producto
        /// </summary>
        /// <param name="ItemCode">Codigo del producto</param>
        /// <returns>json</returns>
        [HttpPost]
        [AccessData(IdAction = 1)]
        public IActionResult DataGetFilesItem(string ItemCode)
        {
            try
            {
                List<Files> list = new Files(@"public_html/store/public/images/img_spl/productos/" + ItemCode + "/", @"/store/public/images/img_spl/productos/" + ItemCode + "/").Getfiles("*.jpg");
                ResponseList<Files> responseLst =  new ResponseList<Files> { Code = 0, Description = "Producto no encontrado", Type = "Suscess", Records = list };
                return Ok(responseLst);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// eliminar imagen del producto seleccionado
        /// </summary>
        /// <param name="Filename">Nombre del archivo</param>
        /// <param name="ItemCode">Codigo del prodcuto de la imagen</param>
        /// <param name="ImagessType">Tipo de imagen a eliminar</param>
        /// <returns>json</returns>
        [HttpPost]
        public IActionResult DataDeleteFile(string Filename,string ItemCode,string ImagessType)
        {
            try
            {
                if (ImagessType.Trim() == "Producto")
                {
                    ValidSessionsActions(7);
                    new Files().DeleteFile(string.Format(@"public_html/store/public/images/img_spl/productos/{0}/{1}", ItemCode, Filename));
                    response = new Response { Code = 0, Description = "Archivo eliminado", Type = "Suscess" };
                    return Ok(response);
                }
                else if (ImagessType.Trim() == "itemDescription")
                {
                    ValidSessionsActions(6);
                    new Files().DeleteFile(string.Format(@"public_html/store/public/images/img_spl/productos/{0}/descripcion/{1}", ItemCode, Filename));
                    response = new Response { Code = 0, Description = "Archivo eliminado", Type = "Suscess" };
                    return Ok(response);
                }
                else if (ImagessType.Trim() == "itemAdditionalInfo")
                {
                    ValidSessionsActions(5);
                    new Files().DeleteFile(string.Format(@"public_html/store/public/images/img_spl/productos/{0}/adicional/{1}", ItemCode, Filename));
                    response = new Response { Code = 0, Description = "Archivo eliminado", Type = "Suscess" };
                    return Ok(response);
                }
                else
                {
                    return BadRequest("el tipo de imagen a eliminar no existe");
                }
            }
            catch (DBException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Cargar nueva imagen al producto
        /// </summary>
        /// <param name="FormFile">archivo nuevo a subir</param>
        /// <param name="ItemCode">Codigo del producto</param>
        /// <param name="ImagessType">Tipo de imagen a subir</param>
        /// <returns>json</returns>
        [HttpPost]
        public IActionResult DataUpdateFile(IFormFile FormFile,string ItemCode, string ImagessType)
        {
            try
            {
                var Filename = FormFile.FileName; //we are using Temp file name just for the example. Add your own file path.
                //string fileName = System.IO.Path.GetFileName(FormFile.FileName);
                if(ImagessType.Trim() == "Producto")
                {
                    ValidSessionsActions(7);
                    new Files().UpdateFile(string.Format(@"public_html/store/public/images/img_spl/productos/{0}/{1}", ItemCode, Filename), FormFile);
                    response = new Response { Code = 0, Description = "Archivo cargado", Type = "Suscess" };
                    return Ok(response);
                }
                if (ImagessType.Trim() == "itemDescription")
                {
                    ValidSessionsActions(6);
                    new Files().UpdateFile(string.Format(@"public_html/store/public/images/img_spl/productos/{0}/descripcion/{1}", ItemCode, Filename), FormFile);
                    response = new Response { Code = 0, Description = "Archivo cargado", Type = "Suscess" };
                    return Ok(response);
                }
                if (ImagessType.Trim() == "itemAdditionalInfo")
                {
                    ValidSessionsActions(5);
                    new Files().UpdateFile(string.Format(@"public_html/store/public/images/img_spl/productos/{0}/adicional/{1}", ItemCode, Filename), FormFile);
                    response = new Response { Code = 0, Description = "Archivo cargado", Type = "Suscess" };
                    return Ok(response);
                }
                else
                {
                    return BadRequest("el tipo de imagen a subir no existe");
                }
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        /// <summary>
        /// modificar descripcion larga del prodcuto
        /// </summary>
        /// <param name="ItemCode">Codigo del producto seleccionado</param>
        /// <param name="LargeDescription">Descripcion nueva del producto</param>
        /// <returns>json</returns>
        [HttpPost]
        [AccessData(IdAction = 4)]
        public IActionResult DataUpdateLargeDescription(string ItemCode, string LargeDescription)
        {
            try
            {
                // conectar abase de  datos splittel
                DBMysql dBMysql = new DBMysql("Ecommerce");
                dBMysql.OpenConnection();
                Articulos articulos = new Articulos(dBMysql);
                // obtener articulos dados de alta en ecommercce
                if (articulos.UpdateLargeDescription(ItemCode, LargeDescription))
                {
                    response = new Response{ Code = 0, Description = "Autorization to access", Type = "Suscess"};
                }
                else
                {
                    response = new Response { Code = 0, Description = "Producto no encontrado", Type = "Suscess" };
                }
                dBMysql.CloseConnection();
                return Ok(response);
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
        /// Obtener listado de imagenes acerca de informacion adicional del producto
        /// </summary>
        /// <param name="ItemCode">Codigo del producto</param>
        /// <returns>json</returns>
        [HttpPost]
        [AccessData(IdAction = 1)]
        public IActionResult DataGetFilesItemAditionalInfo(string ItemCode)
        {
            try
            {
                List<Files> list = new Files(@"public_html/store/public/images/img_spl/productos/" + ItemCode + "/adicional/", @"/store/public/images/img_spl/productos/" + ItemCode + "/adicional/").Getfiles("*.jpg");
                ResponseList<Files> responseLst = new ResponseList<Files> { Code = 0, Description = "Producto encontrado", Type = "Suscess", Records = list };
                return Ok(responseLst);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Obtener listado de imagens de descripcion del producto
        /// </summary>
        /// <param name="ItemCode">Codigo del producto</param>
        /// <returns>json</returns>
        [HttpPost]
        [AccessData(IdAction = 1)]
        public IActionResult DataGetFilesItemDescription(string ItemCode)
        {
            try
            {
                List<Files> list = new Files(@"public_html/store/public/images/img_spl/productos/" + ItemCode + "/descripcion/", @"/store/public/images/img_spl/productos/" + ItemCode + "/descripcion/").Getfiles("*.jpg");
                ResponseList<Files> responseLst = new ResponseList<Files> { Code = 0, Description = "Producto encontrado", Type = "Suscess", Records = list };
                return Ok(responseLst);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="ModeBussiness"></param>
        /// <returns></returns>
        [HttpPost]
        [AccessData(IdAction = 18)]
        public IActionResult DataGetarticulosTo5CotizacionesEcom(DateTime start, DateTime end, string ModeBussiness)
        {
            try
            {
                // conectar abase de  datos splittel
                DBMysql dBMysql = new DBMysql("Ecommerce");
                dBMysql.OpenConnection();
                Articulos articulos = new Articulos(dBMysql);
                // obtener articulos dados de alta en ecommercce
                List<Articulos> result = articulos.GetarticulosTo5CotizacionesEcom(ModeBussiness);
                responseList = new ResponseList<Articulos> { Code = 0, Description = "Autorization to access", Type = "Suscess", Records  = result };
                dBMysql.CloseConnection();
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
        [AccessData(IdAction = 18)]
        public IActionResult GetTopCategoria(DateTime start, DateTime end, string ModeBussiness)
        {
            try
            {
                // conectar abase de  datos splittel
                DBMysql dBMysql = new DBMysql("Ecommerce");
                dBMysql.OpenConnection();
                Categoria articulos = new Categoria(dBMysql);
                // obtener articulos dados de alta en ecommercce
                List<Categoria> result = articulos.GetTopCategoria( start,  end, ModeBussiness);
                ResponseList<Categoria> responseListq = new ResponseList<Categoria> { Code = 0, Description = "Autorization to access", Type = "Suscess", Records = result };
                dBMysql.CloseConnection();
                return Ok(responseListq);
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
        private void ValidSessionsActions(int IdAction)
        {
            try
            {
                if (HttpContext.Session.IsAvailable && HttpContext.Session.GetInt32("USR_IdSplinnet") != null)
                {
                    int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
                    DBMysql dBMysql = new DBMysql("Splinet");
                    dBMysql.OpenConnection();
                    using (Usuario Usuario_ = new Usuario(dBMysql))
                    {
                        bool PermissAction = Usuario_.AccessToAction(USR_IdSplinnet, IdAction);
                        if (!PermissAction)
                        {
                            throw new DBException("Sin permisos para ejecutar esta acción");
                        }
                    }
                    dBMysql.CloseConnection();
                }
                else
                {
                    throw new DBException("Sin sessión activa");
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}