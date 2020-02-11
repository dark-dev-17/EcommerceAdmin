using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAdmin2.Models.Sistema
{
    public class DBMysql : IDisposable
    {
        #region Propiedades
        public string Server { get; private set; }
        public string Database { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Port { get; private set; }
        private string ConnectionString { get; set; }
        public MySqlConnection Connection { get; private set; }
        #endregion
        #region Constructores
        public DBMysql(string mode)
        {
            if(mode == "Splinet")
            {
                //this.Server = "192.168.2.29";
                //this.Database = "fibreco_fmx";
                //this.Username = "fibremex";
                //this.Password = "FBSrvAD*0316.";
                //this.Port = "3306";
                ConnectionString = ConfigurationManager.AppSettings["Splinnet_Database"].ToString();
            }
            else
            {
                //this.Server = "68.71.58.18";
                //this.Database = "fibremex_b2b_test";
                //this.Username = "fibremex_mxgod";
                //this.Password = "1q2w3e4r5t.";
                //this.Port = "3306";
                ConnectionString = ConfigurationManager.AppSettings["Ecommerce_Database"].ToString();
            }
            
        }
        public DBMysql(string Server, string DataBase, string Username, string Password, string Port)
        {
            this.Server = Server;
            this.Database = DataBase;
            this.Username = Username;
            this.Password = Password;
            this.Port = Port;
        }
        #endregion
        #region Metodos
        public void GetDatatoConnection()
        {
            // start the variables with the params of web.config
            // status pending to programming
        }
        public void OpenConnection()
        {
            //myConnectionString="Server=myServerAddress;Port=1234;Database=testDB;Uid=root;Pwd=abc123;
            //string ConnectionString = string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4}", Server, Port, Database, Username, Password);
            Connection = new MySqlConnection(ConnectionString);
            try
            {
                Connection.Open();
                CheckConnection();
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
        private void CheckConnection()
        {
            if (Connection.State != System.Data.ConnectionState.Open)
            {
                throw new DBException("Sin conexion a base de datos MySQL");
            }
        }
        public void CloseConnection()
        {
            Connection.Close();
        }
        public int DoNonQuery(string stattement)
        {
            try
            {
                CheckConnection();
                //Create Command
                MySqlCommand command = new MySqlCommand(stattement, Connection);
                //Create a data reader and Execute the command
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected;
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
        public MySqlDataReader DoQuery(string stattement)
        {
            try
            {
                CheckConnection();
                //Create Command
                MySqlCommand command = new MySqlCommand(stattement, Connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = command.ExecuteReader();
                return dataReader;
                
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
        public bool ExecuteProcedure(string Parameters, string returnValue)
        {
            try
            {
                CheckConnection();
                string ProcedureName = Parameters.Split('|')[0];
                string[] Paramet = Parameters.Split('|')[1].Split('&');
                MySqlCommand cmd = new MySqlCommand(ProcedureName, Connection);
                cmd.CommandType = CommandType.StoredProcedure;
                //campo @ typevar = valor &
                foreach (string item in Paramet)
                {
                    string variable = item.Split('@')[0];
                    string type = item.Split('@')[1].Split('=')[0];
                    string value = item.Split('@')[1].Split('=')[1];
                    cmd.Parameters.AddWithValue("@" + variable, value);
                }
                MySqlDataReader dataReader = cmd.ExecuteReader();
                dataReader.Close();
                return false;
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
        private void processParameters(string Parameters, MySqlCommand cmd)
        {
            string ProcedureName = Parameters.Split('|')[0];
            string[] Paramet = Parameters.Split('|')[1].Split('&');

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = ProcedureName;

            foreach (string item in Paramet)
            {
                string variable = item.Split('@')[0];
                string variableValue = item.Split('@')[1];
                string type = variableValue.Split('=')[0];
                string value = variableValue.Split('=')[1];

                if (type == "DATETIME")
                {
                    cmd.Parameters.AddWithValue("@" + variable, DateTime.Parse(value));
                    cmd.Parameters["@" + variable].Direction = ParameterDirection.Input;
                }
                if (type == "VARCHAR")
                {
                    cmd.Parameters.AddWithValue("@" + variable, value);
                    cmd.Parameters["@" + variable].Direction = ParameterDirection.Input;
                }

            }
        }
        public MySqlDataReader ExecuteStoreProcedureReader(string Parameters)
        {
            try
            {
                CheckConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Connection;
                processParameters(Parameters, cmd);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                return dataReader;
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
        public int ExecuteProcedureInt(string Parameters, string output)
        {
            try
            {
                CheckConnection();
                //string ProcedureName = Parameters.Split('|')[0];
                //string[] Paramet = Parameters.Split('|')[1].Split('&');
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Connection;
                processParameters(Parameters,cmd);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.CommandText = ProcedureName;
                ////campo @ typevar = valor &
                //foreach (string item in Paramet)
                //{
                //    string variable = item.Split('@')[0];
                //    string variableValue = item.Split('@')[1];
                //    string type = variableValue.Split('=')[0];
                //    string value = variableValue.Split('=')[1];

                //    if (type == "DATETIME")
                //    {
                //        cmd.Parameters.AddWithValue("@" + variable, DateTime.Parse(value));
                //        cmd.Parameters["@" + variable].Direction = ParameterDirection.Input;
                //    }
                //    if (type == "VARCHAR")
                //    {
                //        cmd.Parameters.AddWithValue("@" + variable, value);
                //        cmd.Parameters["@" + variable].Direction = ParameterDirection.Input;
                //    }

                //}
                //value returns
                cmd.Parameters.AddWithValue("@" + output, Int32.Parse("1"));
                cmd.Parameters["@" + output].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                int RequestStatus = (int)cmd.Parameters["@" + output].Value;

                return RequestStatus;
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
        public double ExecuteProcedureDouble(string Parameters, string output)
        {
            try
            {
                CheckConnection();
                string ProcedureName = Parameters.Split('|')[0];
                string[] Paramet = Parameters.Split('|')[1].Split('&');
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = ProcedureName;
                //campo @ typevar = valor &
                foreach (string item in Paramet)
                {
                    string variable = item.Split('@')[0];
                    string variableValue = item.Split('@')[1];
                    string type = variableValue.Split('=')[0];
                    string value = variableValue.Split('=')[1];

                    if (type == "DATETIME")
                    {
                        cmd.Parameters.AddWithValue("@" + variable, DateTime.Parse(value));
                        cmd.Parameters["@" + variable].Direction = ParameterDirection.Input;
                    }
                    if (type == "VARCHAR")
                    {
                        cmd.Parameters.AddWithValue("@" + variable, value);
                        cmd.Parameters["@" + variable].Direction = ParameterDirection.Input;
                    }

                }
                //value returns
                cmd.Parameters.AddWithValue("@" + output, double.Parse("1"));
                cmd.Parameters["@" + output].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                double RequestStatus = (double)cmd.Parameters["@"+ output].Value;

                return RequestStatus;
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
        public int ExecuteScalarInt(string stattement)
        {
            try
            {
                CheckConnection();
                MySqlCommand cmd = new MySqlCommand(stattement, Connection);
                //Create a data reader and Execute the command
                return (int)cmd.ExecuteScalar();
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
        public int DoQuerySingle(string stattement)
        {
            try
            {
                CheckConnection();
                MySqlCommand cmd = new MySqlCommand(stattement, Connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader.GetType() == typeof(UInt32))
                {
                    return Convert.ToInt32(dataReader.GetUInt32(0));
                }
                else if (dataReader.GetType() == typeof(Int32))
                {
                    return dataReader.GetInt32(0);
                }
                else
                {
                    return -1;
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
        }
        public int CountDataReader(MySqlDataReader dataReader)
        {
            int count = 0;
            while (dataReader.Read())
            {
                count++;
            }
            return count;
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
        // ~DBMysql()
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
