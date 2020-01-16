using EcommerceAdmin2.Models.Sistema;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAdmin2.Models.Documents
{
    public class DocumentLinesGeneral
    {
        #region Propiedades
        public string DocEntry { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public DateTime DocDate { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public string Currency { get; set; }
        public double Rate { get; set; }
        public double LineTotal { get; set; }
        public double TotalFrgn { get; set; }
        public double VatPercent { get; set; }
        private DBSqlServer SqlServer { get; set; }
        #endregion
        #region Constructores
        public DocumentLinesGeneral()
        {

        }
        public DocumentLinesGeneral(DBSqlServer SqlServer)
        {
            this.SqlServer = SqlServer;
        }
        #endregion
        #region Metodos
        public void GetDocumentLines(List<DocumentLinesGeneral> ListDocumentLinesGeneral, string DocEntry, string TypeDoc)
        { 
                string sqlStatement = string.Format("EXEC Eco_GetDocumentLines @DocumentType = '{0}', @DocEntry = '{1}'", TypeDoc, DocEntry);
                try
                {
                    SqlDataReader data = SqlServer.GetDataReader(sqlStatement);
                    if (data.HasRows)
                    {
                        while (data.Read())
                        {
                            ListDocumentLinesGeneral.Add(new DocumentLinesGeneral
                            {
                                ItemCode = data.GetString(0),
                                Dscription = data.GetString(1),
                                Quantity = double.Parse(data.GetDecimal(2) + ""),
                                Currency = data.GetString(3),
                                Price = double.Parse(data.GetDecimal(4) + ""),
                                VatPercent = double.Parse(data.GetDecimal(5) + ""),
                            });
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
