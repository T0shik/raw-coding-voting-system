using System;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            int yesCounter = 4;
            int noCounter = 2;
            int maybeCounter = 3;

            int total = yesCounter + noCounter + maybeCounter;

            var yesPercent = Math.Round(yesCounter * 100.0 / total, 2);
            var noPercent = Math.Round(noCounter * 100.0 / total, 2);
            var maybePercent = Math.Round(maybeCounter * 100.0 / total, 2);

            var excess = Math.Round(100 - yesPercent - noPercent - maybePercent, 2);

            Console.WriteLine($"Excess: {excess}");

            if (yesCounter > noCounter)
            {
                if (yesCounter > maybeCounter)
                {
                    Console.WriteLine($"Yes Won");
                    yesPercent += excess;
                }
                else if (maybeCounter > yesCounter)
                {
                    Console.WriteLine($"Maybe Won");
                    maybePercent += excess;
                }
                else
                {
                    Console.WriteLine($"DRAW");
                    noPercent += excess;
                }
            }
            else if (noCounter > yesCounter)
            {
                if (noCounter > maybeCounter)
                {
                    Console.WriteLine($"No Won");
                    noPercent += excess;
                }
                else if (maybeCounter > noCounter)
                {
                    Console.WriteLine($"Maybe Won");
                    maybePercent += excess;
                }
                else
                {
                    Console.WriteLine($"DRAW");
                    yesPercent += excess;
                }
            }
            else if (maybeCounter > yesCounter)
            {
                Console.WriteLine($"Maybe Won");
                maybePercent += excess;
            }
            else
            {
                Console.WriteLine($"DRAW");
            }

            Console.WriteLine($"Yes Counts: {yesCounter}, Percentage: {Math.Round(yesPercent, 2)}%");
            Console.WriteLine($"No Counts: {noCounter}, Percentage: {Math.Round(noPercent, 2)}%");
            Console.WriteLine($"Maybe Counts: {maybeCounter}, Percentage: {Math.Round(maybePercent, 2)}%");

            Console.WriteLine($"Total Percentage: {Math.Round(yesPercent + noPercent + maybePercent, 2)}%");
        }
    }
}
