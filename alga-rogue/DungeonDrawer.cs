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
                for (int i = 0; i < 3; i++)
                {
                    while (current != null)
                    {
                        if (i == 0)
                            DrawUpLine(current);

                        if (i == 1)
                            DrawMiddleLine(current, dungeon);

                        if (i == 2)
                            DrawDownLine(current);

                        current = current.Right;
                    }
                    Console.WriteLine();
                    current = startOfLine;
                }

                startOfLine = startOfLine.Down;
                current = startOfLine;
            }
        }

        void DrawUpLine(Chamber current)
        {
            if (current.Up == null)
                return;

            if (current.Left != null)
                Console.Write("   ");

            if (current.UpPassable)
                Console.Write(" | ");

            if (!current.UpPassable)
                Console.Write(" / ");

            if (current.Right != null)
                Console.Write("   ");
        }

        void DrawMiddleLine(Chamber current, Dungeon dungeon)
        {
            if (current.Left != null)
            {
                if (current.LeftPassable)
                    Console.Write("---");
                else
                    Console.Write("~~~");
            }

            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Gray;

            if (current?.WasVisitedForSearch == true)
            {
                Console.BackgroundColor = ConsoleColor.Green;
            }

            if (current != null && current == dungeon.Player.Position)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
            }

            Console.Write($" {current.Print()} ");
            Console.ResetColor();

            if (current.Right != null)
            {
                if (current.RightPassable)
                    Console.Write("---");
                else
                    Console.Write("~~~");
            }
        }

        void DrawDownLine(Chamber current)
        {
            if (current.Down == null)
                return;

            if (current.Left != null)
                Console.Write("   ");

            if (current.DownPassable)
                Console.Write(" | ");

            if (!current.DownPassable)
                Console.Write(" / ");

            if (current.Right != null)
                Console.Write("   ");
        }
    }
}
