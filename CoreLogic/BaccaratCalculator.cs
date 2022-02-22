using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreLogic
{
    

    /// <summary>
    /// This class is built to get fast results, in case it takes too long time to process 
    /// </summary>
    public class FastResult
    { 
        public int Total { get; set; }
        public int NumberOfZero { get; set; }
        public int NumberOfOne { get; set; }

        public int Volume { get; set; }
        public int Value { get; set; }
    }  

    
    public class BaccaratCalculator
    {
        public BaccaratCalculator()
        {
            FastResult = new List<FastResult>();
            FastResult.Add(new FastResult { Total = 10, NumberOfOne = 0, NumberOfZero = 10, Value = 0, Volume = 1023 });
            FastResult.Add(new FastResult { Total = 10, NumberOfOne = 1, NumberOfZero = 9, Value = 0, Volume = 1012 });
            FastResult.Add(new FastResult { Total = 10, NumberOfOne = 2, NumberOfZero = 8, Value = 0, Volume = 957 });
            FastResult.Add(new FastResult { Total = 10, NumberOfOne = 3, NumberOfZero = 7, Value = 0, Volume = 792 });
            FastResult.Add(new FastResult { Total = 10, NumberOfOne = 4, NumberOfZero = 6, Value = 0, Volume = 462 });
            FastResult.Add(new FastResult { Total = 10, NumberOfOne = 5, NumberOfZero = 5, Value = -1, Volume = 0 });
            FastResult.Add(new FastResult { Total = 10, NumberOfOne = 6, NumberOfZero = 4, Value = 1, Volume = 462 });
            FastResult.Add(new FastResult { Total = 10, NumberOfOne = 7, NumberOfZero = 3, Value = 1, Volume = 792 });
            FastResult.Add(new FastResult { Total = 10, NumberOfOne = 8, NumberOfZero = 2, Value = 1, Volume = 957 });
            FastResult.Add(new FastResult { Total = 10, NumberOfOne = 9, NumberOfZero = 1, Value = 1, Volume = 1012 });
            FastResult.Add(new FastResult { Total = 10, NumberOfOne = 10, NumberOfZero = 0, Value = 1, Volume = 1023 });
        }
        void CombinationUtil(int[] arr, int[] data, int start, int end, int index, int r)
        {
           
            if (index == r)
            {
                var list = new List<int>();
                var count0 = 0;
                for (int j = 0; j < r; j++)
                {
                    list.Add(data[j]);
                    if (data[j] == 0)
                    {
                        count0++; 
                    }
                }
                var count1 = r - count0;

                FinalList.Add(new CalculatorItem
                {
                    DetailArray = list,
                    Value = count0 == count1 ? -1
                            : count0 > count1 ? 0
                            : 1
                }); ;
                return;
            }
           
            for (int i = start; i <= end &&
                      end - i + 1 >= r - index; i++)
            {
                data[index] = arr[i];
                CombinationUtil(arr, data, i + 1,
                                end, index + 1, r);
            }
        }

         void GetCombination(int[] arr, int n, int r)
        {           
            int[] data = new int[r];
           
            CombinationUtil(arr, data, 0,
                            n - 1, 0, r);
        }

        public List<CalculatorItem> FinalList = new List<CalculatorItem>();

        public List<FastResult> FastResult { get; set; }
        public BaccaratResult Calculate(int[] inputs)
        {
            var count0 = inputs.Where(c => c == 0).Count();
            var count1 = inputs.Length - count0;
            var arrLeng = inputs.Length;

            FinalList.Clear();

            for (var i = 1; i <= arrLeng; i++)
            {
                GetCombination(inputs, arrLeng, i);
            }

            var countValue0 = FinalList.Where(c => c.Value == 0).Count();
            var countValue1 = FinalList.Where(c => c.Value == 1).Count();
            var volume = Math.Abs(countValue0 - countValue1);


            return new BaccaratResult 
            { 
                Value = volume == 0 ? -1 : 
                            countValue0 > countValue1 ? 1 : 0,
                Volume = volume 
            };
        }
    }

    public enum UIValidationEnum
    { 
        WrongValue, 
        HasEmptyInMiddle,
        Success, 
        NeedFirstValue
    }
}
