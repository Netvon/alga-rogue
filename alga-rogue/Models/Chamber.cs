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
        readonly Dictionary<Direction, bool> chamberPassible;

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

        public Chamber this[Direction direction] => chamberDictionary[direction];

        public bool IsPassable(Direction direction)
        {
            return chamberPassible[direction];
        }

        public void SetPassable(Direction direction, bool isPassable)
        {
            chamberPassible[direction] = isPassable;
        }

        public bool UpPassable
        {
            get => chamberPassible[Direction.Up];
            set => chamberPassible[Direction.Up] = value;
        }

        public bool DownPassable
        {
            get => chamberPassible[Direction.Down];
            set => chamberPassible[Direction.Down] = value;
        }

        public bool LeftPassable
        {
            get => chamberPassible[Direction.Left];
            set => chamberPassible[Direction.Left] = value;
        }

        public bool RightPassable
        {
            get => chamberPassible[Direction.Right];
            set => chamberPassible[Direction.Right] = value;
        }

        public uint? LeftWeight => Left?.Enemy.Level;
        public uint? RightWeight => Right?.Enemy.Level;
        public uint? UpWeight => Up?.Enemy.Level;
        public uint? DownWeight => Down?.Enemy.Level;

        public (uint? weigth, bool passable, Chamber chamber, Direction direction) GetDirectionInfo(Direction direction)
        {
            return
            (
                this[direction]?.Enemy.Level, IsPassable(direction), this[direction], direction
            );
        }

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

            chamberPassible = new Dictionary<Direction, bool>();

            UpPassable = true;
            DownPassable = true;
            LeftPassable = true;
            RightPassable = true;
        }

        public string Print()
        {
            // ...
            if (IsStart)
                return "S";

            if (!IsVisible)
                return ".";

            if (IsExit)
                return "E";

            return Enemy.Level.ToString();
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

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
