using System;

namespace alga_rogue.Models
{
    public class Enemy
    {
        public uint Level { get; set; }

        public void GenerateLevel(Random random)
        {
            Level = (uint) random.Next(1, 10);
        }
    }
}