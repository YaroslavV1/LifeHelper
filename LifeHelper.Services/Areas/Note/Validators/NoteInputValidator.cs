using FluentValidation;
using LifeHelper.Services.Areas.Note.DTOs;
using LifeHelper.Services.Areas.User.Validators;

namespace LifeHelper.Services.Areas.Note.Validators;

public class NoteInputValidator : AbstractValidator<NoteInputDto>
{
    public NoteInputValidator()
    {
        RuleFor(note => note.Title)
            .IsRequired()
            .Length(5, 150).WithMessage("Length of the title must be from {MinLength} - {MaxLength} characters");
    }
}