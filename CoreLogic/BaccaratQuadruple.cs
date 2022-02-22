using System;
using System.Collections.Generic;



namespace CoreLogic
{
    
    
    public class BaccaratQuadruple : IBaccaratCalculator
    {
        public BaccaratQuadruple()
        {            
        }

        private int Current_Same { get; set; }
        private int Current_Diff { get; set; }

        private BaccratCard Current_Predict { get; set; }

        public List<BaccratCard> BaccratCards { get; internal set; } = new List<BaccratCard>();

        public List<BaccratCard> SaveBaccratCards { get; set; } = new List<BaccratCard>();

        private bool CompareSame()
        {
            if (BaccratCards.Count != SaveBaccratCards.Count)
                return false; 
            for(int i=0; i< BaccratCards.Count; i++)
            {
                if (BaccratCards[i] != SaveBaccratCards[i])
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Predict next step
        /// </summary>
        /// <returns></returns>
        public QuadrupleResult Predict()
        {
            var currentOrder = (BaccratCards.Count - 1) % 8; //0-8 --> Count needs to minus 1            

            if (currentOrder < 3 ) //If currentOrder in [0,1,2]: cannot predict 
            {
                Current_Predict = BaccratCard.NoTrade;
                return new QuadrupleResult
                {
                    Value = Current_Predict,
                    Volume = 0,
                    Diff_Coff = Current_Diff,
                    Same_Coff = Current_Same
                };
            }
            
            var condition = currentOrder == 3 && Current_Diff == 0 && Current_Same == 0; 

            var assumeSame = condition ? 1 : currentOrder == 3 ? Current_Same : 
                            (Current_Same == 0 || Current_Same == 1 || Current_Same == 2) ? 1
                                : Current_Same < 0 ? Math.Abs(Current_Same) + 2
                                : Current_Same - 2;

            var assumeDiff = condition ? 1 : currentOrder == 3 ? Current_Diff :
                            (Current_Diff == 0 || Current_Diff == 1 || Current_Diff == 2) ? 1
                                : Current_Diff < 0 ? Math.Abs(Current_Diff) + 2
                                : Current_Diff - 2;


            var currentIndex = BaccratCards.Count - 1; //Don't use currentOrder
            var comparedIndex = currentIndex - 3; 
            //Remember we are predicting for next value, so we need to 
            
            var assumeCard = BaccratCards[currentIndex];
            var previousCard = BaccratCards[comparedIndex];


            Current_Same = assumeCard == previousCard ? Math.Abs(assumeSame) : - Math.Abs(assumeSame);

            Current_Diff = assumeCard != previousCard ? Math.Abs(assumeDiff) : -Math.Abs(assumeDiff);

            var predictVolume = (Current_Same + Current_Diff) / 2;

            Current_Predict = predictVolume == 0 ? BaccratCard.NoTrade
                                : predictVolume > 0 ? assumeCard
                                : (assumeCard == BaccratCard.Banker ? BaccratCard.Player : BaccratCard.Banker);

            UpdateSaveCards();

            return new QuadrupleResult
            {
                Value = currentOrder != 7 ? Current_Predict : BaccratCard.NoTrade,
                Volume = currentOrder != 7 ?  Math.Abs( predictVolume) : 0,
                Diff_Coff = Current_Diff, 
                Same_Coff = Current_Same
            };
        }

        /// <summary>
        /// Update current Coeff 
        /// </summary>
        /// <param name="auto">auto = true: Update based on current coeffs, auto = false: Update based on `same` and `diff`</param>
        /// <param name="same">current Same Coeff</param>
        /// <param name="diff">current different Coeff</param>
        public void UpdateCoeff(bool auto = true, int? same = null, int? diff = null)
        {
            #region Main logic
            if (auto)
            {
                if (CompareSame())
                    return;

                var currentOrder = (BaccratCards.Count - 1) % 8; //0-8 --> Count needs to minus 1

                if (currentOrder < 3) //If currentOrder in [0, 1, 2, 3]: 
                {
                    return;
                }

                if (BaccratCards.Count >= 5)
                {
                    if (BaccratCards[BaccratCards.Count - 1] == BaccratCards[BaccratCards.Count - 5])
                    {
                        Current_Same = Math.Abs(Current_Same);
                        Current_Diff = -Math.Abs(Current_Diff);
                    }
                    else
                    {
                        Current_Same = -Math.Abs(Current_Same);
                        Current_Diff = Math.Abs(Current_Diff);
                    }
                }
            }
            else
            {
                if (same.HasValue && diff.HasValue)
                {
                    Current_Same = same.Value;
                    Current_Diff = diff.Value;
                }
            }
            #endregion

        }


        public QuadrupleCoeff ShowCoeff()
        {
            return new QuadrupleCoeff 
            { 
                Diff_Coff = Current_Diff, 
                Same_Coff = Current_Same 
            };
        }

        public void Reset()
        {
            Current_Same = 0;
            Current_Diff = 0;
            Current_Predict = BaccratCard.NoTrade;
            
            BaccratCards.Clear();
            SaveBaccratCards.Clear();
        }

        public void SetCards(List<BaccratCard> baccratCards)
        {
            BaccratCards.Clear();

            for (var i = 0; i < baccratCards.Count; i++)
            {
                BaccratCards.Add(baccratCards[i]);
            }
        }

        private void UpdateSaveCards()
        {
            SaveBaccratCards.Clear();
            for (var i = 0; i < BaccratCards.Count; i++)
            {
                SaveBaccratCards.Add(BaccratCards[i]);
            }
        }


    }
}
