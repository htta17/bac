using System;

namespace DatabaseContext
{  
    public class Root
    {
        public int ID { get; set; }
        public short Card { get; set; }
        public DateTime InputDateTime { get; set; }

        /// <summary>
        /// Hệ số của main thread
        /// </summary>
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

        public string ListCurrentPredicts { get; set; }
        public string MinMaxAccumulate { get; set; }

        /// <summary>
        /// Số thứ tự tổng quan
        /// </summary>
        public int GlobalOrder { get; set; }

        public string LogFile { get; set; }
    }

    public class ProcessSaveInfo
    {
        public int MainAccumulate { get; private set; }
        public int Accumulate0 { get; private set; }
        public int Accumulate1 { get; private set; }
        public int Accumulate2 { get; private set; }
        public int Accumulate3 { get; private set; }
        public int AllSubAccumulate { get; private set; }
    }
}
