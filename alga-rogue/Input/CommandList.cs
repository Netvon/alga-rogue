using alga_rogue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using alga_rogue.Alg;

namespace alga_rogue.Input
{
    class CommandList
    {
        List<Command> commands;

        public CommandList()
        {
            commands = new List<Command>()
            {
                new Command("MoveRight", Move(Direction.Right)),
                new Command("MoveLeft", Move(Direction.Left)),
                new Command("MoveUp", Move(Direction.Up)),
                new Command("MoveDown", Move(Direction.Down)),

                new Command("Talisman", Talisman),
                new Command("Handgranaat", Handgranaat),
                new Command("Kompas", Kompas),
                new Command("KompasCheat", KompassCheat, hidden: true),

                new Command("Cheat", CheatMode, hidden: true)
            };
        }

        void KompassCheat(Dungeon obj)
        {
            var result = obj.Dijkstra();
            obj.SetHighLevels(result);
        }

        void CheatMode(Dungeon obj)
        {
            obj.CheatMode();
        }

        Action<Dungeon> Move(Direction direction)
        {
            return (d) =>
            {
                if (d.Player.Position[direction] == null)
                    return;

                if (!d.Player.Position.IsPassable(direction))
                    return;

                d.Player.Move(direction);
                d.CheckVisibility();
            };
        }

        void Talisman(Dungeon dungeon)
        {
            var talisman = dungeon.Talisman();
            Console.WriteLine($"{Environment.NewLine}De talisman licht op en fluistert dat de trap omhoog {talisman.stepsFrom} kamers ver weg is. Berekend in {talisman.itterations} stappen");
        }

        void Kompas(Dungeon obj)
        {
            obj.ShortestPath(obj.Start, obj.Exit);
        }

        void Handgranaat(Dungeon obj)
        {
            obj.Handgranaat();
            Console.WriteLine($"{Environment.NewLine}De kerker schudt op zijn grondvesten, alle tegenstanders in de kamer zijn verslagen! Een donderend geluid maakt duidelijk dat gedeeltes van de kerker zijn ingestort...");
        }

        public void RunCommand(Dungeon dungeon, string input)
        {
            var cmd = commands.FirstOrDefault(x => x.Key.Equals(input, StringComparison.InvariantCultureIgnoreCase));
            cmd?.Action(dungeon);
        }

        public void AskForCommand(Dungeon dungeon)
        {
            Console.WriteLine("Please enter a command");

            Console.WriteLine("Commands: ");
            Keys.ForEach(cmd => Console.WriteLine($" - {cmd}"));

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("=> ");
            Console.ResetColor();

            RunCommand(dungeon, Console.ReadLine());
        }
#if DEBUG
        public List<string> Keys => commands.Select(x => x.Key).ToList();
#else
        public List<string> Keys => commands.Where(x => !x.Hidden).Select(x => x.Key).ToList();
#endif
    }
}
