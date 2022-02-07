# Rehber Uygulaması


## Kullanılan Teknolojiler
 
 * ORM :  EntityFramework Core
 * Database : PostgreSQL 14
 * Mqtt Broker : MqttNet Library

## Nasıl Çalıştırılır ?

- Veritabanlarının Oluşturulması

Veritabanları oluşturulmadan önce solution'da bulunan ContactService.API ve ReportService.API projeleri 
içinde bulunan appsettings.json dosyalarında veritabanı için connection string konfigurasyonu yapılmalıdır.

Örnek Konfigurasyon:

    "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=ContactServiceDB;Username=postgres;Password=123456"
    }

- EndPoint Konfigurasyonu 

Daha sonra ReportService.API ve PhoneBook.UI projelerindeki appsettings.json dosyasında ContactEndPoint
 ve ReportEndPoint adresleri kontrol edilmeli. Belirtilen servislerin çalıştığı portlarla eşleşmesi gerekmektedir.


 ## Proje Çalışma Mantığı

### Kişiler

* Proje ilk açıldığında rehberde kayıtlı kişiler listelenir.
* Bu kişiler sil butonu ile kaldırılabilir.
* Detay butonu ile kişilerin detaylı bilgilerinin bulunduğu sayfaya yönlendirilebilir.
* Detay sayfasında birden fazla iletişim bilgisi ekleme seçeneği sunulmuştur.
* Detay sayfasında istenilen iletişim bilgisi kaldırılabilir.

### Raporlar
* Raporlar sayfasında mevcut oluşturulmuş raporlar listelenir.
* Raporun tamamlanıp tamamlandmadığı bu sayfada gözlemlenebilir.
* Rapor oluştur butonuyla belirlenen konuma göre rapor oluşturulur.
* Oluşturulan rapor "hazırlanıyor" olarak işaretlenir ve veritabanına kaydı yapılır.
* Hazırlanan rapor Mqtt Broker kuyruğuna eklenir.
* Worker Servis bu kuyruğu dinler ve seçilen konuma göre kişi sayısı ve telefon numarası sayısını almak için contact servisine istek yapar.
* Bu bilgileri doldurduktan sonra rapor için bir excel dosyası oluşturulur.
* Daha önce hazırlanıyor olarak eklenen rapor worker servis tarafından tamamlandı olarak işaretlenir.
* Eğer rapor tamamlandıysa raporun içeriği detay sayfasında gözlemlenebilir.
