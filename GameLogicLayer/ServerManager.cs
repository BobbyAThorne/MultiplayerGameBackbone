using GameDatabaseAccessor;
using GameDataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogicLayer
{
    public class ServerManager
    {

        public Server GetPlayer(string username)
        {
            Server user = null;
            try
            {
                user = ServerAccessor.RetrievePlayerByUsername(username);
            }
            catch
            {
                throw;
            }
            return user;
        }


        public List<Server> GetPlayersOnServer()
        {
            List<Server> users = null;
            try
            {
                users = ServerAccessor.RetrievePlayersOnServer();
            }
            catch
            {
                throw;
            }
            return users;
        }

        public int AddPlayer(int playerId)
        {
            int result = 0;
            try
            {
                ServerAccessor.AddPlayerToServer(playerId);
                result += 1;
            }
            catch
            {
                //throw new ApplicationException("Did not add player to the server.");
            }

            return result;
        }
        public int UpdatePlayerToActive(int playerId)
        {
            int result = 0;
            try
            {
                ServerAccessor.UpdateToActive(playerId);
                result += 1;
            }
            catch
            {
                //throw new ApplicationException("Did not add player to the server.");
            }

            return result;
        }
        public int UpdatePlayerToInactive(int playerId)
        {
            int result = 0;
            try
            {
                ServerAccessor.UpdateToInactive(playerId);
                result += 1;
            }
            catch
            {
                //throw new ApplicationException("Did not add player to the server.");
            }

            return result;
        }
        public int PlayerMoved(int playerId,int x,int y,int z,int image, int ox, int oy, int oz)
        {
            int result = 0;
            try
            {
                ServerAccessor.PlayerMove(playerId,  x,  y,  z,  image,  ox,  oy,  oz);
                result += 1;
            }
            catch
            {
                //throw new ApplicationException("Did not add player to the server.");
            }

            return result;
        }



        //NEEDS WORK
        //public int DeletePlayer(int playerId)
        //{
        //    int result = 0;
        //    try
        //    {
        //        result = ServerAccessor.DeletePlayerFromServer(playerId);
        //    }
        //    catch
        //    {
        //        throw new ApplicationException("Did not add player to the server.");
        //    }

        //    return result;
        //}
    }
}
