using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeAssignment7
{
    public enum Suit : uint
    {
        Hearts = 1,
        Diamonds = 2,
        Clubs = 3,
        Spades = 4,
    }

    public enum Rank : uint
    {
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13,
        Ace = 14,
    }

    public struct Card
    {
        public Suit suit;
        public Rank rank;
        public Card(Rank rank, Suit suit)
        {
            this.suit = suit;
            this.rank = rank;
        }
    }
}
