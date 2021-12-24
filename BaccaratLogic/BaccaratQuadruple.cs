using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CalculationLogic
{
    public enum BaccratCard
    { 
        Player = -1,
        NoTrade =0,
        Banker = 1
    }
    public class QuadrupleResult
    {
        public BaccratCard Value { get; set; }
        public int Volume { get; set; }

        public int Same_Coff { get; set; }
        public int Diff_Coff { get; set; }
    }
    public class BaccaratQuadruple
    {
        public BaccaratQuadruple()
        {            
        }

        private int Current_Same { get; set; }
        private int Current_Diff { get; set; }

        private BaccratCard Current_Predict { get; set; }

        public List<BaccratCard> BaccratCards { get; internal set; } = new List<BaccratCard>();

        public List<BaccratCard> SaveBaccratCards { get; set; } = new List<BaccratCard>();

        public bool CompareSame()
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

        public QuadrupleResult Predict(int? realSame = null, int? realDiff = null)
        {
            var currentOrder = (BaccratCards.Count - 1) % 8; //0-8 --> Count needs to minus 1

            if (realSame.HasValue)
            {
                Current_Same = realSame.Value; 
            }

            if (realDiff.HasValue)
            {
                Current_Diff = realDiff.Value;
            }

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

            SaveBaccratCards = BaccratCards;            

            return new QuadrupleResult
            {
                Value = currentOrder != 7 ? Current_Predict : BaccratCard.NoTrade,
                Volume = currentOrder != 7 ?  Math.Abs( predictVolume) : 0,
                Diff_Coff = Current_Diff, 
                Same_Coff = Current_Same
            };
        }

        public void UpdateCoff()
        {
            if (CompareSame())
                return;
            
            var currentOrder = (BaccratCards.Count - 1) % 8; //0-8 --> Count needs to minus 1

            if (currentOrder < 3 ) //If currentOrder in [0, 1, 2, 3]: 
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

        /// <summary>
        /// Item1: Same
        /// Item2: Diff
        /// </summary>
        /// <returns></returns>
        public Tuple<int, int> ShowCoff()
        {
            return Tuple.Create<int, int>(Current_Same, Current_Diff);
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


    }
}
