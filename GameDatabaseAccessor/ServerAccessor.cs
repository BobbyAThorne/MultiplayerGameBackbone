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
    public class ServerAccessor
    {
        public static Server RetrievePlayerByUsername(string username)
        {
            Server player = null;

            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_retrieve_player_on_server_by_username";
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
                        player = new Server()
                        {
                            PlayerID = reader.GetInt32(0),
                            UserName = reader.GetString(1),
                            LocationX = reader.GetInt32(2),
                            LocationY = reader.GetInt32(3),
                            LocationZ = reader.GetInt32(4),
                            Health = reader.GetInt32(5),
                            Image = reader.GetInt32(6),
                            Size = 5, //These 2 can be changed
                            SizeMultiplier = 5,
                            EquiptItem = reader.GetInt32(7)
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

        public static int AddPlayerToServer(int playerId)
        {
            var count = 0;

            // need a connection
            var conn = DBConnection.GetDBConnection();

            // need command text
            var cmdText = @"sp_add_player_to_server";

            // we need a command object
            var cmd = new SqlCommand(cmdText, conn);

            //set command type
            cmd.CommandType = CommandType.StoredProcedure;

            // parameters?
            cmd.Parameters.Add("@PlayerID", SqlDbType.Int);

            // parameter values?
            cmd.Parameters["@PlayerID"].Value = playerId;


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

        public static int UpdateToActive(int playerId)
        {
            var count = 0;

            // need a connection
            var conn = DBConnection.GetDBConnection();

            // need command text
            var cmdText = @"sp_update_to_active";

            // we need a command object
            var cmd = new SqlCommand(cmdText, conn);

            //set command type
            cmd.CommandType = CommandType.StoredProcedure;

            // parameters?
            cmd.Parameters.Add("@PlayerID", SqlDbType.Int);

            // parameter values?
            cmd.Parameters["@PlayerID"].Value = playerId;


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

        public static int PlayerMove(int playerId, int locationX, int locationY, int locationZ, int imageSet, int oldLocationX, int oldLocationY, int oldLocationZ)
        {
            var count = 0;

            // need a connection
            var conn = DBConnection.GetDBConnection();

            // need command text
            var cmdText = @"sp_player_moved";

            // we need a command object
            var cmd = new SqlCommand(cmdText, conn);

            //set command type
            cmd.CommandType = CommandType.StoredProcedure;

            // parameters?
            cmd.Parameters.Add("@PlayerID", SqlDbType.Int);
            cmd.Parameters.Add("@LocationX", SqlDbType.Int);
            cmd.Parameters.Add("@LocationY", SqlDbType.Int);
            cmd.Parameters.Add("@LocationZ", SqlDbType.Int);
            cmd.Parameters.Add("@ImageSet", SqlDbType.Int);
            cmd.Parameters.Add("@OldLocationX", SqlDbType.Int);
            cmd.Parameters.Add("@OldLocationY", SqlDbType.Int);
            cmd.Parameters.Add("@OldLocationZ", SqlDbType.Int);

            // parameter values?
            cmd.Parameters["@PlayerID"].Value = playerId;
            cmd.Parameters["@LocationX"].Value = locationX;
            cmd.Parameters["@LocationY"].Value = locationY;
            cmd.Parameters["@LocationZ"].Value = locationZ;
            cmd.Parameters["@ImageSet"].Value = imageSet;
            cmd.Parameters["@OldLocationX"].Value = oldLocationX;
            cmd.Parameters["@OldLocationY"].Value = oldLocationY;
            cmd.Parameters["@OldLocationZ"].Value = oldLocationZ;

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

        public static int UpdateToInactive(int playerId)
        {
            var count = 0;

            // need a connection
            var conn = DBConnection.GetDBConnection();

            // need command text
            var cmdText = @"sp_update_to_inactive";

            // we need a command object
            var cmd = new SqlCommand(cmdText, conn);

            //set command type
            cmd.CommandType = CommandType.StoredProcedure;

            // parameters?
            cmd.Parameters.Add("@PlayerID", SqlDbType.Int);

            // parameter values?
            cmd.Parameters["@PlayerID"].Value = playerId;


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

        //NEEDS WORK
        public static int DeletePlayerFromServer(int playerId)
        {
            var count = 0;

            // need a connection
            var conn = DBConnection.GetDBConnection();

            // need command text
            var cmdText = @"sp_delete_player_from_server_complete";

            // we need a command object
            var cmd = new SqlCommand(cmdText, conn);

            //set command type
            cmd.CommandType = CommandType.StoredProcedure;

            // parameters?
            cmd.Parameters.Add("@PlayerID", SqlDbType.Int);

            // parameter values?
            cmd.Parameters["@PlayerID"].Value = playerId;


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
        public static List<Server> RetrievePlayersOnServer()
        {
            List<Server> players = new List<Server>();
            Server player = null;
            var conn = DBConnection.GetDBConnection();
            var cmdText = @"sp_retrieve_players_on_server";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        player = new Server()
                        {
                            PlayerID = reader.GetInt32(0),
                            UserName = reader.GetString(1),
                            LocationX = reader.GetInt32(2),
                            LocationY = reader.GetInt32(3),
                            LocationZ = reader.GetInt32(4),
                            Health = reader.GetInt32(5),
                            Image = reader.GetInt32(6),
                            Size = 5, //These 2 can be changed
                            SizeMultiplier = 5,
                            EquiptItem = reader.GetInt32(7)
                        };
                        players.Add(player);
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

            return players;
        }
    }
}
