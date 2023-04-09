using AutoMapper;
using AutoMapper.QueryableExtensions;
using LifeHelper.Infrastructure;
using LifeHelper.Infrastructure.Entities;
using LifeHelper.Services.Areas.SubNotes.DTOs;
using LifeHelper.Services.Exceptions;
using LifeHelper.Services.Utilities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static LifeHelper.Services.Utilities.LifeHelperUtilities;

namespace LifeHelper.Services.Areas.SubNotes;

public class SubNoteService : ISubNoteService
{
    private readonly LifeHelperDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly TokenInfoDto _currentUserInfo;

    public SubNoteService(LifeHelperDbContext dbContext, IMapper mapper, IHttpContextAccessor context)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _currentUserInfo = ParseInfoFromClaims(context.HttpContext);
    }
    
    public async Task<IList<SubNoteDto>> GetListAsync(int noteId)
    {
        await ThrowIfNoteIsNotExists(noteId);
        
        return await _dbContext.SubNotes
            .Where(subNote => subNote.NoteId == noteId)
            .ProjectTo<SubNoteDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<SubNoteDto> GetByIdAsync(int noteId, int subNoteId)
    {
        await ThrowIfNoteIsNotExists(noteId);
        
        var subNote = await _dbContext.SubNotes
            .Where(subNote => subNote.NoteId == noteId)
            .ProjectTo<SubNoteDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(subNote => subNote.Id == subNoteId);
        
        subNote.ThrowIfNotFound(subNoteId);

        return subNote;
    }

    public async Task<SubNoteDto> CreateAsync(SubNoteInputDto subNoteInputDto)
    {
        await ThrowIfNoteIsNotExists(subNoteInputDto.NoteId);
        
        var subNote = _mapper.Map<SubNote>(subNoteInputDto);
        
        await _dbContext.SubNotes.AddAsync(subNote);
        await _dbContext.SaveChangesAsync();

        var subNoteDto = _mapper.Map<SubNoteDto>(subNote);

        return subNoteDto;
    }

    public async Task<SubNoteDto> UpdateByIdAsync(int subNoteId, SubNoteInputDto subNoteInputDto)
    {
        await ThrowIfNoteIsNotExists(subNoteInputDto.NoteId);
        
        var subNote = await _dbContext.SubNotes
            .FirstOrDefaultAsync(subNote => subNote.Id == subNoteId && subNote.NoteId == subNoteInputDto.NoteId);
        
        subNote.ThrowIfNotFound(subNoteId);

        _mapper.Map(subNoteInputDto, subNote);

        _dbContext.SubNotes.Update(subNote);
        await _dbContext.SaveChangesAsync();
        
        var subNoteDto = _mapper.Map<SubNoteDto>(subNote);

        return subNoteDto;
    }

    public async Task DeleteByIdAsync(int noteId, int subNoteId)
    {
        await ThrowIfNoteIsNotExists(noteId);
        
        var subNote = await _dbContext.SubNotes
            .FirstOrDefaultAsync(subNote => subNote.Id == subNoteId && subNote.NoteId == noteId);
        
        subNote.ThrowIfNotFound(subNoteId);

        _dbContext.SubNotes.Remove(subNote);
        await _dbContext.SaveChangesAsync();
    }

    private async Task ThrowIfNoteIsNotExists(int noteId)
    {
        var user = await _dbContext.Users
            .Include(user => user.Notes)
            .FirstOrDefaultAsync(user => user.Id == _currentUserInfo.Id);
        
        if (user is null || user.Notes.All(note => note.Id != noteId))
        {
            throw new NotFoundException($"Note with Id: {noteId} does not exist");
        }
    }
}