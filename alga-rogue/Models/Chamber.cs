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

        //public Chamber Up => chamberDictionary[Direction.Up];

        public Chamber Up
        {
            get => chamberDictionary[Direction.Up];
            set => AddChamber(Direction.Up, value);
        }

        public Chamber Down
        {
            get => chamberDictionary[Direction.Down];
            set => AddChamber(Direction.Down, value);
        }

        public Chamber Left
        {
            get => chamberDictionary[Direction.Left];
            set => AddChamber(Direction.Left, value);
        }

        public Chamber Right
        {
            get => chamberDictionary[Direction.Right];
            set => AddChamber(Direction.Right, value);
        }

        public Enemy Enemy { get; set; }
        public bool IsVistited { get; set; }

        public bool HasEnemy => Enemy != null;

        public Chamber()
        {
            chamberDictionary = new Dictionary<Direction, Chamber>();

            chamberDictionary.Add(Direction.Up, null);
            chamberDictionary.Add(Direction.Down, null);
            chamberDictionary.Add(Direction.Right, null);
            chamberDictionary.Add(Direction.Left, null);
        }

        public void AddChamber(Direction direction, Chamber chamber)
        {
            chamberDictionary[direction] = chamber;

            if (chamber.GetChamber(direction.Opposite()) == null)
                chamber.AddChamber(direction.Opposite(), this);
        }

        private Chamber GetChamber(Direction direction)
        {
            return chamberDictionary[direction];
        }

        public char Print(Dungeon dungeon)
        {
            // ...
            if(IsVistited == false)
                return '.';

            if (dungeon.Start == this)
                return 'S';

            if (dungeon.Exit == this)
                return 'E';

            return 'N';
        }
    }
}
