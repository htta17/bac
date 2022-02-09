using CalculationLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLogic
{
    public class QuadrupleResult_HistoryInfo
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
