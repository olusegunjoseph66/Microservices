using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Wallet.Application.Validations.Request
{
    public class FormFileValidator : AbstractValidator<IFormFile>
    {

        private const int ImageMaxSize = 1 * 1024 * 1024;
        private const int MaxSize = 10 * 1024 * 1024;
        public FormFileValidator()
        {
            RuleFor(x => x.Length).NotNull().LessThanOrEqualTo(MaxSize)
               .WithMessage($"File is too large, upload a file less than 10 MB");

            RuleFor(x => x.ContentType).NotNull()
                .Must(x => x.Equals("image/jpeg") || x.Equals("image/jpg") || x.Equals("image/png") || x.Equals("video/mp4"))
                .WithMessage("{PropertyName} not supported");

            RuleFor(x => x.Length).NotNull()
                .When(x => x.ContentType.Equals("image/jpeg") || x.ContentType.Equals("image/jpg") || x.ContentType.Equals("image/png"))
                .LessThanOrEqualTo(ImageMaxSize).WithMessage("File is too large, try upload image less than 1 MB");

            RuleFor(x => x.Length).NotNull()
                .When(x => x.ContentType.Equals("video/mp4") || x.ContentType.Equals("video/mp4"))
                .LessThanOrEqualTo(MaxSize).WithMessage("File is too large, try upload video file less than 10 MB");

        }
    }
}
