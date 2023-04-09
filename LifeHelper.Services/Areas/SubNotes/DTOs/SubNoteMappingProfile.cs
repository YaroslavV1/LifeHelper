using AutoMapper;
using LifeHelper.Infrastructure.Entities;

namespace LifeHelper.Services.Areas.SubNotes.DTOs;

public class SubNoteMappingProfile : Profile
{
    public SubNoteMappingProfile()
    {
        CreateMap<SubNote, SubNoteDto>();
        CreateMap<SubNoteInputDto, SubNote>()
            .ForMember(subNote => subNote.NoteId, option => option.MapFrom(subInput => subInput.NoteId));
    }
}