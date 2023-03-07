using FluentValidation;
using LifeHelper.Services.Areas.Note.DTOs;

namespace LifeHelper.Services.Areas.Note.Validators;

public class NoteInputValidator : AbstractValidator<NoteInputDto>
{
    public NoteInputValidator()
    {
        RuleFor(note => note.Title)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .Length(5, 150).WithMessage("Length of the title must be from {MinLength} - {MaxLength} characters");
    }
}