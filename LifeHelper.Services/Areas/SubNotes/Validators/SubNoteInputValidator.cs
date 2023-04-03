using FluentValidation;
using LifeHelper.Services.Areas.Helpers.Validators;
using LifeHelper.Services.Areas.SubNotes.DTOs;

namespace LifeHelper.Services.Areas.SubNotes.Validators;

public class SubNoteInputValidator : AbstractValidator<SubNoteInputDto>
{
    public SubNoteInputValidator()
    {
        RuleFor(note => note.Title)
            .IsRequired()
            .Length(5, 150).WithMessage("Length of the title must be from {MinLength} - {MaxLength} characters");
    }
}