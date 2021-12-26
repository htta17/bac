using System;
using System.Collections.Generic;
using System.Text;

namespace CalculationLogic
{
    public enum ThreadMode
    { 
        Two =2, 
        Four = 4
    }
    public class BaccaratQuadrupleMaster
    {
        /// <summary>
        /// Initial BaccaratQuadrupleMaster
        /// </summary>
        /// <param name="threadMode">2 or 4. Default is 4</param>
        public BaccaratQuadrupleMaster(ThreadMode threadMode)
        {
            TradeFiveToEightCards = new List<BaccratCard>();
            TradeOneToFourCards = new List<BaccratCard>();
            TradeSevenToTwoCards = new List<BaccratCard>();
            TradeThreeToSixCards = new List<BaccratCard>();

            TradeFiveToEightCalculator = new BaccaratQuadruple();
            TradeOneToFourCalculator = new BaccaratQuadruple();
            TradeSevenToTwoCalculator = new BaccaratQuadruple();
            TradeThreeToSixCalculator = new BaccaratQuadruple();

            MasterList = new List<BaccratCard>();
            
            TotalProfit = 0;
            MasterPredict = new QuadruplePredict { Value = BaccratCard.NoTrade, Volume = 0 };

            ThreadMode = threadMode;

            HistoryCoeffs = new List<HistoryCoeff>();
        }
        
        List<BaccratCard> MasterList { get; set; }
        
        List<HistoryCoeff> HistoryCoeffs { get; set; }

        BaccaratQuadruple TradeFiveToEightCalculator { get; set; }
        List<BaccratCard> TradeFiveToEightCards { get; set; }
        BaccaratQuadruple TradeOneToFourCalculator { get; set; }
        List<BaccratCard> TradeOneToFourCards { get; set; }

        BaccaratQuadruple TradeThreeToSixCalculator { get; set; }
        List<BaccratCard> TradeThreeToSixCards { get; set; }

        BaccaratQuadruple TradeSevenToTwoCalculator { get; set; }
        List<BaccratCard> TradeSevenToTwoCards { get; set; }

        public int MasterID 
        { 
            get 
            { 
                return MasterList.Count; 
            } 
        }
        public int TotalProfit { get; internal set;}
        public int LastStepProfit { get; internal set; }

        private ThreadMode ThreadMode { get; set; }

        public QuadruplePredict MasterPredict { get; internal set; }

        QuadrupleResult CurrentPredict14 = new QuadrupleResult { Value = BaccratCard.NoTrade, Volume = 0 };
        QuadrupleResult CurrentPredict36 = new QuadrupleResult { Value = BaccratCard.NoTrade, Volume = 0 };
        QuadrupleResult CurrentPredict58 = new QuadrupleResult { Value = BaccratCard.NoTrade, Volume = 0 };
        QuadrupleResult CurrentPredict72 = new QuadrupleResult { Value = BaccratCard.NoTrade, Volume = 0 };

