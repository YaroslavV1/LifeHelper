using AutoMapper;
using AutoMapper.QueryableExtensions;
using LifeHelper.Infrastructure;
using LifeHelper.Infrastructure.Entities;
using LifeHelper.Services.Areas.Archive.DTOs;
using LifeHelper.Services.Utilities;
using LifeHelper.Services.Utilities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static LifeHelper.Services.Utilities.LifeHelperUtilities;

namespace LifeHelper.Services.Areas.Archive;

public class ArchiveService : IArchiveService
{
    private readonly LifeHelperDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly TokenInfoDto _currentUserInfo;

    public ArchiveService(LifeHelperDbContext dbContext, IMapper mapper, IHttpContextAccessor contextAccessor)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _currentUserInfo = ParseInfoFromClaims(contextAccessor.HttpContext);
    }
    
    public async Task ArchiveByIdAsync(int noteId)
    {
        var note = await _dbContext.Notes
            .Include(note => note.SubNotes)
            .FirstOrDefaultAsync(note => note.Id == noteId && _currentUserInfo.Id == note.UserId);
        
        note.ThrowIfNotFound(noteId);

        var archiveNote = _mapper.Map<ArchiveNote>(note);

        await _dbContext.AddAsync(archiveNote);
        _dbContext.Notes.Remove(note);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UnArchiveByIdAsync(int archiveNoteId)
    {
        var archiveNote = await _dbContext.ArchiveNotes
            .Include(archiveNote => archiveNote.ArchiveSubNotes)
            .FirstOrDefaultAsync(archiveNote =>
                archiveNote.Id == archiveNoteId && _currentUserInfo.Id == archiveNote.UserId);
        
        archiveNote.ThrowIfNotFound(archiveNoteId);

        var note = _mapper.Map<Note>(archiveNote);

        await _dbContext.AddAsync(note);
        _dbContext.ArchiveNotes.Remove(archiveNote);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IList<ArchiveNoteDto>> GetListAsync(bool isDescending)
    {
        var archiveNotes = _dbContext.ArchiveNotes
            .Include(archiveNote => archiveNote.ArchiveSubNotes)
            .Where(archiveNote => _currentUserInfo.Id == archiveNote.UserId);
        
        archiveNotes = isDescending 
            ? archiveNotes.OrderByDescending(arcNote => arcNote.CreatedDate) 
            : archiveNotes.OrderBy(arcNote => arcNote.CreatedDate);

        return await archiveNotes.ProjectTo<ArchiveNoteDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<ArchiveNoteDto> GetByIdAsync(int archiveNoteId)
    {
        var archiveNoteDto = await _dbContext.ArchiveNotes
            .Include(archiveNote => archiveNote.ArchiveSubNotes)
            .Where(archiveNote => _currentUserInfo.Id == archiveNote.UserId)
            .ProjectTo<ArchiveNoteDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(archiveNote => archiveNote.Id == archiveNoteId);

        archiveNoteDto.ThrowIfNotFound(archiveNoteId);
        
        return archiveNoteDto;
    }

    public async Task DeleteByIdAsync(int archiveNoteId)
    {
        var archiveNote = await _dbContext.ArchiveNotes
            .FirstOrDefaultAsync(archiveNote =>
                archiveNote.Id == archiveNoteId && _currentUserInfo.Id == archiveNote.UserId);

        archiveNote.ThrowIfNotFound(archiveNoteId);

        _dbContext.ArchiveNotes.Remove(archiveNote);
        await _dbContext.SaveChangesAsync();
    }
}