using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alga_rogue.Models
{
    class Dungeon
    {
        public int Width { get; set; }
        public int Height { get; set; }


        public Chamber Start { get; set; }
        public Chamber Exit { get; set; }

        private Chamber TopLeftChamber
        {
            get
            {
                var current = Start;

                while(current.Up != null)
                {
                    current = current.Up;
                }

                while (current.Left != null)
                {
                    current = current.Left;
                }

                return current;
            }
        }

        public void Print()
        {
            var startOfLine = TopLeftChamber;
            var current = startOfLine;

            while (startOfLine.Down != null)
            {
                while (current.Right != null)
                {
                    Console.Write(current.Print(this));
                    current = current.Right;
                }

                Console.WriteLine();

                startOfLine = startOfLine.Down;
            }
        }

    }
}
