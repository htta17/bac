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
            ConnectionString = connectionString;
            LogicAllTables = new Dictionary<int, AutoBacRootAlgorithm>();
        }

        string ConnectionString { get; set; }        

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
                logicTable = new AutoBacRootAlgorithm(_tableNo, ConnectionString); //Đã bao gồm reset
                LogicAllTables.Add(_tableNo, logicTable);
            }
            else
            {               
                logicTable.Reset();
            }
            return logicTable.CurrentAutoSessionID;
        }

        Dictionary<int, BaccaratPredict> LastPredicts = new Dictionary<int, BaccaratPredict>();
        public BaccaratPredict Process(int _tableNo, BaccratCard baccratCard, AutomationTableResult uiResult)
        {
            var noTradePredict = new BaccaratPredict { Volume = 0, Value = BaccratCard.NoTrade };
            var table = GetTable(_tableNo);
            if (table != null)
            {
                if (uiResult.Total == 0) //Mới tạo phiên, lấy kết quả dự đoán của bước cuối phiên cũ
                {
                    return LastPredicts.ContainsKey(_tableNo) ? LastPredicts[_tableNo] : noTradePredict;
                }
                var newResult = default(AutoResult);
                using (GlobalDBContext context = new GlobalDBContext(ConnectionString))
                {
                    newResult = new AutoResult
                    {
                        Card = (short)baccratCard,
                        AutoSessionID = table.CurrentAutoSessionID,
                        UIResult = uiResult.TextResult
                    };

                    context.AddAutoResult(newResult);
                }

                if (baccratCard == BaccratCard.Banker || baccratCard == BaccratCard.Player)
                {
                    var prd = table.Process(baccratCard, newResult);
                    if (LastPredicts.ContainsKey(_tableNo))
                    {
                        LastPredicts[_tableNo] = prd;
                    }
                    else
                    {
                        LastPredicts.Add(_tableNo, prd);
                    }
                    return prd;
                }
                else if (baccratCard == BaccratCard.Tie)
                {
                    return LastPredicts.ContainsKey(_tableNo) ? LastPredicts[_tableNo] : noTradePredict;
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
