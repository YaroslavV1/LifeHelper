using LifeHelper.Services.Areas.Archive.DTOs;

namespace LifeHelper.Services.Areas.Archive;

public interface IArchiveService
{
    public Task ArchiveByIdAsync(int noteId);
    public Task UnArchiveByIdAsync(int archiveNoteId);
    public Task<IList<ArchivedNoteDto>> GetListAsync(bool isDescending);
    public Task<ArchivedNoteDto> GetByIdAsync(int archiveNoteId);
    public Task DeleteByIdAsync(int archiveNoteId);
}