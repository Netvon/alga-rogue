using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alga_rogue.Input
{
    public static class Question
    {
        public static int AskForNumber(string question, int min = int.MinValue, int max = int.MaxValue)
        {
            var result = 0;
            var isValid = false;

            while (!isValid)
            {
                Ask();
                isValid = int.TryParse(Console.ReadLine(), out result);

                if (result < min || result > max)
                    isValid = false;

                if (!isValid)
                    NotifyOfError();
            }

            return result;

            void Ask()
            {
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine($"\n   {question}   ");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("=> ");
                Console.ResetColor();
            }

            void NotifyOfError()
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("~ ~ ~ Please enter a valid number ~ ~ ~");
                Console.ResetColor();
            }
        }
    }
}
