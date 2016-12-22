using GameDataObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDatabaseAccessor
{
    public class PlayerAccessor
    {
        public static int VerifyUsernameAndPassword(string username, string passwordHash)
        {
            var result = 0;

            // need a connection
            var conn = DBConnection.GetDBConnection();

            // need command text
            var cmdText = @"sp_authenticate_player";

            // we need a command object
            var cmd = new SqlCommand(cmdText, conn);

            //set command type
            cmd.CommandType = CommandType.StoredProcedure;

            // parameters?
            cmd.Parameters.Add("@Username", SqlDbType.VarChar, 20);
            cmd.Parameters.Add("@PasswordHash", SqlDbType.VarChar, 100);

            // parameter values?
            cmd.Parameters["@Username"].Value = username;
            cmd.Parameters["@PasswordHash"].Value = passwordHash;

            // try, catch, finally
            try
            {
                conn.Open();
                result = (int)cmd.ExecuteScalar();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        public static int AddNewPlayer(string username, string email, string password)
        {
            var count = 0;

            // need a connection
            var conn = DBConnection.GetDBConnection();

            // need command text
            var cmdText = @"sp_add_new_player";

            // we need a command object
            var cmd = new SqlCommand(cmdText, conn);

            //set command type
            cmd.CommandType = CommandType.StoredProcedure;

            // parameters?
            cmd.Parameters.Add("@UserName", SqlDbType.VarChar,20);
            cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100);
            cmd.Parameters.Add("@PasswordHash", SqlDbType.VarChar, 100);

            // parameter values?
            cmd.Parameters["@UserName"].Value = username;
            cmd.Parameters["@Email"].Value = email;
            cmd.Parameters["@PasswordHash"].Value = password;

            // try, catch, finally
            try
            {
                conn.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return count;
        }

        public static int UpdatePasswordHash(int playerID, string oldPasswordHash, string newPasswordHash)
        {
            var count = 0;

            // need a connection
            var conn = DBConnection.GetDBConnection();

            // need command text
            var cmdText = @"sp_update_passwordHash";

            // we need a command object
            var cmd = new SqlCommand(cmdText, conn);

            //set command type
            cmd.CommandType = CommandType.StoredProcedure;

            // parameters?
            cmd.Parameters.Add("@PlayerID", SqlDbType.Int);
            cmd.Parameters.Add("@OldPasswordHash", SqlDbType.VarChar, 100);
            cmd.Parameters.Add("@NewPasswordHash", SqlDbType.VarChar, 100);

            // parameter values?
            cmd.Parameters["@PlayerID"].Value = playerID;
            cmd.Parameters["@OldPasswordHash"].Value = oldPasswordHash;
            cmd.Parameters["@NewPasswordHash"].Value = newPasswordHash;

            // try, catch, finally
            try
            {
                conn.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return count;
        }

        public static Player RetrievePlayerByUsername(string username)
        {
            Player player = null;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_retrieve_player_by_username";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Username", SqlDbType.NVarChar);
            cmd.Parameters["@Username"].Value = username;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        player = new Player()
                        {
                            PlayerID = reader.GetInt32(0),
                            UserName = reader.GetString(1),
                            Email = reader.GetString(2),
                            RoleID = reader.GetInt32(3),
                            SavedLocationX = reader.GetInt32(4),
                            SavedLocationY = reader.GetInt32(5),
                            SavedLocationZ = reader.GetInt32(6),
                            SavedHealth = reader.GetInt32(7),
                            SavedImage = reader.GetInt32(8),
                            SavedItem = reader.GetInt32(9),
                            
                            Ban = reader.GetBoolean(10)
                        };
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return player;
        }

        public static Player RetrievePlayerByEmail(string email)
        {
            Player player = null;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_retrieve_player_by_email";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Username", SqlDbType.NVarChar);
            cmd.Parameters["@Username"].Value = email;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        player = new Player()
                        {
                            PlayerID = reader.GetInt32(0),
                            UserName = reader.GetString(1),
                            Email = reader.GetString(2),
                            RoleID = reader.GetInt32(3),
                            SavedLocationX = reader.GetInt32(4),
                            SavedLocationY = reader.GetInt32(5),
                            SavedLocationZ = reader.GetInt32(6),
                            SavedHealth = reader.GetInt32(7),
                            SavedImage = reader.GetInt32(8),
                            SavedItem = reader.GetInt32(9),
                            Ban = reader.GetBoolean(10)
                        };
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return player;
        }

        public static Role RetrievePlayerRole(int playerID)
        {
            var role = new Role();

            // need a connection
            var conn = DBConnection.GetDBConnection();

            // need command text
            var cmdText = @"sp_retrieve_player_role";

            // we need a command object
            var cmd = new SqlCommand(cmdText, conn);

            //set command type
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@PlayerID", SqlDbType.Int);

            cmd.Parameters["@PlayerID"].Value = playerID;
            // try, catch, finally
            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                   
                    role.RoleID = reader.GetInt32(0);
                    role.RoleName = reader.GetString(1);

                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return role;
        }


    }
}
