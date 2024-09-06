namespace CogentCode.Template.Application.Categories
{
    using CogentCode.Template.Application.Contracts;
    using CogentCode.Template.Domain;
    using FluentValidation;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public record UpdateCategoryCommand(Guid Id, string Name, string Description, string Color, string Icon, decimal Amount) : IRequest;

    internal sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            this.RuleFor(r => r.Name).NotEmpty();

            this.RuleFor(r => r.Description).NotEmpty();

            this.RuleFor(r => r.Color).NotEmpty();

            this.RuleFor(r => r.Icon).NotEmpty();

            this.RuleFor(r => r.Amount).GreaterThan(0);
        }
    }

    internal sealed class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            Category category = await this.unitOfWork.Categories.GetByIdAsync(request.Id, cancellationToken);

            category.Update(
                request.Name,
                request.Description,
                request.Color,
                request.Icon,
                request.Amount);

            this.unitOfWork.Categories.Update(category);

            await this.unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
