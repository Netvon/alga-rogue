using alga_rogue.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alga_rogue.Models
{
    class Dungeon
    {
        Chamber start, exit;
        Random random;

        public Player Player { get; private set; }

        public int Width { get; }
        public int Height { get; }

        public Chamber Start {
            get => start;
            set
            {
                if (start != null)
                {
                    start.IsStart = false;
                }

                start = value;
                start.IsStart = true;
            }
        }

        public Chamber Exit {
            get => exit;
            set
            {
                if (exit != null)
                {
                    exit.IsExit = false;
                }

                exit = value;
                exit.IsExit = true;
            }
        }

        public Chamber TopLeftChamber { get; set; }

        public Dungeon(int width, int height)
        {
            this.Width = width;
            this.Height = height;

            this.random = new Random();
        }

        public Chamber FindChamber(int x, int y)
        {
            // Omdat het 
            x--;
            y--;

            var startOfLine = this.TopLeftChamber;
            var current = this.TopLeftChamber;

            while (startOfLine != null)
            {
                while (current != null)
                {
                    if (current.XPos == x && current.YPos == y)
                        return current;

                    if (current.Right == null)
                        break;

                    current = current.Right;
                }
                Console.WriteLine();

                if (startOfLine.Down == null)
                    break;

                startOfLine = startOfLine.Down;
                current = startOfLine;
            }


            return null;
        }

        public Dungeon GenerateEnemies()
        {
            var startOfLine = this.TopLeftChamber;
            var current = this.TopLeftChamber;

            while (startOfLine != null)
            {
                while (current != null)
                {
                    current.Enemy = new Enemy();
                    current.Enemy.GenerateLevel(random);

                    if (current.Right == null)
                        break;

                    current = current.Right;
                }
                Console.WriteLine();

                if (startOfLine.Down == null)
                    break;

                startOfLine = startOfLine.Down;
                current = startOfLine;
            }

            return this;
        }

        public void SetPlayer()
        {
            Player = new Player();
            Player.Position = this.Start;
        }

        public void DoCommand(string command)
        {
            switch (command)
            {
                case "MoveUp":
                    this.Player.Move(Direction.Up);
                    this.CheckVisibility();
                    break;
                case "MoveDown":
                    this.Player.Move(Direction.Down);
                    this.CheckVisibility();
                    break;
                case "MoveRight":
                    this.Player.Move(Direction.Right);
                    this.CheckVisibility();
                    break;
                case "MoveLeft":
                    this.Player.Move(Direction.Left);
                    this.CheckVisibility();
                    break;
                case "Talisman":
                    var talisman = Talisman();
                    Console.WriteLine($"{Environment.NewLine}De talisman licht op en fluistert dat de trap omhoog {talisman.stepsFrom} kamers ver weg is. Berekend in {talisman.itterations} stappen");
                    break;
                case "Handgranaat":
                    this.Handgranaat();
                    Console.WriteLine($"{Environment.NewLine}De kerker schudt op zijn grondvesten, alle tegenstanders in de kamer zijn verslagen! Een donderend geluid maakt duidelijk dat gedeeltes van de kerker zijn ingestort...");
                    break;
                case "Kompas":

                    break;
                case "Cheat":
                    this.CheatMode();
                    Console.WriteLine();
                    Console.WriteLine("Hidden Command Found");
                    break;
                default:
                    break;
            }
        }

        
        public void Handgranaat()
        {
            // reset all "Visited" values
            // this is used for debugging purposes
            NotVisited();

            var startLookup = GetDirectionInfo(Player.Position);

            //Contains all the Chambers previously visited in the Search
            var previouslyVisited = new HashSet<Chamber>();

            // all the connections previously made
            // since one chamber can have more than one connections the values of this Dictionary are stored as a HashSet
            // the HashSet contains a Tuple for convenience.
            var connections = new Dictionary<Chamber, HashSet<(uint? weigth, Chamber chamber, Direction direction)>>();

            while (true)
            {
                var prev = previouslyVisited.Select(c => GetDirectionInfo(c))               // get all the DirectionIndo
                          .SelectMany(x => x)                                               // flatten the List of Lists to one big list
                          .Where(x => x.chamber != null)                                    // remove Directions that have no Chamber
                          .Where(x => !previouslyVisited.Contains(x.chamber[x.direction]));

                // create the list of possible connections by combined the prev and startLookup lists
                var possibleConnections = prev.Union(startLookup);

                // get all the connections that are possible to make
                // order the list by the weight of the connection (the level of the room)
                // the list will be ordered in ASC (1 -> 9)
                var possibleConnectionsOrdered = possibleConnections
                          .Where(to => to.chamber != null)                          // remove Directions that have no Chamber
                          .Where(to => !previouslyVisited.Contains(to.chamber))     // remove connections that have already been made
                          .OrderBy(x => x.weigth);                                  // order the list by weight

                // are there any connections possible?
                if (possibleConnectionsOrdered.Any())
                {
                    // take the cheapest connection
                    var to = possibleConnectionsOrdered.First();
                    var from = to.chamber[to.direction.Opposite()];

                    CreateOrUpdateConnections(from, to);

                    previouslyVisited.Add(from);
                    previouslyVisited.Add(to.chamber);

                    to.chamber.WasVisitedForSearch = true;
                    from.WasVisitedForSearch = true;

                    // have we arrived at the Exit?
                    //  If so, break the main loop. No need to search any further
                    if (to.chamber == Exit)
                        break;
                } else
                {
                    // it appears there are no more connections possible, stop the loop
                    break;
                }
            }

            DestroyHallways();
            // return; <- this would be the end point of the method

            // all the inline functions start at this point.
            // inline Methods are a new C# language feature
            #region Inline Functions
            bool ExistsInMST(Chamber chamber)
            {
                if (connections.ContainsKey(chamber))
                    return true;

                foreach (var connection in connections)
                {
                    if (connection.Value.Any(x => x.chamber == chamber))
                        return true;
                }

                return false;
            }

            (uint? weigth, Chamber chamber, Direction direction)[] GetDirectionInfo(Chamber c)
            {
                return new[]
                {
                    c.GetDirectionInfo(Direction.Up),
                    c.GetDirectionInfo(Direction.Down),
                    c.GetDirectionInfo(Direction.Left),
                    c.GetDirectionInfo(Direction.Right),
                };
            }

            void CreateOrUpdateConnections(Chamber from, (uint? weigth, Chamber chamber, Direction direction) to)
            {
                if (connections.ContainsKey(from))
                {
                    connections[from].Add(to);
                }
                else
                {
                    var toList = new HashSet<(uint? weigth, Chamber chamber, Direction direction)>() { to };
                    connections.Add(from, toList);
                }
            }

            void DestroyHallways()
            {
                var destoryCount = 0;
                var amountToDestroy = random.Next(10, 15);

                ForEach(chamber =>
                {
                    if (destoryCount >= amountToDestroy)
                        return;

                    var randomDirection = (Direction)random.Next(0, 4);

                    if (!ExistsInMST(chamber))
                    {
                        chamber.SetPassable(randomDirection, false);
                        chamber[randomDirection]?.SetPassable(randomDirection.Opposite(), false);

                        destoryCount++;
                    }
                });
            }
#endregion
        }

        public (int stepsFrom, int itterations) Talisman()
        {
            int itterations = 0;

            NotVisited();

            var visitedInSearch = new List<Chamber>();

            var queue           = new Queue<Chamber>();
            var whoVisitedWho   = new Dictionary<Chamber, Chamber>();
            int stepsFrom       = 0;

            queue.Enqueue(Player.Position);
            SetVisitedInSearch(Player.Position);

            while (queue.Count > 0)
            {
                itterations++;
                var currentChamber = queue.Dequeue();

                if (currentChamber == Exit)
                {
                    Chamber stepCounter = currentChamber;
                    while (stepCounter != Player.Position)
                    {
                        stepCounter = whoVisitedWho[stepCounter];
                        stepsFrom++;
                    }

                    return (stepsFrom, itterations);
                }

                if (currentChamber.Left != null && currentChamber.LeftPassable && WasNotVisitedInSearch(currentChamber.Left))
                {
                    whoVisitedWho[currentChamber.Left] = currentChamber;
                    queue.Enqueue(currentChamber.Left);

                    SetVisitedInSearch(currentChamber.Left);
                }

                if (currentChamber.Right != null && currentChamber.RightPassable && WasNotVisitedInSearch(currentChamber.Right))
                {
                    whoVisitedWho[currentChamber.Right] = currentChamber;
                    queue.Enqueue(currentChamber.Right);

                    SetVisitedInSearch(currentChamber.Right);
                }

                if (currentChamber.Up != null && currentChamber.UpPassable && WasNotVisitedInSearch(currentChamber.Up))
                {
                    whoVisitedWho[currentChamber.Up] = currentChamber;
                    queue.Enqueue(currentChamber.Up);

                    SetVisitedInSearch(currentChamber.Up);
                }

                if (currentChamber.Down != null && currentChamber.DownPassable && WasNotVisitedInSearch(currentChamber.Down))
                {
                    whoVisitedWho[currentChamber.Down] = currentChamber;
                    queue.Enqueue(currentChamber.Down);

                    SetVisitedInSearch(currentChamber.Down);
                }
            }

            return (0, itterations);

            void SetVisitedInSearch(Chamber chamber)
            {
                chamber.WasVisitedForSearch = true;
                visitedInSearch.Add(chamber);
            }

            bool WasNotVisitedInSearch(Chamber chamber)
            {
                return !visitedInSearch.Contains(chamber);
            }
        }

        public void CheatMode()
        {
            var startOfLine = this.TopLeftChamber;
            var current = this.TopLeftChamber;

            while (startOfLine != null)
            {
                while (current != null)
                {
                    current.IsVisible = true;

                    current = current.Right;
                }

                if (startOfLine.Down == null)
                    break;

                startOfLine = startOfLine.Down;
                current = startOfLine;
            }
        }

        public void NotVisited()
        {
            ForEach(chamber => chamber.WasVisitedForSearch = false);
        }

        void ForEach(Action<Chamber> hallo)
        {
            var startOfLine = TopLeftChamber;
            var current = TopLeftChamber;

            while (startOfLine != null)
            {
                while (current != null)
                {
                    hallo(current);

                    current = current.Right;
                }

                if (startOfLine.Down == null)
                    break;

                startOfLine = startOfLine.Down;
                current = startOfLine;
            }
        }

        public void CheckVisibility()
        {
            ForEach(chamber =>
            {
                if (chamber.Up == Player.Position)
                    chamber.IsVisible = true;

                if (chamber.Down == Player.Position)
                    chamber.IsVisible = true;

                if (chamber.Left == Player.Position)
                    chamber.IsVisible = true;

                if (chamber.Right == Player.Position)
                    chamber.IsVisible = true;
            });
        }

        public void DestroyChambers(Dictionary<Chamber, Chamber> MST)
        {
            int destructions = 0;
            Random r = new Random();

            while (destructions < 10)
            {
                ForEach(chamber =>
                {
                    Chamber chamber1 = MST[chamber];
                    Chamber right = MST[chamber.Right];
                    Chamber down = MST[chamber.Down];

                    if (chamber.Right != null && chamber1 != chamber.Right && right != chamber && r.Next(1, 2) == 2)
                    {
                        chamber.RightPassable = false;
                        chamber.Right.LeftPassable = false;
                        destructions++;
                    }
                    
                    if (chamber.Down != null && chamber1 != chamber.Down && down != chamber && r.Next(1, 2) == 2)
                    {
                        chamber.DownPassable = false;
                        chamber.Down.UpPassable = false;
                        destructions++;
                    }
                });

                break;
            }
        }
    }
}
