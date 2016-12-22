using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDataObjects
{
    //Server is not so much a object of a server but Player info on a server that can update the player
    //Table and then empty the Server Table if it becomes to large this will allow for quicker
    //searches when ingame
    public class Server
    {

        public int PlayerID { get; set; }
        public string UserName { get; set; }
        public int LocationX { get; set; }
        public int LocationY { get; set; }
        public int LocationZ { get; set; }
        public int Health { get; set; }
        public int Image { get; set; }
        public int EquiptItem { get; set; }

        public int Size { get; set; }
        public int SizeMultiplier { get; set; }


    }
}
