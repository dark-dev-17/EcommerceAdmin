using EcommerceAdmin2.Models.Documents;
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
        public string ExtraDays { get; private set; }
        public string DescriptPayment { get; private set; }
        public double CreditLine { get; private set; }
        public double Balance { get; private set; }
        public string Phone2 { get; private set; }
        public string E_Mail { get; private set; }
        public string E_MailL_invoice { get; private set; }
        public string E_MailL_account { get; private set; }
        public string SlpName { get; private set; }
        public string Email_employeSales { get; private set; }
        public string Section { get; private set; }
        public string MonexUSD { get; private set; }
        public string MonexMXP { get; private set; }
        public string Currency { get; private set; }
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
                    this.CardName = data.Rows[0].ItemArray[1].ToString();
                    this.ExtraDays = data.Rows[0].ItemArray[2].ToString();
                    this.DescriptPayment = data.Rows[0].ItemArray[3].ToString();
                    this.CreditLine = double.Parse(data.Rows[0].ItemArray[4].ToString());
                    this.Balance = double.Parse(data.Rows[0].ItemArray[5].ToString());
                    this.Phone2 = data.Rows[0].ItemArray[6].ToString();
                    this.E_Mail = data.Rows[0].ItemArray[7].ToString();
                    this.E_MailL_invoice = data.Rows[0].ItemArray[8].ToString();
                    this.E_MailL_account = data.Rows[0].ItemArray[9].ToString();
                    this.SlpName = data.Rows[0].ItemArray[10].ToString();
                    this.Email_employeSales = data.Rows[0].ItemArray[11].ToString();
                    this.Section = data.Rows[0].ItemArray[12].ToString();
                    this.MonexUSD = data.Rows[0].ItemArray[13].ToString();
                    this.MonexMXP = data.Rows[0].ItemArray[14].ToString();
                    this.Currency = data.Rows[0].ItemArray[15].ToString();
                    IsExists = true;
                }
                else
                {
                    throw new DBException(string.Format("Cliente: [{0}] no encontrado", CardCode));
                }
                data.Dispose();
            }
            catch (DBException ex)
            {
                throw ex;
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
