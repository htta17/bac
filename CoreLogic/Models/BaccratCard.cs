using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLogic
{
    public enum BaccratCard
    {
        Player = -1,
        NoTrade = 0,
        Banker = 1, 
        Tie = 8
    }

    public enum OriginalBaccaratCard
    { 
        Player = -1, 
        Tie = 0, 
        Banker = 1
    }
}
