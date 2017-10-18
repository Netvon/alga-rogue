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
            bool playing = true;

            while(playing) 
            {
                Console.WriteLine("Welkom to Alga Rogue");
                Console.WriteLine("We start bij building an dungeon.");
                Console.WriteLine("Press any key to go further");
                Console.ReadKey();

                var dungeon = dungeonBuilder.CreateDungeon();
                dungeon.Print();

                Console.ReadKey();
            }

            
        }
    }
}
