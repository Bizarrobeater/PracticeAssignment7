using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PracticeAssignment7
{
    // 7ø1 - ø3
    public enum Weekday: uint
    {
        Monday = 1,
        Tuesday = 2,
        Wedensday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6,
        Sunday = 7,
    }

    static class Solutions
    {
        // 7ø1
        public static int dayToNumber(Weekday weekday)
        {
            return (int)weekday;
        }

        //7ø2
        public static Weekday nextDay(Weekday weekday)
        {
            int nextDayNumb = (int)weekday + 1;
            nextDayNumb = nextDayNumb != 8 ? nextDayNumb : 1;
            return (Weekday)nextDayNumb;
        }

        // 7ø3
        public static Weekday? numberToDay(int day)
        {
            return (day >= 1 && day <= 7) ? (Weekday)day : null;
        }

        // 7ø4
        public static Suit? succSuit(Suit suit)
        {
            uint nextSuit = (uint)suit + 1;
            return Enum.IsDefined(typeof(Suit), nextSuit) ? (Suit)nextSuit : null;
        }

        // 7ø5
        public static Rank? succRank(Rank rank)
        {
            uint nextRank = (uint)rank + 1;
            return Enum.IsDefined(typeof(Rank), nextRank) ? (Rank)nextRank : null;
        }

        // 7ø6
        public static Card? succCard(Card card)
        {
            Rank? newRank = succRank(card.rank);

            if (newRank != null)
                return new Card((Rank)newRank, card.suit);
            else
            {
                Suit? newSuit = succSuit(card.suit);
                if (newSuit != null)
                    return new Card(Rank.Two, (Suit)newSuit);
                else
                    return null;
            }
        }

        // 7ø7
        public static List<Card> initDeck()
        {
            List<Card> newDeck = new List<Card>();
            
            // First Card
            Card? newCard = new Card(Rank.Two, Suit.Hearts);
            
            bool nextCardExist = true;

            while (nextCardExist)
            {
                newDeck.Add((Card)newCard);
                newCard = succCard((Card)newCard);
                if (newCard == null)
                    nextCardExist = false;
            }
            return newDeck;
        }

        // 7ø8
        public static bool sameRank(Card x, Card y)
        {
            return x.rank == y.rank;
        }

        // 7ø9
        public static bool sameSuit(Card x, Card y)
        {
            return x.suit == y.suit;
        }

        // 7ø10
        public static Card highCard(Card x, Card y)
        {
            return x.rank >= y.rank ? x : y;
        }

        // 7ø11
        public static int? safeDivOption(int a, int b)
        {
            if (b == 0)
                return null;
            else
                return (int)a / b;
        }

        // 7ø12 - closest approx. to solution
        public static int safeDivResult(int a, int b)
        {
            int? result = safeDivOption(a, b);
            if (result != null)
                return (int)result;
            else
                throw new DivideByZeroException();               
        }

        // 7ø13



        
    }


}
