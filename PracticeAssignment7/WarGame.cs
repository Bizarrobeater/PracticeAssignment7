using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PracticeAssignment7.Solutions;

namespace PracticeAssignment7
{
    class WarGameSimulator
    {
        private int numbSims;
        private bool random;
        public int P1Wins { get; private set; }
        public int P2Wins { get; private set; }

        public int Draws { get; private set; }

        public double AverageRoundsPlayed
        {
            get => RoundsPlayed.Count > 0 ? RoundsPlayed.Average() : 0.0;
        }

        public double MedianRoundsPlayed
        {
            get => GetMedianRounds();
        }

        public double ModeRoundsPlayed
        {
            get => GetMode();
        }

        public double StandardDeviation
        {
            get => GetStandardDeviation();
        }

        public int LowestRoundsPlayed
        {
            get => RoundsPlayed.Count > 0 ? RoundsPlayed.Min() : 0;
        }

        public int HighestRoundsPlayed
        {
            get => RoundsPlayed.Count > 0 ? RoundsPlayed.Max() : 0;
        }

        public List<int> RoundsPlayed { get; private set; }

        public WarGameSimulator(int numbSims, bool fullRandom = false)
        {
            this.numbSims = numbSims;
            random = fullRandom;

            P1Wins = 0;
            P2Wins = 0;
            Draws = 0;

            RoundsPlayed = new List<int>();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"Rounds Played        : {numbSims}\n");
            sb.Append($"Player 1 wins        : {P1Wins}\n");
            sb.Append($"Player 2 wins        : {P2Wins}\n");
            sb.Append($"Draws                : {Draws}\n\n");
            sb.Append($"Average rounds played: {AverageRoundsPlayed}\n");
            sb.Append($"Median rounds played : {MedianRoundsPlayed}\n");
            sb.Append($"Mode rounds played   : {ModeRoundsPlayed}\n");
            sb.Append($"Standard Deviation   : {StandardDeviation}\n");
            sb.Append($"Lowest rounds played : {LowestRoundsPlayed} (Game {RoundsPlayed.IndexOf(LowestRoundsPlayed)})\n");
            sb.Append($"Highest rounds played: {HighestRoundsPlayed} (Game {RoundsPlayed.IndexOf(HighestRoundsPlayed)})");

            return sb.ToString();
        }

        public void RunGames()
        {
            for (int i = 0; i < numbSims; i++)
            {
                WarGame game = NewGame(i);
                game.SimulateGame();

                RoundsPlayed.Add(game.RoundsPlayed);
                
                switch (game.winState)
                {
                    case WarGame.WinState.Player1:
                        P1Wins++;
                        break;
                    case WarGame.WinState.Player2:
                        P2Wins++;
                        break;
                    case WarGame.WinState.Draw:
                        Draws++;
                        break;
                }
            }
        }

        private WarGame NewGame(int seed)
        {
            if (random)
                return new WarGame(seed);
            else
                return new WarGame();
        }

        private double GetMedianRounds()
        {
            if (RoundsPlayed.Count == 0)
                return 0.0;

            List<int> sortedRound = new List<int>(RoundsPlayed);
            sortedRound.Sort();
            int halfIndex = sortedRound.Count() / 2;
            
            if (sortedRound.Count % 2 == 0)
                return ((sortedRound.ElementAt(halfIndex) +
                    sortedRound.ElementAt(halfIndex - 1)) / 2);
            else
                return sortedRound.ElementAt(halfIndex);      
        }

        private double GetMode()
        {
            var groups = RoundsPlayed.GroupBy(v => v);
            int maxCount = groups.Max(g => g.Count());
            int mode = groups.First(g => g.Count() == maxCount).Key;
            return mode;
        }

