using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDataObjects
{
    public class Player
    {
        
        public int PlayerID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int RoleID { get; set; }
        public int SavedLocationX { get; set; }
        public int SavedLocationY { get; set; }
        public int SavedLocationZ { get; set; }
        public int SavedHealth { get; set; }
        public int SavedImage { get; set; }
        public int SavedItem { get; set; }

        public bool Ban { get; set; }

    }
}
