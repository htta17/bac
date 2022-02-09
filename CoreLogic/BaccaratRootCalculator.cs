using DatabaseContext;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CalculationLogic
{   
    /// <summary>
    /// Main algorithm.
    /// </summary>
    public class BaccaratRootCalculator
    {
        public BaccaratRootCalculator(string connectionString)
        {
            BaccaratDBContext = new GlobalDBContext(connectionString);

            Reload(true);

            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }
            
        }
        GlobalDBContext BaccaratDBContext { get; set; }

        public int GlobalOrder { get; private set; } = 0;

        public BaccratCard ShowLastCard()
        {
            if (MainRoots.Count > 0)
                return (BaccratCard)MainRoots.Last().Card;
            return BaccratCard.NoTrade;
        }

        public void Backward()
        {
            if (MainRoots.Count == 0)
                return;
            
            var lastItem = MainRoots.Last();
            var deletedProfitInfo = new List<int> 
            { 
                lastItem.MainProfit, 
                lastItem.Profit0, 
                lastItem.Profit1,
                lastItem.Profit2,
                lastItem.Profit3, 
                lastItem.AllSubProfit
            };
            BaccaratDBContext.DeleteRoot(lastItem.ID);
            MainRoots.RemoveAt(MainRoots.Count - 1);
            Reload(false, deletedProfitInfo);                        
        }

        public void Reset()
        {
            MainRoots = new List<Root>();
            
            GlobalOrder = 0;
            MainCoeff = 1;
            Coeff0 = 1;
            Coeff1 = 1;
            Coeff2 = 1;
            Coeff3 = 1;
            AllSubCoeff = 1;
            CurrentPredicts = new List<BaccaratPredict>();


            MainAccumulate =
            Accumulate0 =
            Accumulate1 =
            Accumulate2 =
            Accumulate3 =
            AllSubAccumulate = 0;

            Roots0 = new List<Root>();
            Roots1 = new List<Root>();
            Roots2 = new List<Root>();
            Roots3 = new List<Root>();

            FILENAME_DATETIME = DateTime.Now;
            WriteFirstTime = true;
        }


        public void Reload(bool ressetFileName, List<int> deletedProfitInfo = null)
        {
            var rootCount = BaccaratDBContext.Roots.Count();
            MainRoots = BaccaratDBContext.Roots
                                        .OrderBy(c => c.ID)
                                        .Skip(rootCount > 100 ? rootCount - 100 : 0)
                                        .Take(rootCount > 100 ? 100 : rootCount)
                                        .ToList();

            if (MainRoots.Count() > 0)
            {
                GlobalOrder = MainRoots.Last().GlobalOrder;
                MainCoeff = MainRoots.Last().MainCoeff;
                Coeff0 = MainRoots.Last().Coeff0;
                Coeff1 = MainRoots.Last().Coeff1;
                Coeff2 = MainRoots.Last().Coeff2;
                Coeff3 = MainRoots.Last().Coeff3;
                AllSubCoeff = MainRoots.Last().AllSubCoeff;
                CurrentPredicts = JsonConvert.DeserializeObject<List<BaccaratPredict>>(MainRoots.Last().ListCurrentPredicts);

                //Khi user nhấn nút BACK (ressetFileName = false), lấy lại các thông tin về tích lũy bằng cách
                //      trừ đi Profit cuả record đã xóa (Móa, tâm trí không tập trung phải gõ ngơ ngơ thế này) 
                //      ví dụ đi ha: 
                //          Thôi để sau
                if (ressetFileName == false && deletedProfitInfo != null)
                {
                    MainAccumulate -= deletedProfitInfo[0]; 
                    Accumulate0 -= deletedProfitInfo[1];
                    Accumulate1 -= deletedProfitInfo[2];
                    Accumulate2 -= deletedProfitInfo[3];
                    Accumulate3 -= deletedProfitInfo[4];
                    AllSubAccumulate -= deletedProfitInfo[5];
                }
            }
            else
            {
                GlobalOrder = 0;
                MainCoeff = 1;
                Coeff0 = 1;
                Coeff1 = 1;
                Coeff2 = 1;
                Coeff3 = 1;
                AllSubCoeff = 1;

                MainAccumulate =
                Accumulate0 =
                Accumulate1 =
                Accumulate2 =
                Accumulate3 =
                AllSubAccumulate = 0;

                CurrentPredicts = new List<BaccaratPredict>();
            }

            Roots0 = MainRoots.Where(c => c.GlobalOrder % 4 == 0).ToList();
            Roots1 = MainRoots.Where(c => c.GlobalOrder % 4 == 1).ToList();
            Roots2 = MainRoots.Where(c => c.GlobalOrder % 4 == 2).ToList();
            Roots3 = MainRoots.Where(c => c.GlobalOrder % 4 == 3).ToList();

            if (ressetFileName)
            {
                FILENAME_DATETIME = DateTime.Now;
                WriteFirstTime = true;
            }
        }

        private List<Root> MainRoots { get; set; }

        private List<Root> Roots0 { get; set; }
        private List<Root> Roots1 { get; set; }
        private List<Root> Roots2 { get; set; }
        private List<Root> Roots3 { get; set; }

        public int MainCoeff { get; private set; }
        public int Coeff0 { get; private set; }
        public int Coeff1 { get; private set; } 
        public int Coeff2 { get; private set; } 
        public int Coeff3 { get; private set; } 
        public int AllSubCoeff { get; private set; } 
        
        public int MainAccumulate { get; private set; }
        public int Accumulate0 { get; private set; }
        public int Accumulate1 { get; private set; }
        public int Accumulate2 { get; private set; }
        public int Accumulate3 { get; private set; }
        public int AllSubAccumulate { get; private set; }

        public List<BaccaratPredict> CurrentPredicts { get; set; }

        private bool WriteFirstTime { get; set; }

        private DateTime? FILENAME_DATETIME = null;
        private const string LogTitle = "ID,Time,Card,Main Profit,Main Accumulate,Profit0,Accumulate0,Profit1,Accumulate1,Profit2,Accumulate2,Profit3,Accumulate3,All Sub Accumulate \r\n";
        const string LOG_FILE_FORMAT = "Logs\\{0:yyyy-MM-dd}\\{0:HH.mm.ss}.csv";

        string FULL_PATH_FILE
        {
            get
            {
                return string.Format(LOG_FILE_FORMAT, FILENAME_DATETIME.Value);
            }
        }
        
        public void AddNewCard(BaccratCard card)
        {
            if (card == BaccratCard.NoTrade)
                throw new Exception("Input card must be Banker or Player");

            //Start  to add new card
            GlobalOrder++;
            var logger = "";

            //Update coeff (get the current predict and compare to card)
            if (CurrentPredicts.Count > 0)
            {
                var lastRoot = MainRoots.Last();

                var mainProfit = UpdateCurrentCoeff(card, 0, MainCoeff);
                MainCoeff = mainProfit.Item2;
                lastRoot.MainProfit = mainProfit.Item1;
                MainAccumulate += mainProfit.Item1;

                if (GlobalOrder % 4 == 0)
                {
                    var profit0 = UpdateCurrentCoeff(card, 1, Coeff0);
                    Coeff0 = profit0.Item2;
                    lastRoot.Profit0 = profit0.Item1;
                    Accumulate0 += profit0.Item1;
                }
                else if (GlobalOrder % 4 == 1)
                {
                    var profit1 = UpdateCurrentCoeff(card, 2, Coeff1);
                    Coeff1 = profit1.Item2;
                    lastRoot.Profit1 = profit1.Item1;
                    Accumulate1 += profit1.Item1;
                }
                else if (GlobalOrder % 4 == 2)
                {
                    var profit2 = UpdateCurrentCoeff(card, 3, Coeff2);
                    Coeff2 = profit2.Item2;
                    lastRoot.Profit2 = profit2.Item1;
                    Accumulate2 += profit2.Item1;
                }
                else if (GlobalOrder % 4 == 3)
                {
                    var profit3 = UpdateCurrentCoeff(card, 4, Coeff3);
                    Coeff3 = profit3.Item2;
                    lastRoot.Profit3 = profit3.Item1;
                    Accumulate3 += profit3.Item1;
                }

                //Tính toán cho AllSubCoeff
                var allSubProfit = UpdateCurrentCoeff(card, 5, AllSubCoeff);
                AllSubCoeff = allSubProfit.Item2;
                lastRoot.AllSubProfit = allSubProfit.Item1;

                //Cập nhật lại DB
                BaccaratDBContext.UpdateRoot(lastRoot);

                //Ghi vào log
                AllSubAccumulate += lastRoot.Profit0 + lastRoot.Profit1 + lastRoot.Profit2 + lastRoot.Profit3;

                logger = $"{GlobalOrder},{DateTime.Now},{card},{lastRoot.MainProfit},{MainAccumulate},{lastRoot.Profit0},{Accumulate0}," +
                                $"{lastRoot.Profit1},{Accumulate1},{lastRoot.Profit2},{Accumulate2},{lastRoot.Profit3},{Accumulate3},{AllSubAccumulate} \r\n";

            }
            else
            {
                logger = $"{GlobalOrder},{DateTime.Now},{card},0,0,0,0,0,0,0,0,0,0,0 \r\n";                
            }

            //Ghi log file
            if (WriteFirstTime)
            {
                if (!Directory.Exists(string.Format("Logs\\{0:yyyy-MM-dd}", DateTime.Now)))
                {
                    Directory.CreateDirectory(string.Format("Logs\\{0:yyyy-MM-dd}", DateTime.Now));
                }
                File.AppendAllText(FULL_PATH_FILE, LogTitle);
                WriteFirstTime = false;
            }
            try
            {
                File.AppendAllText(FULL_PATH_FILE, logger);
            }
            catch
            {
            }

            var datetimeNow = DateTime.Now;
            var newRoot = new Root
            {
                Card = (short)card,
                InputDateTime = datetimeNow,                
                
                MainCoeff = MainCoeff,
                Coeff0 = Coeff0,
                Coeff1 = Coeff1,
                Coeff2 = Coeff2,
                Coeff3 = Coeff3,
                AllSubCoeff = AllSubCoeff,

                MainProfit = 0,
                Profit0 = 0,
                Profit1 = 0,
                Profit2 = 0,
                Profit3 = 0,

                GlobalOrder = GlobalOrder, 
                ListCurrentPredicts = ""
            };            

            //Add for main root
            MainRoots.Add(newRoot);

            //Add for sub roots
            if (GlobalOrder % 4 == 0)
                Roots0.Add(newRoot);
            else if (GlobalOrder % 4 == 1)
                Roots1.Add(newRoot);
            else if (GlobalOrder % 4 == 2)
                Roots2.Add(newRoot); 
            else if (GlobalOrder % 4 == 3)
                Roots3.Add(newRoot);

            BaccaratDBContext.AddRoot(newRoot);
        }

        /// <summary>
        /// Update Coeff and return Profit
        /// </summary>
        /// <param name="card">Thẻ vừa bốc</param>
        /// <param name="index">Thứ tự trong CurrentPredicts, bắt đầu từ 0</param>
        /// <param name="Coeff">Hệ số cần cập nhật</param>
        /// <returns></returns>
        private Tuple<int,int> UpdateCurrentCoeff(BaccratCard card, int index, int Coeff)
        {
            var threadPredict0 = CurrentPredicts[index];
            var profit = 0;
            if (threadPredict0.Value != BaccratCard.NoTrade)
            {
                if (threadPredict0.Value != card)
                {
                    profit = -Coeff; //Âm tiền
                    //Coeff = Coeff + 2; //Tăng hệ số
                }
                else
                {
                    profit = Coeff; // Lụm tiền
                    //if (Coeff >= 3) //Giảm hệ số hoặc giữ nguyên
                    //    Coeff = Coeff - 2;
                }
            }
            return Tuple.Create<int,int>( profit, Coeff) ; 
        }

        private BaccaratPredict PredictNextCard(List<BaccratCard> cards, int volume)
        {
            if (cards.Count < 3)
                return new BaccaratPredict { Value = BaccratCard.NoTrade, Volume = 0 };

            if (cards[0] != cards[1] && cards[1] == cards[2]) //Trigger to predict
            {
                return new BaccaratPredict
                {
                    Value = cards[2] == BaccratCard.Banker ? BaccratCard.Player : BaccratCard.Banker,
                    Volume = 1//ToDo: Remove volume
                };
            }
            return new BaccaratPredict { Value = BaccratCard.NoTrade, Volume = 0 };
        }

        public BaccaratPredict Predict()
        {
            var mainThread = PredicThread(MainRoots, MainCoeff);           

            var thread0 = PredicThread(Roots0, Coeff0);
            var thread1 = PredicThread(Roots1, Coeff1);
            var thread2 = PredicThread(Roots2, Coeff2);
            var thread3 = PredicThread(Roots3, Coeff3);

            var _predictGlobalOrder = GlobalOrder + 1;
            var subThread = _predictGlobalOrder % 4 == 0 ? thread0 :
                                _predictGlobalOrder % 4 == 1 ? thread1 :
                                _predictGlobalOrder % 4 == 2 ? thread2 :
                                thread3;

            var volume = (int)mainThread.Value * mainThread.Volume  //Main thread                            
                           + (int)subThread.Value * (subThread.Volume + AllSubCoeff) ;   //Dùng cho trường hợp hệ số từng thread đi riêng rẽ

            var result = new BaccaratPredict
            {
                Value = volume > 0 ? BaccratCard.Banker : volume == 0 ? BaccratCard.NoTrade : BaccratCard.Player,
                Volume = Math.Abs(volume)
            };

            CurrentPredicts.Clear();
            CurrentPredicts.AddRange(new List<BaccaratPredict>
            {
                mainThread,
                thread0,
                thread1,
                thread2,
                thread3,
                subThread //All subs thread
            });

            //Update Root 
            if (MainRoots.Count > 0)
            {
                var lastRoot = MainRoots.Last();
                lastRoot.ListCurrentPredicts = JsonConvert.SerializeObject(CurrentPredicts);
                BaccaratDBContext.UpdateRoot(lastRoot);
            }

            return result;
        }

        private BaccaratPredict PredicThread(List<Root> roots, int volume)
        {
            var skip = roots.Count > 3 ? roots.Count - 3 : 0;
            var take = roots.Count > 3 ? 3 : roots.Count;
            var mainThreadCards = roots.Skip(skip).Take(take)
                                        .Select(c => (BaccratCard)c.Card)
                                        .ToList();
            return PredictNextCard(mainThreadCards, volume);
        }

        /// <summary>
        /// Show [N] last steps
        /// </summary>
        /// <param name="N">Number of steps you want to see</param>
        /// <returns></returns>
        public List<BaccaratPredict> RootsHistory(int N = 50)
        { 
            var rootCount = BaccaratDBContext.Roots.Count();
            var historyRoots = BaccaratDBContext.Roots
                                        .OrderBy(c => c.ID)
                                        .Skip(rootCount > N ? rootCount - N : 0)
                                        .Take(rootCount > N ? N : rootCount)
                                        .ToList();
            var baccaratPredicts = new List<BaccaratPredict>();            
            for (int i = 0; i < historyRoots.Count; i++)
            {
                if (i == 0)
                {
                    baccaratPredicts.Add(new BaccaratPredict
                    {
                        Volume = 1,
                        Value = (BaccratCard)historyRoots[i].Card
                    });
                }
                else
                {
                    if (historyRoots[i].Card == historyRoots[i - 1].Card)
                    {
                        baccaratPredicts[baccaratPredicts.Count - 1].Volume += 1;
                    }
                    else
                    {
                        baccaratPredicts.Add(new BaccaratPredict
                        {
                            Volume = 1,
                            Value = (BaccratCard)historyRoots[i].Card
                        });
                    }                
                }
            }
            return baccaratPredicts;
        }

        public string EndSessionReport()
        {
            var text = $"Tổng kết lúc { DateTime.Now: yyyy-MM-dd HH:mm:ss} tại máy {Environment.MachineName}:\r\n" +
                        $"Phiên chính: `{MainAccumulate}` units.\r\n" +
                        $"Các phiên con (1-4): `{Accumulate0}`, `{Accumulate1}`, `{Accumulate2}`, `{Accumulate3}` units.\r\n" +
                        $"Phiên tổng hợp của 4 phiên con: `{AllSubAccumulate}` units.\r\n" +
                        $"TỔNG KẾT: `{MainAccumulate + Accumulate0 + Accumulate1 + Accumulate2 + Accumulate3 + AllSubAccumulate}` units.\r\n";
                
            return text;
        }
    }
}
