using DatabaseContext;
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

            SaveCards = new List<BaccratCard>();

            var rootCount = BaccaratDBContext.Roots.Count();
            Roots = BaccaratDBContext.Roots
                                        .OrderBy(c => c.ID)
                                        .Skip(rootCount > 100 ? rootCount - 100 : 0)
                                        .Take(rootCount > 100 ? 100 : rootCount)
                                        .ToList();            

            SaveCards.AddRange(Roots.Select(c => (BaccratCard)c.Card));
            
            if (Roots.Count() > 0)
            {
                GlobalOrder = Roots.Last().GlobalOrder;
                MainCoeff = Roots.Last().MainCoeff;
            }            
        }
        GlobalDBContext BaccaratDBContext { get; set; }

        public int GlobalOrder { get; private set; } = 0;

        public BaccratCard ShowLastCard()
        {
            if (SaveCards.Count > 0)
                return SaveCards.Last();
            return BaccratCard.NoTrade;
        }
        
        
        private List<BaccratCard> SaveCards { get; set; }
        private List<Root> Roots { get; set; }

        
        public int MainCoeff { get; private set; } = 1;
        

        public void AddNewCard(BaccratCard card)
        {
            if (card == BaccratCard.NoTrade)
                throw new Exception("Input card must be Banker or Player");

            var currentMainPredict = PredictMainThread();
            var currentSubPredict = PredictSubThread();

            if (currentMainPredict.Value != BaccratCard.NoTrade)
            {
                if (currentMainPredict.Value == card) //Đoán đúng
                {
                    if (MainCoeff > 1) MainCoeff = MainCoeff - 1;
                }
                else
                {
                    MainCoeff = MainCoeff + 1;
                }
            }

            var currentRoot = Roots.FirstOrDefault(c => c.GlobalOrder == GlobalOrder - 3);
            var subCoeff = currentRoot == null ? 1 : currentRoot.SubCoeff; 
            if (currentSubPredict.Value != BaccratCard.NoTrade)
            {
                if (currentSubPredict.Value == card) //Đoán đúng
                {
                    if (subCoeff > 1) subCoeff = subCoeff - 1;
                }
                else
                {
                    subCoeff = subCoeff + 1;
                }
            }

            SaveCards.Add(card); 

            SaveToDatabase(card, subCoeff);
        }


        private void SaveToDatabase(BaccratCard baccratCard, int SubCoeff)
        {
            GlobalOrder++;            
            var datetimeNow = DateTime.Now;
            var newRoot = new Root
            {
                Card = (short)baccratCard,
                InputDateTime = datetimeNow,
                GlobalCoeff = 0 ,
                MainCoeff = MainCoeff,
                SubCoeff = SubCoeff,
                GlobalOrder = GlobalOrder
            };            
            BaccaratDBContext.AddRoot(newRoot);
        }

        private BaccaratPredict PredictNextCard(List<BaccratCard> cards)
        {
            if (cards.Count < 3)
                return new BaccaratPredict { Value = BaccratCard.NoTrade, Volume = 0 };

            if (cards[0] != cards[1] && cards[1] == cards[2]) //Trigger to predict
            {
                return new BaccaratPredict
                {
                    Value = cards[2] == BaccratCard.Banker ? BaccratCard.Player : BaccratCard.Banker,
                    Volume = 1
                };
            }
            return new BaccaratPredict { Value = BaccratCard.NoTrade, Volume = 0 };
        }

        public BaccaratPredict Predict()
        {
            var mainThreadPredict = PredictMainThread();
            mainThreadPredict.Volume = MainCoeff; 
            
            var subThreadPredict = PredictSubThread();
            var currentRoot = Roots.FirstOrDefault(c => c.GlobalOrder == GlobalOrder - 3);
            subThreadPredict.Volume = currentRoot == null ? 1 : currentRoot.SubCoeff;

            var volume = (int)mainThreadPredict.Value * mainThreadPredict.Volume
                            + (int)subThreadPredict.Value * subThreadPredict.Volume;

            var result = new BaccaratPredict
            {
                Value = volume > 0 ? BaccratCard.Banker : volume == 0 ? BaccratCard.NoTrade : BaccratCard.Player , 
                Volume = Math.Abs(volume)
            };
            return result;
        }

        private BaccaratPredict PredictMainThread()
        {
            var skip = SaveCards.Count > 3 ? SaveCards.Count - 3 : 0;
            var take = SaveCards.Count > 3 ? 3 : SaveCards.Count;
            var mainThreadCards = SaveCards.Skip(skip).Take(take).ToList();
            return PredictNextCard(mainThreadCards);
        }

        private BaccaratPredict PredictSubThread()
        {
            var _subThreadCards = new List<BaccratCard>();
            var lastPosition = SaveCards.Count - 1;

            //Tại vị trí 11, lấy các điểm: 0, 4, 8 để dự đoán điểm 12
            //Tại vị trí N, lấy các điểm N - 11, N - 7 và N - 3 để dự đoán N + 1
            if (lastPosition >= 11) 
            {
                _subThreadCards.AddRange(new List<BaccratCard>
                {   SaveCards[lastPosition - 11],
                    SaveCards[lastPosition - 7],
                    SaveCards[lastPosition - 3]
                });
            }
            return PredictNextCard(_subThreadCards);
        }
    }
}
