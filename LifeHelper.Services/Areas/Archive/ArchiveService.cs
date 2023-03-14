using AutoMapper;
using AutoMapper.QueryableExtensions;
using LifeHelper.Infrastructure;
using LifeHelper.Infrastructure.Entities;
using LifeHelper.Infrastructure.Exceptions;
using LifeHelper.Services.Areas.Archive.DTOs;
using LifeHelper.Services.Areas.Helpers.Jwt;
using LifeHelper.Services.Areas.Helpers.Jwt.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LifeHelper.Services.Areas.Archive;

public class ArchiveService : IArchiveService
{
    private readonly LifeHelperDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly TokenInfoDto _currentUserInfo;

    public ArchiveService(
        LifeHelperDbContext dbContext,
        IMapper mapper,
        IHttpContextAccessor contextAccessor,
        IClaimParserService claimParserService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _currentUserInfo = claimParserService.ParseInfoFromClaims(contextAccessor.HttpContext);
    }
    
    public async Task ArchiveByIdAsync(int noteId)
    {
        var note = await _dbContext.Notes.Include(note => note.SubNotes)
                       .FirstOrDefaultAsync(note => note.Id == noteId && note.UserId == _currentUserInfo.Id) 
                   ?? throw new NotFoundException($"Note with Id: {noteId} not found");

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
                                  archiveNote.Id == archiveNoteId && archiveNote.UserId == _currentUserInfo.Id) 
                          ?? throw new NotFoundException($"Archive Note with Id: {archiveNoteId} not found");

        var note = _mapper.Map<Note>(archiveNote);

        await _dbContext.AddAsync(note);
        _dbContext.ArchiveNotes.Remove(archiveNote);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IList<ArchivedNoteDto>> GetListAsync(bool isDescending)
    {
        var archiveNote = _dbContext.ArchiveNotes
            .Include(archiveNote => archiveNote.ArchiveSubNotes)
            .Where(archiveNote => archiveNote.UserId == _currentUserInfo.Id);
        
        archiveNote = isDescending 
            ? archiveNote.OrderByDescending(arcNote => arcNote.CreatedDate) 
            : archiveNote.OrderBy(arcNote => arcNote.CreatedDate);

        return await archiveNote.ProjectTo<ArchivedNoteDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<ArchivedNoteDto> GetByIdAsync(int archiveNoteId)
    {
        return await _dbContext.ArchiveNotes
            .Include(archiveNote => archiveNote.ArchiveSubNotes)
            .Where(archiveNote => archiveNote.UserId == _currentUserInfo.Id)
            .ProjectTo<ArchivedNoteDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(archiveNote => archiveNote.Id == archiveNoteId)
            ?? throw new NotFoundException($"Archive Note with Id: {archiveNoteId} not found");
    }

    public async Task DeleteByIdAsync(int archiveNoteId)
    {
        var archiveNote = await _dbContext.ArchiveNotes.FirstOrDefaultAsync(archiveNote => 
                              archiveNote.Id == archiveNoteId && archiveNote.UserId == _currentUserInfo.Id) 
                          ?? throw new NotFoundException($"Archive Note with Id: {archiveNoteId} not found");

        _dbContext.ArchiveNotes.Remove(archiveNote);
        await _dbContext.SaveChangesAsync();
    }
}