using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerAI
{
    class Player
    {
        private readonly int initMoney = 1000;
        private Table Table;

        private bool _folded;
        public bool Folded { 
            get { return _folded; }
            private set { 
                _folded = value;
                CanPlay = !value && Stack != 0;
            }
        }

        private int _stack;
        public int Stack
        {
            get { return _stack; }
            set
            {
                if (value >= 0) _stack = value;
                else return;
                CanPlay = !_folded && value != 0;
            }
        }

        private int _myCurrentBet;
        public int MyCurrentBet
        {
            get { return _myCurrentBet; }
            private set
            {
                if(value - _myCurrentBet > 0) Stack -= value - _myCurrentBet;
                _myCurrentBet = value;
            }
        }

        private int _sidePot;
        public int SidePot
        {
            get { return _sidePot; }
            set
            {
                if (value >= -1) _sidePot = value;
                else return;
            }
        }

        public PowerRating PowerRating { get; private set; }


        public List<Card> Hand;
        public bool CanPlay { get; private set; }

        public string name;

        public Player(Table table, string name)
        {
            Table = table;
            Stack = initMoney;
            Hand = new List<Card>();

            this.name = name;
        }

        public void PrepareHand()
        {
            _myCurrentBet = 0;
            SidePot = -1;
            if (Stack == 0) Folded = true;
            else Folded = false;

            Console.WriteLine(name + " has: " + Stack + " dollars");
        }

        public Action Action(int currentBet)
        {
            Action action;
            updatePowerRating();

            if (currentBet != MyCurrentBet)
            {
                int callAmount = currentBet - MyCurrentBet;
                if (callAmount > Stack)
                {
                    action = new Action(ActionType.ALLIN, Stack, 0);
                    MyCurrentBet += Stack;
                }
                else if(PowerRating.first == 8)
                {
                    action = new Action(ActionType.ALLIN, callAmount, Stack - callAmount);
                    MyCurrentBet += Stack;
                }
                else if(PowerRating.first > 2)
                {
                    int bet = PowerRating.first*(initMoney/200);
                    if (callAmount + bet >= Stack)
                    {
                        action = new Action(ActionType.ALLIN, callAmount, Stack - callAmount);
                        MyCurrentBet += Stack;
                    }
                    else
                    {
                        action = new Action(ActionType.BET, callAmount, bet);
                        MyCurrentBet += callAmount + bet;
                    }
                }
                else
                {
                    Folded = true;
                    return new Action(ActionType.FOLD, 0, 0);
                }
            }
            else
            {
                return null;
            }
            return action;
        }

        public int PutBlind(int blind)
        {
            if (Stack > blind)
            {
                MyCurrentBet = blind;
            }
            else
            {
                MyCurrentBet = Stack;
            }

            //Stack -= MyCurrentBet;
            return MyCurrentBet;
        }

        public void ActionMade(Action action, Player player)
        {

        }

        public void updatePowerRating()
        {
            List<Card> temp = new List<Card>();
            temp.AddRange(Hand);
            temp.AddRange(Table.getCommunityCards());
            PowerRating = new PowerRating(temp);
        }
    }

    class Action
    {
        public ActionType type;
        public int callAmount;
        public int betAmount;

        public Action(ActionType type, int callAmount, int betAmount){
            this.type = type;
            this.callAmount = callAmount;
            this.betAmount = betAmount;

        }
    }

    enum ActionType {FOLD, CHECK, CALL, BET, RAISE, RERAISE, ALLIN}
}
