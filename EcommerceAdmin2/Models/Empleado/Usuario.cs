using EcommerceAdmin2.Models.Sistema;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAdmin2.Models.Empleado
{
    public class Usuario : IDisposable
    {
        #region Propiedades
        [Required]
        public string User { get; set; }
        [Required]
        public string Contrasena { get; set; }
        private DBMysql DBMysql;
        public Response Response { get; private set; }
        private Empleado Empleado;
        public int Id { get; private set; }
        #endregion
        #region Constructores
        public Usuario()
        {

        }
        public Usuario(DBMysql DBMysql)
        {
            this.DBMysql = DBMysql;
        }
        #endregion
        #region Metodos
        public int DoLogin()
        {
            string Statement = string.Format("SELECT ID FROM signup where username = '{0}' and password = '{1}';", User, Contrasena);
            try
            {
                MySqlDataReader DataReader = DBMysql.DoQuery(Statement);
                if (DBMysql.CountDataReader(DataReader) == 1)
                {
                    Response = new Response { Code = 0, Description = "Login exitoso", Type = "Success" };
                    Id = (int)DataReader.GetUInt32(0);
                    
                    DataReader.Close();
                    return 0;
                }
                else
                {
                    Response = new Response { Code = 10, Description = "Login no exitoso", Type = "Success" };
                    return 10;
                }
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public int GetId()
        {
            return Id;
        }
        public void SetConnectionMysql(DBMysql DBMysql)
        {
            this.DBMysql = DBMysql;
        }
        public void SetMessage(Response Response)
        {
            this.Response = Response;
        }

        #region IDisposable Support
        private bool disposedValue = false; // Para detectar llamadas redundantes

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: elimine el estado administrado (objetos administrados).
                }

                // TODO: libere los recursos no administrados (objetos no administrados) y reemplace el siguiente finalizador.
                // TODO: configure los campos grandes en nulos.

                disposedValue = true;
            }
        }

        // TODO: reemplace un finalizador solo si el anterior Dispose(bool disposing) tiene código para liberar los recursos no administrados.
        // ~Usuario()
        // {
        //   // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
        //   Dispose(false);
        // }

        // Este código se agrega para implementar correctamente el patrón descartable.
        public void Dispose()
        {
            // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
            Dispose(true);
            // TODO: quite la marca de comentario de la siguiente línea si el finalizador se ha reemplazado antes.
            // GC.SuppressFinalize(this);
        }
        #endregion
        #endregion
    }
}