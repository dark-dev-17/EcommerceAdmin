using EcommerceAdmin2.Models.Sistema;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAdmin2.Models.Empleado
{
    public class Empleado : IDisposable
    {
        #region Propiedades
        public int IdSplinnet { private set; get; }
        public string Username { private set; get; }
        public string Nombre { private set; get; }
        public string ApellidoPaterno { private set; get; }
        public string Apellidomaterno { private set; get; }
        public string Correo { private set; get; }
        public string Sociedad { private set; get; }
        public int IdArea { private set; get; }
        public List<int> Id_sap { private set; get; }
        private DBMysql DBMysql;
        private string ErrorMessage;
        #endregion
        #region Constructores
        public Empleado()
        {
        }
        public Empleado(DBMysql DBMysql)
        {
            this.DBMysql = DBMysql;
        }

        #endregion
        #region Metodos
        public void setConectionMysql(DBMysql DBMysql)
        {
            this.DBMysql = DBMysql;
        }
        public int GetEmpleado(int Id)
        {
            string Statement = string.Format("SELECT ID,username,email,nombre,apaterno,amaterno,id_area,sociedad FROM signup where ID = '{0}';", Id);
            int ResponseProces = 0;
            try
            {

                MySqlDataReader DataReader = DBMysql.DoQuery(Statement);
                if (DBMysql.CountDataReader(DataReader) == 1)
                {
                    IdSplinnet = DataReader.IsDBNull(0) ? 0 : (int)DataReader.GetUInt32(0);
                    Username = DataReader.IsDBNull(1) ? "" : DataReader.GetString(1);
                    Correo = DataReader.IsDBNull(2) ? "" : DataReader.GetString(2);
                    Nombre = DataReader.IsDBNull(3) ? "" : DataReader.GetString(3);
                    ApellidoPaterno = DataReader.IsDBNull(4) ? "" : DataReader.GetString(4);
                    Apellidomaterno = DataReader.IsDBNull(5) ? "" : DataReader.GetString(5);
                    IdArea = DataReader.IsDBNull(6) ? 0 : DataReader.GetInt32(6);
                    Sociedad = DataReader.IsDBNull(7) ? "" : DataReader.GetString(7);
                    DataReader.Close();
                    //Id_sap = GetIdSapDB(Id);
                }
                else
                {
                    ErrorMessage = "Mas de un usuario con las mismas credenciales";
                    ResponseProces = 200;
                    DataReader.Close();
                }
                
            }
            catch (DBException ex)
            {
                throw ex;
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ResponseProces;
        }
        public List<Empleado> SelectAllSplit()
        {
            string Statement = string.Format("SELECT ID,username,email,nombre,apaterno,amaterno,id_area,sociedad FROM signup ");
            try
            {
                MySqlDataReader DataReader = DBMysql.DoQuery(Statement);
                List<Empleado> empleados = new List<Empleado>();
                while (DataReader.Read())
                {
                    Empleado empleado = new Empleado();
                    empleado.IdSplinnet = DataReader.IsDBNull(0) ? 0 : (int)DataReader.GetUInt32(0);
                    empleado.Username = DataReader.IsDBNull(1) ? "" : DataReader.GetString(1);
                    empleado.Correo = DataReader.IsDBNull(2) ? "" : DataReader.GetString(2);
                    empleado.Nombre = DataReader.IsDBNull(3) ? "" : DataReader.GetString(3);
                    empleado.ApellidoPaterno = DataReader.IsDBNull(4) ? "" : DataReader.GetString(4);
                    empleado.Apellidomaterno = DataReader.IsDBNull(5) ? "" : DataReader.GetString(5);
                    empleado.IdArea = DataReader.IsDBNull(6) ? 0 : DataReader.GetInt32(6);
                    empleado.Sociedad = DataReader.IsDBNull(7) ? "" : DataReader.GetString(7);
                    empleados.Add(empleado);
                }
                DataReader.Close();
                return empleados;
            }
            catch (DBException ex)
            {
                throw ex;
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
        public void GetIdSapDB(int Id)
        {
            Id_sap = new List<int>();
            string Statement = string.Format("SELECT id_sap FROM id_split_sap where id_splittel = '{0}';", Id);
            try
            {
                MySqlDataReader DataReader = DBMysql.DoQuery(Statement);
                if (DataReader.HasRows)
                {
                    while (DataReader.Read())
                    {
                        Id_sap.Add(DataReader.IsDBNull(0) ? 0 : (int)DataReader.GetInt32(0));
                    }
                    DataReader.Close();
                }
                else
                {
                    ErrorMessage = "Sin registros";
                }
                DataReader.Close();
            }
            catch (DBException ex)
            {
                throw ex;
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
        // ~Empleado()
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