        private double GetStandardDeviation()
        {
            if (RoundsPlayed.Count <= 1)
                return 0.0;

            double stdDev = 0;
            double mean = AverageRoundsPlayed;
            foreach (int rounds in RoundsPlayed)
            {
                stdDev += Math.Pow(rounds - mean, 2);
            }
            
            stdDev = stdDev / numbSims;

            stdDev = Math.Sqrt(stdDev);

            return stdDev;
        }
    }

    class WarGame
    {
        private Random rnd;
        public Player player1;
        public Player player2;
    
        public int RoundsPlayed { get; private set; }

        public WinState winState { get; private set; }

        public WarGame(int? rndSeed = null)
        {
            if (rndSeed == null)
                rnd = new Random();
            else
                rnd = new Random((int)rndSeed);

            // Inits players
            player1 = new Player();
            player2 = new Player();

            player1.Deck = new List<Card>();
            player2.Deck = new List<Card>();

            // Inits list of cards - shuffles the cards
            List<Card> newDeck = NewDeck();
            Shuffle(ref newDeck);

            List<Card>[] splitDeck = Deal(newDeck);
            // Gives the players the cards
            player1.Deck.AddRange(splitDeck[0]);
            player2.Deck.AddRange(splitDeck[1]);

            winState = WinState.Undecided;
            RoundsPlayed = 0;
        }

        public void PrintPlayers()
        {
            Console.WriteLine("Player 1:");
            Console.WriteLine(player1.ToString() + Environment.NewLine);
            Console.WriteLine("Player 2:");
            Console.WriteLine(player2.ToString());
        }


        public void SimulateGame()
        {   
            List<Card> warPrizeCards = new List<Card>();
            Card? p1Card = player1.getCard();
            Card? p2Card = player2.getCard();

            WinState warWinner;
           
            while (winState == WinState.Undecided)
            {
                warWinner = CompareCards((Card)p1Card, (Card)p2Card);
                warPrizeCards.AddRange(new List<Card> { (Card)p1Card, (Card)p2Card });
                switch (warWinner)
                {
                    case WinState.Player1:
                        AddCardsToPlayer(warPrizeCards, player1);
                        warPrizeCards = new List<Card>();
                        break;
                    case WinState.Player2:
                        AddCardsToPlayer(warPrizeCards, player2);
                        warPrizeCards = new List<Card>();
                        break;
                    case WinState.Draw:
                        warPrizeCards.AddRange(CardsForWar());
                        break;
                }

                RoundsPlayed++;

                p1Card = player1.getCard();
                p2Card = player2.getCard();
                CheckForWinStateChange(p1Card, p2Card);
            }
        }

        private List<Card> CardsForWar()
        {
            List<Card> prizePool = new List<Card>();
            Card? cardP1;
            Card? cardP2;

            for (int i = 0; i < 3; i++)
            {
                cardP1 = player1.getCard();
                cardP2 = player2.getCard();

                if (!CheckForWinStateChange(cardP1, cardP2))
                {
                    prizePool.Add((Card)cardP1);
                    prizePool.Add((Card)cardP2);
                }
                else
                {
                    return new List<Card>();
                }
            }
            return prizePool;
        }

        private bool CheckForWinStateChange(Card? p1, Card? p2)
        {
            if (p1 != null && p2 != null)
                return false;
            else if (p1 != null && p2 == null)
                winState = WinState.Player1;
            else if (p1 == null && p2 != null)
                winState = WinState.Player2;
            else
                winState = WinState.Draw;
            return true;

        }

        private WinState CompareCards(Card p1, Card p2)
        {
            if (p1.rank == p2.rank)
                return WinState.Draw;
            else
                return p1.rank > p2.rank ? WinState.Player1 : WinState.Player2;
        }

        private List<Card> NewDeck()
        {
            return initDeck();
        }

        private List<Card>[] Deal(List<Card> cardList)
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

        private void Shuffle(ref List<Card> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                Card value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private void AddCardsToPlayer(List<Card> newCards, Player player)
        {
            Shuffle(ref newCards);
            player.Deck.AddRange(newCards);
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

            public override string ToString()
            {
                StringBuilder sB = new StringBuilder();
                foreach (Card card in Deck)
                {
                    sB.Append($"{card.ToString()} ");
                }
                sB.Length--;
                return sB.ToString();
                
            }
        }

        internal enum WinState
        {
            Undecided,
            Player1,
            Player2,
            Draw,
            Infinite,
        }
    }
}
