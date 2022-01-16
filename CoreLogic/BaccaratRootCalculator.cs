using DatabaseContext;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

            Reset();
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
            if (MainRoots.Count > 0)
            {
                var lastItem = MainRoots.Last();
                BaccaratDBContext.DeleteRoot(lastItem.ID);
                MainRoots.RemoveAt(MainRoots.Count - 1);
            }
            Reset();
        }


        public void Reset()
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
                CurrentPredicts = new List<BaccaratPredict>();
            }

            Roots0 = MainRoots.Where(c => c.GlobalOrder % 4 == 0).ToList();
            Roots1 = MainRoots.Where(c => c.GlobalOrder % 4 == 1).ToList();
            Roots2 = MainRoots.Where(c => c.GlobalOrder % 4 == 2).ToList();
            Roots3 = MainRoots.Where(c => c.GlobalOrder % 4 == 3).ToList();
        }

        private List<Root> MainRoots { get; set; }

        private List<Root> Roots0 { get; set; }
        private List<Root> Roots1 { get; set; }
        private List<Root> Roots2 { get; set; }
        private List<Root> Roots3 { get; set; }

        public int MainCoeff { get; private set; } = 1;
        public int Coeff0 { get; private set; } = 1;
        public int Coeff1 { get; private set; } = 1;
        public int Coeff2 { get; private set; } = 1;
        public int Coeff3 { get; private set; } = 1;
        public int AllSubCoeff { get; private set; } = 1;       

        public List<BaccaratPredict> CurrentPredicts { get; set; }

        
        public void AddNewCard(BaccratCard card)
        {
            if (card == BaccratCard.NoTrade)
                throw new Exception("Input card must be Banker or Player");

            //Start  to add new card
            GlobalOrder++;

            //Update coeff (get the current predict and compare to card)
            if (CurrentPredicts.Count > 0)
            {
                var lastRoot = MainRoots.Last();

                var mainProfit = UpdateCurrentCoeff(card, 0, MainCoeff);
                MainCoeff = mainProfit.Item2;
                lastRoot.MainProfit = mainProfit.Item1;

                if (GlobalOrder  % 4 == 0)
                {
                    var profit0 = UpdateCurrentCoeff(card, 1, Coeff0);
                    Coeff0 = profit0.Item2;
                    lastRoot.Profit0 = profit0.Item1;                    
                }
                else if (GlobalOrder  % 4 == 1)
                {
                    var profit1 = UpdateCurrentCoeff(card, 2, Coeff1);
                    Coeff1 = profit1.Item2;
                    lastRoot.Profit1 = profit1.Item1;
                }
                else if (GlobalOrder  % 4 == 2)
                {
                    var profit2 = UpdateCurrentCoeff(card, 3, Coeff2);
                    Coeff2 = profit2.Item2;
                    lastRoot.Profit2 = profit2.Item1;
                }
                else if (GlobalOrder  % 4 ==3)
                {
                    var profit3 = UpdateCurrentCoeff(card, 4, Coeff3);
                    Coeff3 = profit3.Item2;
                    lastRoot.Profit3 = profit3.Item1;
                }

                //Tính toán cho AllSubCoeff
                var allSubProfit = UpdateCurrentCoeff(card, 5, AllSubCoeff);
                AllSubCoeff = allSubProfit.Item2;
                lastRoot.AllSubProfit = allSubProfit.Item1;

                //Cập nhật lại DB
                BaccaratDBContext.UpdateRoot(lastRoot);
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
                    Coeff = Coeff + 2; //Tăng hệ số
                }
                else
                {
                    profit = Coeff; // Lụm tiền
                    if (Coeff >= 3) //Giảm hệ số hoặc giữ nguyên
                        Coeff = Coeff - 2;
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
                    Volume = volume
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
    }
}
