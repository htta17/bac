﻿using DatabaseContext;
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

    public class SaveRootInfo
    { 
        public BaccratCard BaccratCard { get; set; }
        public int GlobalIndex { get; set; }
        public int ID { get; set; }
        public string ListCurrentModCoeffs { get; set; }
        public string ListCurrentPredicts { get; set; }
    }

    /// <summary>
    /// Tham khảo thêm ở BaccaratRootCalculator. 
    /// Các bước chính
    /// </summary>
    public class AutoBacRootAlgorithm : IAutoBacAlgorithm<AutoBacRootInputCoeff, AutoBacRootOutputCoeff>
    {
        #region Properties      
       
        private List<SaveRootInfo> MainAutoRoots { get; set; }       

        /// <summary>
        /// Shoe đang chơi
        /// </summary>
        public int CurrentAutoSessionID { get; private set; }

        /// <summary>
        /// Số bàn
        /// </summary>
        public int TableNumber { get; private set; }
        const int FlatCoeffBase = 1;
        const int ModCoeffBase = 11;

        RootInputUpdateModel FlatRootUpdateModel = new RootInputUpdateModel(0, FlatCoeffBase, 0.95m);

        RootInputUpdateModel LowRiskUpdateModel = new RootInputUpdateModel(2, ModCoeffBase, 0.95m);  
        
        string ConnectionString { get; set; }
        #endregion

        public AutoBacRootAlgorithm( int tableNumber, string _conn)
        {
            TableNumber = tableNumber;
            ConnectionString = _conn;

            Initial(TableNumber);
        }
        public void AddNewCard(BaccratCard baccratCard, AutoResult autoResult = null)
        {
            //Giải thuật này chỉ dùng BANKER hoặc PLAYER
            if (baccratCard == BaccratCard.NoTrade || baccratCard == BaccratCard.Tie)
                throw new Exception("Input card must be Banker or Player");

            // Tìm AutoRoot cuối
            var lastAutoRoot = MainAutoRoots.LastOrDefault();
            var globalIndex = lastAutoRoot == null ? 0 : lastAutoRoot.GlobalIndex;

            //Tăng globalIndex
            globalIndex++;

            var newAutoRoot = default(AutoRoot);

            using (GlobalDBContext dBContext = new GlobalDBContext(ConnectionString))
            {
                if (autoResult == null)
                {
                    autoResult = new AutoResult
                    {
                        AutoSessionID = CurrentAutoSessionID,
                        Card = (short)baccratCard
                    };
                    dBContext.AddAutoResult(autoResult);
                }

                newAutoRoot = new AutoRoot
                {
                    Card = (short)baccratCard,
                    ListCurrentPredicts = string.Empty,
                    ID = autoResult.ID,
                    AutoSessionID = CurrentAutoSessionID,
                    GlobalIndex = globalIndex,
                    ListCurrentModCoeffs = string.Empty
                };
                dBContext.AddAutoRoot(newAutoRoot);
            }

            MainAutoRoots.Add(new SaveRootInfo 
            { 
                GlobalIndex = globalIndex , 
                BaccratCard = baccratCard, 
                ID = autoResult.ID,
                ListCurrentPredicts = string.Empty, 
                ListCurrentModCoeffs = string.Empty
            });
        }

        /// <summary>
        /// Làm việc trên 1 thread duy nhất
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="volume"></param>
        /// <returns></returns>
        private BaccaratPredict PredictSingleThread(List<SaveRootInfo> autoRoots, int volume)
        {
            //Lấy 3 card cuối
            var skip = autoRoots.Count > 3 ? autoRoots.Count - 3 : 0;
            var take = autoRoots.Count > 3 ? 3 : autoRoots.Count;
            var cards = autoRoots.Skip(skip).Take(take)
                                        .Select(c => c.BaccratCard)
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

        public int CalculateModCoeff(List<int> historyProfits, RootInputUpdateModel rootInputUpdate)
        {
            var initialValue = rootInputUpdate.StartNumber;
            foreach (var profit in historyProfits)
            {
                if (profit < 0) //Lỗ, tăng hệ số
                    initialValue += rootInputUpdate.Increment;
                else if (profit > 0)
                { 
                    if (initialValue > rootInputUpdate.StartNumber)
                        initialValue -= rootInputUpdate.Increment;
                }
            }
            return initialValue;
        }

        public BaccaratPredict Predict()
        {
            var predictMainThread = PredictSingleThread(MainAutoRoots, FlatCoeffBase);            

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

            var predictSubThread = PredictSingleThread(SubAutoRoots, FlatCoeffBase);
            #endregion

            /*
            #region Dự đoán cho modification
            var allRootsInThisSession = new GlobalDBContext(ConnectionString)
                                            .FindAutoRootsBySession(CurrentAutoSessionID)
                                            .ToList();
            
            var _mainCoeff = 0;
            {
                var historyModMainProfits = allRootsInThisSession.Select(c => c.ModMainProfit).ToList();
                _mainCoeff = CalculateModCoeff(historyModMainProfits, LowRiskUpdateModel);
            }
            var modPredictMainThread = PredictSingleThread(MainAutoRoots, _mainCoeff);

            var _subCoeff = 0;
            {
                var historyModSubProfits = (predictGlobalIndex % 4 == 0 ? allRootsInThisSession.Select(c => c.ModProfit0)
                                                : predictGlobalIndex % 4 == 1 ? allRootsInThisSession.Select(c => c.ModProfit1)
                                                : predictGlobalIndex % 4 == 2 ? allRootsInThisSession.Select(c => c.ModProfit2)
                                                : allRootsInThisSession.Select(c => c.ModProfit3))
                                                .ToList();
                _subCoeff = CalculateModCoeff(historyModSubProfits, LowRiskUpdateModel);
            }            
            var modPredictSubThread = PredictSingleThread(SubAutoRoots, _subCoeff);

            var _allSubCoeff = 0;
            {
                var historyModAllSubProfits = allRootsInThisSession.Select(c => c.ModAllSubProfit).ToList();
                _allSubCoeff = CalculateModCoeff(historyModAllSubProfits, LowRiskUpdateModel);
            }            
            var modPredictAllSubThread = PredictSingleThread(SubAutoRoots, _allSubCoeff);

            #endregion
            */

            //Cập nhật database cho dự đoán
            var listCurrentPredicts = new List<BaccaratPredict>
            {
                predictMainThread,
                predictSubThread,
                //modPredictMainThread, 
                //modPredictSubThread,
                //modPredictAllSubThread
            };

            /*
            var currentModCoeffs = new List<int>
            {
                _mainCoeff,
                _subCoeff, 
                _allSubCoeff                
            };
            */

            if (lastAutoRoot != null)
            {                
                using (GlobalDBContext dBContext = new GlobalDBContext(ConnectionString))
                {
                    var dbLastAutoRoot = dBContext.FindAllAutoRoots().AsQueryable().Where(c => c.ID == lastAutoRoot.ID).FirstOrDefault();
                    dbLastAutoRoot.ListCurrentPredicts = JsonConvert.SerializeObject(listCurrentPredicts);
                    dbLastAutoRoot.ListCurrentModCoeffs = String.Empty; // JsonConvert.SerializeObject(currentModCoeffs);
                    dBContext.UpdateAutoRoot(dbLastAutoRoot);
                }
                lastAutoRoot.ListCurrentPredicts = JsonConvert.SerializeObject(listCurrentPredicts);
            }

            var flatVolume = (int)predictMainThread.Value * predictMainThread.Volume  //Main thread                            
                           + (int)predictSubThread.Value * (predictSubThread.Volume + FlatCoeffBase);

            /*
            var modVolume = (int)modPredictMainThread.Value * modPredictMainThread.Volume
                                + (int)modPredictSubThread.Value * modPredictSubThread.Volume
                                + (int)modPredictAllSubThread.Value * modPredictAllSubThread.Volume;
            */

            var flatResult = new BaccaratPredict
            {
                Value = flatVolume > 0 ? BaccratCard.Banker : flatVolume == 0 ? BaccratCard.NoTrade : BaccratCard.Player,
                Volume = Math.Abs(flatVolume)
            };

            return flatResult;
        }

        public void Reset()
        {
            using (GlobalDBContext dBContext = new GlobalDBContext(ConnectionString))
            {
                var currentSession = dBContext.FindAllAutoSessions(TableNumber)
                                        .Where(c => c.ID == CurrentAutoSessionID)
                                        .FirstOrDefault();
                if (currentSession != null)
                {
                    currentSession.IsClosed = true;
                    dBContext.UpdateAutoSession(currentSession);
                }
                var newAutoSession = new AutoSession { TableNumber = TableNumber };

                dBContext.AddAutoSession(newAutoSession);
                CurrentAutoSessionID = newAutoSession.ID;
            }
        }

        public void Initial(int tableNumber)
        {            
            using (GlobalDBContext dBContext = new GlobalDBContext(ConnectionString))
            {
                var currentAutoSession = dBContext
                                        .FindAllAutoSessions(tableNumber)
                                        .Where(c => !c.IsClosed)
                                        .OrderByDescending(c => c.ID)
                                        .FirstOrDefault();
                if (currentAutoSession == null)
                {
                    //currentAutoSession = CreateNewSession();
                    var newAutoSession = new AutoSession { TableNumber = TableNumber };
                    dBContext.AddAutoSession(newAutoSession);
                    CurrentAutoSessionID = newAutoSession.ID;
                }
                else if (currentAutoSession.NoOfStepsRoot > 1) //Bàn đã quá lâu
                {
                    currentAutoSession.IsClosed = true;

                    dBContext.UpdateAutoSession(currentAutoSession);

                    //currentAutoSession = CreateNewSession();
                    var newAutoSession = new AutoSession { TableNumber = TableNumber };
                    dBContext.AddAutoSession(newAutoSession);
                    CurrentAutoSessionID = newAutoSession.ID;
                }
                else 
                {
                    CurrentAutoSessionID = currentAutoSession.ID;
                }

                //Lấy 30 bước gần nhất của bàn 
                MainAutoRoots = dBContext.FindLastNAutoRoots(TableNumber, 30)
                                    .Select(c => new SaveRootInfo 
                                    {
                                        BaccratCard = (BaccratCard)c.Card,
                                        GlobalIndex = c.GlobalIndex, 
                                        ListCurrentModCoeffs = c.ListCurrentModCoeffs,
                                        ListCurrentPredicts = c.ListCurrentPredicts,
                                        ID = c.ID,
                                    })
                                    .ToList();
            }
        }    

        public BaccaratPredict Process(BaccratCard baccratCard, AutoResult autoResult = null)
        {
            if ((baccratCard != BaccratCard.Banker) && (baccratCard != BaccratCard.Player))
                return new BaccaratPredict { Value = BaccratCard.NoTrade, Volume = 0 }; 

            var profits = TakeProfit(baccratCard);

            AddNewCard(baccratCard, autoResult);

            if (profits != null)
            {
                UpdateDatabase(profits);
            }            

            return Predict();
        }

        public AutoBacRootOutputCoeff UpdateCoeff(AutoBacRootInputCoeff autoRootCoeff)
        {
            var rawProfit = 0;   //Profit 
            decimal commissionedProfit = 0m; //Profit đã bị trừ comission
            var currentPredictValue = autoRootCoeff.CurrentPredict.Value;
            var card = autoRootCoeff.NewCard;
            var coeff = autoRootCoeff.CurrentCoeff;
            var isBankerWin = false;

            if (autoRootCoeff.CurrentPredict.Value != BaccratCard.NoTrade)
            {
                //Dự đoán sai (THUA)
                if (currentPredictValue != card)
                {
                    rawProfit = -autoRootCoeff.CurrentCoeff;
                    commissionedProfit = -autoRootCoeff.CurrentCoeff;
                    coeff += autoRootCoeff.RootUpdateModel.Increment; //Tăng hệ số
                }
                else //Dự đoán đúng (THẮNG)
                {
                    rawProfit = coeff;
                    commissionedProfit = card == BaccratCard.Banker ? coeff * autoRootCoeff.RootUpdateModel.Commission : coeff;
                    isBankerWin = card == BaccratCard.Banker;
                    //Nếu hệ số vẫn lớn hơn StartNumber
                    if (coeff > autoRootCoeff.RootUpdateModel.StartNumber)
                    {
                        coeff -= autoRootCoeff.RootUpdateModel.Increment;
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

        public Tuple<RootProfit<int>, RootProfit<int>> TakeProfit(BaccratCard newCard)
        {
            //Giải thuật này chỉ dùng BANKER hoặc PLAYER
            if (newCard == BaccratCard.NoTrade)
                return default;

            var lastAutoRoot = MainAutoRoots.LastOrDefault();
            if (lastAutoRoot == null)
                return default;
            Tuple<RootProfit<int>, RootProfit<int>> retVal = Tuple.Create(new RootProfit<int>(), new RootProfit<int>());

            //Dự đoán hiện tại, xử lý nếu không đủ 8 items
            //index = 0: Main thread cho flat
            //index = 1: Dự đoán của 1 thread

            var currentPredicts = string.IsNullOrEmpty(lastAutoRoot.ListCurrentPredicts)
                            ? new List<BaccaratPredict>()
                            : JsonConvert.DeserializeObject<List<BaccaratPredict>>(lastAutoRoot.ListCurrentPredicts);
            
            if (currentPredicts == null)
            {
                currentPredicts = new List<BaccaratPredict>();                
            }
            while (currentPredicts.Count < 2) currentPredicts.Add(new BaccaratPredict { Value = BaccratCard.NoTrade, Volume = 0 });

            /*
            var currentModCoeff = JsonConvert.DeserializeObject<List<int>>(lastAutoRoot.ListCurrentModCoeffs);
            if (currentModCoeff == null)
            {
                currentModCoeff = new List<int>();
                while (currentModCoeff.Count < 3) currentModCoeff.Add(LowRiskUpdateModel.StartNumber);
            }
            */

            var flatBacRootInputCoeff = new AutoBacRootInputCoeff
            {
                NewCard = newCard,
                CurrentPredict = currentPredicts[0],
                CurrentCoeff = FlatCoeffBase,  //Hệ số flat
                RootUpdateModel = FlatRootUpdateModel
            };

            //Cập nhật dự đoán cho 1 trong các thread con
            var globalIndex = lastAutoRoot.GlobalIndex;
            globalIndex++;            

            //Lấy dự đoán và so sánh với [newCard] để tính toán lỗ lãi, sau đó nhập cập nhật database
            var mainThreadPredict = UpdateCoeff(flatBacRootInputCoeff);             

            flatBacRootInputCoeff.CurrentPredict = currentPredicts[1];
            var subThreadPredict = UpdateCoeff(flatBacRootInputCoeff);

            //Cập nhật dự đoán cho all sub threads
            flatBacRootInputCoeff.CurrentPredict = currentPredicts[1];
            var allSubsThreadPredict = UpdateCoeff(flatBacRootInputCoeff);
                        
            retVal.Item1.Main = mainThreadPredict.TheoricalProfit;            
            if (globalIndex % 4 == 0)
                retVal.Item1.Sub0 = subThreadPredict.TheoricalProfit;
            else if(globalIndex % 4 == 1)
                retVal.Item1.Sub1 = subThreadPredict.TheoricalProfit;
            else if (globalIndex % 4 == 2)
                retVal.Item1.Sub2 = subThreadPredict.TheoricalProfit;
            else if (globalIndex % 4 == 3)
                retVal.Item1.Sub3 = subThreadPredict.TheoricalProfit;
            retVal.Item1.AllSub = allSubsThreadPredict.TheoricalProfit;

            //Làm việc với các hệ số thay đổi    
            //Main thead
            /*
            var _lowRiskBacRootInputCoeff = new AutoBacRootInputCoeff
            {
                NewCard = newCard,
                CurrentPredict = currentPredicts[2],
                CurrentCoeff = currentModCoeff[0],  
                RootUpdateModel = LowRiskUpdateModel
           };
           var modMainThreadPredict = UpdateCoeff(_lowRiskBacRootInputCoeff);
            retVal.Item2.Main = modMainThreadPredict.TheoricalProfit;
            
            //Sub thread
            _lowRiskBacRootInputCoeff.CurrentPredict = currentPredicts[3];
            _lowRiskBacRootInputCoeff.CurrentCoeff = currentModCoeff[1];
            var modSubThreadPredict = UpdateCoeff(_lowRiskBacRootInputCoeff);
            if (globalIndex % 4 == 0)
                retVal.Item2.Sub0 = modSubThreadPredict.TheoricalProfit;
            else if (globalIndex % 4 == 1)
                retVal.Item2.Sub1 = modSubThreadPredict.TheoricalProfit;
            else if (globalIndex % 4 == 2)
                retVal.Item2.Sub2 = modSubThreadPredict.TheoricalProfit;
            else if (globalIndex % 4 == 3)
                retVal.Item2.Sub3 = modSubThreadPredict.TheoricalProfit;

            //all sub thread
            _lowRiskBacRootInputCoeff.CurrentPredict = currentPredicts[4];
            _lowRiskBacRootInputCoeff.CurrentCoeff = currentModCoeff[2];
            var modAllSubThreadPredict = UpdateCoeff(_lowRiskBacRootInputCoeff);
            retVal.Item2.AllSub = modAllSubThreadPredict.TheoricalProfit; 
            */

            return retVal;
        }

        public void UpdateDatabase(Tuple<RootProfit<int>, RootProfit<int>> profits)
        {
            var lastAutoRoot1 = MainAutoRoots.LastOrDefault();
            if (lastAutoRoot1 == null)
                return;

            using (GlobalDBContext dBContext = new GlobalDBContext(ConnectionString))
            {
                var lastAutoRoot = dBContext.FindAllAutoRoots().Where(c => c.ID == lastAutoRoot1.ID)
                                        .FirstOrDefault();

                lastAutoRoot.MainProfit = profits.Item1.Main;
                lastAutoRoot.Profit0 = profits.Item1.Sub0;
                lastAutoRoot.Profit1 = profits.Item1.Sub1;
                lastAutoRoot.Profit2 = profits.Item1.Sub2;
                lastAutoRoot.Profit3 = profits.Item1.Sub3;
                lastAutoRoot.AllSubProfit = profits.Item1.AllSub;

                //lastAutoRoot.ModMainProfit = profits.Item2.Main;
                //lastAutoRoot.ModProfit0 = profits.Item2.Sub0;
                //lastAutoRoot.ModProfit1 = profits.Item1.Sub1;
                //lastAutoRoot.ModProfit2 = profits.Item2.Sub2;
                //lastAutoRoot.ModProfit3 = profits.Item2.Sub3;
                //lastAutoRoot.ModAllSubProfit = profits.Item2.AllSub;

                var allAutoRootsThisSession = new List<AutoRoot>();
            
                dBContext.UpdateAutoRoot(lastAutoRoot);


                //Cập nhật số lượng bước cho AutoSession (Dễ dàng cho việc tổng hợp)
                //và Max, Min (cho tiền) 
                allAutoRootsThisSession = dBContext.FindAutoRootsBySession(CurrentAutoSessionID).ToList();

                var sumProfit = allAutoRootsThisSession.Select(c => c.GlobalProfit).Sum();
                var updateSession = dBContext.FindAllAutoSessions(TableNumber).Where(c => c.ID == CurrentAutoSessionID)
                                        .FirstOrDefault();

                updateSession.NoOfStepsRoot = allAutoRootsThisSession.Count;
                updateSession.NoOfSteps = updateSession.AutoResults.Count;

                if (updateSession.MaxRoot < sumProfit)
                    updateSession.MaxRoot = sumProfit;
                if (updateSession.MinRoot > sumProfit)
                    updateSession.MinRoot = sumProfit;
                //Cập nhật PROFITS 
                updateSession.RootMainProfit = allAutoRootsThisSession.Select(c => c.MainProfit).Sum();
                updateSession.RootProfit0 = allAutoRootsThisSession.Select(c => c.Profit0).Sum();
                updateSession.RootProfit1 = allAutoRootsThisSession.Select(c => c.Profit1).Sum();
                updateSession.RootProfit2 = allAutoRootsThisSession.Select(c => c.Profit2).Sum();
                updateSession.RootProfit3 = allAutoRootsThisSession.Select(c => c.Profit3).Sum();
                updateSession.RootAllSub = allAutoRootsThisSession.Select(c => c.AllSubProfit).Sum();

                dBContext.UpdateAutoSession(updateSession);
            }
        }
    }
}
