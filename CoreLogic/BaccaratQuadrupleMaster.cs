using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLogic
{
    [Flags]
    public enum ThreadMode
    { 
        One_Four = 1,
        Two_Five =2, 
        Three_Six = 4,
        Four_Seven = 8, 
        Five_Eight = 16, 
        Six_One = 32, 
        Seven_Two = 64, 
        Eight_Three = 128
    }
    public class BaccaratQuadrupleMaster
    {
        /// <summary>
        /// Initial BaccaratQuadrupleMaster
        /// </summary>
        /// <param name="threadMode">2 or 4. Default is 4</param>
        public BaccaratQuadrupleMaster(ThreadMode threadMode)
        {
            TradeOneToFourCards = new List<BaccratCard>();
            TradeTwoToFiveCards = new List<BaccratCard>();
            TradeThreeToSixCards = new List<BaccratCard>();
            TradeFourToSevenCards = new List<BaccratCard>();
            TradeFiveToEightCards = new List<BaccratCard>();
            TradeSixToOneCards = new List<BaccratCard>();
            TradeSevenToTwoCards = new List<BaccratCard>();
            TradeEightToThreeCards = new List<BaccratCard>();

            TradeOneToFourCalculator = new BaccaratQuadruple();
            TradeTwoToFiveCalculator = new BaccaratQuadruple();
            TradeThreeToSixCalculator = new BaccaratQuadruple();
            TradeFourToSevenCalculator = new BaccaratQuadruple();
            TradeFiveToEightCalculator = new BaccaratQuadruple();
            TradeSixToOneCalculator = new BaccaratQuadruple();
            TradeSevenToTwoCalculator = new BaccaratQuadruple();
            TradeEightToThreeCalculator = new BaccaratQuadruple();

            MasterList = new List<BaccratCard>();
            
            Trade_TotalProfit =
            Trade_LastStepProfit =
            Profit14 =
            Profit25 =
            Profit36 =
            Profit47 =
            Profit58 =
            Profit61 =
            Profit72 =
            Profit83 = 0;

            MasterPredict = new BaccaratPredict { Value = BaccratCard.NoTrade, Volume = 0 };

            ThreadMode = threadMode;

            HistoryCoeffs = new List<HistoryInfo>();
        }
        
        List<BaccratCard> MasterList { get; set; }        
        List<HistoryInfo> HistoryCoeffs { get; set; }

        BaccaratQuadruple TradeFiveToEightCalculator { get; set; }
        List<BaccratCard> TradeFiveToEightCards { get; set; }

        BaccaratQuadruple TradeSixToOneCalculator { get; set; }
        List<BaccratCard> TradeSixToOneCards { get; set; }


        BaccaratQuadruple TradeOneToFourCalculator { get; set; }
        List<BaccratCard> TradeOneToFourCards { get; set; }

        BaccaratQuadruple TradeTwoToFiveCalculator { get; set; }
        List<BaccratCard> TradeTwoToFiveCards { get; set; }

        BaccaratQuadruple TradeThreeToSixCalculator { get; set; }
        List<BaccratCard> TradeThreeToSixCards { get; set; }

        BaccaratQuadruple TradeFourToSevenCalculator { get; set; }
        List<BaccratCard> TradeFourToSevenCards { get; set; }

        BaccaratQuadruple TradeSevenToTwoCalculator { get; set; }
        List<BaccratCard> TradeSevenToTwoCards { get; set; }

        BaccaratQuadruple TradeEightToThreeCalculator { get; set; }
        List<BaccratCard> TradeEightToThreeCards { get; set; }

        public int MasterID 
        { 
            get 
            { 
                return MasterList.Count; 
            } 
        }
        /// <summary>
        /// Accumulate of thead 1-4
        /// </summary>
        public int Profit14 { get; internal set; }

        /// <summary>
        /// Accumulate of thead 2-5
        /// </summary>
        public int Profit25 { get; internal set; }

        /// <summary>
        /// Accumulate of thead 3-6
        /// </summary>
        public int Profit36 { get; internal set; }

        /// <summary>
        /// Accumulate of thead 4-7
        /// </summary>
        public int Profit47 { get; internal set; }

        /// <summary>
        /// Accumulate of thead 5-8
        /// </summary>
        public int Profit58 { get; internal set; }

        /// <summary>
        /// Accumulate of thead 6-1
        /// </summary>
        public int Profit61 { get; internal set; }

        /// <summary>
        /// Accumulate of thead 7-2
        /// </summary>
        public int Profit72 { get; internal set; }

        /// <summary>
        /// Accumulate of thead 8-3
        /// </summary>

        public int Profit83 { get; internal set; }

        /// <summary>
        /// Total Profit, only calculate based on running threads
        /// </summary>
        public int Trade_TotalProfit { get; internal set;}
        public int Trade_LastStepProfit { get; internal set; }

        private ThreadMode ThreadMode { get; set; }

        public BaccaratPredict MasterPredict { get; internal set; }

        QuadrupleResult CurrentPredict14 = new QuadrupleResult { Value = BaccratCard.NoTrade, Volume = 0 };
        QuadrupleResult CurrentPredict25 = new QuadrupleResult { Value = BaccratCard.NoTrade, Volume = 0 };
        QuadrupleResult CurrentPredict36 = new QuadrupleResult { Value = BaccratCard.NoTrade, Volume = 0 };
        QuadrupleResult CurrentPredict47 = new QuadrupleResult { Value = BaccratCard.NoTrade, Volume = 0 };
        QuadrupleResult CurrentPredict58 = new QuadrupleResult { Value = BaccratCard.NoTrade, Volume = 0 };
        QuadrupleResult CurrentPredict61 = new QuadrupleResult { Value = BaccratCard.NoTrade, Volume = 0 };
        QuadrupleResult CurrentPredict72 = new QuadrupleResult { Value = BaccratCard.NoTrade, Volume = 0 };
        QuadrupleResult CurrentPredict83 = new QuadrupleResult { Value = BaccratCard.NoTrade, Volume = 0 };

        /// <summary>
        /// Do everything in 1 step
        /// </summary>
        /// <param name="inputValue"></param>
        public void Process(BaccratCard inputValue)
        {
            if (inputValue == BaccratCard.NoTrade) //Do nothing
                return;

            //Reset if arraray get 8 cards
            #region Reset if arraray get 8 cards
            if (MasterID % 8 == 0)
            {
                TradeFiveToEightCards.Clear();
            }
            else if (MasterID % 8 == 1)
            {
                TradeSixToOneCards.Clear();
            }
            else if (MasterID % 8 == 2)
            {
                TradeSevenToTwoCards.Clear();
            }
            else if (MasterID % 8 == 3)
            {
                TradeEightToThreeCards.Clear();
            }
            else if (MasterID % 8 == 4)
            {
                TradeOneToFourCards.Clear();
            }
            else if (MasterID % 8 == 5)
            {
                TradeTwoToFiveCards.Clear();
            }
            else if (MasterID % 8 == 6)
            {
                TradeThreeToSixCards.Clear();
            }
            else if (MasterID % 8 == 7)
            {
                TradeFourToSevenCards.Clear();
            }
            #endregion

            //Add cards
            #region Add cards
            MasterList.Add(inputValue);

            if (MasterID > 0)
            {
                TradeFiveToEightCards.Add(inputValue);
            }

            if (MasterID > 1)
            {
                TradeSixToOneCards.Add(inputValue);
            }

            if (MasterID > 2)
            {
                TradeSevenToTwoCards.Add(inputValue);
            }

            if (MasterID > 3)
            {
                TradeEightToThreeCards.Add(inputValue);
            }

            if (MasterID > 4)
            {
                TradeOneToFourCards.Add(inputValue);
            }

            if (MasterID > 5)
            {
                TradeTwoToFiveCards.Add(inputValue);
            }

            if (MasterID > 6)
            {
                TradeThreeToSixCards.Add(inputValue);
            }

            if (MasterID > 7)
            {
                TradeFourToSevenCards.Add(inputValue);
            }

            TradeOneToFourCalculator.SetCards(TradeOneToFourCards);
            TradeTwoToFiveCalculator.SetCards(TradeTwoToFiveCards);
            TradeThreeToSixCalculator.SetCards(TradeThreeToSixCards);
            TradeFourToSevenCalculator.SetCards(TradeFourToSevenCards);
            TradeFiveToEightCalculator.SetCards(TradeFiveToEightCards);
            TradeSixToOneCalculator.SetCards(TradeSixToOneCards);
            TradeSevenToTwoCalculator.SetCards(TradeSevenToTwoCards);
            TradeEightToThreeCalculator.SetCards(TradeEightToThreeCards);
            #endregion


            #region Calculate Profit/Loss based on last predict and new input
            Trade_LastStepProfit = 0;
            var lastStepProfit14 = (CurrentPredict14.Value == BaccratCard.NoTrade ? 0 : inputValue != CurrentPredict14.Value ? -CurrentPredict14.Volume : CurrentPredict14.Volume);
            var lastStepProfit25 = (CurrentPredict25.Value == BaccratCard.NoTrade ? 0 : inputValue != CurrentPredict25.Value ? -CurrentPredict25.Volume : CurrentPredict25.Volume);
            var lastStepProfit36 = (CurrentPredict36.Value == BaccratCard.NoTrade ? 0 : inputValue != CurrentPredict36.Value ? -CurrentPredict36.Volume : CurrentPredict36.Volume);
            var lastStepProfit47 = (CurrentPredict47.Value == BaccratCard.NoTrade ? 0 : inputValue != CurrentPredict47.Value ? -CurrentPredict47.Volume : CurrentPredict47.Volume);
            var lastStepProfit58 = (CurrentPredict58.Value == BaccratCard.NoTrade ? 0 : inputValue != CurrentPredict58.Value ? -CurrentPredict58.Volume : CurrentPredict58.Volume);
            var lastStepProfit61 = (CurrentPredict61.Value == BaccratCard.NoTrade ? 0 : inputValue != CurrentPredict61.Value ? -CurrentPredict61.Volume : CurrentPredict61.Volume);
            var lastStepProfit72 = (CurrentPredict72.Value == BaccratCard.NoTrade ? 0 : inputValue != CurrentPredict72.Value ? -CurrentPredict72.Volume : CurrentPredict72.Volume);
            var lastStepProfit83 = (CurrentPredict83.Value == BaccratCard.NoTrade ? 0 : inputValue != CurrentPredict83.Value ? -CurrentPredict83.Volume : CurrentPredict83.Volume);

            Profit14 += lastStepProfit14;
            Profit25 += lastStepProfit25;
            Profit36 += lastStepProfit36;
            Profit47 += lastStepProfit47;
            Profit58 += lastStepProfit58;
            Profit61 += lastStepProfit61;
            Profit72 += lastStepProfit72;
            Profit83 += lastStepProfit83;

            if (ThreadMode.HasFlag(ThreadMode.One_Four))
            {                
                Trade_LastStepProfit += lastStepProfit14;
            }
            if (ThreadMode.HasFlag(ThreadMode.Two_Five))
            {
                Trade_LastStepProfit += lastStepProfit25;
            }
            if (ThreadMode.HasFlag(ThreadMode.Three_Six))
            {
                Trade_LastStepProfit += lastStepProfit36;
            }
            if (ThreadMode.HasFlag(ThreadMode.Four_Seven))
            {
                Trade_LastStepProfit += lastStepProfit47;
            }
            if (ThreadMode.HasFlag(ThreadMode.Five_Eight))
            {
                Trade_LastStepProfit += lastStepProfit58;
            }
            if (ThreadMode.HasFlag(ThreadMode.Six_One))
            {
                Trade_LastStepProfit += lastStepProfit61;
            }
            if (ThreadMode.HasFlag(ThreadMode.Seven_Two))
            {
                Trade_LastStepProfit += lastStepProfit72;
            }
            if (ThreadMode.HasFlag(ThreadMode.Eight_Three))
            {
                Trade_LastStepProfit += lastStepProfit83;
            }
            Trade_TotalProfit += Trade_LastStepProfit;
            #endregion

            #region Update coeffs based on input
            TradeFiveToEightCalculator.UpdateCoeff();
            TradeSixToOneCalculator.UpdateCoeff();
            TradeSevenToTwoCalculator.UpdateCoeff();
            TradeEightToThreeCalculator.UpdateCoeff();
            TradeOneToFourCalculator.UpdateCoeff();
            TradeTwoToFiveCalculator.UpdateCoeff();
            TradeThreeToSixCalculator.UpdateCoeff();
            TradeFourToSevenCalculator.UpdateCoeff();
            #endregion


            #region Predict next step 
            CurrentPredict58 = TradeFiveToEightCalculator.Predict();
            CurrentPredict61 = TradeSixToOneCalculator.Predict();            
            CurrentPredict72 = TradeSevenToTwoCalculator.Predict();
            CurrentPredict83 = TradeEightToThreeCalculator.Predict();
            CurrentPredict14 = TradeOneToFourCalculator.Predict();
            CurrentPredict25 = TradeTwoToFiveCalculator.Predict();
            CurrentPredict36 = TradeThreeToSixCalculator.Predict();
            CurrentPredict47 = TradeFourToSevenCalculator.Predict();

            var predict14 = CurrentPredict14.Value == BaccratCard.Banker ? CurrentPredict14.Volume : -CurrentPredict14.Volume;
            var predict25 = CurrentPredict25.Value == BaccratCard.Banker ? CurrentPredict25.Volume : -CurrentPredict25.Volume;
            var predict36 = CurrentPredict36.Value == BaccratCard.Banker ? CurrentPredict36.Volume : -CurrentPredict36.Volume;
            var predict47 = CurrentPredict47.Value == BaccratCard.Banker ? CurrentPredict47.Volume : -CurrentPredict47.Volume;
            var predict58 = CurrentPredict58.Value == BaccratCard.Banker ? CurrentPredict58.Volume : -CurrentPredict58.Volume;
            var predict61 = CurrentPredict61.Value == BaccratCard.Banker ? CurrentPredict61.Volume : -CurrentPredict61.Volume;
            var predict72 = CurrentPredict72.Value == BaccratCard.Banker ? CurrentPredict72.Volume : -CurrentPredict72.Volume;
            var predict83 = CurrentPredict83.Value == BaccratCard.Banker ? CurrentPredict83.Volume : -CurrentPredict83.Volume;

            var totalPredict = 0;
            totalPredict += ThreadMode.HasFlag(ThreadMode.One_Four) ? predict14 : 0;
            totalPredict += ThreadMode.HasFlag(ThreadMode.Two_Five) ? predict25 : 0;
            totalPredict += ThreadMode.HasFlag(ThreadMode.Three_Six) ? predict36 : 0;
            totalPredict += ThreadMode.HasFlag(ThreadMode.Four_Seven) ? predict47 : 0;
            totalPredict += ThreadMode.HasFlag(ThreadMode.Five_Eight) ? predict58 : 0;
            totalPredict += ThreadMode.HasFlag(ThreadMode.Six_One) ? predict61 : 0;
            totalPredict += ThreadMode.HasFlag(ThreadMode.Seven_Two) ? predict72 : 0;
            totalPredict += ThreadMode.HasFlag(ThreadMode.Eight_Three) ? predict83 : 0;

            MasterPredict.Volume = Math.Abs(totalPredict);
            MasterPredict.Value = totalPredict == 0 
                                    ? BaccratCard.NoTrade : totalPredict > 0 
                                    ? BaccratCard.Banker : BaccratCard.Player;
            #endregion

            #region Save all coeffs for rollback (If trader makes a mistake and want to re-trade last position
            HistoryCoeffs.Add(new HistoryInfo
            {
                Diff14 = CurrentPredict14.Diff_Coff,
                Same14 = CurrentPredict14.Same_Coff, 
                Diff36 = CurrentPredict36.Diff_Coff, 
                Same36 = CurrentPredict36.Same_Coff, 
                Diff58 = CurrentPredict58.Diff_Coff, 
                Same58 = CurrentPredict58.Same_Coff, 
                Diff72 = CurrentPredict72.Diff_Coff, 
                Same72 = CurrentPredict72.Same_Coff,

                Diff25 = CurrentPredict25.Diff_Coff,
                Same25 = CurrentPredict25.Same_Coff,
                Diff47 = CurrentPredict47.Diff_Coff,
                Same47 = CurrentPredict47.Same_Coff,
                Diff61 = CurrentPredict61.Diff_Coff,
                Same61 = CurrentPredict61.Same_Coff,
                Diff83 = CurrentPredict83.Diff_Coff,
                Same83 = CurrentPredict83.Same_Coff,

                SavedPredict14 = CurrentPredict14,
                SavedPredict25 = CurrentPredict25,
                SavedPredict36 = CurrentPredict36,
                SavedPredict47 = CurrentPredict47,
                SavedPredict58 = CurrentPredict58,
                SavedPredict61 = CurrentPredict61,
                SavedPredict72 = CurrentPredict72,
                SavedPredict83 = CurrentPredict83, 

                SavedTotalProfit = Trade_TotalProfit, 

                AccumulatedProfit14 = Profit14, 
                AccumulatedProfit25 = Profit25, 
                AccumulatedProfit36 = Profit36, 
                AccumulatedProfit47 = Profit47, 
                AccumulatedProfit58 = Profit58, 
                AccumulatedProfit61 = Profit61, 
                AccumulatedProfit72 = Profit72, 
                AccumulatedProfit83 = Profit83

            });
            #endregion
        }

        public void ResetAll()
        {
            TradeOneToFourCards.Clear();
            TradeTwoToFiveCards.Clear();
            TradeThreeToSixCards.Clear();
            TradeFourToSevenCards.Clear();
            TradeFiveToEightCards.Clear();
            TradeSixToOneCards.Clear();
            TradeSevenToTwoCards.Clear();
            TradeEightToThreeCards.Clear();

            TradeOneToFourCalculator.Reset();
            TradeTwoToFiveCalculator.Reset();
            TradeThreeToSixCalculator.Reset();
            TradeFourToSevenCalculator.Reset();
            TradeFiveToEightCalculator.Reset();
            TradeSixToOneCalculator.Reset();
            TradeSevenToTwoCalculator.Reset();
            TradeEightToThreeCalculator.Reset();            

            MasterList.Clear();

            HistoryCoeffs.Clear();

            Trade_TotalProfit =
            Trade_LastStepProfit =
            Profit14 =
            Profit25 =
            Profit36 =
            Profit47 =
            Profit58 =
            Profit61 =
            Profit72 =
            Profit83 = 0;

            MasterPredict = new BaccaratPredict { Value = BaccratCard.NoTrade, Volume = 0 };

            CurrentPredict14 = new QuadrupleResult { Value = BaccratCard.NoTrade, Volume = 0 };
            CurrentPredict25 = new QuadrupleResult { Value = BaccratCard.NoTrade, Volume = 0 };
            CurrentPredict36 = new QuadrupleResult { Value = BaccratCard.NoTrade, Volume = 0 };
            CurrentPredict47 = new QuadrupleResult { Value = BaccratCard.NoTrade, Volume = 0 };
            CurrentPredict58 = new QuadrupleResult { Value = BaccratCard.NoTrade, Volume = 0 };
            CurrentPredict61 = new QuadrupleResult { Value = BaccratCard.NoTrade, Volume = 0 };
            CurrentPredict72 = new QuadrupleResult { Value = BaccratCard.NoTrade, Volume = 0 };
            CurrentPredict83 = new QuadrupleResult { Value = BaccratCard.NoTrade, Volume = 0 };
        }

        public void Reverse()
        {
            //Delete last cards
            MasterList.RemoveAt(MasterList.Count - 1);

            if (TradeFiveToEightCards.Count > 0)
                TradeFiveToEightCards.RemoveAt(TradeFiveToEightCards.Count -1);

            if (TradeSixToOneCards.Count > 0)
                TradeSixToOneCards.RemoveAt(TradeSixToOneCards.Count - 1);

            if (TradeSevenToTwoCards.Count > 0)
                TradeSevenToTwoCards.RemoveAt(TradeSevenToTwoCards.Count - 1);

            if (TradeEightToThreeCards.Count > 0)
                TradeEightToThreeCards.RemoveAt(TradeEightToThreeCards.Count - 1);

            if (TradeOneToFourCards.Count > 0)
                TradeOneToFourCards.RemoveAt(TradeOneToFourCards.Count - 1);

            if (TradeTwoToFiveCards.Count > 0)
                TradeTwoToFiveCards.RemoveAt(TradeTwoToFiveCards.Count - 1);

            if (TradeThreeToSixCards.Count > 0)
                TradeThreeToSixCards.RemoveAt(TradeThreeToSixCards.Count - 1);

            if (TradeFourToSevenCards.Count > 0)
                TradeFourToSevenCards.RemoveAt(TradeFourToSevenCards.Count - 1);

            HistoryCoeffs.RemoveAt(HistoryCoeffs.Count - 1);

            //Set coeff for calculators
            var lastCoeffs = HistoryCoeffs.Count > 0 ? HistoryCoeffs.FindLast(c => 1 == 1)
                                    : default (HistoryInfo) ;
            if (lastCoeffs != null)
            {
                TradeOneToFourCalculator.UpdateCoeff(false, lastCoeffs.Same14, lastCoeffs.Diff14);
                TradeThreeToSixCalculator.UpdateCoeff(false, lastCoeffs.Same36, lastCoeffs.Diff36);
                TradeFiveToEightCalculator.UpdateCoeff(false, lastCoeffs.Same58, lastCoeffs.Diff58);
                TradeSevenToTwoCalculator.UpdateCoeff(false, lastCoeffs.Same72, lastCoeffs.Diff72);

                TradeTwoToFiveCalculator.UpdateCoeff(false, lastCoeffs.Same25, lastCoeffs.Diff25);
                TradeFourToSevenCalculator.UpdateCoeff(false, lastCoeffs.Same47, lastCoeffs.Diff47);
                TradeSixToOneCalculator.UpdateCoeff(false, lastCoeffs.Same61, lastCoeffs.Diff61);
                TradeEightToThreeCalculator.UpdateCoeff(false, lastCoeffs.Same83, lastCoeffs.Diff83);

                CurrentPredict14 = lastCoeffs.SavedPredict14; 
                CurrentPredict25 = lastCoeffs.SavedPredict25;
                CurrentPredict36 = lastCoeffs.SavedPredict36;
                CurrentPredict47 = lastCoeffs.SavedPredict47;
                CurrentPredict58 = lastCoeffs.SavedPredict58;
                CurrentPredict61 = lastCoeffs.SavedPredict61;
                CurrentPredict72 = lastCoeffs.SavedPredict72;
                CurrentPredict83 = lastCoeffs.SavedPredict83;

                Trade_TotalProfit = lastCoeffs.SavedTotalProfit;

                Profit14 = lastCoeffs.AccumulatedProfit14;
                Profit25 = lastCoeffs.AccumulatedProfit25;
                Profit36 = lastCoeffs.AccumulatedProfit36;
                Profit47 = lastCoeffs.AccumulatedProfit47;
                Profit58 = lastCoeffs.AccumulatedProfit58;
                Profit61 = lastCoeffs.AccumulatedProfit61;
                Profit72 = lastCoeffs.AccumulatedProfit72;
                Profit83 = lastCoeffs.AccumulatedProfit83;
            }
        }

        public BaccratCard ShowLastCard()
        {
            return MasterID > 0 ? MasterList[MasterList.Count - 1] : default;
        }
    }

    public class HistoryInfo
    { 
        public int Same14 { get; set; }
        public int Same36 { get; set; }
        public int Same58 { get; set; }
        public int Same72 { get; set; }

        public int Same25 { get; set; }
        public int Same47 { get; set; }
        public int Same61 { get; set; }
        public int Same83 { get; set; }

        public int Diff14 { get; set; }
        public int Diff36 { get; set; }
        public int Diff58 { get; set; }
        public int Diff72 { get; set; }

        public int Diff25 { get; set; }
        public int Diff47 { get; set; }
        public int Diff61 { get; set; }
        public int Diff83 { get; set; }

        public QuadrupleResult SavedPredict14 { get; set; }
        public QuadrupleResult SavedPredict25 { get; set; }
        public QuadrupleResult SavedPredict36 { get; set; }
        public QuadrupleResult SavedPredict47 { get; set; }
        public QuadrupleResult SavedPredict58 { get; set; }
        public QuadrupleResult SavedPredict61 { get; set; }
        public QuadrupleResult SavedPredict72 { get; set; }
        public QuadrupleResult SavedPredict83 { get; set; }

        public int AccumulatedProfit14 { get; set; }
        public int AccumulatedProfit25 { get; set; }
        public int AccumulatedProfit36 { get; set; }
        public int AccumulatedProfit47 { get; set; }
        public int AccumulatedProfit58 { get; set; }
        public int AccumulatedProfit61 { get; set; }
        public int AccumulatedProfit72 { get; set; }
        public int AccumulatedProfit83 { get; set; }

        public int SavedTotalProfit { get; set; }
    }


}
