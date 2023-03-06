using FluentValidation;
using LifeHelper.Services.Areas.Note.DTOs;

namespace LifeHelper.Services.Areas.Note.Validators;

public class NoteInputValidator : AbstractValidator<NoteInputDto>
{
    public NoteInputValidator()
    {
        RuleFor(note => note.Title)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MinimumLength(5).WithMessage("Minimum number of characters - {MinLength}")
            .MaximumLength(150).WithMessage("Maximum number of characters - {MaxLength}");
    }
}