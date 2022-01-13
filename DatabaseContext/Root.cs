using System;

namespace DatabaseContext
{  
    public class Root
    {
        public int ID { get; set; }
        public short Card { get; set; }
        public DateTime InputDateTime { get; set; }
        public int Coeff { get; set; }

        public int GlobalOrder { get; set; }
    }
}
