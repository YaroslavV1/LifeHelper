using FluentValidation;
using LifeHelper.Services.Areas.Notes.DTOs;
using LifeHelper.Services.Extensions;

namespace LifeHelper.Services.Areas.Notes.Validators;

public class NoteInputValidator : AbstractValidator<NoteInputDto>
{
    public NoteInputValidator()
    {
        RuleFor(note => note.Title)
            .IsRequired()
            .Length(5, 150).WithMessage("Length of the title must be from {MinLength} - {MaxLength} characters");
    }
}