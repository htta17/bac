using System;
using System.Collections.Generic;
using System.Text;
using DatabaseContext;
using System.Linq;

namespace CalculationLogic
{
    public class BaccaratRootMaster
    {
        public BaccaratRootMaster(string connectionString)
        {
            BaccaratDBContext = new GlobalDBContext(connectionString);



            /*Load 100 recentl */
            var rootCount = BaccaratDBContext.Roots.Count();
            var last100Roots = BaccaratDBContext.Roots.Skip(rootCount - 100)
                                        .Take(100)
                                        .Select(c => c.Card == 1 ? BaccratCard.Banker : BaccratCard.Player); 
            MainRoot = new BaccaratRoot(last100Roots);
            
            
            //SubRoot1 = new BaccaratRoot();
            //SubRoot2 = new BaccaratRoot();
            //SubRoot3 = new BaccaratRoot();
            //SubRoot4 = new BaccaratRoot();

            
        }

        BaccaratRoot MainRoot { get; set; }

        int GlobalOrder { get; set; }
        //BaccaratRoot SubRoot1 { get; set; }
        //BaccaratRoot SubRoot2 { get; set; }

        //BaccaratRoot SubRoot3 { get; set; }

        //BaccaratRoot SubRoot4 { get; set; }

        GlobalDBContext BaccaratDBContext { get; set; }



        public void AddCard(BaccratCard baccratCard)
        {
            MainRoot.AddNewCard(baccratCard);

            #region Save to database
            var datetimeNow = DateTime.Now;
            GlobalOrder++;

            BaccaratDBContext.AddRoot(new Root
            {
                Card = (short)(baccratCard == BaccratCard.Banker ? 1 : -1),
                InputDateTime = datetimeNow, 
                Coeff = 1, 
                GlobalOrder = GlobalOrder
            });
            #endregion
        }

        public BaccaratPredict Predict()
        {
            return MainRoot.PredictNextCard();
        }


    }
}
