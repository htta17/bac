using CoreLogic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midas.Utils
{
    public static class GenerateAllThreadLogs
    {
        #region ProcessFile
        public static void ProcessFile(string filePath, int num)
        {            
            var random = new Random().Next(100);
            
            var processedFilePath = $"{filePath.Replace(".csv", "")}_processed_9_threads_{DateTime.Now.ToString("yyMMddHHmmss_")}{num}.csv";
            var allLines = File.ReadAllLines(filePath).ToList();

            BaccaratQuadruple _tradeOneToFourCalculator = new BaccaratQuadruple { SaveBaccratCards = new List<BaccratCard>() };
            List<BaccratCard> _tradeOneToFourCards = new List<BaccratCard>();

            BaccaratQuadruple _tradeTwoToFiveCalculator = new BaccaratQuadruple { SaveBaccratCards = new List<BaccratCard>() };
            List<BaccratCard> _tradeTwoToFiveCards = new List<BaccratCard>();

            BaccaratQuadruple _tradeThreeToSixCalculator = new BaccaratQuadruple { SaveBaccratCards = new List<BaccratCard>() };
            List<BaccratCard> _tradeThreeToSixCards = new List<BaccratCard>();

            BaccaratQuadruple _tradeFourToSevenCalculator = new BaccaratQuadruple { SaveBaccratCards = new List<BaccratCard>() };
            List<BaccratCard> _tradeFourToSevenCards = new List<BaccratCard>();

            BaccaratQuadruple _tradeFiveToEightCalculator = new BaccaratQuadruple { SaveBaccratCards = new List<BaccratCard>() };
            List<BaccratCard> _tradeFiveToEightCards = new List<BaccratCard>();

            BaccaratQuadruple _tradeSixToOneCalculator = new BaccaratQuadruple { SaveBaccratCards = new List<BaccratCard>() };
            List<BaccratCard> _tradeSixToOneCards = new List<BaccratCard>();

            BaccaratQuadruple _tradeSevenToTwoCalculator = new BaccaratQuadruple {  SaveBaccratCards = new List<BaccratCard>() };
            List<BaccratCard> _tradeSevenToTwoCards = new List<BaccratCard>();

            BaccaratQuadruple _tradeEightToThreeCalculator = new BaccaratQuadruple { SaveBaccratCards = new List<BaccratCard>() };
            List<BaccratCard> _tradeEightToThreeCards = new List<BaccratCard>();

            BaccaratQuadruple _tradeFiveToEight_2_Calculator = new BaccaratQuadruple { SaveBaccratCards = new List<BaccratCard>() };
            List<BaccratCard> _tradeFiveToEight_2_Cards = new List<BaccratCard>();

            var loss_Profit_Total58 = 0;
            var loss_Profit_Total14 = 0;
            var loss_Profit_Total36 = 0;
            var loss_Profit_Total72 = 0;

            var loss_Profit_Total25 = 0;
            var loss_Profit_Total47 = 0;
            var loss_Profit_Total61 = 0;
            var loss_Profit_Total83 = 0;
            var loss_Profit_Total58_2 = 0;

            var same_58 = 0;
            var diff_58 = 0;

            var same_61 = 0;
            var diff_61 = 0;

            var same_72 = 0;
            var diff_72 = 0;

            var same_83 = 0;
            var diff_83 = 0;

            var same_14 = 0;
            var diff_14 = 0;

            var same_25 = 0;
            var diff_25 = 0;

            var same_36 = 0;
            var diff_36 = 0;

            var same_47 = 0;
            var diff_47 = 0;

            var same_58_2 = 0;
            var diff_58_2 = 0;

            foreach (var line in allLines)
            {
                var texts = line.Split(',');
                var card = texts[2];
                if (card == "Banker" || card == "Player")
                {
                    var id = int.Parse(texts[0]);
                    if (id % 8 == 1)
                    {
                        _tradeFiveToEightCards.Clear();
                        _tradeFiveToEight_2_Cards.Clear();
                    }
                    else if (id % 8 == 2)
                    {
                        _tradeSixToOneCards.Clear();
                    }
                    else if (id % 8 == 3)
                    {
                        _tradeSevenToTwoCards.Clear();
                    }
                    else if (id % 8 == 4)
                    {
                        _tradeEightToThreeCards.Clear();
                    }
                    else if (id % 8 == 5)
                    {
                        _tradeOneToFourCards.Clear();
                    }
                    else if (id % 8 == 6)
                    {
                        _tradeTwoToFiveCards.Clear();
                    }
                    else if (id % 8 == 7)
                    {
                        _tradeThreeToSixCards.Clear();
                    }
                    else if (id % 8 == 0)
                    {
                        _tradeFourToSevenCards.Clear();
                    }
                    

                    var _inputValue = card == "Banker"
                                    ? BaccratCard.Banker : BaccratCard.Player;
                    if (id > 0)
                    {
                        _tradeFiveToEightCards.Add(_inputValue);                       
                    }
                    
                    if (id > 1)
                    {
                        _tradeSixToOneCards.Add(_inputValue);
                    }
                    
                    if (id > 2)
                    {
                        _tradeSevenToTwoCards.Add(_inputValue);
                    }

                    if (id > 3)
                    {
                        _tradeEightToThreeCards.Add(_inputValue);
                    }

                    if (id > 4)
                    {
                        _tradeOneToFourCards.Add(_inputValue);
                    }

                    if (id > 5)
                    {
                        _tradeTwoToFiveCards.Add(_inputValue);
                    }

                    if (id > 6)
                    {
                        _tradeThreeToSixCards.Add(_inputValue);
                    }

                    if (id > 7)
                    {
                        _tradeFourToSevenCards.Add(_inputValue);
                    }

                    if (id > 8)
                    {
                        _tradeFiveToEight_2_Cards.Add(_inputValue);
                    }

                    _tradeOneToFourCalculator.SetCards(_tradeOneToFourCards);
                    _tradeTwoToFiveCalculator.SetCards(_tradeTwoToFiveCards);
                    _tradeThreeToSixCalculator.SetCards(_tradeThreeToSixCards);
                    _tradeFourToSevenCalculator.SetCards(_tradeFourToSevenCards);
                    _tradeFiveToEightCalculator.SetCards(_tradeFiveToEightCards);
                    _tradeSixToOneCalculator.SetCards(_tradeSixToOneCards);
                    _tradeSevenToTwoCalculator.SetCards(_tradeSevenToTwoCards);
                    _tradeEightToThreeCalculator.SetCards(_tradeEightToThreeCards);
                    _tradeFiveToEight_2_Calculator.SetCards(_tradeFiveToEight_2_Cards);

                    _tradeOneToFourCalculator.UpdateCoeff();
                    _tradeTwoToFiveCalculator.UpdateCoeff();
                    _tradeThreeToSixCalculator.UpdateCoeff();
                    _tradeFourToSevenCalculator.UpdateCoeff();
                    _tradeFiveToEightCalculator.UpdateCoeff();
                    _tradeSixToOneCalculator.UpdateCoeff();
                    _tradeSevenToTwoCalculator.UpdateCoeff();
                    _tradeEightToThreeCalculator.UpdateCoeff();
                    _tradeFiveToEight_2_Calculator.UpdateCoeff();

                    var coeff14 = _tradeOneToFourCalculator.ShowCoeff();
                    same_14 = (id % 8 >= 1 && id % 8 <= 4 && id > 4) ? coeff14.Same_Coff : 0;
                    diff_14 = (id % 8 >= 1 && id % 8 <= 4 && id > 4) ? coeff14.Diff_Coff : 0;
                    loss_Profit_Total14 += (same_14 + diff_14) / 2;

                    var coeff25 = _tradeTwoToFiveCalculator.ShowCoeff();
                    same_25 = (id % 8 >= 2 && id % 8 <= 5 && id > 5) ? coeff25.Same_Coff : 0;
                    diff_25 = (id % 8 >= 2 && id % 8 <= 5 && id > 5) ? coeff25.Diff_Coff : 0;
                    loss_Profit_Total25 += (same_25 + diff_25) / 2;

                    var coeff36 = _tradeThreeToSixCalculator.ShowCoeff();
                    same_36 = (id % 8 >= 3 && id % 8 <= 6 && id > 6) ? coeff36.Same_Coff : 0;
                    diff_36 = (id % 8 >= 3 && id % 8 <= 6 && id > 6) ? coeff36.Diff_Coff : 0;
                    loss_Profit_Total36 += (same_36 + diff_36) / 2;

                    var coeff47 = _tradeFourToSevenCalculator.ShowCoeff();
                    same_47 = (id % 8 >= 4 && id % 8 <= 7 && id > 7) ? coeff47.Same_Coff : 0;
                    diff_47 = (id % 8 >= 4 && id % 8 <= 7 && id > 7) ? coeff47.Diff_Coff : 0;
                    loss_Profit_Total47 += (same_47 + diff_47) / 2;

                    var coeff58 = _tradeFiveToEightCalculator.ShowCoeff();
                    same_58 = ((id % 8 >= 5) || (id % 8 == 0)) ? coeff58.Same_Coff : 0;
                    diff_58 = ((id % 8 >= 5) || (id % 8 == 0)) ? coeff58.Diff_Coff : 0;
                    loss_Profit_Total58 += (same_58 + diff_58) / 2;

                    var coeff61 = _tradeSixToOneCalculator.ShowCoeff();
                    same_61 = ((id % 8 >= 6) || (id % 8 <= 1)) ? coeff61.Same_Coff : 0;
                    diff_61 = ((id % 8 >= 6) || (id % 8 <= 1)) ? coeff61.Diff_Coff : 0;
                    loss_Profit_Total61 += (same_61 + diff_61) / 2;

                    var coeff72 = _tradeSevenToTwoCalculator.ShowCoeff();
                    same_72 = ((id % 8 >= 7 || id % 8 <= 2) && id > 2) ? coeff72.Same_Coff : 0;
                    diff_72 = ((id % 8 >= 7 || id % 8 <= 2) && id > 2) ? coeff72.Diff_Coff : 0;
                    loss_Profit_Total72 += (same_72 + diff_72) / 2;

                    var coeff83 = _tradeEightToThreeCalculator.ShowCoeff();
                    same_83 = ((id % 8 >= 0 && id % 8 <= 3) && id > 3) ? coeff83.Same_Coff : 0;
                    diff_83 = ((id % 8 >= 0 && id % 8 <= 3) && id > 3) ? coeff83.Diff_Coff : 0;
                    loss_Profit_Total83 += (same_83 + diff_83) / 2;

                    var coeff58_2 = _tradeFiveToEight_2_Calculator.ShowCoeff();
                    same_58_2 = ((id % 8 >= 5) || (id % 8 == 0)) ? coeff58_2.Same_Coff : 0;
                    diff_58_2 = ((id % 8 >= 5) || (id % 8 == 0)) ? coeff58_2.Diff_Coff : 0;
                    loss_Profit_Total58_2 += (same_58_2 + diff_58_2) / 2;

                    var loss_Profit_Total = loss_Profit_Total14 +
                                            loss_Profit_Total25 +
                                            loss_Profit_Total36 +
                                            loss_Profit_Total47 +
                                            loss_Profit_Total58 +
                                            loss_Profit_Total61 +
                                            loss_Profit_Total72 +
                                            loss_Profit_Total83 +
                                            loss_Profit_Total58_2
                                            ;

                    _tradeOneToFourCalculator.Predict();
                    _tradeTwoToFiveCalculator.Predict();
                    _tradeThreeToSixCalculator.Predict();
                    _tradeFourToSevenCalculator.Predict();
                    _tradeFiveToEightCalculator.Predict();
                    _tradeSixToOneCalculator.Predict();
                    _tradeSevenToTwoCalculator.Predict();
                    _tradeEightToThreeCalculator.Predict();
                    _tradeFiveToEight_2_Calculator.Predict();

                    var log = string.Format("{0},{1},{2},{3},{4},,{5},{6},{7},{8},,{9},{10},{11},{12},,{13},{14},{15},{16},,{17},{18},{19},{20},,{21},{22},{23},{24},,{25},{26},{27},{28},,{29},{30},{31},{32},,{33},{34},{35},{36},,{37}\r\n",
                                        _inputValue == BaccratCard.Banker ? 1 : -1, //Result --> {0}
                                                                                    
                                        same_58, 
                                        diff_58, 
                                        (same_58 + diff_58) / 2, 
                                        loss_Profit_Total58, //{4}

                                        same_61, 
                                        diff_61, 
                                        (same_61 + diff_61) / 2, 
                                        loss_Profit_Total61, //{8}

                                        same_72, 
                                        diff_72,
                                        (same_72 + diff_72) / 2, 
                                        loss_Profit_Total72, //{12}

                                        same_83,
                                        diff_83,
                                        (same_83 + diff_83) / 2,
                                        loss_Profit_Total83, //{16}

                                        same_14, 
                                        diff_14,
                                        (same_14 + diff_14) / 2, 
                                        loss_Profit_Total14,//--> {20}

                                        same_25, 
                                        diff_25,
                                        (same_25 + diff_25) / 2,
                                        loss_Profit_Total25, //--> {24}

                                        same_36, 
                                        diff_36,
                                        (same_36 + diff_36) / 2, 
                                        loss_Profit_Total36, //--> {28}

                                        same_47,
                                        diff_47,
                                        (same_47 + diff_47) / 2,
                                        loss_Profit_Total47,  //--> {32}

                                        same_58_2,
                                        diff_58_2,
                                        (same_58_2 + diff_58_2) / 2,
                                        loss_Profit_Total58_2,   //--> {36}

                                        loss_Profit_Total); //-> 17

                    File.AppendAllText(processedFilePath, log);
                }
            }

            //End of function
        }

        #endregion
    }
}
