using alga_rogue.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alga_rogue.Models
{
    class Chamber
    {
        public Guid Id { get; } = Guid.NewGuid();

        readonly Dictionary<Direction, Chamber> chamberDictionary;

        // Just for testing the dungeon
        public int XPos { get; }
        public int YPos { get; }

        public bool WasVisitedForSearch { get; set; }

        public Chamber Up
        {
            get => chamberDictionary[Direction.Up];
            set => chamberDictionary[Direction.Up] = value;
        }

        public Chamber Down
        {
            get => chamberDictionary[Direction.Down];
            set => chamberDictionary[Direction.Down] = value;
        }

        public Chamber Left
        {
            get => chamberDictionary[Direction.Left];
            set => chamberDictionary[Direction.Left] = value;
        }

        public Chamber Right
        {
            get => chamberDictionary[Direction.Right];
            set => chamberDictionary[Direction.Right] = value;
        }

        public bool UpPassable { get; set; }
        public bool DownPassable { get; set; }
        public bool LeftPassable { get; set; }
        public bool RightPassable { get; set; }

        public Enemy Enemy { get; set; }

        public bool IsVisible { get; set; }
        public bool IsStart { get; set; }
        public bool IsExit { get; set; }

        public Chamber(int xPos, int yPos)
        {
            XPos = xPos;
            YPos = yPos;

            IsVisible = false;

            chamberDictionary = new Dictionary<Direction, Chamber>
            {
                { Direction.Up, null },
                { Direction.Down, null },
                { Direction.Right, null },
                { Direction.Left, null }
            };

            UpPassable = true;
            DownPassable = true;
            LeftPassable = true;
            RightPassable = true;
        }

        public char Print()
        {
            // ...
            if (IsStart)
                return 'S';

            if (!IsVisible)
                return '.';

            if (IsExit)
                return 'E';

            return Convert.ToChar(Enemy.Level.ToString());
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if(obj is Chamber other)
            {
                return other.Id.Equals(Id);
            }

            return false;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
