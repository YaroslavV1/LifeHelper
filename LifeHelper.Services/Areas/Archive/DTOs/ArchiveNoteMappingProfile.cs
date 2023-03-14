using AutoMapper;

namespace LifeHelper.Services.Areas.Archive.DTOs;

using Infrastructure.Entities;

public class ArchiveNoteMappingProfile : Profile
{
    public ArchiveNoteMappingProfile()
    {
        CreateMap<ArchiveNote, ArchiveNoteDto>()
            .ForMember(archNote => archNote.ArchivedSubNotes,
                options => options.MapFrom(archDto => archDto.ArchiveSubNotes));

        CreateMap<Note, ArchiveNote>()
            .ForMember(archNote => archNote.Id, options => options.Ignore())
            .ForMember(archNote => archNote.ArchiveSubNotes, options => options.MapFrom(note => note.SubNotes));

        CreateMap<ArchiveNote, Note>()
            .ForMember(note => note.Id, options => options.Ignore())
            .ForMember(note => note.SubNotes, options => options.MapFrom(note => note.ArchiveSubNotes));
    }
}