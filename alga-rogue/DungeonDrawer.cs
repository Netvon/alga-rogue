using alga_rogue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alga_rogue
{
    class DungeonDrawer
    {
 
        public void Draw(Dungeon dungeon)
        {
            var startOfLine = dungeon.TopLeftChamber;
            var current = dungeon.TopLeftChamber;

            while (startOfLine != null)
            {
                for (int i = 0; i < 3; i++) { 
                
                    while (current != null)
                    {
                        if (i == 0)
                            drawUpLine(current);

                        if (i == 1)
                            drawMiddleLine(current, dungeon);

                        if (i == 2)
                            drawDownLine(current);

                        current = current.Right;
                    }
                    Console.WriteLine();
                    current = startOfLine;
                }

                startOfLine = startOfLine.Down;
                current = startOfLine;
            }
        }

        public void drawUpLine(Chamber current)
        {
            if (current.Up == null)
                return;

            if (current.Left != null)
                Console.Write(" ");

            if (current.UpPassable)
                Console.Write("|");

            if (!current.UpPassable)
                Console.Write("/");

            if (current.Right != null)
                Console.Write(" ");
        }

        public void drawMiddleLine(Chamber current, Dungeon dungeon)
        {
            if (current.Left != null)
            {
                if (current.LeftPassable)
                    Console.Write("-");
                else
                    Console.Write("~");
            }

            if (current?.WasVisitedForSearch == true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }

            if (current != null && current == dungeon.Player.Position)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }

            Console.Write(current.Print());
            Console.ForegroundColor = ConsoleColor.White;

            if (current.Right != null)
            {
                if (current.RightPassable)
                    Console.Write("-");
                else
                    Console.Write("~");
            }
        }

        public void drawDownLine(Chamber current)
        {
            if (current.Down == null)
                return;

            if (current.Left != null)
                Console.Write(" ");

            if (current.DownPassable)
                Console.Write("|");

            if (!current.DownPassable)
                Console.Write("/");

            if (current.Right != null)
                Console.Write(" ");
        }
    }
}
