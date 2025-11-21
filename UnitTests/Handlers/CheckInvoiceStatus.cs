
using Application.UseCases.InvoiceUseCases.Commands;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace UnitTests.Handlers
{
    public class CheckInvoiceStatus
    {
        [Fact(DisplayName = "CheckInvoiceStatus | Should return cached response if exists using in-memory list")]
        public async Task CheckInvoiceStatus_Should_Return_Cached_Response_FromList()
        {
            string taxNumber = "1234567890";
            string invoiceNumber = "INV2025000000043";
            string cacheKey = $"{taxNumber}-{invoiceNumber}";

            List<GIBStatusLog> invoices =
            [

                new GIBStatusLog
                {


                    Id = 1,
                    InvoiceNumber = invoiceNumber,
                    TaxNumber = taxNumber,
                    InvoiceStatus = Status.Approved,
                    Message = "Fatura onaylandı",
                    RequestTime = DateTime.UtcNow
                }
            ];

            MemoryCache memoryCache = new(new MemoryCacheOptions());
            MemoryCacheService cacheService = new(memoryCache);

            CheckInvoiceStatusCommand command = new(invoiceNumber, taxNumber);

            CheckInvoiceStatusResponse response = await cacheService.GetOrCreateAsync<CheckInvoiceStatusResponse>(cacheKey, async () =>
            {
                GIBStatusLog? invoice = invoices.FirstOrDefault(x => x.InvoiceNumber == invoiceNumber && x.TaxNumber == taxNumber);

                if (invoice == null)
                    throw new Exception("Fatura bulunamadı.");

                return new CheckInvoiceStatusResponse
                {
                    InvoiceNumber = invoice.InvoiceNumber,
                    TaxNumber = invoice.TaxNumber,
                    ResponseCode = invoice.InvoiceStatus.ToString(),
                    ResponseMessage = invoice.Message,
                };
            });

            Assert.NotNull(response);
            Assert.Equal(invoiceNumber, response.InvoiceNumber);
            Assert.Equal(taxNumber, response.TaxNumber);
            Assert.Equal("Approved", response.ResponseCode);
            Assert.Equal("Fatura onaylandı", response.ResponseMessage);

            CheckInvoiceStatusResponse cachedResponse = await cacheService.GetOrCreateAsync<CheckInvoiceStatusResponse>(cacheKey);
        }

        [Fact(DisplayName = "CheckInvoiceStatusResponse | Should correctly set properties")]
        public void CheckInvoiceStatusResponse_Should_Assign_Properties_Correctly()
        {
            string expectedInvoiceNumber = "INV2025000000043";
            string expectedTaxNumber = "1234567890";
            string expectedResponseCode = "Approved";
            string expectedResponseMessage = "Fatura onaylandı";

            CheckInvoiceStatusResponse response = new()
            {
                InvoiceNumber = expectedInvoiceNumber,
                TaxNumber = expectedTaxNumber,
                ResponseCode = expectedResponseCode,
                ResponseMessage = expectedResponseMessage,
            };

            Assert.NotNull(response);
            Assert.Equal(expectedInvoiceNumber, response.InvoiceNumber);
            Assert.Equal(expectedTaxNumber, response.TaxNumber);
            Assert.Equal(expectedResponseCode, response.ResponseCode);
            Assert.Equal(expectedResponseMessage, response.ResponseMessage);
        }
    }
}
