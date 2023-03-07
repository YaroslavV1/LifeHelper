using LifeHelper.Services.Areas.Note.DTOs;

namespace LifeHelper.Services.Areas.Note;

public interface INoteService
{
    public Task<IList<NoteDto>> GetListAsync(bool isDescending, int userId);
    public Task<NoteDto> GetByIdAsync(int id, int userId);
    public Task<NoteDto> CreateAsync(NoteInputDto noteInput, int userId);
    public Task<NoteDto> UpdateByIdAsync(int id, NoteInputDto noteInput, int userId);
    public Task DeleteByIdAsync(int id, int userId);
}