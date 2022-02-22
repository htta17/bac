using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLogic.VietnameseLottery
{
    public class VL_Digit
    {
        public VL_Digit(int currentNumber)
        {            
            Current = currentNumber;
        }
        
        public int Current { get; set; }

        #region Step 1: Filter
        public EO_Filter_Type Filter_Type04
        {
            get 
            {
                return Current >= 0 && Current <= 4 ? EO_Filter_Type.IN : EO_Filter_Type.OUT;
            }
        }
        public EO_Filter_Type Filter_Type15
        {
            get
            {
                return Current >= 1 && Current <= 5 ? EO_Filter_Type.IN : EO_Filter_Type.OUT;
            }
        }
        public EO_Filter_Type Filter_Type26
        {
            get
            {
                return Current >= 2 && Current <= 6 ? EO_Filter_Type.IN : EO_Filter_Type.OUT;
            }
        }
        public EO_Filter_Type Filter_Type37
        {
            get
            {
                return Current >= 3 && Current <= 7 ? EO_Filter_Type.IN : EO_Filter_Type.OUT;
            }
        }
        public EO_Filter_Type Filter_Type48
        {
            get
            {
                return Current >= 4 && Current <= 8 ? EO_Filter_Type.IN : EO_Filter_Type.OUT;
            }
        }
        #endregion

    }

    public enum EO_Filter_Type
    {
        IN = 0,
        OUT = 1
    }

    public class VL_Digit_Return
    {
        public int Ret0 { get; set; } = 0;
        public int Ret1 { get; set; } = 0;
        public int Ret2 { get; set; } = 0;
        public int Ret3 { get; set; } = 0;
        public int Ret4 { get; set; } = 0;
        public int Ret5 { get; set; } = 0;
        public int Ret6 { get; set; } = 0;
        public int Ret7{ get; set; } = 0;
        public int Ret8 { get; set; } = 0;
        public int Ret9 { get; set; } = 0;
    }

    public class VL_Digit_Predict
    {
        /// <summary>
        /// Initialize an prediction
        /// </summary>
        /// <param name="firstDigit"></param>
        /// <param name="secondDigit"></param>
        /// <param name="thirdDigit"></param>
        /// <param name="forthDigit"></param>
        /// <param name="lastDigit"></param>
        public VL_Digit_Predict(int firstDigit, int secondDigit, int thirdDigit, int forthDigit)
        {
            
            First = new VL_Digit(firstDigit);
            Second = new VL_Digit(secondDigit);
            Third = new VL_Digit(thirdDigit);
            Fourth = new VL_Digit(forthDigit);
        }
       

        private VL_Digit Fourth { get; set; }
        private VL_Digit First { get; set; }
        private VL_Digit Second { get; set; }
        private VL_Digit Third { get; set; } 
        
        public int Filter_Predict_Type04
        {
            get
            {
                var count = (First.Filter_Type04 == EO_Filter_Type.IN ? 1 : 0)
                        + (Second.Filter_Type04 == EO_Filter_Type.IN ? 1 : 0)
                        + (Third.Filter_Type04 == EO_Filter_Type.IN ? 1 : 0)
                        + (Fourth.Filter_Type04 == EO_Filter_Type.IN ? 1 : 0);
                return count == 2 ? 1 : -1;
            }
        }
        
        public int Filter_Predict_Type15
        {
            get
            {
                var count = (First.Filter_Type15 == EO_Filter_Type.IN ? 1 : 0)
                        + (Second.Filter_Type15 == EO_Filter_Type.IN ? 1 : 0)
                        + (Third.Filter_Type15 == EO_Filter_Type.IN ? 1 : 0)
                        + (Fourth.Filter_Type15 == EO_Filter_Type.IN ? 1 : 0);
                return count == 2 ? 1 : -1;
            }
        }
        
        public int Filter_Predict_Type26
        {
            get
            {
                var count = (First.Filter_Type26 == EO_Filter_Type.IN ? 1 : 0)
                            + (Second.Filter_Type26 == EO_Filter_Type.IN ? 1 : 0)
                            + (Third.Filter_Type26 == EO_Filter_Type.IN ? 1 : 0)
                            + (Fourth.Filter_Type26 == EO_Filter_Type.IN ? 1 : 0);
                return count == 2 ? 1 : -1;
            }
        }
        
        public int Filter_Predict_Type37
        {
            get
            {
                var count = (First.Filter_Type37 == EO_Filter_Type.IN ? 1 : 0)
                        + (Second.Filter_Type37 == EO_Filter_Type.IN ? 1 : 0)
                        + (Third.Filter_Type37 == EO_Filter_Type.IN ? 1 : 0)
                        + (Fourth.Filter_Type37 == EO_Filter_Type.IN ? 1 : 0);
                return count == 2 ? 1 : -1;
            }
        }
        
        public int Filter_Predict_Type48
        {
            get
            {
                var count = (First.Filter_Type48 == EO_Filter_Type.IN ? 1 : 0)
                            + (Second.Filter_Type48 == EO_Filter_Type.IN ? 1 : 0)
                            + (Third.Filter_Type48 == EO_Filter_Type.IN ? 1 : 0)
                             + (Fourth.Filter_Type48 == EO_Filter_Type.IN ? 1 : 0);
                return count == 2 ? 1 : -1;
            }
        }

        public EO_Filter_Type NextRound04
        {
            get
            {
                //Count IN
                var count_IN = (Second.Filter_Type04 == EO_Filter_Type.IN ? 1 : 0)
                        + (Third.Filter_Type04 == EO_Filter_Type.IN ? 1 : 0)
                        + (Fourth.Filter_Type04 == EO_Filter_Type.IN ? 1 : 0);
                
               
                //if (Filter_Predict_Type04 == 1)//Current = 1 (balance), predict 1
                //{
                //    if (count_IN >= 2) // Nhiều IN, dự đoán OUT
                //    {
                //        return EO_Filter_Type.OUT;
                //    }
                //    else
                //    {
                //        return EO_Filter_Type.IN;
                //    }
                //}
                //else  //Current -1, predict -1
                //{
                //    if (count_IN >= 2) // Nhiều IN, dự đoán IN
                //    {
                //        return EO_Filter_Type.IN;
                //    }
                //    else
                //    {
                //        return EO_Filter_Type.OUT;
                //    }
                //}
                if ((count_IN >= 2 && Filter_Predict_Type04 == -1) 
                    || (count_IN <= 1 && Filter_Predict_Type04 == 1)) 
                {
                    return EO_Filter_Type.IN;
                }
                else
                {
                    return EO_Filter_Type.OUT;
                }
            }
        }

        public EO_Filter_Type NextRound15
        {
            get
            {
                //Count E
                var count_IN = (Second.Filter_Type15 == EO_Filter_Type.IN ? 1 : 0)
                        + (Third.Filter_Type15 == EO_Filter_Type.IN ? 1 : 0)
                        + (Fourth.Filter_Type15 == EO_Filter_Type.IN ? 1 : 0);
                if ((count_IN >= 2 && Filter_Predict_Type15 == -1)
                    || (count_IN <= 1 && Filter_Predict_Type15 == 1)) 
                {
                    return EO_Filter_Type.IN;
                }
                else
                {
                    return EO_Filter_Type.OUT;
                }
            }
        }

        public EO_Filter_Type NextRound26
        {
            get
            {
                //Count E
                var count_IN = (Second.Filter_Type26 == EO_Filter_Type.IN ? 1 : 0)
                        + (Third.Filter_Type26 == EO_Filter_Type.IN ? 1 : 0)
                        + (Fourth.Filter_Type26 == EO_Filter_Type.IN ? 1 : 0);
                if ((count_IN >= 2 && Filter_Predict_Type26 == -1)
                    || (count_IN <= 1 && Filter_Predict_Type26 == 1)) //Current state: Balance, want not balance
                {
                    return EO_Filter_Type.IN;
                }
                else
                {
                    return EO_Filter_Type.OUT;
                }
            }
        }

        public EO_Filter_Type NextRound37
        {
            get
            {
                //Count E
                var count_IN = (Second.Filter_Type37 == EO_Filter_Type.IN ? 1 : 0)
                        + (Third.Filter_Type37 == EO_Filter_Type.IN ? 1 : 0)
                        + (Fourth.Filter_Type37 == EO_Filter_Type.IN ? 1 : 0);
                if ((count_IN >= 2 && Filter_Predict_Type37 == -1)
                    || (count_IN <= 1 && Filter_Predict_Type37 == 1)) //Current state: Balance, want not balance
                {
                    return EO_Filter_Type.IN;
                }
                else
                {
                    return EO_Filter_Type.OUT;
                }
            }
        }

        public EO_Filter_Type NextRound48
        {
            get
            {
                //Count E
                var count_IN = (Second.Filter_Type48 == EO_Filter_Type.IN ? 1 : 0)
                        + (Third.Filter_Type48 == EO_Filter_Type.IN ? 1 : 0)
                        + (Fourth.Filter_Type48 == EO_Filter_Type.IN ? 1 : 0);
                if ((count_IN >= 2 && Filter_Predict_Type48 == -1)
                    || (count_IN <= 1 && Filter_Predict_Type48 == 1)) 
                {
                    return EO_Filter_Type.IN;
                }
                else
                {
                    return EO_Filter_Type.OUT;
                }
            }
        }


        private VL_Digit_Return _range = null;
        /// <summary>
        /// Based on E04_First_Win to E48_First_Win
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>        
        public VL_Digit_Return Range
        {
            get
            {
                if (_range == null)
                {
                    _range = new VL_Digit_Return();
                    if (NextRound04 == EO_Filter_Type.OUT) 
                    {
                        _range.Ret5 += 1;
                        _range.Ret6 += 1;
                        _range.Ret7 += 1;
                        _range.Ret8 += 1;
                        _range.Ret9 += 1;
                    }
                    else
                    {
                        _range.Ret0 += 1;
                        _range.Ret1 += 1;
                        _range.Ret2 += 1;
                        _range.Ret3 += 1;
                        _range.Ret4 += 1;
                    }

                    if (NextRound15 == EO_Filter_Type.OUT)
                    {
                        _range.Ret6 += 1;
                        _range.Ret7 += 1;
                        _range.Ret8 += 1;
                        _range.Ret9 += 1;
                        _range.Ret0 += 1;
                    }
                    else
                    {
                        _range.Ret1 += 1;
                        _range.Ret2 += 1;
                        _range.Ret3 += 1;
                        _range.Ret4 += 1;
                        _range.Ret5 += 1;
                    }

                    if (NextRound26 == EO_Filter_Type.OUT) 
                    {
                        _range.Ret7 += 1;
                        _range.Ret8 += 1;
                        _range.Ret9 += 1;
                        _range.Ret0 += 1;
                        _range.Ret1 += 1;
                    }
                    else
                    {
                        _range.Ret2 += 1;
                        _range.Ret3 += 1;
                        _range.Ret4 += 1;
                        _range.Ret5 += 1;
                        _range.Ret6 += 1;
                    }

                    if (NextRound37 == EO_Filter_Type.OUT) 
                    {
                        _range.Ret8 += 1;
                        _range.Ret9 += 1;
                        _range.Ret0 += 1;
                        _range.Ret1 += 1;
                        _range.Ret2 += 1;
                    }
                    else
                    {
                        _range.Ret3 += 1;
                        _range.Ret4 += 1;
                        _range.Ret5 += 1;
                        _range.Ret6 += 1;
                        _range.Ret7 += 1;
                    }

                    if (NextRound48 == EO_Filter_Type.OUT) 
                    {
                        _range.Ret9 += 1;
                        _range.Ret0 += 1;
                        _range.Ret1 += 1;
                        _range.Ret2 += 1;
                        _range.Ret3 += 1;
                    }
                    else
                    {
                        _range.Ret4 += 1;
                        _range.Ret5 += 1;
                        _range.Ret6 += 1;
                        _range.Ret7 += 1;
                        _range.Ret8 += 1;
                    }
                }
                return _range; 
            }
        }

    }
}
