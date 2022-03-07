using DatabaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLogic.StandardlizedAlgorithms
{
    /// <summary>
    /// Kết quả của 1 bàn (bao nhiêu Player, Banker và Tie) cho 1 shoe
    /// </summary>
    public class AutomationTableResult
    {
        public int TotalBanker { get; set; }
        public int TotalPlayer { get; set; }
        public int TotalTie { get; set; }
        public string TableNumber { get; set; }
        public int Total
        {
            get { return TotalBanker + TotalPlayer + TotalTie; }
        }

        public string TextResult
        {
            get
            {
                return $"[All,B,P,T]: [{Total},{TotalBanker},{TotalPlayer},{TotalTie}]";
            }
        }
    }

    public class AutoBacMaster
    {
        public AutoBacMaster(string connectionString)
        {
            GlobalDBContext = new GlobalDBContext(connectionString);
            LogicAllTables = new Dictionary<int, AutoBacRootAlgorithm>();
        }


        private GlobalDBContext GlobalDBContext { get;set;}

        private AutoBacRootAlgorithm GetTable(int _tableNo)
        {
            return LogicAllTables.ContainsKey(_tableNo)
                    ? LogicAllTables[_tableNo]
                    : default;
        }

        public bool TableIsNull(int _tableNo)
        {
            return GetTable(_tableNo) == null;
        }

        public int ResetTable(int _tableNo)
        {
            var logicTable = GetTable(_tableNo);
            if (logicTable == null)
            {
                logicTable = new AutoBacRootAlgorithm(_tableNo, GlobalDBContext);
                logicTable.Reset();
                LogicAllTables.Add(_tableNo, logicTable);
            }
            else
            {               
                logicTable.Reset();
                LogicAllTables[_tableNo] = logicTable;
            }
            return logicTable.CurrentAutoSession.ID;
        }

        public BaccaratPredict Process(int _tableNo, BaccratCard baccratCard, AutomationTableResult uiResult)
        {
            var noTradePredict = new BaccaratPredict { Volume = 0, Value = BaccratCard.NoTrade };
            var table = GetTable(_tableNo);
            if (table != null)
            {
                var newResult = new AutoResult
                {
                    Card = (short)baccratCard,
                    AutoSessionID = table.CurrentAutoSession.ID, 
                    UIResult = uiResult.TextResult
                };

                GlobalDBContext.AddAutoResult(newResult);

                if (baccratCard == BaccratCard.Banker || baccratCard == BaccratCard.Player)
                {
                    return table.Process(baccratCard, newResult);
                }
                return noTradePredict;
            }
            return noTradePredict;
        }


        /// <summary>
        /// Tất cả các bàn với thuật giải 
        /// </summary>
        private Dictionary<int, AutoBacRootAlgorithm> LogicAllTables { get; set; }
    }
}
