using CalculationLogic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midas.Utils
{
    public static class Generate4ThreadLogs
    {
        public static void ProcessFile(string filePath, int num)
        {
            var processedFilePath = filePath.Replace(".csv", "") + "_process_" + DateTime.Now.ToString("yyMMddHHmmss_") + num.ToString() + ".csv";
            var allLines = File.ReadAllLines(filePath).ToList();

            BaccaratQuadruple _tradeFiveToEightCalculator = new BaccaratQuadruple { SaveBaccratCards = new List<BaccratCard>() };
            List<BaccratCard> _tradeFiveToEightCards = new List<BaccratCard>();

            BaccaratQuadruple _tradeOneToFourCalculator = new BaccaratQuadruple {  SaveBaccratCards = new List<BaccratCard>() };
            List<BaccratCard> _tradeOneToFourCards = new List<BaccratCard>();

            BaccaratQuadruple _tradeThreeToSixCalculator = new BaccaratQuadruple {  SaveBaccratCards = new List<BaccratCard>() };
            List<BaccratCard> _tradeThreeToSixCards = new List<BaccratCard>();

            BaccaratQuadruple _tradeSevenToTwoCalculator = new BaccaratQuadruple {  SaveBaccratCards = new List<BaccratCard>() };
            List<BaccratCard> _tradeSevenToTwoCards = new List<BaccratCard>();

            var loss_Profit_Total58 = 0;
            var loss_Profit_Total14 = 0;
            var loss_Profit_Total36 = 0;
            var loss_Profit_Total72 = 0;

            var same_58 = 0;
            var diff_58 = 0;
            var same_14 = 0;
            var diff_14 = 0;

            var same_36 = 0;
            var diff_36 = 0;
            var same_72 = 0;
            var diff_72 = 0;

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
                    }
                    else if (id % 8 == 3)
                    {
                        _tradeSevenToTwoCards.Clear();
                    }
                    else if (id % 8 == 5)
                    {
                        _tradeOneToFourCards.Clear();
                    }
                    else if (id % 8 == 7)
                    {
                        _tradeThreeToSixCards.Clear();
                    }

                    var _inputValue = card == "Banker"
                                    ? BaccratCard.Banker : BaccratCard.Player;

                    _tradeFiveToEightCards.Add(_inputValue);
                    if (id > 2)
                    {
                        _tradeSevenToTwoCards.Add(_inputValue);
                    }

                    if (id > 4)
                    {
                        _tradeOneToFourCards.Add(_inputValue);
                    }

                    if (id > 6)
                    {
                        _tradeThreeToSixCards.Add(_inputValue);
                    }

                    _tradeFiveToEightCalculator.SetCards(_tradeFiveToEightCards);
                    _tradeOneToFourCalculator.SetCards(_tradeOneToFourCards);
                    _tradeSevenToTwoCalculator.SetCards(_tradeSevenToTwoCards);
                    _tradeThreeToSixCalculator.SetCards(_tradeThreeToSixCards);

                    _tradeFiveToEightCalculator.UpdateCoeff();
                    _tradeOneToFourCalculator.UpdateCoeff();
                    _tradeSevenToTwoCalculator.UpdateCoeff();
                    _tradeThreeToSixCalculator.UpdateCoeff();

                    var coeff58 = _tradeFiveToEightCalculator.ShowCoeff();
                    same_58 = ((id % 8 >= 5) || (id % 8 == 0)) ? coeff58.Same_Coff : 0;
                    diff_58 = ((id % 8 >= 5) || (id % 8 == 0)) ? coeff58.Diff_Coff : 0;
                    loss_Profit_Total58 += (same_58 + diff_58) / 2;

                    var coeff14 = _tradeOneToFourCalculator.ShowCoeff();
                    same_14 = (id % 8 >= 1 && id % 8 <= 4 && id > 4) ? coeff14.Same_Coff : 0;
                    diff_14 = (id % 8 >= 1 && id % 8 <= 4 && id > 4) ? coeff14.Diff_Coff : 0;
                    loss_Profit_Total14 += (same_14 + diff_14) / 2;

                    var coeff36 = _tradeThreeToSixCalculator.ShowCoeff();
                    same_36 = (id % 8 >= 3 && id % 8 <= 6 && id > 6) ? coeff36.Same_Coff : 0;
                    diff_36 = (id % 8 >= 3 && id % 8 <= 6 && id > 6) ? coeff36.Diff_Coff : 0;
                    loss_Profit_Total36 += (same_36 + diff_36) / 2;

                    var coeff72 = _tradeSevenToTwoCalculator.ShowCoeff();
                    same_72 = ((id % 8 >= 7 || id % 8 <= 2) && id > 2) ? coeff72.Same_Coff : 0;
                    diff_72 = ((id % 8 >= 7 || id % 8 <= 2) && id > 2) ? coeff72.Diff_Coff : 0;
                    loss_Profit_Total72 += (same_72 + diff_72) / 2;

                    //var loss_Profit_Total = texts[6];
                    var loss_Profit_Total = loss_Profit_Total58 +
                                            loss_Profit_Total14 +
                                            loss_Profit_Total36 +
                                            loss_Profit_Total72;

                    _tradeFiveToEightCalculator.Predict();
                    _tradeOneToFourCalculator.Predict();
                    _tradeThreeToSixCalculator.Predict();
                    _tradeSevenToTwoCalculator.Predict();

                    var log = string.Format("{0},{1},{2},{3},{4},,{5},{6},{7},{8},,{9},{10},{11},{12},,{13},{14},{15},{16},{17}\r\n",
                                        _inputValue == BaccratCard.Banker ? 1 : -1, //Result --> {0}
                                                                                    //Group 1
                                        same_58, //Same 5-8 --> {1}
                                        diff_58, //Diff 5-8 --> {2}
                                        (same_58 + diff_58) / 2, //--> {3}
                                        loss_Profit_Total58, //--> {4}

                                        //Group 2
                                        same_14, //--> {5}
                                        diff_14,//--> {6}
                                        (same_14 + diff_14) / 2, //--> {7}
                                        loss_Profit_Total14,//--> {8}

                                        //Group 3
                                        same_36, //--> {9}
                                        diff_36,//--> {10}
                                        (same_36 + diff_36) / 2, //--> {11}
                                        loss_Profit_Total36,//--> {12}

                                        //Group 4
                                        same_72, //--> {13}
                                        diff_72,//--> {14}
                                        (same_72 + diff_72) / 2, //--> {15}
                                        loss_Profit_Total72,//--> {16}

                                        loss_Profit_Total); //-> 17

                    File.AppendAllText(processedFilePath, log);
                }
            }
        }
    }
}
