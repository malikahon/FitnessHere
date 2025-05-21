using FitnessHere.DAL.DTOs;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using static System.Net.Mime.MediaTypeNames;

namespace FitnessHere.DAL.Repositories
{
    public class AdoNetMemberRepository : IRepository<MemberDTO>
    {

        private readonly string _connStr;

        public AdoNetMemberRepository(string connStr)
        {
            _connStr = connStr;
        }


        public IList<MemberDTO> GetAll()
        {
            try
            {
                var list = new List<MemberDTO>();
                using (var connection = new SqlConnection(_connStr))
                {
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = @"usp_Members_GetAll";
                        cmd.CommandType = CommandType.StoredProcedure;

                        connection.Open();
                        using var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            var member = MapReader(reader);
                            list.Add(member);
                        }

                        return list;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private MemberDTO MapReader(DbDataReader reader)
        {
            return new MemberDTO()
            {
                MemberID = reader.GetInt32(reader.GetOrdinal("MemberID")),
                FirstName = reader.IsDBNull(reader.GetOrdinal("FirstName")) ? null : reader.GetString(reader.GetOrdinal("FirstName")),
                LastName = reader.IsDBNull(reader.GetOrdinal("LastName")) ? null : reader.GetString(reader.GetOrdinal("LastName")),
                PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber")) ? null : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                DateOfBirth = (DateTime)(reader.IsDBNull(reader.GetOrdinal("DateOfBirth")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DateOfBirth"))),
                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                RegistrationDate = reader.IsDBNull(reader.GetOrdinal("RegistrationDate")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("RegistrationDate")),
                ProfilePicture = reader.IsDBNull(reader.GetOrdinal("ProfilePicture")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("ProfilePicture")),
                IsDisabled = reader.IsDBNull(reader.GetOrdinal("IsDisabled")) ? false : reader.GetBoolean(reader.GetOrdinal("IsDisabled")),
                IsMale = reader.IsDBNull(reader.GetOrdinal("IsMale")) ? false : reader.GetBoolean(reader.GetOrdinal("IsMale"))
            };
        }

        public MemberDTO? GetById(int id)
        {
            try
            {
                MemberDTO? member = null;
                using (var connection = new SqlConnection(_connStr))
                {
                    connection.Open();
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = @"usp_Members_GetMemberByID";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MemberID", SqlDbType.Int) { Value = id });


                        using var reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            member = MapReader(reader);
                        }

                        return member;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public void Update(MemberDTO member)
        {
            try
            {
                using (var connection = new SqlConnection(_connStr))
                {
                    connection.Open();
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "usp_Members_UpdateMember";
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Pass MemberID (required for updating)
                        cmd.Parameters.Add(new SqlParameter("@MemberID", SqlDbType.Int) { Value = member.MemberID });

                        // Handle possible NULL 
                        cmd.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.NVarChar, 255) { Value = (object?)member.FirstName ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter("@LastName", SqlDbType.NVarChar, 255) { Value = (object?)member.LastName ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter("@PhoneNumber", SqlDbType.NVarChar, 20) { Value = (object?)member.PhoneNumber ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter("@DateOfBirth", SqlDbType.Date) { Value = (object?)member.DateOfBirth ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 255) { Value = (object?)member.Email ?? DBNull.Value });

                        cmd.Parameters.Add(new SqlParameter("@ProfilePicture", SqlDbType.VarBinary) { Value = (member.ProfilePicture != null && member.ProfilePicture.Length > 0) ? (object)member.ProfilePicture : DBNull.Value });

                        cmd.Parameters.Add(new SqlParameter("@IsDisabled", SqlDbType.Bit) { Value = (object?)member.IsDisabled ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter("@IsMale", SqlDbType.Bit) { Value = (object?)member.IsMale ?? DBNull.Value });

                        cmd.ExecuteNonQuery();


                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public void Create(MemberDTO member)
        {
            try
            {
                using (var connection = new SqlConnection(_connStr))
                {
                    connection.Open();
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "usp_Members_CreateMember";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.NVarChar, 255) { Value = member.FirstName });
                        cmd.Parameters.Add(new SqlParameter("@LastName", SqlDbType.NVarChar, 255) { Value = member.LastName });
                        cmd.Parameters.Add(new SqlParameter("@PhoneNumber", SqlDbType.NVarChar, 20) { Value = member.PhoneNumber });
                        cmd.Parameters.Add(new SqlParameter("@DateOfBirth", SqlDbType.Date) { Value = member.DateOfBirth });
                        cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 255) { Value = member.Email });
                        cmd.Parameters.Add(new SqlParameter("@ProfilePicture", SqlDbType.VarBinary) { Value = (object)member.ProfilePicture ?? DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter("@IsDisabled", SqlDbType.Bit) { Value = member.IsDisabled });
                        cmd.Parameters.Add(new SqlParameter("@IsMale", SqlDbType.Bit) { Value = member.IsMale });


                        cmd.ExecuteNonQuery();


                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connStr))
                {
                    connection.Open();
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = @"usp_Members_DeleteMember";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MemberID", SqlDbType.Int) { Value = id });


                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public void Dispose()
        {
           
        }


    }
}
