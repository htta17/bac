using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseContext
{
    public class AutoSession
    {      
        public AutoSession()
        {
            this.AutoResults = new HashSet<AutoResult>();
            this.AutoRoots = new HashSet<AutoRoot>();
        }
    
        public int ID { get; set; }
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// QUAD: Thread từ 1-8 threads
        /// </summary>
        public int MaxQuad { get; set; }
        public int MinQuad { get; set; }
        public int Profit14 { get; set; }
        public int Profit25 { get; set; }
        public int Profit36 { get; set; }
        public int Profit47 { get; set; }
        public int Profit58 { get; set; }
        public int Profit61 { get; set; }
        public int Profit72 { get; set; }
        public int Profit83 { get; set; }        

        public int TableNumber { get; set; }

        /// <summary>
        /// Số lượng các bước trong SHOE (Bao gồm BANKER, PLAYER và TIE) 
        /// </summary>
        public int NoOfSteps { get; set; }

        /// <summary>
        /// Số lượng các bước giải thuật ROOT dùng để trade (BANKER và PLAYER)
        /// </summary>
        public int NoOfStepsRoot { get; set; }
        /// <summary>
        /// Số lượng các bước giải thuật QUAD dùng để trade (BANKER và PLAYER)
        /// </summary>
        public int NoOfStepsQuad { get; set; }

        public int MaxRoot { get; set; }
        public int MinRoot { get; set; }
       
        public int RootMainProfit { get; set; }
        public int RootProfit0 { get; set; }
        public int RootProfit1 { get; set; }
        public int RootProfit2 { get; set; }
        public int RootProfit3 { get; set; }

        public int RootAllSub { get; set; }

        /// <summary>
        /// = 1: Bàn đã đóng, = 0: Bàn đang mở
        /// </summary>
        public bool IsClosed { get; set; }

        public virtual ICollection<AutoResult> AutoResults { get; set; }
        public virtual ICollection<AutoRoot> AutoRoots { get; set; }
    }
}
