using System;

namespace DatabaseContext
{  
    public class Root
    {
        public int ID { get; set; }
        public short Card { get; set; }
        public DateTime InputDateTime { get; set; }
                
        public int MainCoeff { get; set; }
        public int Coeff0 { get;  set; }
        public int Coeff1 { get;  set; }
        public int Coeff2 { get;  set; }
        public int Coeff3 { get;  set; }
        public int AllSubCoeff { get;  set; }

        public int MainProfit { get; set; }
        public int Profit0 { get;  set; }
        public int Profit1 { get;  set; }
        public int Profit2 { get;  set; }
        public int Profit3 { get;  set; }
        public int AllSubProfit { get;  set; }

        public decimal Flat095Main { get; set; }
        public decimal Flat095Profit0 { get; set; }
        public decimal Flat095Profit1 { get; set; }
        public decimal Flat095Profit2 { get; set; }
        public decimal Flat095Profit3 { get; set; }
        public decimal Flat095AllSub { get; set; }

        public int ModMainCoeff { get; set; }
        public int ModCoeff0 { get; set; }
        public int ModCoeff1 { get; set; }
        public int ModCoeff2 { get; set; }
        public int ModCoeff3 { get; set; }
        public int ModAllSubCoeff { get; set; }

        public int ModMainProfit { get; set; }
        public int ModProfit0 { get; set; }
        public int ModProfit1 { get; set; }
        public int ModProfit2 { get; set; }
        public int ModProfit3 { get; set; }
        public int ModAllSubProfit { get; set; }

        public decimal Mod095Main { get; set; }
        public decimal Mod095Profit0 { get; set; }
        public decimal Mod095Profit1 { get; set; }
        public decimal Mod095Profit2 { get; set; }
        public decimal Mod095Profit3 { get; set; }
        public decimal Mod095AllSub { get; set; }


        public string ListCurrentPredicts { get; set; }

        /// <summary>
        /// Số thứ tự tổng quan
        /// </summary>
        public int GlobalOrder { get; set; }        
    }
}
