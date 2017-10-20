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
                    Console.WriteLine("De talisman licht op en fluistert dat de trap omhoog" + aantalKamers + "kamers ver weg is");
                    break;
                case "Handgranaat":

                    break;
                case "Kompas":

                    break;
                default:
                    break;
            }
        }

        public int Talisman()
        {



            return 0;
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
                Console.WriteLine();

                if (startOfLine.Down == null)
                    break;

                startOfLine = startOfLine.Down;
                current = startOfLine;
            }

        }

    }
}
