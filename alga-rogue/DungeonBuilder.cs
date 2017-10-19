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
        int xStart, yStart, xExit, yExit, dungeonWidth, dungeonHeight;


        public void PrepareDungeon(int xStart, int yStart, int xExit, int yExit, int dungeonWidth, int dungeonHeight)
        {
            this.xStart = xStart;
            this.yStart = yStart;
            this.xExit = xExit;
            this.yExit = yExit;
            this.dungeonWidth = dungeonWidth;
            this.dungeonHeight = dungeonHeight;
        }

        public Dungeon CreateDungeon()
        {
            Dungeon dungeon = new Dungeon(dungeonWidth, dungeonHeight);
            Chamber[] previousRow = null;

            for (int h = 0; h < dungeon.Height; h++)
            {
                Chamber[] currentRow = new Chamber[dungeon.Width];
                Chamber previousChamber = null;
                Chamber currentChamber = null;

                for (int w = 0; w < dungeon.Width; w++)
                {
                    currentChamber = new Chamber(w, h);
                    if (previousChamber != null)
                    {
                        currentChamber.Left = previousChamber;
                        previousChamber.Right = currentChamber;
                    }

                    if (h == 0 && w == 0) { dungeon.TopLeftChamber = currentChamber; }
                    currentRow[w] = currentChamber;
                    previousChamber = currentChamber;
                }

                if (previousRow != null)
                {
                    for (int w = 0; w < dungeon.Width; w++)
                    {
                        var upperChamber = previousRow[w];
                        var lowerChamber = currentRow[w];

                        lowerChamber.Up = upperChamber;
                        upperChamber.Down = lowerChamber;
                    }
                }

                previousRow = currentRow;
            }

            dungeon = this.SetStart(dungeon);
            dungeon = this.SetExit(dungeon);
            dungeon = this.GenerateEnemies(dungeon);


            return dungeon;
        }

        public Dungeon SetStart(Dungeon dungeon)
        {
            Chamber startChamber = dungeon.FindChamber(xStart, yStart);

            if (startChamber == null)
                Console.WriteLine("No Chamber found with the position: " + xStart + " " + yStart);

            dungeon.Start = startChamber;
            return dungeon;
        }


        public Dungeon SetExit(Dungeon dungeon)
        {
            Chamber exitChamber = dungeon.FindChamber(xExit, yExit);

            if (exitChamber == null)
                Console.WriteLine("No Chamber found with the position: " + xExit + " " + yExit);

            dungeon.Exit = exitChamber;
            return dungeon;
        }

        public Dungeon GenerateEnemies(Dungeon dungeon)
        {
            dungeon = dungeon.GenerateEnemies();
            return dungeon;
        }

    }
}
