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

        public Chamber TopLeftChamber { get; set; }

        public Dungeon(int width, int height)
        {
            this.Width = width;
            this.Height = height;
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
