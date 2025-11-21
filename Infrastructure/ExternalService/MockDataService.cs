namespace Infrastructure.ExternalService
{
    public static class MockDataService
    {
        private static readonly List<GIBStatusLog> GIBStatusLogData = [
            new GIBStatusLog
            {
                Id = 1,
                InvoiceNumber = "EFT2025000000011",
                TaxNumber = "1234567890",
                InvoiceStatus = Status.Approved,
                Message = "Fatura başarıyla işlendi.",
                RequestTime = DateTime.Now.AddMinutes(-15)
            },
            new GIBStatusLog
            {
                Id = 2,
                InvoiceNumber = "EFT2025000000003",
                TaxNumber = "9876543210",
                InvoiceStatus = Status.Approved,
                Message = "Başarılı.",
                RequestTime = DateTime.Now.AddMinutes(-10)
            },
            new GIBStatusLog
            {
                Id = 3,
                InvoiceNumber = "SBL2025000000043",
                TaxNumber = "5566778899",
                   InvoiceStatus =Status.Rejected,
                Message = "Mükellef bulunamadı.",
                RequestTime = DateTime.Now.AddMinutes(-7)
            },
            new GIBStatusLog
            {
                Id = 4,
                InvoiceNumber = "HKM2025000000052",
                TaxNumber = "4455667788",
                 InvoiceStatus =Status.Rejected,
                Message = "Geçersiz belge numarası.",
                RequestTime = DateTime.Now.AddMinutes(-4)
            },
            new GIBStatusLog
            {
                Id = 5,
                InvoiceNumber = "EFT2025000000005",
                TaxNumber = "9988776550",
                InvoiceStatus =Status.SystemError,
                Message = "Mali mühür doğrulanamadı.",
                RequestTime = DateTime.Now.AddMinutes(-1)
            }
                ];
        public static List<GIBStatusLog> GIBStatusList = GIBStatusLogData;
    }

}
public class GIBStatusLog
{
    public long Id { get; set; }
    public string? InvoiceNumber { get; set; }
    public string? TaxNumber { get; set; }
    public Status InvoiceStatus { get; set; }
    public string? Message { get; set; }
    public DateTime RequestTime { get; set; }
}

public enum Status
{
    SystemError,
    Rejected,
    Blocked,
    Approved
}