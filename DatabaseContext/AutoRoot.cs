using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace DatabaseContext
{  
    public class AutoRoot
    {
        [ForeignKey("AutoResult")]
        public int ID { get; set; }

        public DateTime InputDateTime { get; private set; } = DateTime.Now;
        public short Card { get; set; }

        #region Hệ số FLAT
        public int MainProfit { get; set; }
        public int Profit0 { get;  set; }
        public int Profit1 { get;  set; }
        public int Profit2 { get;  set; }
        public int Profit3 { get;  set; }
        public int AllSubProfit { get;  set; }
        #endregion

        #region Hệ số thay đổi
        public int ModMainProfit { get; set; }
        public int ModProfit0 { get; set; }
        public int ModProfit1 { get; set; }
        public int ModProfit2 { get; set; }
        public int ModProfit3 { get; set; }
        public int ModAllSubProfit { get; set; }
        #endregion

        public string ListCurrentPredicts { get; set; }

        public string ListCurrentModCoeffs { get; set; }

        /// <summary>
        /// Tổng cuả tất cả các lợi nhuận 
        /// </summary>
        [NotMapped]
        public int GlobalProfit 
        { 
            get
            { 
                return MainProfit + Profit0 + Profit1 + Profit2 + Profit3 + AllSubProfit;
            } 
        }

        

        /// <summary>
        /// Số thứ tự các bước, không được thay đổi
        /// Mỗi bàn có 1 số thứ tự riêng
        /// </summary>        
        public int GlobalIndex { get; set; }

        public int AutoSessionID { get; set; }
        public virtual AutoSession AutoSession { get; set; }

        public virtual AutoResult AutoResult{ get; set; }
    }
}
