using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerAI
{
    class Table
    {
        private readonly int _roundCount = 10;

        private List<Player> players;
        private Deck deck;
        private List<Card> communityCards;
        private int pot;
        //private Dictionary<Player, int> sidePots;

        private int blind;
        private int currentRoundNumber;
        private int currentBet;

        public Table()
        {
            players = new List<Player>()
            {
                new Player(this, "Julius"),
                new Player(this, "Martin"),
                new Player(this, "Dan Herman"),
                new Player(this, "Isak"),
                new Player(this, "Leif"),
                new Player(this, "Gustav"),
            };

            deck = new Deck();
            communityCards = new List<Card>();
            currentRoundNumber = 0;
        }

        public void Play()
        {
            for (currentRoundNumber = 0; currentRoundNumber < _roundCount; currentRoundNumber++)
            {   
                if (currentRoundNumber % 10 == 0) blind += 10;

                List<Player> winners;

                if (!prepareHand())
                {
                    Console.WriteLine("Game Over! " + players[0].name + " is the fucking winner!");
                    return;
                }
                Console.WriteLine("New round was prepared!");

                Console.WriteLine("Starting preflop bets...");
                doBets();
                winners = players.Where(p => !p.Folded).ToList();
                if (winners.Count == 1)
                {
                    endRound(winners.First());
                    return;
                }

                Console.WriteLine("Dealing flop...");
                communityCards.Add(deck.DrawCard());
                communityCards.Add(deck.DrawCard());
                communityCards.Add(deck.DrawCard());
                Console.WriteLine("Starting flop-betround...");
                currentBet = 0;
                doBets();
                winners = players.Where(p => !p.Folded).ToList();
                if (winners.Count == 1)
                {
                    endRound(winners.First());
                    return;
                }

                Console.WriteLine("Dealing turn...");
                communityCards.Add(deck.DrawCard());
                Console.WriteLine("Starting turn-betround");
                currentBet = 0;
                doBets();
                winners = players.Where(p => !p.Folded).ToList();
                if (winners.Count == 1)
                {
                    endRound(winners.First());
                    break;
                }

                Console.WriteLine("Dealing river...");
                communityCards.Add(deck.DrawCard());
                Console.WriteLine("Starting river-betround");
                currentBet = 0;
                doBets();
                winners = players.Where(p => !p.Folded).ToList();
                if (winners.Count == 1)
                {
                    endRound(winners.First());
                    return;
                }
                else
                {
                    List<List<Player>> winnerGroupings = showCards(winners);
                    endRound(winnerGroupings);
                }
            }
        }

        private bool prepareHand()
        {
            pot = 0;

            int i = 0;
            while (i < players.Count)
            {
                if (players[i].Stack == 0)
                    players.RemoveAt(i);
                else i++;
            }
            if (players.Count == 1) return false;

            Player dealer = players[players.Count - 1];
            players.RemoveAt(players.Count - 1);
            players.Insert(0, dealer);
            
            deck.Shuffle();
            communityCards.Clear();

            foreach (Player p in players)
            {
                p.Hand.Clear();
                p.Hand.Add(deck.DrawCard());
                p.Hand.Add(deck.DrawCard());
                p.PrepareHand();
            }

            pot =  players[players.Count - 2].PutBlind(blind);
            pot += players[players.Count - 1].PutBlind(blind * 2);

            currentBet = blind * 2;

            return true;
        }

        private void doBets()
        {
            Action currentAction;
            string output;

            List<Player> canPlay = players.Where(p => !p.Folded && !(p.Stack == 0)).ToList();

            int canPlayCount = canPlay.Count;
            if (canPlayCount == 0) foreach (Player p in players) p.updatePowerRating();
            while (canPlayCount > 1)
            {
                foreach (Player p in canPlay)
                {
                    currentAction = p.Action(currentBet);
                    currentBet += currentAction.betAmount;
                    pot += currentAction.callAmount + currentAction.betAmount;

                    if (currentAction == null) return;
                    else if (currentAction.type == ActionType.ALLIN)
                        p.SidePot = pot;
                    else if (currentAction.type == ActionType.FOLD && canPlayCount == 2)
                        break;

                    for(int i = 0; i < players.Count; i++)
                    {
                        if (players[i].SidePot != -1 && players[i] != p)
                        {
                            int differanse = players[i].MyCurrentBet - (p.MyCurrentBet - (currentAction.callAmount + currentAction.betAmount));
                            if (differanse > 0) players[i].SidePot += differanse; 
                        }
                    }

                    #region Writing in console
                    output = p.name + " ";
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
                        case ActionType.ALLIN:
                            output += "... OMG! Hes going all in with his " + (currentAction.callAmount + currentAction.betAmount) + " dollars!";
                            break;
                    }
                    Console.WriteLine(output);
#endregion

                    foreach (Player p2 in players) p2.ActionMade(currentAction, p);
                }

                foreach (Player p in players)
                {
                    if (!p.CanPlay)
                    {
                        if (canPlay.Remove(p))
                            canPlayCount--;
                    }
                }
            }
        }

        private List<List<Player>> showCards(List<Player> remainingPlayers)
        {
            List<IGrouping<PowerRating, Player>> tempWinners = remainingPlayers.GroupBy(p => p.PowerRating).ToList();
            if (tempWinners.Count == 1) return tempWinners.Select(g => g.ToList()).ToList();
            else
            {
                List<IGrouping<PowerRating, Player>> winners = new List<IGrouping<PowerRating, Player>>();
                winners.Add(tempWinners[0]);
                for (int i = 1; i < tempWinners.Count; i++)
                {
                    for (int j = 0; j < winners.Count; j++)
                    {
                        if(tempWinners[i].Key.betterThan(winners[j].Key) == 1)
                        {
                            winners.Insert(j, tempWinners[i]);
                            break;
                        }
                    }
                    if (!winners.Contains(tempWinners[i])) winners.Add(tempWinners[i]);
                }
                return winners.Select(g => g.ToList()).ToList();
            }
        }

        private void endRound(Player winner)
        {
            winner.Stack += pot;
            Console.WriteLine("HAND " + currentRoundNumber + " OVER");
        }

        private void endRound(List<List<Player>> winners)
        {
            int win;
            foreach (List<Player> winnerGroup in winners)
            {
                List<Player> orderedWinnerGroup = winnerGroup.OrderBy(wg => wg.MyCurrentBet).ToList();
                foreach(Player winner in winnerGroup)
                {
                    if (winner.SidePot != -1) win = winner.SidePot / winnerGroup.Count;
                    else win = pot / winnerGroup.Count;

                    winner.Stack += win;
                    pot -= win;
                    foreach (Player p in players) if (p.SidePot != -1) p.SidePot -= win;

                    if(win > 0) Console.WriteLine(winner.name + " won " + win);
                    else Console.WriteLine(winner.name + " lost his " + winner.MyCurrentBet);
                }
            }

            Console.WriteLine("HAND " + currentRoundNumber + " OVER");
        }

        public List<Card> getCommunityCards()
        {
            return communityCards;
        }
    }
}
