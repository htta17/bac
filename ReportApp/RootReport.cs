using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportApp
{
    public class RootReport
    {
        public int ID { get; set; }
        public DateTime Time { get; set; }
        public int NoSteps { get; set; }
        public int MinRoot { get; set; }
        public int MaxRoot { get; set; }
        public int MainProfit { get; set; }
        public int AccMainProfit { get; set; }
        public int Profit0 { get; set; }
        public int AccProfit0 { get; set; }
        public int Profit1 { get; set; }
        public int AccProfit1 { get; set; }
        public int Profit2 { get; set; }
        public int AccProfit2 { get; set; }
        public int Profit3 { get; set; }
        public int AccProfit3 { get; set; }        
        
        private int? _accAllSub = null; 
        public int AccAllSub
        {
            get 
            {
                if (_accAllSub == null)
                    _accAllSub = AccProfit0 + AccProfit1 + AccProfit2 + AccProfit3;
                return _accAllSub.Value;
            }
        }
        
    }
}
