using LifeHelper.Services.Areas.SubNotes.DTOs;

namespace LifeHelper.Services.Areas.SubNotes;

public interface ISubNoteService
{
    public Task<IList<SubNoteDto>> GetListAsync(int noteId);
    public Task<SubNoteDto> GetByIdAsync(int noteId, int subNoteId);
    public Task<SubNoteDto> CreateAsync(SubNoteInputDto subNoteInputDto);
    public Task<SubNoteDto> UpdateByIdAsync(int subNoteId, SubNoteInputDto subNoteInputDto);
    public Task DeleteByIdAsync(int noteId, int subNoteId);
}