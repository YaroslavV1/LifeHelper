using FluentValidation;
using LifeHelper.Services.Areas.SubNote.DTOs;
using LifeHelper.Services.Areas.User.Validators;

namespace LifeHelper.Services.Areas.SubNote.Validators;

public class SubNoteInputValidator : AbstractValidator<SubNoteInputDto>
{
    public SubNoteInputValidator()
    {
        RuleFor(note => note.Title)
            .IsRequired()
            .Length(5, 150).WithMessage("Length of the title must be from {MinLength} - {MaxLength} characters");
    }
}