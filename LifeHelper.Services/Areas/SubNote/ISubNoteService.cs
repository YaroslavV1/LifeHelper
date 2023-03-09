using LifeHelper.Services.Areas.SubNote.DTOs;

namespace LifeHelper.Services.Areas.SubNote;

using Infrastructure.Entities;

public interface ISubNoteService
{
    public Task<IList<SubNoteDto>> GetListAsync(int noteId);
    public Task<SubNoteDto> GetByIdAsync(int noteId, int id);
    public Task<SubNoteDto> CreateAsync(SubNoteInputDto subNoteInputDto);
    public Task<SubNoteDto> UpdateByIdAsync(int id, SubNoteInputDto subNoteInputDto);
    public Task DeleteByIdAsync(int noteId, int id);
}