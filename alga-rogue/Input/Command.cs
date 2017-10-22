using alga_rogue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alga_rogue.Input
{
    class Command
    {
        public string Key { get; }
        public Action<Dungeon> Action { get; }
        public bool Hidden { get; } = false;

        public Command(string key, Action<Dungeon> action, bool hidden = false)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("message", nameof(key));
            }

            Key = key;
            Action = action ?? throw new ArgumentNullException(nameof(action));

            Hidden = hidden;
        }
    }
}
