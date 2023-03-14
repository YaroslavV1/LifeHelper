using AutoMapper;

namespace LifeHelper.Services.Areas.Archive.DTOs;

using Infrastructure.Entities;

public class ArchiveNoteMappingProfile : Profile
{
    public ArchiveNoteMappingProfile()
    {
        CreateMap<ArchiveNote, ArchivedNoteDto>()
            .ForMember(archiveNote => archiveNote.ArchivedSubNotes,
                options => options.MapFrom(archiveDto => archiveDto.ArchiveSubNotes));

        CreateMap<Note, ArchiveNote>()
            .ForMember(arcNote => arcNote.Id, options => options.Ignore())
            .ForMember(arcNote => arcNote.ArchiveSubNotes, options => options.MapFrom(note => note.SubNotes));

        CreateMap<ArchiveNote, Note>()
            .ForMember(note => note.Id, options => options.Ignore())
            .ForMember(note => note.SubNotes, options => options.MapFrom(note => note.ArchiveSubNotes));
    }
}