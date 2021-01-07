using System;
using System.Collections.Generic;
using static PracticeAssignment7.Solutions;
using System.Diagnostics;

namespace PracticeAssignment7
{
    class Program
    {
        static void Main(string[] args)
        {
            var simulator = new WarGameSimulator(10000, true);

            simulator.RunGames();
            Console.WriteLine(simulator.ToString());
        }
    }
}
