using FitnessHere.DAL.DTOs;
using FitnessHere.Models;
using Humanizer;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FitnessHere.DAL.Repositories
{
    public class AdoNetFiltering
    {
        private readonly string _connStr;

        public AdoNetFiltering(string connStr)
        {
            _connStr = connStr;
        }

     public IList<MemberFilteringViewModel> Filter(
         out int TotalCount,
     string? MemberFirstName = null,
     string? MemberLastName = null,
     string? ClassName = null,
     string? TrainerName = null,
     DateTime? RegistrationDate = null,
     string SortColumnName = "MemberID",
     bool SortDesc = false,
     int Page = 1,
     int PageSize = 10
 ){
            try
            {
                var list = new List<MemberFilteringViewModel>();
                TotalCount = 0; // Default value

                using (var connection = new SqlConnection(_connStr))
                {
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "usp_Member_Filter";
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Adding input parameters
                        cmd.Parameters.AddWithValue("@MemberFirstName", (object?)MemberFirstName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@MemberLastName", (object?)MemberLastName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ClassName", (object?)ClassName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@TrainerName", (object?)TrainerName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@RegistrationDate", (object?)RegistrationDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@SortColumnName", SortColumnName);
                        cmd.Parameters.AddWithValue("@SortDesc", SortDesc);
                        cmd.Parameters.AddWithValue("@Page", Page);
                        cmd.Parameters.AddWithValue("@PageSize", PageSize);

                        // Adding output parameter for TotalCount
                        SqlParameter totalCountParam = new SqlParameter("@TotalCount", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output,
                            Value = 0,
                        };
                        cmd.Parameters.Add(totalCountParam);

                        connection.Open();
                        using var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            var member = MapReader(reader);
                            list.Add(member);
                        }

                        // Close the reader before accessing output parameters
                        reader.Close();

                        // Now retrieve TotalCount after the reader is closed
                        TotalCount = (int)totalCountParam.Value;

                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        private MemberFilteringViewModel MapReader(SqlDataReader reader)
        {
            return new MemberFilteringViewModel()
            {
                MemberID = reader.GetInt32(reader.GetOrdinal("MemberID")),
                FirstName = reader.IsDBNull(reader.GetOrdinal("FirstName")) ? null : reader.GetString(reader.GetOrdinal("FirstName")),
                LastName = reader.IsDBNull(reader.GetOrdinal("LastName")) ? null : reader.GetString(reader.GetOrdinal("LastName")),
               RegistrationDate = reader.IsDBNull(reader.GetOrdinal("RegistrationDate")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("RegistrationDate")),
                Trainers = reader.IsDBNull(reader.GetOrdinal("TrainersList"))? [] : reader.GetString(reader.GetOrdinal("TrainersList")).Split(','),
                MemberClasses = reader.IsDBNull(reader.GetOrdinal("MemberClassesList")) ? [] : reader.GetString(reader.GetOrdinal("MemberClassesList")).Split(',')
            };
        }
    }
}
