using alga_rogue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alga_rogue.Util
{
    static class ChamberLocationExtension
    {
        public static Direction Opposite(this Direction location)
        {
            var type = typeof(Direction);
            var memberInfo = type.GetMember(location.ToString());

            var attribute = memberInfo.First().GetCustomAttributes(typeof(OppositeAttribute), false);
            var opposite = attribute.First() as OppositeAttribute;

            return opposite.Opposite;
        }
    }
}
