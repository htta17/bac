﻿using DatabaseContext;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace CoreLogic
{
    /// <summary>
    /// Giải thuật Root
    /// </summary>
    public class BaccaratRootCalculator : IBaccaratCalculator
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


        #region Properties

        GlobalDBContext BaccaratDBContext { get; set; }
        public int GlobalIndex { get; private set; } = 0;

        private List<Root> MainRoots { get; set; }

        private List<Root> Roots0 { get; set; }
        private List<Root> Roots1 { get; set; }
        private List<Root> Roots2 { get; set; }
        private List<Root> Roots3 { get; set; }
        #endregion

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
            BaccaratDBContext.DeleteRoot(lastItem.ID);
            MainRoots.RemoveAt(MainRoots.Count - 1);
            Reload(false);                        
        }

        public void Reset()
        {
            MainRoots = new List<Root>();
            
            GlobalIndex = 0;
            CurrentPredicts = new List<BaccaratPredict>();

            FlatCoeff.SetValue(1);
            ModCoeff.SetValue(Eleven);

            AccumulateFlat100.Reset();
            AccumulateFlat095.Reset();
            AccumulateMod100.Reset();
            AccumulateMod095.Reset();

            Roots0 = new List<Root>();
            Roots1 = new List<Root>();
            Roots2 = new List<Root>();
            Roots3 = new List<Root>();

            FILENAME_DATETIME = DateTime.Now;
            WriteFirstTime = true;
        }

        public void Reload(bool ressetFileName)
        {
            var rootCount = BaccaratDBContext.Roots.Count();
            MainRoots = BaccaratDBContext.Roots
                                        .OrderBy(c => c.ID)
                                        .Skip(rootCount > 100 ? rootCount - 100 : 0)
                                        .Take(rootCount > 100 ? 100 : rootCount)
                                        .ToList();

            if (MainRoots.Count > 0)
            {
                var lastItem = MainRoots.Last();
                GlobalIndex = lastItem.GlobalOrder;

                if (ressetFileName)
                {
                    FlatCoeff.SetValue(1);
                    ModCoeff.SetValue(Eleven);

                    AccumulateFlat100.Reset();
                    AccumulateFlat095.Reset();
                    AccumulateMod100.Reset();
                    AccumulateMod095.Reset();
                }
                else
                {
                    FlatCoeff.Main = lastItem.MainCoeff;
                    FlatCoeff.Sub0 = lastItem.Coeff0;
                    FlatCoeff.Sub1 = lastItem.Coeff1;
                    FlatCoeff.Sub2 = lastItem.Coeff2;
                    FlatCoeff.Sub3 = lastItem.Coeff3;
                    FlatCoeff.AllSub = lastItem.AllSubCoeff;

                    AccumulateFlat100.Main -= lastItem.MainProfit;
                    AccumulateFlat100.Sub0 -= lastItem.Profit0;
                    AccumulateFlat100.Sub1 -= lastItem.Profit1;
                    AccumulateFlat100.Sub2 -= lastItem.Profit2;
                    AccumulateFlat100.Sub3 -= lastItem.Profit3;
                    AccumulateFlat100.AllSub -= lastItem.AllSubProfit;

                    AccumulateFlat095.Main -= lastItem.Flat095Main;
                    AccumulateFlat095.Sub0 -= lastItem.Flat095Profit0;
                    AccumulateFlat095.Sub1 -= lastItem.Flat095Profit1;
                    AccumulateFlat095.Sub2 -= lastItem.Flat095Profit2;
                    AccumulateFlat095.Sub3 -= lastItem.Flat095Profit3;
                    AccumulateFlat095.AllSub -= lastItem.Flat095AllSub;

                    ModCoeff.Main = lastItem.ModMainCoeff;
                    ModCoeff.Sub0 = lastItem.ModCoeff0;
                    ModCoeff.Sub1 = lastItem.ModCoeff1;
                    ModCoeff.Sub2 = lastItem.ModCoeff2;
                    ModCoeff.Sub3 = lastItem.ModCoeff3;
                    ModCoeff.AllSub = lastItem.ModAllSubCoeff;

                    AccumulateMod100.Main -= lastItem.ModMainProfit;
                    AccumulateMod100.Sub0 -= lastItem.ModProfit0;
                    AccumulateMod100.Sub1 -= lastItem.ModProfit1;
                    AccumulateMod100.Sub2 -= lastItem.ModProfit2;
                    AccumulateMod100.Sub3 -= lastItem.ModProfit3;
                    AccumulateMod100.AllSub -= lastItem.ModAllSubProfit;

                    AccumulateMod095.Main -= lastItem.Mod095Main;
                    AccumulateMod095.Sub0 -= lastItem.Mod095Profit0;
                    AccumulateMod095.Sub1 -= lastItem.Mod095Profit1;
                    AccumulateMod095.Sub2 -= lastItem.Mod095Profit2;
                    AccumulateMod095.Sub3 -= lastItem.Mod095Profit3;
                    AccumulateMod095.AllSub -= lastItem.Mod095AllSub;
                }
                

                CurrentPredicts = JsonConvert.DeserializeObject<List<BaccaratPredict>>(lastItem.ListCurrentPredicts);

                //Khi user nhấn nút BACK (ressetFileName = false), lấy lại các thông tin về tích lũy bằng cách
                //      trừ đi Profit cuả record đã xóa (Móa, tâm trí không tập trung phải gõ ngơ ngơ thế này) 
                //      ví dụ đi ha: 
                //          Thôi để sau
                
            }
            else
            {
                GlobalIndex = 0;

                FlatCoeff.SetValue(1);
                ModCoeff.SetValue(Eleven);

                AccumulateFlat100.Reset();
                AccumulateFlat095.Reset();
                AccumulateMod100.Reset();
                AccumulateMod095.Reset();

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

        

        /// <summary>
        /// Lợi nhuận tích lũy cơ bản KHÔNG comission
        /// </summary>
        private RootProfit<int> AccumulateFlat100 { get; set; } = new RootProfit<int>();        

        /// <summary>
        /// Lợi nhuận tích lũy cơ bản CÓ comission
        /// </summary>
        private RootProfit<decimal> AccumulateFlat095 { get; set; } = new RootProfit<decimal>();

        private RootProfit<int> AccumulateMod100 { get; set; } = new RootProfit<int>();

        private RootProfit<decimal> AccumulateMod095 { get; set; } = new RootProfit<decimal>();

        private RootProfit<int> FlatCoeff { get; set; } = new RootProfit<int>();

        private RootProfit<int> ModCoeff { get; set; } = new RootProfit<int>();

        public List<BaccaratPredict> CurrentPredicts { get; set; }

        /// <summary>
        /// Hệ số luôn là 1 (flat, no increment), giá trị bắt đầu của hệ số là 1, commission cho sàn khi Banker thắng là 5%
        /// </summary>
        RootInputUpdateModel FlatNoIncrement = new RootInputUpdateModel(0, 1, 0.95m);

        const int Eleven = 11;
        /// <summary>
        /// Giá trị ban đầu của hệ số là 11, bước nhảy 2 (increment = 2), commission cho sàn khi Banker thắng là 5%
        /// </summary>
        RootInputUpdateModel LowRiskIcrement = new RootInputUpdateModel(2, Eleven, 0.95m);

        private bool WriteFirstTime { get; set; }

        private DateTime? FILENAME_DATETIME = null;
        private const string LogTitle = 
            "ID,Time,Card," +
            //"Main Profit (Flat)," +
            "Main Accumulate (Flat)," +
            //"Profit0 (Flat)," +
            "Accumulate0 (Flat)," +
            //"Profit1 (Flat)," +
            "Accumulate1 (Flat)," +
            //"Profit2 (Flat)," +
            "Accumulate2 (Flat)," +
            //"Profit3 (Flat)," +
            "Accumulate3 (Flat)," +
            //"All Sub Profit (Flat), " +
            "All Sub Accumulate  (Flat), SUM FLAT," +

            /*
            "Main Profit (0.95),Main Accumulate (0.95)," +
            "Profit0 (0.95),Accumulate0 (0.95)," +
            "Profit1 (0.95),Accumulate1 (0.95)," +
            "Profit2 (0.95),Accumulate2 (0.95)," +
            "Profit3 (0.95),Accumulate3 (0.95), " +
            "All Sub Profit (0.95), All Sub Accumulate  (0.95), SUM FLAT 0.95," +
            */

            //"Main Profit (Mod)," +
            "Main Accumulate (Mod)," +
            //"Profit0 (Mod)," +
            "Accumulate0 (Mod)," +
            //"Profit1 (Mod)," +
            "Accumulate1 (Mod)," +
            //"Profit2 (Mod)," +
            "Accumulate2 (Mod)," +
            //"Profit3 (Mod)," +
            "Accumulate3 (Mod)," +
            //"All Sub Profit (Flat), " +
            "All Sub Accumulate (Mod), SUM MOD" +

            /*
            "Main Profit (Mod 0.95),Main Accumulate (Mod 0.95)," +
            "Profit0 (Mod 0.95),Accumulate0 (Mod 0.95)," +
            "Profit1 (Mod 0.95),Accumulate1 (Mod 0.95)," +
            "Profit2 (Mod 0.95),Accumulate2 (Mod 0.95)," +
            "Profit3 (Mod 0.95),Accumulate3 (Mod 0.95), " +
            "All Sub Profit (0.95), All Sub Accumulate  (0.95), SUM MOD 0.95" +
            */

            " \r\n";
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
            GlobalIndex++;
            var logger = "";

            //Update coeff (get the current predict and compare to card)
            if (CurrentPredicts.Count > 0 && MainRoots.Count > 0)
            {
                var lastRoot = MainRoots.Last();

                #region Main Thread
                {
                    var mainProfit = UpdateCurrentCoeff(card, 0, FlatCoeff.Main, FlatNoIncrement);
                    FlatCoeff.Main = mainProfit.Coeff;
                    lastRoot.MainProfit = mainProfit.Profit;
                    lastRoot.Flat095Main = mainProfit.ComissionedProfit;
                    AccumulateFlat100.Main += mainProfit.Profit;
                    AccumulateFlat095.Main += mainProfit.ComissionedProfit;

                    var modMainProfit = UpdateCurrentCoeff(card, 6, ModCoeff.Main, LowRiskIcrement);
                    ModCoeff.Main = modMainProfit.Coeff;
                    lastRoot.ModMainProfit = modMainProfit.Profit;
                    lastRoot.Mod095Main = modMainProfit.ComissionedProfit;
                    AccumulateMod100.Main += modMainProfit.Profit;
                    AccumulateMod095.Main += modMainProfit.ComissionedProfit;
                }
                #endregion

                if (GlobalIndex % 4 == 0)
                {
                    var profit0 = UpdateCurrentCoeff(card, 1, FlatCoeff.Sub0, FlatNoIncrement);
                    FlatCoeff.Sub0 = profit0.Coeff;
                    lastRoot.Profit0 = profit0.Profit;
                    lastRoot.Flat095Profit0 = profit0.ComissionedProfit;                    
                    AccumulateFlat100.Sub0 += profit0.Profit;
                    AccumulateFlat095.Sub0 += profit0.ComissionedProfit;

                    var modProfit0 = UpdateCurrentCoeff(card, 7, ModCoeff.Sub0, LowRiskIcrement);
                    ModCoeff.Sub0 = modProfit0.Coeff;
                    lastRoot.ModProfit0 = modProfit0.Profit;
                    lastRoot.Mod095Profit0 = modProfit0.ComissionedProfit;
                    AccumulateMod100.Sub0 += modProfit0.Profit;
                    AccumulateMod095.Sub0 += modProfit0.ComissionedProfit;
                }
                else if (GlobalIndex % 4 == 1)
                {
                    var profit1 = UpdateCurrentCoeff(card, 2, FlatCoeff.Sub1, FlatNoIncrement);
                    FlatCoeff.Sub1 = profit1.Coeff;
                    lastRoot.Profit1 = profit1.Profit;
                    lastRoot.Flat095Profit1 = profit1.ComissionedProfit;                    
                    AccumulateFlat100.Sub1 += profit1.Profit;
                    AccumulateFlat095.Sub1 += profit1.ComissionedProfit;

                    var modProfit1 = UpdateCurrentCoeff(card, 8, ModCoeff.Sub1, LowRiskIcrement);
                    ModCoeff.Sub1 = modProfit1.Coeff;
                    lastRoot.ModProfit1 = modProfit1.Profit;
                    lastRoot.Mod095Profit1 = modProfit1.ComissionedProfit;
                    AccumulateMod100.Sub1 += modProfit1.Profit;
                    AccumulateMod095.Sub1 += modProfit1.ComissionedProfit;
                }
                else if (GlobalIndex % 4 == 2)
                {
                    var profit2 = UpdateCurrentCoeff(card, 3, FlatCoeff.Sub2, FlatNoIncrement);
                    FlatCoeff.Sub2 = profit2.Coeff;
                    lastRoot.Profit2 = profit2.Profit;
                    lastRoot.Flat095Profit2 = profit2.ComissionedProfit;                    
                    AccumulateFlat100.Sub2 += profit2.Profit;
                    AccumulateFlat095.Sub2 += profit2.ComissionedProfit;

                    var modProfit2 = UpdateCurrentCoeff(card, 9, ModCoeff.Sub2, LowRiskIcrement);
                    ModCoeff.Sub2 = modProfit2.Coeff;
                    lastRoot.ModProfit2 = modProfit2.Profit;
                    lastRoot.Mod095Profit2 = modProfit2.ComissionedProfit;
                    AccumulateMod100.Sub2 += modProfit2.Profit;
                    AccumulateMod095.Sub2 += modProfit2.ComissionedProfit;
                }
                else if (GlobalIndex % 4 == 3)
                {
                    var profit3 = UpdateCurrentCoeff(card, 4, FlatCoeff.Sub3, FlatNoIncrement);
                    FlatCoeff.Sub3 = profit3.Coeff;
                    lastRoot.Profit3 = profit3.Profit;
                    lastRoot.Flat095Profit3 = profit3.ComissionedProfit;
                    AccumulateFlat100.Sub3 += profit3.Profit;
                    AccumulateFlat095.Sub3 += profit3.ComissionedProfit;

                    var modProfit3 = UpdateCurrentCoeff(card, 10, ModCoeff.Sub3, LowRiskIcrement);
                    ModCoeff.Sub3 = modProfit3.Coeff;
                    lastRoot.ModProfit3 = modProfit3.Profit;
                    lastRoot.Mod095Profit3 = modProfit3.ComissionedProfit;
                    AccumulateMod100.Sub3 += modProfit3.Profit;
                    AccumulateMod095.Sub3 += modProfit3.ComissionedProfit;
                }

                #region All Sub Thread
                {
                    //Tính toán cho AllSubCoeff
                    var allSubProfit = UpdateCurrentCoeff(card, 5, FlatCoeff.AllSub, FlatNoIncrement);
                    FlatCoeff.AllSub = allSubProfit.Coeff;
                    lastRoot.AllSubProfit = allSubProfit.Profit;
                    lastRoot.Flat095AllSub = allSubProfit.ComissionedProfit;
                    AccumulateFlat100.AllSub += allSubProfit.Profit;
                    AccumulateFlat095.AllSub += allSubProfit.ComissionedProfit;

                    var modAllSubProfit = UpdateCurrentCoeff(card, 11, ModCoeff.AllSub, LowRiskIcrement);
                    ModCoeff.AllSub = modAllSubProfit.Coeff;
                    lastRoot.ModAllSubProfit = modAllSubProfit.Profit;
                    lastRoot.Mod095AllSub = modAllSubProfit.ComissionedProfit;
                    AccumulateMod100.AllSub += modAllSubProfit.Profit;
                    AccumulateMod095.AllSub += modAllSubProfit.ComissionedProfit;
                }
                #endregion

                //Cập nhật lại DB
                BaccaratDBContext.UpdateRoot(lastRoot);

                //Ghi vào log
                logger = $"{GlobalIndex},{DateTime.Now},{card}," +
                                //$"{lastRoot.MainProfit}," +
                                $"{AccumulateFlat100.Main}," +
                                //$"{lastRoot.Profit0}," +
                                $"{AccumulateFlat100.Sub0}," +
                                //$"{lastRoot.Profit1}," +
                                $"{AccumulateFlat100.Sub1}," +
                                //$"{lastRoot.Profit2}," +
                                $"{AccumulateFlat100.Sub2}," +
                                //$"{lastRoot.Profit3}," +
                                $"{AccumulateFlat100.Sub3}," +
                                //$"{lastRoot.AllSubProfit}, " +
                                $"{AccumulateFlat100.AllSub}, {AccumulateFlat100.SumAll() }, " +

                                /*
                                $"{lastRoot.Flat095Main},{AccumulateFlat095.Main}," +
                                $"{lastRoot.Flat095Profit0},{AccumulateFlat095.Sub0}," +
                                $"{lastRoot.Flat095Profit1},{AccumulateFlat095.Sub1}," +
                                $"{lastRoot.Flat095Profit2},{AccumulateFlat095.Sub2}," +
                                $"{lastRoot.Flat095Profit3},{AccumulateFlat095.Sub3}," +
                                $"{lastRoot.Flat095AllSub}, {AccumulateFlat095.AllSub}, {AccumulateFlat095.SumAll() }," +
                                */

                                //$"{lastRoot.ModMainProfit}," +
                                $"{AccumulateMod100.Main}," +
                                //$"{lastRoot.ModProfit0}," +
                                $"{AccumulateMod100.Sub0}," +
                                //$"{lastRoot.ModProfit1}," +
                                $"{AccumulateMod100.Sub1}," +
                                //$"{lastRoot.ModProfit2}," +
                                $"{AccumulateMod100.Sub2}," +
                                //$"{lastRoot.ModProfit3}," +
                                $"{AccumulateMod100.Sub3}," +
                                //$"{lastRoot.ModAllSubProfit}, " +
                                $"{AccumulateMod100.AllSub}, {AccumulateMod100.SumAll() }" +

                                /*
                                $"{lastRoot.Mod095Main},{AccumulateMod095.Main}," +
                                $"{lastRoot.Mod095Profit0},{AccumulateMod095.Sub0}," +
                                $"{lastRoot.Mod095Profit1},{AccumulateMod095.Sub1}," +
                                $"{lastRoot.Mod095Profit2},{AccumulateMod095.Sub2}," +
                                $"{lastRoot.Mod095Profit3},{AccumulateMod095.Sub3}," +
                                $"{lastRoot.Mod095AllSub}, {AccumulateMod095.AllSub}, {AccumulateMod095.SumAll() }" +
                                */

                                $" \r\n";

            }
            else
            {
                logger = $"{GlobalIndex},{DateTime.Now},{card} \r\n";

                for (var i = 1; i <= 14; i++)
                    logger += ",0";
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
                
                MainCoeff = FlatCoeff.Main,
                Coeff0 = FlatCoeff.Sub0,
                Coeff1 = FlatCoeff.Sub1,
                Coeff2 = FlatCoeff.Sub2,
                Coeff3 = FlatCoeff.Sub3,
                AllSubCoeff = FlatCoeff.AllSub,

                MainProfit = 0,
                Profit0 = 0,
                Profit1 = 0,
                Profit2 = 0,
                Profit3 = 0,
                AllSubProfit =0,

                Flat095Main = 0, 
                Flat095Profit0 = 0,
                Flat095Profit1 = 0, 
                Flat095Profit2 = 0,
                Flat095Profit3 = 0, 
                Flat095AllSub = 0, 

                ModMainCoeff = ModCoeff.Main,
                ModCoeff0 = ModCoeff.Sub0,
                ModCoeff1 = ModCoeff.Sub1,
                ModCoeff2 = ModCoeff.Sub2,
                ModCoeff3 = ModCoeff.Sub3,
                ModAllSubCoeff = ModCoeff.AllSub,

                ModMainProfit = 0,
                ModProfit0 = 0,
                ModProfit1 = 0,
                ModProfit2 = 0,
                ModProfit3 = 0,
                ModAllSubProfit = 0,

                Mod095Main = 0,
                Mod095Profit0 = 0,
                Mod095Profit1 = 0,
                Mod095Profit2 = 0,
                Mod095Profit3 = 0,
                Mod095AllSub = 0,

                GlobalOrder = GlobalIndex, 
                ListCurrentPredicts = ""
            };            

            //Add for main root
            MainRoots.Add(newRoot);

            //Add for sub roots
            if (GlobalIndex % 4 == 0)
                Roots0.Add(newRoot);
            else if (GlobalIndex % 4 == 1)
                Roots1.Add(newRoot);
            else if (GlobalIndex % 4 == 2)
                Roots2.Add(newRoot); 
            else if (GlobalIndex % 4 == 3)
                Roots3.Add(newRoot);

            BaccaratDBContext.AddRoot(newRoot);
        }

        /// <summary>
        /// Update Coeff and return Profit
        /// </summary>
        /// <param name="card">Thẻ vừa bốc</param>
        /// <param name="index">Thứ tự trong CurrentPredicts, bắt đầu từ 0</param>
        /// <param name="Coeff">Hệ số cần cập nhật</param>
        /// <returns>
        ///     Item1: Profit (Âm hoặc Dương)
        ///     Item2: Hệ số sau khi cập nhật
        /// </returns>
        private RootUpdateModel UpdateCurrentCoeff(BaccratCard card, int index, int coeff, RootInputUpdateModel input)
        {

            var threadPredict0 = index + 1 <= CurrentPredicts.Count
                                    ? CurrentPredicts[index]
                                    : new BaccaratPredict { Value = BaccratCard.NoTrade, Volume = 0 };
            var rawProfit = 0;
            decimal profitWithCommission = 0m;
            if (threadPredict0.Value != BaccratCard.NoTrade)
            {
                if (threadPredict0.Value != card) //THUA
                {
                    rawProfit = -coeff; 
                    profitWithCommission = -coeff;
                    coeff += input.Increment; //Tăng hệ số
                }
                else //THẮNG 
                {
                    rawProfit = coeff;
                    profitWithCommission = card == BaccratCard.Banker ? coeff * input.Commission : coeff;
                    //Nếu hệ số vẫn lớn hơn StartNumber
                    if (coeff > input.StartNumber) 
                    {
                        coeff -= input.Increment;
                    }                    
                }
            }
            return new RootUpdateModel(coeff, rawProfit, profitWithCommission); 
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
                    Volume = volume
                };
            }
            return new BaccaratPredict { Value = BaccratCard.NoTrade, Volume = 0 };
        }

        public BaccaratPredict Predict()
        {
            var mainThread = PredicThread(MainRoots, FlatCoeff.Main);
            var thread0 = PredicThread(Roots0, FlatCoeff.Sub0);
            var thread1 = PredicThread(Roots1, FlatCoeff.Sub1);
            var thread2 = PredicThread(Roots2, FlatCoeff.Sub2);
            var thread3 = PredicThread(Roots3, FlatCoeff.Sub3);

            var modMainThread = PredicThread(MainRoots, ModCoeff.Main);
            var modThread0 = PredicThread(Roots0, ModCoeff.Sub0);
            var modThread1 = PredicThread(Roots1, ModCoeff.Sub1);
            var modThread2 = PredicThread(Roots2, ModCoeff.Sub2);
            var modThread3 = PredicThread(Roots3, ModCoeff.Sub3);
            

            var _predictGlobalOrder = GlobalIndex + 1;
            var subThread = default(BaccaratPredict);
            var modsubThread = default(BaccaratPredict);
            var modAllSubThread = default(BaccaratPredict);

            if (_predictGlobalOrder % 4 == 0)
            {
                subThread = thread0;
                modsubThread = modThread0;
                modAllSubThread = PredicThread(Roots0, ModCoeff.AllSub);
            }
            else if (_predictGlobalOrder % 4 == 1)
            {
                subThread = thread1;
                modsubThread = modThread1;
                modAllSubThread = PredicThread(Roots1, ModCoeff.AllSub);
            }
            else if (_predictGlobalOrder % 4 == 2)
            {
                subThread = thread2;
                modsubThread = modThread2;
                modAllSubThread = PredicThread(Roots2, ModCoeff.AllSub);
            }
            else
            {
                subThread = thread3;
                modsubThread = modThread3;
                modAllSubThread = PredicThread(Roots3, ModCoeff.AllSub);
            }

            var volumeFlat = (int)mainThread.Value * mainThread.Volume  //Main thread                            
                           + (int)subThread.Value * (subThread.Volume + FlatCoeff.AllSub) ;

            var volumeMod = (int)modMainThread.Value * modMainThread.Volume
                                + (int)modsubThread.Value * modsubThread.Volume
                                + (int)modAllSubThread.Value * modAllSubThread.Volume;

            var resultFlat = new BaccaratPredict
            {
                Value = volumeFlat > 0 ? BaccratCard.Banker : volumeFlat == 0 ? BaccratCard.NoTrade : BaccratCard.Player,
                Volume = Math.Abs(volumeFlat)
            };
            var resultMod = new BaccaratPredict
            {
                Value = volumeMod > 0 ? BaccratCard.Banker : volumeMod == 0 ? BaccratCard.NoTrade : BaccratCard.Player,
                Volume = Math.Abs(volumeMod)
            };

            if (CurrentPredicts == null)
                CurrentPredicts = new List<BaccaratPredict>();
            else
                CurrentPredicts.Clear();
            CurrentPredicts.AddRange(new List<BaccaratPredict>
            {
                mainThread,
                thread0,
                thread1,
                thread2,
                thread3,
                subThread,

                modMainThread,
                modThread0, 
                modThread1, 
                modThread2, 
                modThread3, 
                modAllSubThread
            });

            //Update Root 
            if (MainRoots.Count > 0)
            {
                var lastRoot = MainRoots.Last();
                lastRoot.ListCurrentPredicts = JsonConvert.SerializeObject(CurrentPredicts);
                BaccaratDBContext.UpdateRoot(lastRoot);
            }
            
            return 1 == 1 ? resultFlat : resultMod;           
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
                        "Format: Phiên chính, phiên con 1-4, phiên tổng hợp của 4 phiên con\r\n" +
                        $":one: Flat: `{AccumulateFlat100.Main}`, `{AccumulateFlat100.Sub0}`, `{ AccumulateFlat100.Sub1}`, `{AccumulateFlat100.Sub2}`, `{AccumulateFlat100.Sub3}`, `{ AccumulateFlat100.AllSub}`, Tổng: `{AccumulateFlat100.SumAll()}`  units.\r\n" +
                        $":two: Flat (0.95):  `{AccumulateFlat095.Main}`, `{AccumulateFlat095.Sub0}`, `{ AccumulateFlat095.Sub1}`, `{AccumulateFlat095.Sub2}`, `{AccumulateFlat095.Sub3}`, `{ AccumulateFlat095.AllSub}`, Tổng: `{AccumulateFlat095.SumAll() }`  units.\r\n" +
                        $":three: Modification: `{AccumulateMod100.Main}`, `{AccumulateMod100.Sub0}`, `{ AccumulateMod100.Sub1}`, `{AccumulateMod100.Sub2}`, `{AccumulateMod100.Sub3}`, `{ AccumulateMod100.AllSub}`, Tổng: `{AccumulateMod100.SumAll()}`  units.\r\n" +
                        $":four: Modification (0.95):  `{AccumulateMod095.Main}`, `{AccumulateMod095.Sub0}`, `{ AccumulateMod095.Sub1}`, `{AccumulateMod095.Sub2}`, `{AccumulateMod095.Sub3}`, `{ AccumulateMod095.AllSub}`, Tổng: `{AccumulateMod095.SumAll() }`  units.\r\n"; 
            return text;
        }
    }
}
