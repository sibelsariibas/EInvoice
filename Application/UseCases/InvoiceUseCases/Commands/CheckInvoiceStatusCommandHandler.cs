using Entity;
using Infrastructure.ExternalService;
using MediatR;
using Persistence;
using System.Data.Entity;

namespace Application.UseCases.InvoiceUseCases.Commands
{
    public class CheckInvoiceStatusCommand : IRequest<CheckInvoiceStatusResponse>
    {
        public string InvoiceNumber { get; }
        public string TaxNumber { get; }
        public CheckInvoiceStatusCommand(string invoiceNumber, string taxNumber)
        {
            InvoiceNumber = invoiceNumber;
            TaxNumber = taxNumber;
        }
    }
    public class CheckInvoiceStatusResponse
    {
        public string? InvoiceNumber { get; set; }
        public string? TaxNumber { get; set; }
        public string? ResponseCode { get; set; }
        public string? ResponseMessage { get; set; }
    }

    public class CheckInvoiceStatusCommandHandler : IRequestHandler<CheckInvoiceStatusCommand, CheckInvoiceStatusResponse>
    {
        private readonly ICacheService _cacheService;
        private readonly Context _context;
        public CheckInvoiceStatusCommandHandler(ICacheService cacheService, Context context)
        {
            _cacheService = cacheService;
            _context = context;
        }

        public async Task<CheckInvoiceStatusResponse> Handle(CheckInvoiceStatusCommand request, CancellationToken cancellationToken)
        {
            CheckInvoiceStatusResponse response;
            string cacheKey = $"{request.TaxNumber}-{request.InvoiceNumber}";

            IEnumerable<InvoiceStatusLog> invoiceLogs = _context.InvoiceStatusLogs
                .Where(x => x.InvoiceNumber == request.InvoiceNumber && x.TaxNumber == request.TaxNumber)
                .OrderByDescending(x => x.RequestTime).Take(2).ToList();

            if (invoiceLogs.Count() > 0 && invoiceLogs.All(x => x.ResponseCode == "Rejected"))
            {
                InvoiceStatusLog latestLog = invoiceLogs.First();
                response = new CheckInvoiceStatusResponse
                {

                    InvoiceNumber = latestLog.InvoiceNumber,
                    TaxNumber = latestLog.TaxNumber,
                    ResponseCode = "Blocked",
                    ResponseMessage = "Bu faturaya ait art arda 2 red cevabı alındı. Manuel inceleme gerekiyor."
                };
            }

            else
            {
                response = await _cacheService.GetOrCreateAsync<CheckInvoiceStatusResponse>(cacheKey);
                if (response == null)
                {
                    GIBStatusLog? invoice = MockDataService.GIBStatusList.FirstOrDefault(
                        x => x.InvoiceNumber == request.InvoiceNumber &&
                        x.TaxNumber == request.TaxNumber)
                        ?? throw new Exception("Belirtilen fatura numarası ve vergi kimlik numarası ile fatura kaydı bulunamadı.");

                    response = new CheckInvoiceStatusResponse
                    {
                        InvoiceNumber = invoice.InvoiceNumber,
                        TaxNumber = invoice.TaxNumber,
                        ResponseCode = invoice.InvoiceStatus.ToString(),
                        ResponseMessage = invoice.Message
                    };
                }

            }

            InvoiceStatusLog invoiceStatusLog = InvoiceStatusLog.Create(
                    request.InvoiceNumber,
                    request.TaxNumber,
                    response.ResponseCode ?? string.Empty,
                    response.ResponseMessage ?? string.Empty);

            _context.InvoiceStatusLogs.Add(invoiceStatusLog);
            await _context.SaveChangesAsync();

            _cacheService.Set(cacheKey, response);

            return response;
        }
    }
}





