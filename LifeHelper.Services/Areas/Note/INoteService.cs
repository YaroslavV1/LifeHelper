using LifeHelper.Services.Areas.Note.DTOs;

namespace LifeHelper.Services.Areas.Note;

public interface INoteService
{
    public Task<IList<NoteDto>> GetListAsync(bool isDescending);
    public Task<NoteDto> GetByIdAsync(int id);
    public Task<NoteDto> CreateAsync(NoteInputDto noteInput);
    public Task<NoteDto> UpdateByIdAsync(int id, NoteInputDto noteInput);
    public Task DeleteByIdAsync(int id);
}