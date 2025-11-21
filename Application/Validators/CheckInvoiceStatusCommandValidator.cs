using Application.UseCases.InvoiceUseCases.Commands;
using FluentValidation;
namespace Application.Validators
{
    public class CheckInvoiceStatusCommandValidator : AbstractValidator<CheckInvoiceStatusCommand>
    {
        public CheckInvoiceStatusCommandValidator()
        {
            RuleFor(x => x.InvoiceNumber)
                .NotEmpty().WithMessage("Fatura numarası boş olamaz.")
                .Length(16).WithMessage("Fatura numarası 16 haneli olmalıdır.");

            RuleFor(x => x.TaxNumber)
                .NotEmpty().WithMessage("Vergi kimlik numarası boş olamaz.")
                .Length(10).WithMessage("Vergi kimlik numarası 10 haneli olmalıdır.")
                .Matches(@"^\d+$").WithMessage("Vergi kimlik numarası sadece rakamlardan oluşmalıdır.");
        }
    }
}
