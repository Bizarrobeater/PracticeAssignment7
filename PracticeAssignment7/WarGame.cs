using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PracticeAssignment7.Solutions;

namespace PracticeAssignment7
{
    class WarGame
    {
        public static List<Card> newdeck()
        {
            return initDeck();
        }

        public static List<Card>[] deal(List<Card> cardList)
        {
            List<Card>[] playerCards =
            {
                new List<Card>(),
                new List<Card>(),
            };

            for (int i = 0; i < cardList.Count; i++)
            {
                playerCards[i % 2].Add(cardList[i]);
            }
            return playerCards;
        }

        public static List<Card> shuffle(List<Card> cardList)
        {
            Random rnd = new Random();
            List<Card> shuffledCards = new List<Card>();
            int rndInt;
            while (cardList.Count > 0)
            {
                rndInt = rnd.Next(0, cardList.Count - 1);
                shuffledCards.Add(cardList[rndInt]);
                cardList.RemoveAt(rndInt);
            }

            return shuffledCards;
        }

        internal struct Player
        {
            public List<Card> Deck { get; set; }

            public Card? getCard()
            {
                if (Deck.Count == 0)
                    return null;
                else
                {
                    Card tempCard = Deck[0];
                    Deck.RemoveAt(0);
                    return tempCard;
                }

            }

            public void addCards(List<Card> newCards)
            {
                newCards = shuffle(newCards);
                Deck.AddRange(newCards);
            }
        }
    }
}
