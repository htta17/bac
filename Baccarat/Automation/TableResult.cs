using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midas.Automation
{
    /// <summary>
    /// Kết quả của 1 bàn (bao nhiêu Player, Banker và Tie) cho 1 shoe
    /// </summary>
    public class AutomationTableResult
    {
        public int TotalBanker { get; set; }
        public int TotalPlayer { get; set; }
        public int TotalTie { get; set; }
        public string TableNumber { get; set; }
        public int Total
        {
            get { return TotalBanker + TotalPlayer + TotalTie; }
        }
        //public DateTime LastUpdatedTime { get; set; }

        public string TextResult
        {
            get
            {
                return $"B {TotalBanker} - P {TotalPlayer} - T {TotalTie} - All {Total}";
            }            
        }
    }


    public enum AutomationCardResult
    { 
        BANKER = 1, 
        PLAYER = -1, 
        TIE = 0, 
        SHOE_CHANGE_OR_CLOSE = 7,
        NO_CARD = 9
    }
    
}
