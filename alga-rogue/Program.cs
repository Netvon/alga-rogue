using alga_rogue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alga_rogue
{
    class Program
    {
        static void Main(string[] args)
        {
            var dungeonBuilder = new DungeonBuilder();
            var dungeonDrawer = new DungeonDrawer();
            bool playing = true;

            while(playing) 
            {
                Console.WriteLine("Welkom to Alga Rogue");
                Console.WriteLine("We start bij building an dungeon.");
                Console.WriteLine("Press any key to go further");
                Console.ReadKey();
                Console.WriteLine("");

                Console.WriteLine("Please insert the dungeon width");
                int width = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("");

                Console.WriteLine("Please insert the dungeon height");
                int height = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("");

                Console.WriteLine("Please insert the x position of the Start");
                int xStart = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("");

                Console.WriteLine("Please insert the y position of the Start");
                int yStart = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("");

                Console.WriteLine("Please insert the x position of the Exit");
                int xExit = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("");

                Console.WriteLine("Please insert the y position of the Exit");
                int yExit = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("");

                dungeonBuilder.PrepareDungeon(xStart, yStart, xExit, yExit, width, height);
                var dungeon = dungeonBuilder.CreateDungeon();

                dungeon.SetPlayer();
                dungeon.CheckVisibility();

                while (true)
                {

                    dungeonDrawer.draw(dungeon);

                    Console.WriteLine("Commands: ");
                    Console.WriteLine("MoveUp");
                    Console.WriteLine("MoveDown");
                    Console.WriteLine("MoveRight");
                    Console.WriteLine("MoveLeft");
                    Console.WriteLine("Talisman");
                    Console.WriteLine("Handgranaat");
                    Console.WriteLine("Kompas");
                    Console.WriteLine();
                    Console.WriteLine("Choose one of the commands.");
                    string command = Console.ReadLine();

                    dungeon.DoCommand(command);

                }

                //Console.ReadKey();
            }

            
        }
    }
}
