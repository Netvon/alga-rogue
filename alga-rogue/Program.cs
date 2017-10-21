﻿using alga_rogue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alga_rogue
{
    class Program
    {
        static int AskForNumber(string question, int min = int.MinValue, int max = int.MaxValue)
        {
            var result = 0;
            var isValid = false;

            while (!isValid)
            {
                Ask();
                isValid = int.TryParse(Console.ReadLine(), out result);

                if (result < min || result > max)
                    isValid = false;

                if (!isValid)
                    NotifyOfError();
            }

            return result;

            void Ask()
            {
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine($"\n   {question}   ");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("=> ");
                Console.ResetColor();
            }

            void NotifyOfError()
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("~ ~ ~ Please enter a valid number ~ ~ ~");
                Console.ResetColor();
            }
        }

        static void Main(string[] args)
        {
            var dungeonBuilder = new DungeonBuilder();
            var dungeonDrawer = new DungeonDrawer();
            var playing = true;

            while(playing)
            {
                Console.WriteLine("Welcome to Alga Rogue");
                Console.WriteLine("We start by building a dungeon.");
                Console.WriteLine("Press any key to go continue...");
                Console.ReadKey();

                var width  = AskForNumber("Please insert the dungeon width");
                var height = AskForNumber("Please insert the dungeon height");
                var xStart = AskForNumber("Please insert the 'x' position of the Start", min: 0, max: width);
                var yStart = AskForNumber("Please insert the 'y' position of the Start", min: 0, max: height);

                var xExit  = AskForNumber("Please insert the 'x' position of the Exit", min: 0, max: width);
                var yExit  = AskForNumber("Please insert the 'y' position of the Exit", min: 0, max: height);

                Console.WriteLine();

                dungeonBuilder.PrepareDungeon(xStart, yStart, xExit, yExit, width, height);
                var dungeon = dungeonBuilder.CreateDungeon();

                dungeon.SetPlayer();
                dungeon.CheckVisibility();

                while (true)
                {
                    dungeonDrawer.Draw(dungeon);

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

                    if (dungeon.Player.Position == dungeon.Exit)
                    {
                        break;
                    }

                }

                Console.WriteLine();
                Console.WriteLine("You found the exit! Good Job!");
                Console.WriteLine("Wanna try again? Y or N?");
                string yorN = Console.ReadLine();

                if (yorN.Equals("N"))
                    break;
            }
            
        }
    }
}
