using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alga_rogue.Models
{
    class Dungeon
    {
        private Chamber start, exit;
        private Random random;
        
        public Player Player { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

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
            x = x - 1;
            y = y - 1;

            var startOfLine = this.TopLeftChamber;
            var current = this.TopLeftChamber;

            while (startOfLine != null)
            {
                while (current != null)
                {
                    if (current.xPos == x && current.yPos == y)
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
                    Console.WriteLine();
                    Console.WriteLine("De talisman licht op en fluistert dat de trap omhoog " + aantalKamers + " kamers ver weg is");
                    break;
                case "Handgranaat":
                    this.Handgranaat();
                    Console.WriteLine();
                    Console.WriteLine("De kerker schudt op zijn grondvesten, alle tegenstanders in de kamer zijn verslagen! Een donderend geluid maakt duidelijk dat gedeeltes van de kerker zijn ingestort...");
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

            Queue<Chamber> q = new Queue<Chamber>();
            Dictionary<Chamber, Chamber> MST = new Dictionary<Chamber, Chamber>();

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

                if (current.Left != null && current.LeftPassable != false && current.Left.Visited != true && current.Left.Enemy.Level < current.Down.Enemy.Level && current.Left.Enemy.Level < current.Up.Enemy.Level && current.Left.Enemy.Level < current.Right.Enemy.Level)
                {
                    MST[current.Left] = current;
                    q.Enqueue(current.Left);
                }

                if (current.Right != null && current.RightPassable != false && current.Right.Visited != true && current.Right.Enemy.Level < current.Down.Enemy.Level && current.Right.Enemy.Level < current.Up.Enemy.Level && current.Right.Enemy.Level < current.Left.Enemy.Level)
                {
                    MST[current.Right] = current;
                    q.Enqueue(current.Right);
                }

                if (current.Up != null && current.UpPassable != false && current.Up.Visited != true && current.Up.Enemy.Level < current.Down.Enemy.Level && current.Up.Enemy.Level < current.Right.Enemy.Level && current.Up.Enemy.Level < current.Left.Enemy.Level)
                {
                    MST[current.Up] = current;
                    q.Enqueue(current.Up);
                }

                if (current.Down != null && current.DownPassable != false && current.Down.Visited != true && current.Down.Enemy.Level < current.Up.Enemy.Level && current.Down.Enemy.Level < current.Right.Enemy.Level && current.Down.Enemy.Level < current.Left.Enemy.Level)
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
            var startOfLine = this.TopLeftChamber;
            var current = this.TopLeftChamber;

            while (startOfLine != null)
            {
                while (current != null)
                {
                    current.Visited = false;

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
            var startOfLine = this.TopLeftChamber;
            var current = this.TopLeftChamber;

            while (startOfLine != null)
            {
                while (current != null)
                {

                    // Check
                    if (current.Up == Player.Position)
                        current.IsVisible = true;

                    if (current.Down == Player.Position)
                        current.IsVisible = true;

                    if (current.Left == Player.Position)
                        current.IsVisible = true;

                    if (current.Right == Player.Position)
                        current.IsVisible = true;

                    current = current.Right;
                }

                if (startOfLine.Down == null)
                    break;

                startOfLine = startOfLine.Down;
                current = startOfLine;
            }

        }

        public void DestroyChambers(Dictionary<Chamber, Chamber> MST)
        {
            int destructions = 0;

            while (destructions < 10)
            {
                Random r = new Random();
                var startOfLine = this.TopLeftChamber;
                var current = this.TopLeftChamber;

                while (startOfLine != null)
                {
                    while (current != null)
                    {

                        if (current.Right != null && MST[current] != current.Right && MST[current.Right] != current && r.Next(1, 2) == 2)
                        {    
                            current.RightPassable = false;
                            current.Right.LeftPassable = false;
                            destructions++;
                        }

                        if (current.Down != null && MST[current] != current.Down && MST[current.Down] != current && r.Next(1, 2) == 2)
                        {
                            current.DownPassable = false;
                            current.Down.UpPassable = false;
                            destructions++;
                        }

                        current = current.Right;
                    }

                    if (startOfLine.Down == null)
                        break;

                    startOfLine = startOfLine.Down;
                    current = startOfLine;
                }

                break;
            }
        }
    }
}
