namespace BaskentExpressApi.Model
{
    public class RezervasyonPost
    {
        public Tren Tren { get; set; }
        public int RezervasyonYapilacakKisiSayisi { get; set; }
        public bool KisilerFarkliVagonlaraYerlestirilebilir { get; set; }
    }

    public class Tren
    {
        public string Ad { get; set; }
        public List<Vagon> Vagonlar { get; set; }

    }

    public class Vagon
    {
        public string Ad { get; set; }
        public int Kapasite { get; set; }
        public int DoluKoltukAdet { get; set; }
    }
}
