using System.ComponentModel.DataAnnotations;

namespace Entity
{
    public class InvoiceStatusLog
    {
        [Key]
        public long Id { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? TaxNumber { get; set; }
        public string? ResponseCode { get; set; }
        public string? ResponseMessage { get; set; }
        public DateTime RequestTime { get; set; }

        public static InvoiceStatusLog Create(string invoiceNumber, string taxNumber, string responseCode, string responseMessage)
        {
            return new InvoiceStatusLog
            {
                InvoiceNumber = invoiceNumber,
                TaxNumber = taxNumber,
                ResponseCode = responseCode,
                ResponseMessage = responseMessage,
                RequestTime = DateTime.Now
            };
        }
    }
}
