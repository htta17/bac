using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLogic
{
    public class BaccaratResult
    {
        public int Volume { get; set; }
        public int Value { get; set; }

        
    }

    public class Constants
    {
        public const string BANKER_VALUE = "BANKER";
        public const string PLAYER_VALUE = "PLAYER";
    }
}
