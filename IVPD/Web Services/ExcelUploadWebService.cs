using IVPD.Helpers;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace IVPD.Web_Services
{
    [ServiceContract]

    public interface IExcelUploadWebService
    {
        [OperationContract]
        void GetDataTabletFromCSVFile();
    }
    public class ExcelUploadWebService : IExcelUploadWebService
    {
        public void GetDataTabletFromCSVFile()
        {
            DataTable csvData = new DataTable();
          
                using (TextFieldParser csvReader = new TextFieldParser("StaticFiles/LIBTESOUR_GERFIP_TL_TES_GERFIP_TIPO_RECEITA.xls"))
                {
                    csvReader.SetDelimiters(new string[] { "   " });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();
                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvData.Columns.Add(datecolumn);
                    }
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData);
                    }
                }
           
            InsertDataIntoSQLServerUsingSQLBulkCopy(csvData);
        }
        static APIResponse InsertDataIntoSQLServerUsingSQLBulkCopy(DataTable csvFileData)
        {
            
            using (SqlConnection dbConnection = new SqlConnection("Server=LAPTOP-41FTOKKD\\SQLEXPRESS2019;Database=REVENUEDB;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                dbConnection.Open();
                using (SqlBulkCopy s = new SqlBulkCopy(dbConnection))
                {
                    s.DestinationTableName = "RevenueTable";
                    foreach (var column in csvFileData.Columns)
                        s.ColumnMappings.Add(column.ToString(), column.ToString());
                    s.WriteToServer(csvFileData);
                    return new APIResponse(true, "", "Added");

                }
            }

        }

    }
}

