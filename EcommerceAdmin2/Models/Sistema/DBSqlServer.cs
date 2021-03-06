﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAdmin2.Models.Sistema
{
    public class DBSqlServer : IDisposable
    {
        private string Server = "";
        private string Username = "";
        private string Password = "";
        private string Database = "";
        private string ErrorMessage = "";
        private DataTable DataTable;
        private SqlDataReader DataReader;

        public SqlConnection sqlConnection;
        public DBSqlServer()
        {
            //Server = "192.168.2.17";
            //Username = "USR_LECTURA";
            //Password = "splitel.lectura16";
            //Database = "FIBREMX_TEST";
        }
        public void ForceConnection(bool forceConnection)
        {
            if(sqlConnection.State != ConnectionState.Open && forceConnection == true)
            {
                OpenDataBaseAccess();
            }
        }
        public bool OpenDataBaseAccess()
        {
            //string ConnectionString = "Data Source=" + Server + ";";
            //ConnectionString += "User ID=" + Username + ";";
            //ConnectionString += "Password=" + Password + ";";
            //ConnectionString += "Initial Catalog=" + Database + ";";
            //ConnectionString += "Connect Timeout=9000;Persist Security Info=True;MultipleActiveResultSets=true;";
            string ConnectionString = ConfigurationManager.AppSettings["SAP_Database"].ToString();
            try
            {
                sqlConnection = new SqlConnection(ConnectionString);
                sqlConnection.Open();
                CheckConnection();
                return true;
            }
            catch(DBException ex)
            {
                throw ex;
            }
            catch (Exception exception)
            {
                ErrorMessage = exception.ToString();
                return false;
            }
        }
        public string getError()
        {
            return ErrorMessage;
        }
        public void CloseDataBaseAccess()
        {
            sqlConnection.Close();
        }
        private void CheckConnection()
        {
            if (sqlConnection.State != ConnectionState.Open)
            {
                throw new DBException("Sin conexion a base de datos SQLServer");
            }
        }
        public DataTable GetData(string sqlStatement)
        {
            try
            {
                CheckConnection();
                using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, sqlConnection))
                {
                    sqlCommand.CommandTimeout = 120;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                    {
                        DataTable = new DataTable();
                        sqlDataAdapter.Fill(DataTable);
                        sqlDataAdapter.Dispose();
                        return DataTable;
                    }
                }
            }
            catch (DBException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public SqlDataReader GetDataReader(string sqlStatement)
        {
            try
            {
                CheckConnection();
                using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, sqlConnection))
                {
                    sqlCommand.CommandTimeout = 120;
                    DataReader = sqlCommand.ExecuteReader();
                    return DataReader;
                }
            }
            catch (DBException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public int GetIntegerValue(string sqlStatement)
        {
            try
            {
                CheckConnection();
                using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, sqlConnection))
                {
                    return string.IsNullOrEmpty(sqlCommand.ExecuteScalar().ToString()) ? 0 : int.Parse(sqlCommand.ExecuteScalar().ToString());
                }
            }
            catch (DBException ex)
            {
                throw ex;
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }
        public string GetStringValue(string sqlStatement)
        {
            try
            {
                CheckConnection();
                using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, sqlConnection))
                {
                    return sqlCommand.ExecuteScalar().ToString();
                }
            }
            catch (DBException ex)
            {
                throw ex;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public double GetDoublelValue(string sqlStatement)
        {
            try
            {
                CheckConnection();
                using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, sqlConnection))
                {
                    return string.IsNullOrEmpty(sqlCommand.ExecuteScalar().ToString()) ? 0 : double.Parse(sqlCommand.ExecuteScalar().ToString());
                }
            }
            catch (DBException ex)
            {
                throw ex;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public DateTime GetDateTimeValue(string sqlStatement)
        {
            try
            {
                CheckConnection();
                using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, sqlConnection))
                {
                    return DateTime.Parse(sqlCommand.ExecuteScalar().ToString());
                }
            }
            catch (DBException ex)
            {
                throw ex;
            }
            catch (Exception exception)
            {
                throw exception;
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
                CloseDataBaseAccess();
                disposedValue = true;
            }
        }

        // TODO: reemplace un finalizador solo si el anterior Dispose(bool disposing) tiene código para liberar los recursos no administrados.
        // ~DataBaseEx()
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
            GC.SuppressFinalize(this);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
        #endregion
    }
}