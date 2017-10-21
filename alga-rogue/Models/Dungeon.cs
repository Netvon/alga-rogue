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
            this.NotVisited();

            var q   = new Queue<Chamber>();
            var MST = new Dictionary<Chamber, Chamber>();

            q.Enqueue(this.Player.Position);
            while (q.Count > 0)
            {
                Chamber current = q.Dequeue();
                current.WasVisitedForSearch = true;

                if (current == this.Exit)
                {
                    this.DestroyChambers(MST);
                    break;
                }

                if (current.Left != null
                    && current.LeftPassable
                    && !current.Left.WasVisitedForSearch
                    && current.Left.Enemy.Level < current.Down.Enemy.Level
                    && current.Left.Enemy.Level < current.Up.Enemy.Level
                    && current.Left.Enemy.Level < current.Right.Enemy.Level)
                {
                    MST[current.Left] = current;
                    q.Enqueue(current.Left);
                }

                if (current.Right != null
                    && current.RightPassable
                    && !current.Right.WasVisitedForSearch
                    && current.Right.Enemy.Level < current.Down.Enemy.Level
                    && current.Right.Enemy.Level < current.Up.Enemy.Level
                    && current.Right.Enemy.Level < current.Left.Enemy.Level)
                {
                    MST[current.Right] = current;
                    q.Enqueue(current.Right);
                }

                if (current.Up != null
                    && current.UpPassable
                    && !current.Up.WasVisitedForSearch
                    && current.Up.Enemy.Level < current.Down.Enemy.Level
                    && current.Up.Enemy.Level < current.Right.Enemy.Level
                    && current.Up.Enemy.Level < current.Left.Enemy.Level)
                {
                    MST[current.Up] = current;
                    q.Enqueue(current.Up);
                }

                if (current.Down != null
                    && current.DownPassable
                    && !current.Down.WasVisitedForSearch
                    && current.Down.Enemy.Level < current.Up.Enemy.Level
                    && current.Down.Enemy.Level < current.Right.Enemy.Level
                    && current.Down.Enemy.Level < current.Left.Enemy.Level)
                {
                    MST[current.Down] = current;
                    q.Enqueue(current.Down);
                }
            }
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
