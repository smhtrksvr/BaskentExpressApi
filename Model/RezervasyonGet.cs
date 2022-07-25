namespace BaskentExpressApi.Model
{
    public class RezervasyonGet
    {
        public bool RezervasyonYapilabilir { get; set; }
        public List<Yerlesim> YerlesimAyrinti { get; set; }
    }

    public class Yerlesim
    {
        public string VagonAdi { get; set; }
        public int KisiSayisi { get; set; }
    }
}
