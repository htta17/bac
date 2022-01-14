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

        /// <summary>
        /// Hệ số của sub thead
        /// </summary>
        public int SubCoeff { get; set; }

        /// <summary>
        /// Hệ số của main + sub, đi riêng rẽ 
        /// </summary>
        public int GlobalCoeff { get; set; }

        /// <summary>
        /// Số thứ tự tổng quan
        /// </summary>
        public int GlobalOrder { get; set; }
    }
}
