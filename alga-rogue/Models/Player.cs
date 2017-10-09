using alga_rogue.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alga_rogue.Models
{
    class Player
    {
        public Chamber Position { get; set; }

        public void Move(Direction direction)
        {
            // ...
            switch (direction)
            {
                case Direction.Up:
                    Position = Position.Up;
                    break;
                case Direction.Down:
                    Position = Position.Down;
                    break;
                case Direction.Left:
                    Position = Position.Left;
                    break;
                case Direction.Right:
                    Position = Position.Right;
                    break;
                default:
                    break;
            }
        }
    }

    enum Direction
    {
        [Opposite(Down)]
        Up,
        [Opposite(Up)]
        Down,
        [Opposite(Right)]
        Left,
        [Opposite(Left)]
        Right
    }
}
