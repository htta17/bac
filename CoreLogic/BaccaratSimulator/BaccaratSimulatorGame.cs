using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CoreLogic.BaccaratSimulator
{
    public class BaccaratSimulatorGame
    {
        public BaccaratSimulatorGame(int numOfDecks)
        {
            Cards = new List<Card>();
            for (int i = 1; i <= numOfDecks; i++)
            {
                var newDecks = new Deck();
                Cards.AddRange(newDecks.GetCards());
            }
        }

        public void Shuffle()
        {
            var firstCardIndex = 3;
            var numberOfTakingCard = 4;
            var pullingCardTime = 0; 
            var takeCards = new List<Card>();
            
            while (pullingCardTime < numberOfTakingCard)
            {
                takeCards.Add(Cards[firstCardIndex]);
                Cards.RemoveAt(firstCardIndex);
                pullingCardTime++;
            }
            Cards.InsertRange(0, takeCards);
        }

        private List<Card> Cards { get; set; }
    }
}
