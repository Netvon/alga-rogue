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
            var dungeon = new Dungeon()
            {
                Start = new Chamber()
            };

            //dungeon.Start.AddChamber(Direction.Up, new Chamber());
            Chamber chamber = new Chamber();
            dungeon.Start.AddChamber(Direction.Right, chamber);
            Chamber chamber1 = new Chamber();
            chamber.AddChamber(Direction.Down, chamber1);
            chamber1.AddChamber(Direction.Left, chamber1);
            //dungeon.Start.AddChamber(Direction.Right, new Chamber());
            //dungeon.Start.AddChamber(Direction.Down, new Chamber());

            dungeon.Print();

            Console.ReadKey();
        }
    }
}
