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
                    int aantalKamers = this.Talisman();
                    Console.WriteLine($"{Environment.NewLine}De talisman licht op en fluistert dat de trap omhoog {aantalKamers} kamers ver weg is");
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
                current.Visited = true;

                if (current == this.Exit)
                {
                    this.DestroyChambers(MST);
                    break;
                }

                if (current.Left != null
                    && current.LeftPassable
                    && !current.Left.Visited
                    && current.Left.Enemy.Level < current.Down.Enemy.Level
                    && current.Left.Enemy.Level < current.Up.Enemy.Level
                    && current.Left.Enemy.Level < current.Right.Enemy.Level)
                {
                    MST[current.Left] = current;
                    q.Enqueue(current.Left);
                }

                if (current.Right != null
                    && current.RightPassable
                    && !current.Right.Visited
                    && current.Right.Enemy.Level < current.Down.Enemy.Level
                    && current.Right.Enemy.Level < current.Up.Enemy.Level
                    && current.Right.Enemy.Level < current.Left.Enemy.Level)
                {
                    MST[current.Right] = current;
                    q.Enqueue(current.Right);
                }

                if (current.Up != null
                    && current.UpPassable
                    && !current.Up.Visited
                    && current.Up.Enemy.Level < current.Down.Enemy.Level
                    && current.Up.Enemy.Level < current.Right.Enemy.Level
                    && current.Up.Enemy.Level < current.Left.Enemy.Level)
                {
                    MST[current.Up] = current;
                    q.Enqueue(current.Up);
                }

                if (current.Down != null
                    && current.DownPassable
                    && !current.Down.Visited
                    && current.Down.Enemy.Level < current.Up.Enemy.Level
                    && current.Down.Enemy.Level < current.Right.Enemy.Level
                    && current.Down.Enemy.Level < current.Left.Enemy.Level)
                {
                    MST[current.Down] = current;
                    q.Enqueue(current.Down);
                }

            }

        }

        public int Talisman()
        {
            this.NotVisited();

            Queue<Chamber> q = new Queue<Chamber>();
            Dictionary<Chamber, Chamber> whoVisitedWho = new Dictionary<Chamber, Chamber>();
            int stepsFrom = 0;

            q.Enqueue(this.Player.Position);
            while (q.Count > 0)
            {
                Chamber current = q.Dequeue();
                current.Visited = true;


                if (current == this.Exit)
                {
                    Chamber stepCounter = current;
                    while (stepCounter != this.Player.Position)
                    {
                        stepCounter = whoVisitedWho[stepCounter];
                        stepsFrom++;
                    }

                    return stepsFrom;
                }

                if (current.Left != null && current.LeftPassable != false && current.Left.Visited != true)
                {
                    whoVisitedWho[current.Left] = current;
                    q.Enqueue(current.Left);
                }

                if (current.Right != null && current.RightPassable != false && current.Right.Visited != true)
                {
                    whoVisitedWho[current.Right] = current;
                    q.Enqueue(current.Right);
                }

                if (current.Up != null && current.UpPassable != false && current.Up.Visited != true)
                {
                    whoVisitedWho[current.Up] = current;
                    q.Enqueue(current.Up);
                }

                if (current.Down != null && current.DownPassable != false && current.Down.Visited != true)
                {
                    whoVisitedWho[current.Down] = current;
                    q.Enqueue(current.Down);
                }
               
            }

            return 0;
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
            ForEach(chamber => chamber.Visited = false);
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
