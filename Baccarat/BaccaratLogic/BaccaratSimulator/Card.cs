using System;
using System.Collections.Generic;
using System.Text;

namespace CalculationLogic.BaccaratSimulator
{
    public enum CardRank
    {        
        Two = 2,
        Three = 3, 
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        King,
        Queen,
        Jack,
        Ten,
        Ace 
    }

    public enum CardSuit
    {
        Spades, //Bích
        Diamonds, //Rô
        Clubs, //Chuồn
        Hearts //Cơ
    }

    public class Card
    {
        public Card(CardRank cardRank, CardSuit cardSuit)
        {
            CardRank = cardRank;
            CardSuit = cardSuit;
        }
        public CardRank CardRank { get; internal set; }
        public CardSuit CardSuit { get; internal set; }
    }
}
