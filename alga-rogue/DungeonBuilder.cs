using alga_rogue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alga_rogue
{
    class DungeonBuilder
    {
        public Dungeon CreateDungeon(int width, int height)
        {
            Dungeon dungeon = new Dungeon(width, height);
            

            // Tijdelijke dungeon
            for (int h = 0; h < dungeon.Height; h++)
            {
                for (int w = 0; w < dungeon.Width; w++)
                {
                    if (h == 0) { dungeon.TopLeftChamber = new Chamber(); }
                    



                }

            }


            return dungeon;
        }

    }
}
