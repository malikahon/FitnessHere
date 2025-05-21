using Microsoft.Data.SqlClient;
using System.Data;

namespace FitnessHere.DAL.Repositories
{
    public class AdoNetImport
    {
        private readonly string _connStr;

        public AdoNetImport(string connStr)
        {
            _connStr = connStr;
        }

        public bool ImportXmlToDatabase(string xmlData)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connStr))
                {
                    connection.Open();
                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "usp_Members_XMLImport";

                        cmd.CommandType = CommandType.StoredProcedure;

                        // Pass XML data as an input parameter
                        cmd.Parameters.Add(new SqlParameter("@Data", SqlDbType.Xml)
                        {
                            Value = xmlData
                        });

                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public bool ImportJsonToDatabase(string jsonData)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connStr))
                {
                    connection.Open();
                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "usp_Members_JSONImport";

                        cmd.CommandType = CommandType.StoredProcedure;

                        // Pass XML data as an input parameter
                        cmd.Parameters.Add(new SqlParameter("@JsonData", SqlDbType.NVarChar)
                        {
                            Value = jsonData
                        });

                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

    }
}
