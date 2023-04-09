using LifeHelper.Services.Areas.Notes.DTOs;

namespace LifeHelper.Services.Areas.Notes;

public interface INoteService
{
    public Task<IList<NoteDto>> GetListAsync(bool? inputFilter);
    public Task<NoteDto> GetByIdAsync(int id);
    public Task<NoteDto> CreateAsync(NoteInputDto noteInput);
    public Task<NoteDto> UpdateByIdAsync(int id, NoteInputDto noteInput);
    public Task DeleteByIdAsync(int id);
}