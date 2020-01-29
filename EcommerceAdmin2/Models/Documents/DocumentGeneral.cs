using EcommerceAdmin2.Models.Sistema;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAdmin2.Models.Documents
{
    public class DocumentGeneral
    {
        #region Propiedades
        public string DocEntry { get; set; }
        public string DocNum { get; set; }
        public DateTime DocDate { get; set; }
        public double DocTotal { get; set; }
        public double DocSubTotal { get; set; }
        public double DocIva { get; set; }
        public double DocRate { get; set; }
        public string DocType { get; set; }
        public string CardCode { get; set; }
        public string Cardname{ get; set; }
        public string DocCur { get; set; }
        public string TrackNo { get; set; }
        public string Status { get; set; }
        public string DocNumEcommerce { get; set; }
        private DBSqlServer SqlServer { get; set; }
        private DBMysql DBMysql;
        #endregion
        #region Constructores
        public DocumentGeneral()
        {

        }
        public DocumentGeneral(DBSqlServer SqlServer)
        {
            this.SqlServer = SqlServer;
        }
        public DocumentGeneral(DBMysql DBMysql)
        {
            this.DBMysql = DBMysql;
        }
        #endregion
        #region Metodos
        public void GetSalesQuotationEcomerce(List<DocumentGeneral> ListDocumentGeneral, string CardCode, string Cardname)
        {
            string Statement = string.Format("SELECT * FROM admin_Cotizaciones where cardcode = '{0}';", CardCode);
            try
            {
                MySqlDataReader DataReader = DBMysql.DoQuery(Statement);
                while (DataReader.Read())
                {
                    DocumentGeneral DocumentGeneral = new DocumentGeneral();
                    DocumentGeneral.DocNumEcommerce = DataReader.IsDBNull(0) ? "" : ((int)DataReader.GetInt32(0) + "");
                    DocumentGeneral.CardCode = DataReader.IsDBNull(8) ? "" : DataReader.GetString(8);
                    DocumentGeneral.Cardname = Cardname;
                    DocumentGeneral.DocSubTotal = DataReader.IsDBNull(2) ? 0 : (double)DataReader.GetDouble(2);
                    DocumentGeneral.DocIva = DataReader.IsDBNull(3) ? 0 : (double)DataReader.GetDouble(3);
                    DocumentGeneral.DocTotal = DataReader.IsDBNull(4) ? 0 : (double)DataReader.GetDouble(4);
                    DocumentGeneral.DocDate = DataReader.GetDateTime(5);
                    DocumentGeneral.DocRate = DataReader.GetDouble(6);
                    DocumentGeneral.DocCur = "USD";
                    ListDocumentGeneral.Add(DocumentGeneral);
                    //ListDocumentGeneral.Add();
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
        public void GetDocumentsByCustomer(List<DocumentGeneral> ListDocumentGeneral,string CardCode,string Cardname)
        {
            try
            {
                string sqlStatement = string.Format("exec [Eco_GetOrderInProcessEcommerce] @CardCode = '{0}'", CardCode);
                SqlDataReader data = SqlServer.GetDataReader(sqlStatement);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        DocumentGeneral DocumentGeneral = new DocumentGeneral();
                        DocumentGeneral.DocNum = data.GetInt32(1) + "";
                        DocumentGeneral.DocEntry = data.GetInt32(2) + "";
                        DocumentGeneral.DocDate = data.GetDateTime(3);
                        DocumentGeneral.CardCode = data.GetString(0);
                        DocumentGeneral.DocType = data.GetString(5);
                        DocumentGeneral.DocTotal = double.Parse(data.GetDecimal(6) + "");
                        DocumentGeneral.DocNumEcommerce = data.IsDBNull(4) ? "" : data.GetInt32(4) + "";
                        DocumentGeneral.DocCur = data.GetString(7);
                        DocumentGeneral.Status = data.GetInt32(9) + "";
                        DocumentGeneral.TrackNo = data.IsDBNull(8) ? "" : data.GetString(8) + "";
                        DocumentGeneral.Cardname = Cardname;
                        ListDocumentGeneral.Add(DocumentGeneral);
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
        public void GetDocumentsRejectedByCustomer(List<DocumentGeneral> ListDocumentGeneral, string CardCode, string Cardname)
        {
            try
            {
                string sqlStatement = string.Format("exec Eco_GetOrdersRejected @CardCode = '{0}', @DocDate = '2019-05-01'", CardCode);
                SqlDataReader data = SqlServer.GetDataReader(sqlStatement);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        DocumentGeneral DocumentGeneral = new DocumentGeneral();
                        DocumentGeneral.CardCode = CardCode;
                        DocumentGeneral.DocNum = data.IsDBNull(1) ? "" : data.GetInt32(1) + "";
                        DocumentGeneral.DocDate =  DateTime.Parse(data.GetString(2) + "");
                        DocumentGeneral.Status = data.IsDBNull(3) ? "" : data.GetString(3);
                        DocumentGeneral.DocType = data.IsDBNull(0) ? "" : data.GetString(0);
                        DocumentGeneral.Cardname = Cardname;
                        ListDocumentGeneral.Add(DocumentGeneral);
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
        #endregion
    }
}
