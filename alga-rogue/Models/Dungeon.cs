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
            NotVisited();

            Func<Chamber, (uint? weigth, bool passable, Chamber chamber, Direction direction)[]> getDirectionInfo = (Chamber c) => new[] {
                c.GetDirectionInfo(Direction.Up),
                c.GetDirectionInfo(Direction.Down),
                c.GetDirectionInfo(Direction.Left),
                c.GetDirectionInfo(Direction.Right),
            };

            var startLookup = getDirectionInfo(Exit);

            var previouslyVisited = new HashSet<Chamber>();
            var connections = new Dictionary<Chamber, HashSet<(uint? weigth, bool passable, Chamber chamber, Direction direction)>>();

            while (true)
            {

                var prev = previouslyVisited.Select(c => getDirectionInfo(c))
                          .SelectMany(x => x)
                          .Where(x => x.chamber != null)
                          .Where(x => !previouslyVisited.Contains(x.chamber[x.direction]));

                var h = prev.Union(startLookup);

                var h2 = h.Where(to => to.chamber != null)
                          .Where(to => !previouslyVisited.Contains(to.chamber))
                          .OrderBy(x => x.weigth);

                if (h2.Any())
                {
                    var to = h2.First();
                    var from = to.chamber[to.direction.Opposite()];

                    if(connections.ContainsKey(from))
                    {
                        connections[from].Add(to);
                    } else
                    {
                        var toList = new HashSet<(uint? weigth, bool passable, Chamber chamber, Direction direction)>() { to };
                        connections.Add(from, toList);
                    }

                    previouslyVisited.Add(from);
                    previouslyVisited.Add(to.chamber);

                    to.chamber.WasVisitedForSearch = true;
                    from.WasVisitedForSearch = true;

                    if (to.chamber == Exit)
                        break;
                } else
                {
                    break;
                }
               
            }

            var destoryCount = 0;

            ForEach(chamber =>
            {
                if (destoryCount > 10)
                    return;

                var randomDirection = (Direction)random.Next(0, 4);

                if(!ExistsInMST(chamber))
                {
                    chamber.SetPassable(randomDirection, false);
                    chamber[randomDirection]?.SetPassable(randomDirection.Opposite(), false);

                    destoryCount++;
                }
            });

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

            //var visitedInSearch = new List<Chamber>();























            

            //var q   = new Queue<Chamber>();
            //var MST = new Dictionary<Chamber, Chamber>();

            //q.Enqueue(Player.Position);
            //SetVisitedInSearch(Player.Position);

            //while (q.Count > 0)
            //{
            //    Chamber current = q.Dequeue();

            //    if (current == Exit)
            //    {
            //        DestroyChambers(MST);
            //        return;
            //    }

            //    ProcessChamber(current, Direction.Left);
            //    ProcessChamber(current, Direction.Right);
            //    ProcessChamber(current, Direction.Up);
            //    ProcessChamber(current, Direction.Down);

            //    //var testsLeft = new[] {
            //    //    current.Left?.Enemy.Level < current.Up?.Enemy.Level,
            //    //    current.Left?.Enemy.Level < current.Down?.Enemy.Level,
            //    //    current.Left?.Enemy.Level < current.Right?.Enemy.Level
            //    //};

            //    //var testsRight = new[] {
            //    //    current.Right?.Enemy.Level < current.Up?.Enemy.Level,
            //    //    current.Right?.Enemy.Level < current.Down?.Enemy.Level,
            //    //    current.Right?.Enemy.Level < current.Left?.Enemy.Level
            //    //};

            //    //var testsUp = new[] {
            //    //    current.Up?.Enemy.Level < current.Down?.Enemy.Level,
            //    //    current.Up?.Enemy.Level < current.Right?.Enemy.Level,
            //    //    current.Up?.Enemy.Level < current.Left?.Enemy.Level
            //    //};

            //    //var testsDown = new[] {
            //    //    current.Down?.Enemy.Level < current.Up?.Enemy.Level,
            //    //    current.Down?.Enemy.Level < current.Right?.Enemy.Level,
            //    //    current.Down?.Enemy.Level < current.Left?.Enemy.Level
            //    //};

            //    //if (current.Left != null
            //    //    && current.LeftPassable
            //    //    && WasNotVisitedInSearch(current.Left)
            //    //    && current.Left.Enemy.Level < current.Down?.Enemy.Level
            //    //    && current.Left.Enemy.Level < current.Up?.Enemy.Level
            //    //    && current.Left.Enemy.Level < current.Right?.Enemy.Level)
            //    //{
            //    //    MST[current.Left] = current;
            //    //    q.Enqueue(current.Left);

            //    //    SetVisitedInSearch(current.Left);
            //    //}

            //    //if (current.Right != null
            //    //    && current.RightPassable
            //    //    && WasNotVisitedInSearch(current.Right)
            //    //    && current.Right.Enemy.Level < current.Down?.Enemy.Level
            //    //    && current.Right.Enemy.Level < current.Up?.Enemy.Level
            //    //    && current.Right.Enemy.Level < current.Left?.Enemy.Level)
            //    //{
            //    //    MST[current.Right] = current;
            //    //    q.Enqueue(current.Right);

            //    //    SetVisitedInSearch(current.Right);
            //    //}

            //    //if (current.Up != null
            //    //    && current.UpPassable
            //    //    && WasNotVisitedInSearch(current.Up)
            //    //    && current.Up.Enemy.Level < current.Down?.Enemy.Level
            //    //    && current.Up.Enemy.Level < current.Right?.Enemy.Level
            //    //    && current.Up.Enemy.Level < current.Left?.Enemy.Level)
            //    //{
            //    //    MST[current.Up] = current;
            //    //    q.Enqueue(current.Up);

            //    //    SetVisitedInSearch(current.Up);
            //    //}

            //    //if (current.Down != null
            //    //    && current.DownPassable
            //    //    && WasNotVisitedInSearch(current.Down)
            //    //    && current.Down.Enemy.Level < current.Up?.Enemy.Level
            //    //    && current.Down.Enemy.Level < current.Right?.Enemy.Level
            //    //    && current.Down.Enemy.Level < current.Left?.Enemy.Level)
            //    //{
            //    //    MST[current.Down] = current;
            //    //    q.Enqueue(current.Down);

            //    //    SetVisitedInSearch(current.Down);
            //    //}
            //}

            //void ProcessChamber(Chamber chamber, Direction direction)
            //{
            //    if (chamber[direction] == null)
            //        return;

            //    if (!chamber.IsPassable(direction))
            //        return;

            //    if (!WasNotVisitedInSearch(chamber[direction]))
            //        return;

            //    var otherDirections = direction.Others();
            //    var selected = otherDirections
            //        .Select(dir => new { Chamber = chamber[dir], Level = chamber[dir]?.Enemy.Level })
            //        .Where(x => x.Chamber != null)
            //        .OrderBy(x => x.Level)
            //        .FirstOrDefault();


            //    if(selected != null)
            //    //if (chamber.IsPassable(direction)
            //    //    && WasNotVisitedInSearch(chamber[direction])
            //    //    && allDirections)
            //    {
            //        MST[chamber[direction]] = selected.Chamber;
            //        q.Enqueue(chamber[direction]);

            //        SetVisitedInSearch(chamber[direction]);
            //    }
            //}

            //void SetVisitedInSearch(Chamber chamber)
            //{
            //    chamber.WasVisitedForSearch = true;
            //    visitedInSearch.Add(chamber);
            //}

            //bool WasNotVisitedInSearch(Chamber chamber)
            //{
            //    return !visitedInSearch.Contains(chamber);
            //}
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
