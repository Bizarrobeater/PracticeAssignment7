using System;
using System.Collections.Generic;

namespace PracticeAssignment7
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Card> newDeck = Solutions.initDeck();
            Console.WriteLine(newDeck.Count);

          
        }

    }
}
