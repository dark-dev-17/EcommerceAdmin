﻿using EcommerceAdmin2.Models.Documents;
using EcommerceAdmin2.Models.Sistema;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAdmin2.Models.BussinesPartner
{
    public class BussinesPartner : IDisposable
    {
        #region Propiedades
        public string CardCode { private set; get; }
        public string CardName { private set; get; }
        public string SplName { private set; get; }
        public bool IsActive { private set; get; }
        public bool IsActiveEcomerce { private set; get; }
        // objetos
        private DBSqlServer SqlServer { get; set; }
        #endregion
        #region Constructores
        public BussinesPartner()
        {

        }
        public BussinesPartner(DBSqlServer SqlServer)
        {
            this.SqlServer = SqlServer;
        }
        #endregion
        #region Metodos
        public bool GetBussinesPartner(string CardCode)
        {
            bool IsExists = false;
            string sqlStatement = string.Format("EXEC Eco_GetInfoGeneralCustomer @CardCode = '{0}'", CardCode);
            try
            {
                SqlServer.ForceConnection(true);
                DataTable data = SqlServer.GetData(sqlStatement);
                if (data.Rows.Count == 1)
                {
                    this.CardCode = data.Rows[0].ItemArray[0].ToString();
                    CardName = data.Rows[0].ItemArray[1].ToString();
                    IsExists = true;
                }
                data.Dispose();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return IsExists;
        }
        public List<BussinesPartner> GetBussinesPartnersBySalesEmp(List<int> IdSalesEmpl)
        {
            List<BussinesPartner> ListBussinesPartner = new List<BussinesPartner>();
            foreach (int IdSalesEmp in IdSalesEmpl)
            {
                try
                {
                    string sqlStatement = string.Format("EXEC [Eco_getCustomerBYSalesEmployer] @SlpCode = '{0}'", IdSalesEmp);
                    SqlDataReader data = SqlServer.GetDataReader(sqlStatement);
                    if (data.HasRows)
                    {
                        while (data.Read())
                        {
                            BussinesPartner BussinesPartner = new BussinesPartner();
                            BussinesPartner.CardCode = data.IsDBNull(0) ? "" : data.GetString(0) + "";
                            BussinesPartner.CardName = data.IsDBNull(1) ? "" : data.GetString(1) + "";
                            BussinesPartner.SplName = data.IsDBNull(2) ? "" : data.GetString(2) + "";
                            BussinesPartner.IsActive = data.IsDBNull(3) ? false : data.GetString(3) == "Si" ? true : false;
                            BussinesPartner.IsActiveEcomerce = data.IsDBNull(4) ? false : data.GetString(4) == "Si" ? true : false;
                            ListBussinesPartner.Add(BussinesPartner);
                        }
                    }
                    else
                    {
                        //sin registros
                    }
                    data.Close();
                    
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
            return ListBussinesPartner;
        }
        public List<BussinesPartner> SelectAllactive()
        {
            try
            {
                string sqlStatement = string.Format("EXEC Eco_getCustomerActive @CardCode = '{0}'", " ");
                List<BussinesPartner> bussinesPartners = new List<BussinesPartner>();
                SqlDataReader data = SqlServer.GetDataReader(sqlStatement);
                while (data.Read())
                {
                    BussinesPartner BussinesPartner = new BussinesPartner();
                    BussinesPartner.CardCode = data.IsDBNull(0) ? "" : data.GetString(0) + "";
                    BussinesPartner.CardName = data.IsDBNull(1) ? "" : data.GetString(1) + "";
                    BussinesPartner.SplName = data.IsDBNull(2) ? "" : data.GetString(2) + "";
                    BussinesPartner.IsActive = data.IsDBNull(3) ? false : data.GetString(3) == "Si" ? true : false;
                    BussinesPartner.IsActiveEcomerce = data.IsDBNull(4) ? false : data.GetString(4) == "Si" ? true : false;
                    bussinesPartners.Add(BussinesPartner);
                }
                data.Close();
                return bussinesPartners;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        public void GetBussinesPartnersBySalesEmp(List<int> IdSalesEmpl, List<BussinesPartner> List)
        {
            foreach (int IdSalesEmp in IdSalesEmpl)
            {
                string sqlStatement = string.Format("EXEC [Eco_getCustomerBYSalesEmployer] @SlpCode = '{0}'", IdSalesEmp);
                try
                {
                    SqlDataReader data = SqlServer.GetDataReader(sqlStatement);
                    if (data.HasRows)
                    {
                        while (data.Read())
                        {
                            BussinesPartner BussinesPartner = new BussinesPartner();
                            BussinesPartner.CardCode = data.IsDBNull(0) ? "" : data.GetString(0) + "";
                            BussinesPartner.CardName = data.IsDBNull(1) ? "" : data.GetString(1) + "";
                            BussinesPartner.SplName = data.IsDBNull(2) ? "" : data.GetString(2) + "";
                            BussinesPartner.IsActive = data.IsDBNull(3) ? false : data.GetString(3) == "Si" ? true : false;
                            BussinesPartner.IsActiveEcomerce = data.IsDBNull(4) ? false : data.GetString(4) == "Si" ? true : false;
                            List.Add(BussinesPartner);
                        }
                    }
                    else
                    {
                        //sin registros
                    }
                    data.Close();
                }
                catch (Exception Ex)
                {
                    throw Ex;
                }
            }
            
        }

        #endregion
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
        // ~BussinesPartner()
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

    }
}
