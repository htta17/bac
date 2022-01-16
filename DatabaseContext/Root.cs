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

        /// <summary>
        /// Số thứ tự tổng quan
        /// </summary>
        public int GlobalOrder { get; set; }
    }
}
