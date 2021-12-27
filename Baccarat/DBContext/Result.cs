
namespace Midas
{  
    public class Result
    {
        public int ID { get; set; }
        public short Card { get; set; }
        public System.DateTime InputDateTime { get; set; }
        public int SessionID { get; set; }    
        public virtual Session Session { get; set; }
    }
}