        /// <summary>
        /// Do everything in 1 step
        /// </summary>
        /// <param name="inputValue"></param>
        public void Process(BaccratCard inputValue)
        {
            if (inputValue == BaccratCard.NoTrade) //Do nothing
                return;             
            
            //Reset if needed            
            if (MasterID % 8 == 0)
            {
                TradeFiveToEightCards.Clear();
            }
            else if (MasterID % 8 == 2)
            {
                TradeSevenToTwoCards.Clear();
            }
            else if (MasterID % 8 == 4)
            {
                TradeOneToFourCards.Clear();
            }
            else if (MasterID % 8 == 6)
            {
                TradeThreeToSixCards.Clear();
            }            
            
            //Add cards
            MasterList.Add(inputValue);

            if (MasterID > 0)
            {
                TradeFiveToEightCards.Add(inputValue);
            }

            if (MasterID > 2)
            {
                TradeSevenToTwoCards.Add(inputValue);
            }

            if (MasterID > 4)
            {
                TradeOneToFourCards.Add(inputValue);
            }

            if (MasterID > 6)
            {
                TradeThreeToSixCards.Add(inputValue);
            }

            TradeFiveToEightCalculator.SetCards(TradeFiveToEightCards);
            TradeOneToFourCalculator.SetCards(TradeOneToFourCards);
            TradeSevenToTwoCalculator.SetCards(TradeSevenToTwoCards);
            TradeThreeToSixCalculator.SetCards(TradeThreeToSixCards);

            LastStepProfit = 0;
            LastStepProfit += (CurrentPredict14.Value == BaccratCard.NoTrade ? 0 : inputValue != CurrentPredict14.Value ? -CurrentPredict14.Volume : CurrentPredict14.Volume);
            LastStepProfit += (CurrentPredict58.Value == BaccratCard.NoTrade ? 0 : inputValue != CurrentPredict58.Value ? -CurrentPredict58.Volume : CurrentPredict58.Volume);
            if (ThreadMode == ThreadMode.Four)
            {
                LastStepProfit += (CurrentPredict36.Value == BaccratCard.NoTrade ? 0 : inputValue != CurrentPredict36.Value ? -CurrentPredict36.Volume : CurrentPredict36.Volume);
                LastStepProfit += (CurrentPredict72.Value == BaccratCard.NoTrade ? 0 : inputValue != CurrentPredict72.Value ? -CurrentPredict72.Volume : CurrentPredict72.Volume);
            }
            TotalProfit += LastStepProfit;

            TradeFiveToEightCalculator.UpdateCoeff();
            TradeOneToFourCalculator.UpdateCoeff();
            TradeThreeToSixCalculator.UpdateCoeff();
            TradeSevenToTwoCalculator.UpdateCoeff();

            CurrentPredict58 = TradeFiveToEightCalculator.Predict();
            CurrentPredict14 = TradeOneToFourCalculator.Predict();
            CurrentPredict36 = TradeThreeToSixCalculator.Predict();
            CurrentPredict72 = TradeSevenToTwoCalculator.Predict();

            var predict14 = CurrentPredict14.Value == BaccratCard.Banker ? CurrentPredict14.Volume : -CurrentPredict14.Volume;
            var predict36 = CurrentPredict36.Value == BaccratCard.Banker ? CurrentPredict36.Volume : -CurrentPredict36.Volume;
            var predict58 = CurrentPredict58.Value == BaccratCard.Banker ? CurrentPredict58.Volume : -CurrentPredict58.Volume;
            var predict72 = CurrentPredict72.Value == BaccratCard.Banker ? CurrentPredict72.Volume : -CurrentPredict72.Volume;
                        
            var totalPredict = predict14 + predict58;
            if (ThreadMode == ThreadMode.Four)
            {
                totalPredict += predict36 + predict72; 
            }

            MasterPredict.Volume = Math.Abs(totalPredict);
            MasterPredict.Value = totalPredict == 0 
                                    ? BaccratCard.NoTrade : totalPredict > 0 
                                    ? BaccratCard.Banker : BaccratCard.Player;

            HistoryCoeffs.Add(new HistoryCoeff
            {
                Diff14 = CurrentPredict14.Diff_Coff,
                Same14 = CurrentPredict14.Same_Coff, 
                Diff36 = CurrentPredict36.Diff_Coff, 
                Same36 = CurrentPredict36.Same_Coff, 
                Diff58 = CurrentPredict58.Diff_Coff, 
                Same58 = CurrentPredict58.Same_Coff, 
                Diff72 = CurrentPredict72.Diff_Coff, 
                Same72 = CurrentPredict72.Same_Coff
            });

        }

        public void ResetAll()
        {
            TradeFiveToEightCards.Clear();
            TradeOneToFourCards.Clear();
            TradeSevenToTwoCards.Clear();
            TradeThreeToSixCards.Clear();

            TradeFiveToEightCalculator.Reset();
            TradeOneToFourCalculator.Reset();
            TradeSevenToTwoCalculator.Reset();
            TradeThreeToSixCalculator.Reset(); 

            MasterList.Clear();

            TotalProfit = 0;

            MasterPredict = new QuadruplePredict { Value = BaccratCard.NoTrade, Volume = 0 };
        }

        public void Reverse()
        {
            //Delete last cards
            MasterList.RemoveAt(MasterList.Count - 1);

            if (TradeFiveToEightCards.Count > 0)
                TradeFiveToEightCards.RemoveAt(TradeFiveToEightCards.Count -1);

            if (TradeOneToFourCards.Count > 0)
                TradeOneToFourCards.RemoveAt(TradeOneToFourCards.Count - 1);

            if (TradeSevenToTwoCards.Count > 0)
                TradeSevenToTwoCards.RemoveAt(TradeSevenToTwoCards.Count - 1);

            if (TradeThreeToSixCards.Count > 0)
                TradeThreeToSixCards.RemoveAt(TradeThreeToSixCards.Count - 1);

            HistoryCoeffs.RemoveAt(HistoryCoeffs.Count - 1);

            //Set coeff for calculators
            var lastCoeffs = HistoryCoeffs.Count > 0 ? HistoryCoeffs.FindLast(c => 1 == 1)
                                    : default (HistoryCoeff) ;
            if (lastCoeffs != null)
            {
                TradeFiveToEightCalculator.UpdateCoeff(false, lastCoeffs.Same58, lastCoeffs.Diff58);
                TradeOneToFourCalculator.UpdateCoeff(false, lastCoeffs.Same14, lastCoeffs.Diff14);
                TradeThreeToSixCalculator.UpdateCoeff(false, lastCoeffs.Same36, lastCoeffs.Diff36);
                TradeSevenToTwoCalculator.UpdateCoeff(false, lastCoeffs.Same72, lastCoeffs.Diff72);
            }
        }
    }

    public class HistoryCoeff
    { 
        public int Same14 { get; set; }
        public int Same36 { get; set; }
        public int Same58 { get; set; }
        public int Same72 { get; set; }
        public int Diff14 { get; set; }
        public int Diff36 { get; set; }
        public int Diff58 { get; set; }
        public int Diff72 { get; set; }
    }


}
