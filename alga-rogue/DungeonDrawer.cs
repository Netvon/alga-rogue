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
                    if(current.Left?.LeftPassable == true)
                    {
                        Console.Write("-");
                    } else if (current.Left?.LeftPassable == false)
                    {
                        Console.Write("~");
                    }

                    Console.Write(current.Print(dungeon));

                    if (current.Right?.RightPassable == true)
                    {
                        Console.Write("-");
                    }
                    else if (current.Right?.RightPassable == false)
                    {
                        Console.Write("~");
                    }

                    if (current.Right == null)
                        break;

                    current = current.Right;
                }

                Console.WriteLine();

                // 1ste lijn met de current.Down

                Console.WriteLine();

                // 2de lijn met de current.Up

                Console.WriteLine();

                if (startOfLine.Down == null)
                    break;

                startOfLine = startOfLine.Down;
                current = startOfLine;
            }
           
        }

    }
}
