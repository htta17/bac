using System;
using System.Collections.Generic;
using System.Text;

namespace CalculationLogic
{
    /// <summary>
    /// Main algorithm.
    /// </summary>
    public class BaccaratRoot
    {
        public BaccaratRoot()
        {
            Cards = new List<BaccratCard>();
            SaveCards = new List<BaccratCard>();
        }

        /// <summary>
        /// Input cards
        /// </summary>
        private List<BaccratCard> Cards { get; set; }

        /// <summary>
        /// Saved cards for rollback,
        /// </summary>
        private List<BaccratCard> SaveCards { get; set; }

        public void AddNewCard(BaccratCard card)
        {
            if (card == BaccratCard.NoTrade)
                throw new Exception("Input card must be Banker or Player");    
            
            Cards.Add(card);

            //Need only 4 cards or fewer
            while (Cards.Count > 3)
            {
                Cards.RemoveAt(0);
            }
            SaveCards.Add(card);
        }

        /// <summary>
        /// Currently, we have 3 cards or fewer
        /// </summary>
        public void Rollback()
        {
            //ToDo: Implement
            throw new NotImplementedException();
        }

        public void Reset()
        {
            Cards.Clear();
            SaveCards.Clear();
        }

        public BaccaratPredict PredictNextCard()
        {
            if (Cards.Count < 3)
                return new BaccaratPredict { Value = BaccratCard.NoTrade, Volume = 0 };

            if (Cards[0] != Cards[1] && Cards[1] == Cards[2]) //Trigger to predict
            {
                return new BaccaratPredict
                {
                    Value = Cards[2] == BaccratCard.Banker ? BaccratCard.Player : BaccratCard.Banker,
                    Volume = 1
                };
            }

            return new BaccaratPredict { Value = BaccratCard.NoTrade, Volume = 0 };
        }
    }
}
