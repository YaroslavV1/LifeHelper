using AutoMapper;

namespace LifeHelper.Services.Areas.Notes.DTOs;

using Infrastructure.Entities;

public class NoteMappingProfile : Profile
{
    public NoteMappingProfile()
    {
        CreateMap<Note, NoteDto>();
        CreateMap<NoteInputDto, Note>();
    }
}