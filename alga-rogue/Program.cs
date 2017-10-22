using alga_rogue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using alga_rogue.Util;
using alga_rogue.Input;

namespace alga_rogue
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "ALGA Rogue";

            var cmdList = new CommandList();

            var dungeonDrawer = new DungeonDrawer();
            var playing = true;

            while(playing)
            {
                Console.WriteLine("Welcome to Alga Rogue");
                Console.WriteLine("We start by building a dungeon.");
                Console.WriteLine("Press any key to go continue...");
                Console.ReadKey();

                Dungeon dungeon = BuildDungeon();

                dungeon.SetPlayer();
                dungeon.CheckVisibility();
                dungeon.CheatMode();

                while (true)
                {
                    dungeonDrawer.Draw(dungeon);
#if DEBUG
                    DrawDebug(dungeon);
#endif

                    cmdList.AskForCommand(dungeon);

                    if (dungeon.Player.Position == dungeon.Exit)
                    {
                        break;
                    }

                    Console.WriteLine("\nPress any key to go continue...");
                    Console.ReadKey();

                    Console.Clear();
                }

                Console.WriteLine();
                Console.WriteLine("You found the exit! Good Job!");
                Console.WriteLine("Wanna try again? Y or N?");
                string yorN = Console.ReadLine();

                if (yorN.Equals("N"))
                    break;
            }
        }

        private static Dungeon BuildDungeon()
        {
            var dungeonBuilder = new DungeonBuilder();
#if DEBUG
            var width = 10;
            var height = 10;
            var xStart = 3;
            var yStart = 3;

            var xExit = 8;
            var yExit = 8;
#else
            var width = Question.AskForNumber("Please insert the dungeon width");
            var height = Question.AskForNumber("Please insert the dungeon height");
            var xStart = Question.AskForNumber("Please insert the 'x' position of the Start", min: 0, max: width);
            var yStart = Question.AskForNumber("Please insert the 'y' position of the Start", min: 0, max: height);

            var xExit = Question.AskForNumber("Please insert the 'x' position of the Exit", min: 0, max: width);
            var yExit = Question.AskForNumber("Please insert the 'y' position of the Exit", min: 0, max: height);
#endif

            Console.Clear();

            dungeonBuilder.PrepareDungeon(xStart, yStart, xExit, yExit, width, height);
            return dungeonBuilder.CreateDungeon();
        }

        private static void DrawDebug(Dungeon dungeon)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Debug Info:");
            Console.WriteLine($"\tUpPassible:\t{dungeon.Player.Position.UpPassable}");
            Console.WriteLine($"\tDownPassible:\t{dungeon.Player.Position.DownPassable}");
            Console.WriteLine($"\tLeftPassible:\t{dungeon.Player.Position.LeftPassable}");
            Console.WriteLine($"\tRightPassible:\t{dungeon.Player.Position.RightPassable}");
            Console.WriteLine();
            Console.ResetColor();
        }
    }
}
