using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace WorldTravel.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<WorldTravelWebModule>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.InitializeApplication();
        }
    }
}

//tüm imagelere Alt ve title ver.

//GEREKSIZ DLLLERI CSS RESIM VS KALDIr.
//PROJE SONUNDA eng.json doldur.


//LAYOUTTA >>    @Html.Partial("_Scripts") YAP VE SYTYLE ICINDE YAP


//  ülke güncelleme vs 
// county Detailda resim ve videoları görünmesi.

//popüler hizmet bolumune > modal ekle statik değerler al.

//Yorum bolumu ekle

//goruntulenme sayısını artır.


//VİDEO BOLUMUNE EXT EKLE , PAGE AFTER EKLE POST EKLE ACCOU:NT MANAGEDE VAR


// DIREK YAP IL KBAKTIGINDA >> CIZE TURLERI VE IS FIRSATLARI ANA SAYFA RESIMELR COK BUYUK CCOUTRY GIBI YAP