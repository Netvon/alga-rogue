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

        public void draw(Dungeon dungeon)
        {
            var startOfLine = dungeon.TopLeftChamber;
            var current = dungeon.TopLeftChamber;

            while (startOfLine != null)
            {

                while (current != null)
                {
                    if(current.Left != null)
                    {
                        Console.Write(" ");
                    }
                    
                    if (current.Up?.UpPassable == true)
                    {
                        Console.Write("|");
                    }
                    else if (current.Up?.UpPassable == false)
                    {
                        Console.Write("/");
                    }

                    if (current.Right != null)
                    {
                        Console.Write(" ");
                    }

                    if (current.Right == null)
                    {
                        current = startOfLine;
                        break;
                    }

                    current = current.Right;
                }

                Console.WriteLine();

                while (current != null)
                {
                    if(current.Left?.LeftPassable == true)
                    {
                        Console.Write("-");
                    } else if (current.Left?.LeftPassable == false)
                    {
                        Console.Write("~");
                    }

                    if (current != null && current == dungeon.Player.Position)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }

                    Console.Write(current.Print(dungeon));
                    Console.ForegroundColor = ConsoleColor.White;

                    if (current.Right?.RightPassable == true)
                    {
                        Console.Write("-");
                    }
                    else if (current.Right?.RightPassable == false)
                    {
                        Console.Write("~");
                    }

                    if (current.Right == null)
                    {
                        current = startOfLine; 
                        break;
                    }

                    current = current.Right;
                }

                Console.WriteLine();

                while (current != null)
                {

                    if (current.Left != null)
                    {
                        Console.Write(" ");
                    }
                    
                    if (current.Down?.DownPassable == true)
                    {
                        Console.Write("|");
                    }
                    else if (current.Down?.DownPassable == false)
                    {
                        Console.Write("/");
                    }

                    if (current.Right != null)
                    {
                        Console.Write(" ");
                    }

                    if (current.Right == null)
                    {
                        current = startOfLine;
                        break;
                    }

                    current = current.Right;
                }

                Console.WriteLine();

                if (startOfLine.Down == null)
                    break;

                startOfLine = startOfLine.Down;
                current = startOfLine;
            }
           
        }

    }
}
