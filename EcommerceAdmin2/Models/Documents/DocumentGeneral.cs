using EcommerceAdmin2.Models.Sistema;
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
        public string DocType { get; set; }
        public string CardCode { get; set; }
        public string Cardname{ get; set; }
        public string DocCur { get; set; }
        public string TrackNo { get; set; }
        public string Status { get; set; }
        public string DocNumEcommerce { get; set; }
        private DBSqlServer SqlServer { get; set; }
        #endregion
        #region Constructores
        public DocumentGeneral()
        {

        }
        public DocumentGeneral(DBSqlServer SqlServer)
        {
            this.SqlServer = SqlServer;
        }
        #endregion
        #region Metodos
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
        #endregion
    }
}
