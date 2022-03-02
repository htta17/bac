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
        public int NoOfSteps { get; set; }
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
        /// = 1: Bàn đã đóng, = 0: Bàn đang mở
        /// </summary>
        public bool IsClosed { get; set; }

        public virtual ICollection<AutoResult> AutoResults { get; set; }
        public virtual ICollection<AutoRoot> AutoRoots { get; set; }
    }
}
