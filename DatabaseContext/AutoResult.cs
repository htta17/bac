
namespace DatabaseContext
{  

    /// <summary>
    /// Kết quả chung, được sử dụng cho tất cả các giải thuật
    /// </summary>
    public class AutoResult
    {
        public int ID { get; set; }
        public short Card { get; set; }
        public System.DateTime InputDateTime { get; private set; } = System.DateTime.Now;
        public int AutoSessionID { get; set; }    
        public virtual AutoSession AutoSession { get; set; }

        public virtual AutoRoot AutoRoot { get; set; }
    }
}
