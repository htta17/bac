using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLogic.BaccaratSimulator
{
    public class Deck
    {
        public Deck()
        {
            Cards = new List<Card>();

            foreach (var cardRank in (CardRank[])Enum.GetValues(typeof(CardRank)))
            {
                foreach (var cardSuit in (CardSuit[])Enum.GetValues(typeof(CardSuit)))
                {
                    Cards.Add(new Card(cardRank, cardSuit));
                }
            }
        }

        public List<Card> GetCards()
        {
            return Cards;
        }
        private List<Card> Cards { get; set; }
    }
}
