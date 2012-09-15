using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerAI
{
    class Table
    {
        private readonly int _roundCount = 1000;

        private List<Player> players;
        private Deck deck;
        private List<Card> communityCards;
        private Player dealer;
        private int pot;

        public Table()
        {
            players = new List<Player>()
            {
                new Player(this),
                new Player(this),
                new Player(this),
                new Player(this),
                new Player(this),
                new Player(this),
            };

            deck = new Deck();
            communityCards = new List<Card>();
        }

        public void Play()
        {
            for (int i = 0; i < _roundCount; i++)
            {
                Console.WriteLine("Preparing new round, and dealing pocket cards...");
                prepareHand();

                Console.WriteLine("Starting preflop bets...");
                doBets();

                Console.WriteLine("Dealing flop...");
                communityCards.Add(deck.DrawCard());
                communityCards.Add(deck.DrawCard());
                communityCards.Add(deck.DrawCard());
                Console.WriteLine("Starting flop-betround...");
                doBets();

                Console.WriteLine("Dealing turn...");
                communityCards.Add(deck.DrawCard());
                Console.WriteLine("Starting turn-betround");
                doBets();

                Console.WriteLine("Dealing river...");
                communityCards.Add(deck.DrawCard());
                Console.WriteLine("Starting river-betround");
                doBets();
                
            }
        }

        private void prepareHand()
        {
            deck.Shuffle();
            communityCards.Clear();
            if (dealer == null || dealer == players.Last()) dealer = players[1];
            else dealer = players[players.IndexOf(dealer)+1];
            foreach (Player p in players)
            {
                p.Hand.Clear();
                p.Hand.Add(deck.DrawCard());
                p.Hand.Add(deck.DrawCard());
            }
        }

        private void doBets()
        {
            int currentBet = 0;
            Action currentAction;
            string output;

            List<Player> canPlay = players;
            int canPlayCount = canPlay.Count;
            while (canPlayCount != 0)
            {
                foreach (Player p in canPlay)
                {
                    if (p.CanPlay())
                    {
                        currentAction = p.Action(currentBet);
                        currentBet = currentAction.callAmount + currentAction.betAmount;
                        pot += currentBet;

                        #region Writing in console
                        output = "p" + players.IndexOf(p) + " ";
                        switch (currentAction.type)
                        {
                            case ActionType.FOLD:
                                output += "folded...";
                                break;
                            case ActionType.CHECK:
                                output += "checked...";
                                break;
                            case ActionType.CALL:
                                output += "called (" + currentAction.callAmount + ")...";
                                break;
                            case ActionType.BET:
                                output += "called (" + currentAction.callAmount + "), and placed a new bet (" + currentAction.betAmount + ")...";
                                break;
                            case ActionType.RAISE:
                                output += "called (" + currentAction.callAmount + "), and raised the bet with" + currentAction.betAmount + "...";
                                break;
                            case ActionType.RERAISE:
                                output += "called (" + currentAction.callAmount + "), and reraised the bet with " + currentAction.betAmount + "...";
                                break;
                        }
                        Console.WriteLine(output);
#endregion

                        foreach (Player p2 in players) p2.ActionMade(currentAction, p);
                    }
                }

                foreach (Player p in players)
                {
                    if (!p.CanPlay())
                    {
                        canPlay.Remove(p);
                        canPlayCount--;
                    } 
                }
            }
        }


        public List<Card> getCommunityCards()
        {
            return communityCards;
        }
    }
}
