using Application.UseCases.InvoiceUseCases.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {

        private readonly IMediator _mediator;
        public InvoiceController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Belirtilen fatura numarası ve vergi numarasına göre fatura durumunu kontrol eder.
        /// </summary>
        /// <param name="invoiceNumber">Kontrol edilecek fatura numarası.</param>
        /// <param name="taxNumber">Faturaya ait vergi numarası.</param>
        /// <returns>Fatura durum bilgisini içeren <see cref="CheckInvoiceStatusResponse"/> nesnesi.</returns>
        [HttpPost("check")]
        public Task<CheckInvoiceStatusResponse> InvoiceCheckStatus(string invoiceNumber, string taxNumber)
        {
            CheckInvoiceStatusCommand request = new(invoiceNumber, taxNumber);
            return _mediator.Send(request);
        }
    }
}
