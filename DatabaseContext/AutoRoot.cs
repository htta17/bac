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
        public int MainProfit { get; set; }
        public int Profit0 { get;  set; }
        public int Profit1 { get;  set; }
        public int Profit2 { get;  set; }
        public int Profit3 { get;  set; }
        public int AllSubProfit { get;  set; }
        public string ListCurrentPredicts { get; set; }

        /// <summary>
        /// Các hệ số tính đến thời điểm này
        /// </summary>
        //public string ListModCoeffs { get; set; }

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
