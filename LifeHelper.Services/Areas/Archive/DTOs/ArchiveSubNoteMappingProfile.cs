using AutoMapper;
using LifeHelper.Infrastructure.Entities;

namespace LifeHelper.Services.Areas.Archive.DTOs;

public class ArchiveSubNoteMappingProfile : Profile
{
    public ArchiveSubNoteMappingProfile()
    {
        CreateMap<ArchiveSubNote, ArchivedSubNoteDto>();
        
        CreateMap<SubNote, ArchiveSubNote>()
            .ForMember(arcSubNote => arcSubNote.Id, options => options.Ignore())
            .ForMember(arcSubNote => arcSubNote.ArchiveNote, options => options.MapFrom(subNote => subNote.Note))
            .ForMember(arcSubNote => arcSubNote.ArchiveNoteId, options => options.MapFrom(subNote => subNote.NoteId));
        
        CreateMap<ArchiveSubNote, SubNote>()
            .ForMember(note => note.Id, options => options.Ignore())
            .ForMember(note => note.Note, options => options.MapFrom(subNote => subNote.ArchiveNote))
            .ForMember(note => note.NoteId, options => options.MapFrom(subNote => subNote.ArchiveNoteId));
    }
}