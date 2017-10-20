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
        Dictionary<Direction, Chamber> chamberDictionary;

        // Just for testing the dungeon
        public int xPos, yPos;
        
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

        public bool HasEnemy => Enemy != null;

        public Chamber(int xPos, int yPos)
        {
            this.xPos = xPos;
            this.yPos = yPos;

            this.IsVisible = false;

            chamberDictionary = new Dictionary<Direction, Chamber>();

            chamberDictionary.Add(Direction.Up, null);
            chamberDictionary.Add(Direction.Down, null);
            chamberDictionary.Add(Direction.Right, null);
            chamberDictionary.Add(Direction.Left, null);

            this.UpPassable = true;
            this.DownPassable = true;
            this.LeftPassable = true;
            this.RightPassable = true;
        }

        public char Print(Dungeon dungeon)
        {
            // ...
            if (IsStart)
                return 'S';

            if (!IsVisible)
                return '.';

            if (IsExit)
                return 'E';

            return Convert.ToChar(this.Enemy.Level.ToString());
        }
    }
}
