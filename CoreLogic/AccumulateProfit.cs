using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLogic
{
    [System.Runtime.CompilerServices.SpecialName]
    public class RootProfit<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// Main Thread
        /// </summary>
        public T Main { get; set; }
        public T Sub0 { get; set; }
        public T Sub1 { get; set; }
        public T Sub2 { get; set; }
        public T Sub3 { get; set; }
        public T AllSub { get; set; }

        //Cài đặt giá trị default(T). (Ví dụ: 0 cho integer hoặc float)
        public void Reset()
        {
            Main = Sub0 = Sub1 = Sub2 = Sub3 = AllSub = default;
        }
        /// <summary>
        /// Đặt giá trị cho 6 properties, thường cho tích lũy hoặc giá trị ban đầu của hệ số
        /// </summary>
        /// <param name="val">Các giá trị của biến, số lượng giá trị của mảng val là 1 hoặc 6</param>
        public void SetValue(params object[] val)
        {
            if (val.Length == 1)
                Main = Sub0 = Sub1 = Sub2 = Sub3 = AllSub = (T)val[0];
            else if (val.Length == 6)
            {
                Main = (T)val[0];
                Sub0 = (T)val[1];
                Sub1 = (T)val[2];
                Sub2 = (T)val[3];
                Sub3 = (T)val[4];
                AllSub = (T)val[5];
            }
            else
                throw new Exception($"Kiếm tra số lượng giá trị nhập vào, chỉ hấp nhận 1 hoặc 6 giá trị, không chấp nhận {val.Length}.");
        }

        public T SumAllSub()
        {
            CultureInfo cultures = new CultureInfo("en-US");
            if (typeof(T) == typeof(int))
            {
                var sum = Sub0.ToInt32(cultures) +
                    Sub1.ToInt32(cultures) +
                    Sub2.ToInt32(cultures) +
                    Sub3.ToInt32(cultures);
                return (T)(sum as object);
            }
            else if (typeof(T) == typeof(decimal))
            {
                var sum = Sub0.ToDecimal(cultures) +
                    Sub1.ToDecimal(cultures) +
                    Sub2.ToDecimal(cultures) +
                    Sub3.ToDecimal(cultures);
                return (T)(sum as object);
            }
            return default;
        }

        public T SumAll()
        {
            CultureInfo cultures = new CultureInfo("en-US");
            if (typeof(T) == typeof(int))
            {
                var sum = Main.ToInt32(cultures) +
                    Sub0.ToInt32(cultures) +
                    Sub1.ToInt32(cultures) +
                    Sub2.ToInt32(cultures) +
                    Sub3.ToInt32(cultures) +
                    AllSub.ToInt32(cultures);
                return (T)(sum as object);
            }
            else if (typeof(T) == typeof(decimal))
            {
                var sum =
                    Main.ToDecimal(cultures) +
                    Sub0.ToDecimal(cultures) +
                    Sub1.ToDecimal(cultures) +
                    Sub2.ToDecimal(cultures) +
                    Sub3.ToDecimal(cultures) +
                    AllSub.ToDecimal(cultures);
                return (T)(sum as object);
            }
            return default;
        }
    }

    public class RootUpdateModel
    {
        public int Coeff { get; set; }
        public int Profit { get; set; }
        public decimal ComissionedProfit { get; set; }
        public RootUpdateModel(int _coeff, int _profit, decimal _profitWithComission)
        {
            Coeff = _coeff;
            Profit = _profit;
            ComissionedProfit = _profitWithComission;
        }
    }

    public class RootInputUpdateModel
    {
        public int Increment { get; set; }
        public int StartNumber { get; set; }

        /// <summary>
        /// Tỉ lệ nhận được khi thắng với BANKER (Thường là 95% ~0.95) 
        /// </summary>
        public decimal Commission { get; set; }

        public RootInputUpdateModel(int _Increment, int _StartNumber, decimal _Commission)
        {
            Increment = _Increment;
            StartNumber = _StartNumber;
            Commission = _Commission;
        }
    }
}
