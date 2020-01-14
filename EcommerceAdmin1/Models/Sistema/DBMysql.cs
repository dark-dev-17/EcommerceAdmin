using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAdmin1.Models.Sistema
{
    public class DBMysql : IDisposable
    {
        #region Propiedades
        public string Server { get; private set; }
        public string Database { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Port { get; private set; }
        public MySqlConnection Connection { get; private set; }
        #endregion
        #region Constructores
        public DBMysql()
        {
            this.Server = "192.168.2.29";
            this.Database = "fibreco_fmx";
            this.Username = "fibremex";
            this.Password = "FBSrvAD*0316.";
            this.Port = "3306";
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
            string ConnectionString = string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4}", Server, Port, Database, Username, Password);
            Connection = new MySqlConnection(ConnectionString);
            try
            {
                Connection.Open();
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
        public void CloseConnection()
        {
            Connection.Close();
        }
        public MySqlDataReader DoQuery(string stattement)
        {
            try
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(stattement, Connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                return dataReader;
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
