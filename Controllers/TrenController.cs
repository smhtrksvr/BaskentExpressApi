using BaskentExpressApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaskentExpressApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrenController : ControllerBase
    {
        private bool CheckParameter(RezervasyonPost rezervasyonPost)
        {
            if(rezervasyonPost.RezervasyonYapilacakKisiSayisi <= 0) return false;

            foreach (var item in rezervasyonPost.Tren.Vagonlar)
            {
                if (item.Kapasite < 0) return false;
                if (item.DoluKoltukAdet < 0) return false;
            }

            return true;

        }

        [HttpPost]
        public RezervasyonGet Index(RezervasyonPost rezervasyonPost)
        {
            if (!CheckParameter(rezervasyonPost))
            {
                return (new RezervasyonGet()
                {
                    RezervasyonYapilabilir = false,
                    YerlesimAyrinti = new List<Yerlesim>()
                });
            };

            var vagonlar = rezervasyonPost.Tren.Vagonlar;

            foreach (var vagon in vagonlar)
            {
                vagon.Kapasite = Convert.ToInt32(Convert.ToDouble(vagon.Kapasite) * 0.70);

                //if(vagon.Kapasite < 0)
                //{
                //    vagonlar.Remove(vagon);
                //    continue;
                //}
            }

            if(rezervasyonPost.KisilerFarkliVagonlaraYerlestirilebilir == false)
            {
                var olumlu = false;

                foreach (var vagon in vagonlar)
                {
                    if ((vagon.DoluKoltukAdet + rezervasyonPost.RezervasyonYapilacakKisiSayisi) <= vagon.Kapasite)
                    {
                        olumlu = true;
                        break;
                    }
                }

                if (!olumlu)
                {
                    return (new RezervasyonGet()
                    {

                        RezervasyonYapilabilir = false,
                        YerlesimAyrinti = new List<Yerlesim>()
                    });
                }
            }
            else
            {
                var olumlu = false;
                int dagitilacakKisi = rezervasyonPost.RezervasyonYapilacakKisiSayisi;

                var rezerveEdilenVagonlar = new List<Yerlesim>();

                foreach (var vagon in vagonlar)
                {
                    // Boş Yer
                    int vagonGuncelKapasite = vagon.Kapasite - vagon.DoluKoltukAdet;

                    if(vagonGuncelKapasite <= 0) continue;


                    if(vagonGuncelKapasite >= dagitilacakKisi)
                    {
                        rezerveEdilenVagonlar.Add(new Yerlesim { KisiSayisi = dagitilacakKisi, VagonAdi = vagon.Ad });
                        dagitilacakKisi = 0;
                    }
                    else
                    {
                        rezerveEdilenVagonlar.Add(new Yerlesim { KisiSayisi = vagonGuncelKapasite, VagonAdi = vagon.Ad });
                        dagitilacakKisi -= vagonGuncelKapasite;
                    }

                    if(dagitilacakKisi == 0)
                    {
                        return new RezervasyonGet()
                        {
                            RezervasyonYapilabilir = true,
                            YerlesimAyrinti = rezerveEdilenVagonlar
                        };
                    }
                }

                if (!olumlu)
                {
                    return (new RezervasyonGet()
                    {

                        RezervasyonYapilabilir = false,
                        YerlesimAyrinti = new List<Yerlesim>()
                    });
                }
            }

            return (new RezervasyonGet()
            {

                RezervasyonYapilabilir = false,
                YerlesimAyrinti = new List<Yerlesim>()
            });
        }
    }
}
