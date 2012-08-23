using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerAI
{
    class Table
    {
        private readonly int _roundCount;

        private List<Player> players;
        private Deck deck;
        private List<Card> communityCards;
        private Player dealer;
        private Player currentPlayer;
        private int pot;
        private int currentBet;

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
                prepareHand();
                currentBet = 0;
                doBets();

                communityCards.Add(deck.DrawCard());
                communityCards.Add(deck.DrawCard());
                communityCards.Add(deck.DrawCard());
                doBets();

                communityCards.Add(deck.DrawCard());
                currentBet = 0;
                doBets();

                communityCards.Add(deck.DrawCard());
                currentBet = 0;
                doBets();
                
            }
        }

        private void prepareHand()
        {
            deck.Shuffle();
            communityCards.Clear();
            dealer = dealer == null ? players[1] : players[players.IndexOf(dealer)];
            pot = 0;
        }

        private void doBets()
        {
            foreach (Player p1 in players)
            {
                
                //currentBet = p1.DoTurn();
                //foreach (Player p2 in players) p2.NewAction(p1.DoAction));
            }
        }

    }
}
