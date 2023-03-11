using AutoMapper;

namespace LifeHelper.Services.Areas.SubNote.DTOs;

using Infrastructure.Entities;

public class SubNoteMappingProfile : Profile
{
    public SubNoteMappingProfile()
    {
        CreateMap<SubNote, SubNoteDto>();
        CreateMap<SubNoteInputDto, SubNote>()
            .ForMember(subNote => subNote.NoteId, option => option.MapFrom(subInput => subInput.NoteId));
    }
}