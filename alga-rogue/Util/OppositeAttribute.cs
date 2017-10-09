using alga_rogue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alga_rogue.Util
{
    internal class OppositeAttribute : Attribute
    {
        public Direction Opposite { get; set; }

        public OppositeAttribute(Direction opposite)
        {
            Opposite = opposite;
        }
    }
}
