using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDataObjects
{
    public static class ServerCustomMethods
    {

        public static int Top(this Server player) { return player.LocationY + (player.Size * player.SizeMultiplier); }
        public static int Right(this Server player) { return player.LocationX + (player.Size * player.SizeMultiplier); }

    }
}
