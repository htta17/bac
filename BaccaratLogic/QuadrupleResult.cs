using System;
using System.Collections.Generic;
using System.Text;

namespace CalculationLogic
{
    public class QuadruplePredict
    {
        public BaccratCard Value { get; set; }
        public int Volume { get; set; }
    }

    public class QuadrupleCoeff 
    {
        public int Same_Coff { get; set; }
        public int Diff_Coff { get; set; }
    }

    public class QuadrupleResult
    {
        public BaccratCard Value { get; set; }
        public int Volume { get; set; }

        public int Same_Coff { get; set; }
        public int Diff_Coff { get; set; }
    }
}
