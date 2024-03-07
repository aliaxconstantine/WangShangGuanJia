using Org.BouncyCastle.Asn1.Cms;


namespace Electric.Class.DAO
{
    public class EleOrder:EleDAO
    {
        public override int id { get; set; }
        public string platform { get; set; }
        public string commoidty { get; set; }
        public string purchaser { get; set; }
        public string time { get; set; }
        public int quarter { get;set; }
        public int year { get; set; }
        public int num { get; set; }
        public string state { get;set; }
        public EleOrder() { }
    }
}
