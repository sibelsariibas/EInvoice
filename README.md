# EInvoice - InvoiceStatusCheck
InvoiceStatusCheck, e-Fatura durum sorgulama işlemleri için geliştirilmiş bir **.NET Core 8 API** projesidir.  
Proje **Onion mimarisi** ve **katmanlı mimari** kullanılarak tasarlanmıştır.
---
## Kullanılan Teknolojiler

- **.NET Core 8**  
- **Entity Framework Core (Code First)** ve **MSSQL**  
- **MediatR** ile CQRS yaklaşımı  
- **.NET MemoryCache** ile caching  
- **FluentValidation** ile request doğrulama  
- **Middleware** ile request-response loglama (console)  
- **Unit Tests** (xUnit)  
- **Katmanlı Mimari** (Layered Architecture)
---

## Proje Yapısı

- **Core**: Entity ve DTO modelleri, validatorlar, iş mantığı arayüzleri  
- **Infrastructure**: DbContext, repository, MemoryCache, MockDataService  
- **Application**: Command ve Query handlerlar, MediatR, loglama  
- **API**: Controller ve middleware  
Gerçek veri işlemleri **MSSQL’deki InvoiceStatusLog tablosuna** kaydedilir.
---
## Kurulum

1. **Projeyi klonlayın:**
 ```
git clone <repository-url>
cd InvoiceStatusCheck
```
Veritabanı bağlantısını ayarlayın:
 ```
appsettings.json içindeki ConnectionStrings bölümünü kendi MSSQL veritabanınıza göre değiştirin.
 ```
2. **Migration oluşturun:**
 ```
add-migration MyMigration
 ```

3. **Veritabanını Güncelleyin:**
 ```
update-database
Not: Proje Code First yaklaşımıyla geliştirildiği için, migration oluşturulmadan veritabanı tabloları oluşmaz.
 ```

## Kullanım
API, MediatR üzerinden CQRS pattern ile çalışır.
Tüm request ve response logları middleware aracılığıyla console’a yazdırılır.
MockDataService, geliştirme ve test aşamasında veri sağlamak için kullanılır; fakat son veri InvoiceStatusLog tablosuna kaydedilir.
