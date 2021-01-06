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
            {
                Console.WriteLine(new DivideByZeroException().ToString());
                throw new DivideByZeroException();
            }           
        }

        // 7ø13
        public struct expr
        {
            public expr(int constInt)
            {
                Const = constInt;
            }

            public int Const { get; }

            public static int Add(expr x, expr y)
            {
                return x.Const + y.Const;
            }

            public static int Mul(expr x, expr y)
            {
                return x.Const * y.Const;
            }

            // For 7ø14
            public static int Sub(expr x, expr y)
            {
                return x.Const - y.Const;
            }

            // For 7ø15
            public static int Div(expr x, expr y)
            {
                return safeDivResult(x.Const, y.Const);
            }
        }

        public static void testCase7o13()
        {
            int testExpr =
                expr.Add(new expr(3), new expr(expr.Mul(new expr(2), new expr(4))));
            Console.Write(testExpr);
        }

        // 7ø14
        public static void testCase7o14()
        {
            int testExpr =
                expr.Sub(new expr(3), new expr(2));
            Console.Write(testExpr);
        }

        // 7ø15
        public static void testCase7o15()
        {
            int testExpr =
                expr.Add(new expr(3), new expr(expr.Div(new expr(6), new expr(2))));
            Console.WriteLine(testExpr);
            // Should thrown error
            testExpr = expr.Add(new expr(3), new expr(expr.Div(new expr(6), new expr(0))));
            Console.Write(testExpr);
        }

        // class for 7ø16 - 7ø18
        public class tree<T>
        {
            public T Leaf { get; init; }

            private tree<T> _leftTree = null;
            private tree<T> _rightTree = null;

            public tree<T> leftTree { get => _leftTree; set => _leftTree = value; }
            public tree<T> rightTree { get => _rightTree; set => _rightTree = value; }

            public tree(T data)
            {
                Leaf = data;
            }

        }

        // 7ø16
        public static int sum(tree<int> tree) 
        {
            if (tree == null)
                return 0;

            int result = tree.Leaf;

            result += sum(tree.leftTree) + sum(tree.rightTree);
            return result;
        }

        public static void testCase7o16()
        {
            tree<int> one = new tree<int>(1);
            tree<int> two = new tree<int>(2);
            tree<int> three = new tree<int>(3);
            tree<int> four = new tree<int>(4);
            tree<int> five = new tree<int>(5);

            one.leftTree = two;
            one.rightTree = three;
            two.rightTree = four;
            four.leftTree = five;
            // expected 15, result 15
            Console.WriteLine(sum(one));
        }

        // 7ø17
        public static int leafs<T>(tree<T> tree)
        {
            if (tree == null)
                return 0;

            int result = 1;

            result += leafs(tree.leftTree) + leafs(tree.rightTree);
            return result;
        }

        public static void testCase7o17()
        {
            tree<int> one = new tree<int>(1);
            tree<int> two = new tree<int>(2);
            tree<int> three = new tree<int>(3);
            tree<int> four = new tree<int>(4);
            tree<int> five = new tree<int>(5);

            one.leftTree = two;
            one.rightTree = three;
            two.rightTree = four;
            four.leftTree = five;
            // expected 5, result 5
            Console.WriteLine(leafs(one));
        }




    }


}
