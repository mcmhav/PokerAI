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
        private int pot;
        private Player currentPlayer;

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
            dealer = players[1];
            pot = 0;

        }

        public void play()
        {
            for (int i = 0; i < _roundCount; i++)
            {

            }
        }

        void DoPreFlopBets()
        {

        }

        void DealFlop()
        {

        }

        void DoFlopBets()
        {

        }
        
        void DealTurn()
        {

        }
        
        void DoTurnBets()
        {

        }
        
        void DealRiver()
        {

        }
        
        void DoRiverBets()
        {

        }

    }
}
