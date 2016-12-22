using GameDataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogicLayer
{
    public class ChangePassword
    {
        Player _user;
        public ChangePassword(Player user)
        {
            _user = user;
            
        }

        public int PasswordChangeSubmit(string oldPass, string newPass, string conPass)
        {
            var oldPassword = oldPass;
            var newPassword = newPass;
            var confirmPassword = conPass;

            if (newPassword == oldPassword)
            {
                throw new Exception ("New Password must be different then Old Password.");
                
            }
            if (newPassword != confirmPassword)
            {
                throw new Exception ("New Password must match.");
                
            }
            try
            {
                var usrMgr = new PlayerManager();
                usrMgr.UpdatePassword(_user, oldPassword, newPassword);
                return 1;
                
            }
            catch (Exception)
            {
                throw new Exception("Password not updated");
            }
        }
    }
}

