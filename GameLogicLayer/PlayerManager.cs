using GameDatabaseAccessor;
using GameDataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GameLogicLayer
{
    public class PlayerManager
    {

        public Player GetPlayer(string username)
        {
            Player user = null;
            try
            {
                user = PlayerAccessor.RetrievePlayerByUsername(username);
            }
            catch
            {
                throw;
            }
            return user;
        }

        public Player GetPlayerViaEmail(string email)
        {
            Player user = null;
            try
            {
                user = PlayerAccessor.RetrievePlayerByUsername(email);
            }
            catch
            {
                throw;
            }
            return user;
        }

        public Player AuthenticatePlayer(string username, string password)
        { 
            Player user = null;
            
            if (username.Length > 20 || username.Length< 5)
            {
                throw new ApplicationException("Invalid Username");
            }
            if (password.Length< 7 || password.Length> 30)
            {
                throw new ApplicationException("Invalid Password");
            }

            // need a data access method to check the password
            try
            {
                //Check for valid user
                if (1 == PlayerAccessor.VerifyUsernameAndPassword(username, HashSha256(password)))
                {
                    password = null;
                    user = PlayerAccessor.RetrievePlayerByUsername(username);
                    
                    
                }
                else
                {
                    throw new ApplicationException("Username and Password does not match.");
                }
            }
            catch
            {
                throw;
            }

            return user;
        }

        public int AddPlayer(string username, string email, string password)
        {
            int result = 0;

            if (username.Length > 20 || username.Length < 5)
            {
                throw new ApplicationException("Invalid Username");
            }
            if (password.Length < 7 || password.Length > 30)
            {
                throw new ApplicationException("Invalid Password");
            }

            // need a data access method to check the password
            try
            {
                //Check for valid user
                if (1 == PlayerAccessor.AddNewPlayer(username, email, HashSha256(password)))
                {
                    password = null;
                    result = 1;


                }
                else
                {
                    throw new ApplicationException("Did not create new player.");
                }
            }
            catch
            {
                throw new ApplicationException("Did not create new player.");
            }

            return result;
        }

        private string HashSha256(string source)
        {
            string result = "";

            // create a byte array
            byte[] data;

            // create a .NET Hash provider object
            using (SHA256 sha256hash = SHA256.Create())
            {
                // hash the input
                data = sha256hash.ComputeHash(Encoding.UTF8.GetBytes(source));
            }
            // create a string builder
            var s = new StringBuilder();

            // loop through the data creating letters for the string
            for (int i = 0; i < data.Length; i++)
            {
                s.Append(data[i].ToString("x2"));
            }
            result = s.ToString();
            return result;
        }
        public bool UpdatePassword(Player user, string oldPassword, string newPassword)
        {
            var result = false;

            try
            {
                PlayerAccessor.UpdatePasswordHash(user.PlayerID, HashSha256(oldPassword), HashSha256(newPassword));
                result = true;
            }
            catch (Exception)
            {
                throw;
            }
            return result;

        }

       
    }
}
