using DatabaseContext;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLogic.StandardlizedAlgorithms
{
    public class AutoBacRootInputCoeff : IAutoInputCoeff
    { 
        public BaccratCard NewCard { get; set; }
        public BaccaratPredict CurrentPredict { get; set; }
        public int CurrentCoeff { get; set; }
        public RootInputUpdateModel RootUpdateModel { get; set; }

    }

    public class AutoBacRootOutputCoeff : IAutoOutputCoeff
    {
        public int Coeff { get; set; }

        /// <summary>
        /// Lỗ, lãi không commission
        /// </summary>
        public int TheoricalProfit { get; set; }

        /// <summary>
        /// Lỗ, lãi thực tế (Đã bị trừ commission)
        /// </summary>
        public decimal RealProfit { get; set; }

        public bool IsBankerWin { get; set; }
    }

    /// <summary>
    /// Tham khảo thêm ở BaccaratRootCalculator. 
    /// Các bước chính
    /// </summary>
    public class AutoBacRootAlgorithm : IAutoBacAlgorithm<AutoBacRootInputCoeff, AutoBacRootOutputCoeff>
    {
        #region Properties
        GlobalDBContext BaccaratDBContext { get; set; }

       
        private List<AutoRoot> MainAutoRoots { get; set; }
       

        /// <summary>
        /// Shoe đang chơi
        /// </summary>
        private AutoSession CurrentAutoSession { get; set; }

        /// <summary>
        /// Số bàn
        /// </summary>
        private int TableNumber { get; set; }
        const int FlatCoeff = 1;
       
        #endregion

        public AutoBacRootAlgorithm(string connectionString, int tableNumber)
        {
            BaccaratDBContext = new GlobalDBContext(connectionString);
            TableNumber = tableNumber;

            Initial(TableNumber);
        }
        public void AddNewCard(BaccratCard baccratCard, AutoResult autoResult = null)
        {
            //Giải thuật này chỉ dùng BANKER hoặc PLAYER
            if (baccratCard == BaccratCard.NoTrade)
                throw new Exception("Input card must be Banker or Player");

            // Tìm AutoRoot cuối
            var lastAutoRoot = MainAutoRoots.LastOrDefault();
            var globalIndex = lastAutoRoot == null ? 0 : lastAutoRoot.GlobalIndex;

            //Tăng globalIndex
            globalIndex++;

            if (autoResult == null)
            {
                autoResult = new AutoResult
                {
                    AutoSessionID = CurrentAutoSession.ID,
                    Card = (short)baccratCard
                };
                BaccaratDBContext.AddAutoResult(autoResult);
            }

            var newAutoRoot = new AutoRoot
            {
                Card = (short)baccratCard,
                ListCurrentPredicts = string.Empty,
                ID = autoResult.ID,
                AutoSessionID = CurrentAutoSession.ID,
                GlobalIndex = globalIndex
            };
            BaccaratDBContext.AddAutoRoot(newAutoRoot);
            
            MainAutoRoots.Add(newAutoRoot);            
        }

        /// <summary>
        /// Làm việc trên 1 thread duy nhất
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="volume"></param>
        /// <returns></returns>
        private BaccaratPredict PredictSingleThread(List<AutoRoot> autoRoots, int volume)
        {
            //Lấy 3 card cuối
            var skip = autoRoots.Count > 3 ? autoRoots.Count - 3 : 0;
            var take = autoRoots.Count > 3 ? 3 : autoRoots.Count;
            var cards = autoRoots.Skip(skip).Take(take)
                                        .Select(c => (BaccratCard)c.Card)
                                        .ToList();
            //Nếu không đủ 3 card thì thoát
            if (cards.Count < 3)
                return new BaccaratPredict { Value = BaccratCard.NoTrade, Volume = 0 };

            //Nếu đủ 3 card
            if (cards[0] != cards[1] && cards[1] == cards[2])
            {
                return new BaccaratPredict
                {
                    Value = cards[2] == BaccratCard.Banker ? BaccratCard.Player : BaccratCard.Banker,
                    Volume = volume
                };
            }
            return new BaccaratPredict { Value = BaccratCard.NoTrade, Volume = 0 };
        }

        public BaccaratPredict Predict()
        {
            var predictMainThread = PredictSingleThread(MainAutoRoots, FlatCoeff);            

            #region Dự đoán phiên tổng hợp của các phiên con
            var lastAutoRoot = MainAutoRoots.LastOrDefault();

            //Đã có N kết quả, vậy dự đoán cho bước N + 1
            //Vậy sẽ dùng các điểm: (N + 1) - 4 , (N + 1)- 8 và (N + 1) - 12
            //Giả sử N = 15 --> N + 1 = 16, như vậy sẽ dùng 3 điểm 12, 8, và 4            
            
            //Tìm N
            var lastGlobalIndex = lastAutoRoot == null ? 0 : lastAutoRoot.GlobalIndex;
            
            //Tăng globalIndex lên 1 cho điểm dự đoán (N + 1)
            var predictGlobalIndex = lastGlobalIndex + 1;

            var subThreadIndexes = new List<int> { predictGlobalIndex - 4, predictGlobalIndex - 8, predictGlobalIndex - 12 };
            var SubAutoRoots = MainAutoRoots.Where(c => subThreadIndexes.Contains(c.GlobalIndex))
                                            .OrderBy(c => c.GlobalIndex)
                                            .ToList();
            var predictSubThread = PredictSingleThread(SubAutoRoots, FlatCoeff);             
            #endregion

            //Cập nhật database cho dự đoán
            var listCurrentPredicts = new List<BaccaratPredict>
            {
                predictMainThread,
                predictSubThread
            };

            if (lastAutoRoot != null)
            {
                lastAutoRoot.ListCurrentPredicts = JsonConvert.SerializeObject(listCurrentPredicts);
                BaccaratDBContext.UpdateAutoRoot(lastAutoRoot);
            }

            var flatVolume = (int)predictMainThread.Value * predictMainThread.Volume  //Main thread                            
                           + (int)predictSubThread.Value * (predictSubThread.Volume + FlatCoeff);

            var flatResult = new BaccaratPredict
            {
                Value = flatVolume > 0 ? BaccratCard.Banker : flatVolume == 0 ? BaccratCard.NoTrade : BaccratCard.Player,
                Volume = Math.Abs(flatVolume)
            };

            return flatResult;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
        

        public void Initial(int tableNumber)
        {
            CurrentAutoSession = BaccaratDBContext
                                        .FindAllAutoSessions(tableNumber)
                                        .Where(c => !c.IsClosed)
                                        .OrderByDescending(c => c.ID)
                                        .FirstOrDefault();
            if (CurrentAutoSession == null)
            {
                CurrentAutoSession = CreateNewSession();
            }
            //Lấy 100 bước gần nhất và gán cho các thread (Bao gồm thread chính, 4 thread con)
            MainAutoRoots = BaccaratDBContext.FindLastNAutoRoots(100).ToList();
            
        }        

        public bool InProcessingSession()
        {
            throw new NotImplementedException();
        }        

        public AutoSession CreateNewSession()
        {
            var newAutoSession = new AutoSession { TableNumber = TableNumber};
            BaccaratDBContext.AddAutoSession(newAutoSession);
            return newAutoSession;
        }

        public BaccaratPredict Process(BaccratCard baccratCard)
        {
            TakeProfit(baccratCard);

            AddNewCard(baccratCard);

            return Predict();
        }

        public AutoBacRootOutputCoeff UpdateCoeff(AutoBacRootInputCoeff standardCoeff)
        {
            var rawProfit = 0;   //Profit 
            decimal commissionedProfit = 0m; //Profit đã bị trừ comission
            var currentPredictValue = standardCoeff.CurrentPredict.Value;
            var card = standardCoeff.NewCard;
            var coeff = standardCoeff.CurrentCoeff;
            var isBankerWin = false;

            if (standardCoeff.CurrentPredict.Value != BaccratCard.NoTrade)
            {
                //Dự đoán sai (THUA)
                if (currentPredictValue != card)
                {
                    rawProfit = -standardCoeff.CurrentCoeff;
                    commissionedProfit = -standardCoeff.CurrentCoeff;
                    coeff += standardCoeff.RootUpdateModel.Increment; //Tăng hệ số
                }
                else //Dự đoán đúng (THẮNG)
                {
                    rawProfit = coeff;
                    commissionedProfit = card == BaccratCard.Banker ? coeff * standardCoeff.RootUpdateModel.Commission : coeff;
                    isBankerWin = card == BaccratCard.Banker;
                    //Nếu hệ số vẫn lớn hơn StartNumber
                    if (coeff > standardCoeff.RootUpdateModel.StartNumber)
                    {
                        coeff -= standardCoeff.RootUpdateModel.Increment;
                    }
                }
            }
            return new AutoBacRootOutputCoeff
            {
                Coeff = coeff,
                TheoricalProfit = rawProfit,
                RealProfit = commissionedProfit, 
                IsBankerWin = isBankerWin
            };
        }

        public void TakeProfit(BaccratCard newCard)
        {
            //Giải thuật này chỉ dùng BANKER hoặc PLAYER
            if (newCard == BaccratCard.NoTrade)
                return;

            var lastAutoRoot = MainAutoRoots.LastOrDefault();
            if (lastAutoRoot == null)
                return;

            //Dự đoán hiện tại, xử lý nếu không đủ 6 items
            var currentPredicts = JsonConvert.DeserializeObject<List<BaccaratPredict>>(lastAutoRoot.ListCurrentPredicts);
            if (currentPredicts == null)                
                currentPredicts = new List<BaccaratPredict> (); 
            if (currentPredicts.Count < 2)
            {
                var needToAdd = 2 - currentPredicts.Count;
                for (int i = 1; i <= needToAdd; i++)
                    currentPredicts.Add(new BaccaratPredict { Volume = 0, Value = BaccratCard.NoTrade });
            }

            var flatRootUpdateModel = new RootInputUpdateModel(0, FlatCoeff, 0.95m);

            var autoBacRootInputCoeff = new AutoBacRootInputCoeff
            {
                NewCard = newCard,
                CurrentPredict = currentPredicts[0],
                CurrentCoeff = FlatCoeff,  //Hệ số flat
                RootUpdateModel = flatRootUpdateModel
            };            

            //Lấy dự đoán và so sánh với [newCard] để tính toán lỗ lãi, sau đó nhập cập nhật database
            var mainThreadPredict = UpdateCoeff(autoBacRootInputCoeff);            

            //Cập nhật dự đoán cho 1 trong các thread con
            var globalIndex = lastAutoRoot.GlobalIndex;            
            autoBacRootInputCoeff.CurrentPredict = currentPredicts[1];
            var subThreadPredict = UpdateCoeff(autoBacRootInputCoeff);

            //Cập nhật dự đoán cho all sub threads
            autoBacRootInputCoeff.CurrentPredict = currentPredicts[1];
            var allSubsThreadPredict = UpdateCoeff(autoBacRootInputCoeff);

            //Cập nhật lỗ/lãi trong database
            lastAutoRoot.MainProfit = mainThreadPredict.TheoricalProfit;
            lastAutoRoot.AllSubProfit = allSubsThreadPredict.TheoricalProfit;
            if (globalIndex % 4 == 0)
                lastAutoRoot.Profit0 = subThreadPredict.TheoricalProfit;
            else if(globalIndex % 4 == 1)
                lastAutoRoot.Profit1 = subThreadPredict.TheoricalProfit;
            else if (globalIndex % 4 == 2)
                lastAutoRoot.Profit2 = subThreadPredict.TheoricalProfit;
            else if (globalIndex % 4 == 3)
                lastAutoRoot.Profit3 = subThreadPredict.TheoricalProfit;

            BaccaratDBContext.UpdateAutoRoot(lastAutoRoot);
        }
    }
}
