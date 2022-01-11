using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Midas
{  
    public class Session
    {      
        public Session()
        {
            this.Results = new HashSet<Result>();
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

        public string ImportFileName { get; set; }

        [NotMapped]
        public int TotalProfit 
        { 
            get 
            {
                return Profit14 + Profit25 + Profit36 + Profit47 + Profit58 + Profit61 + Profit72 + Profit83;
            }
        }

        public virtual ICollection<Result> Results { get; set; }
    }
}
