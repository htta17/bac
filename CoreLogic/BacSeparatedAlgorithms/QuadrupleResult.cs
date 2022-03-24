using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLogic
{
    public class BaccaratPredict
    {
        public BaccratCard Value { get; set; }
        public int Volume { get; set; }

        public override string ToString()
        {
            return $"{Value.ToString().ToUpper() } {Volume} units";
        }
    }

    public class QuadrupleCoeff 
    {
        public int Same_Coff { get; set; }
        public int Diff_Coff { get; set; }
    }

    public class QuadrupleResult : BaccaratPredict
    {
        public int Same_Coff { get; set; }
        public int Diff_Coff { get; set; }
    }
}
