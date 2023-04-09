using AutoMapper;
using AutoMapper.QueryableExtensions;
using LifeHelper.Infrastructure;
using LifeHelper.Services.Areas.Notes.DTOs;
using LifeHelper.Services.Utilities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static LifeHelper.Services.Utilities.LifeHelperUtilities;

namespace LifeHelper.Services.Areas.Notes;

using Infrastructure.Entities;

public class NoteService : INoteService
{
    private readonly LifeHelperDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly TokenInfoDto _currentUserInfo;
    
    public NoteService(LifeHelperDbContext dbContext, IMapper mapper, IHttpContextAccessor context)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _currentUserInfo = ParseInfoFromClaims(context.HttpContext);
    }

    public async Task<IList<NoteDto>> GetListAsync(bool? inputFilter)
    {
        var notes = _dbContext.Notes.Where(note => note.UserId == _currentUserInfo.Id);
        
        if (inputFilter is { } isDescending)
        {
            notes = isDescending
                ? notes.OrderByDescending(note => note.CreatedDate) 
                : notes.OrderBy(note => note.CreatedDate);
        }

        return await notes.ProjectTo<NoteDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<NoteDto> GetByIdAsync(int id)
    {
        var note = await _dbContext.Notes
            .Where(note => note.UserId == _currentUserInfo.Id)
            .ProjectTo<NoteDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(note => note.Id == id);
        
        note.ThrowIfNotFound(id);

        return note;
    }

    public async Task<NoteDto> CreateAsync(NoteInputDto noteInput)
    {
        var note = _mapper.Map<Note>(noteInput);
        note.UserId = _currentUserInfo.Id;

        await _dbContext.Notes.AddAsync(note);
        await _dbContext.SaveChangesAsync();

        var noteDto = _mapper.Map<NoteDto>(note);
        
        return noteDto;
    }

    public async Task<NoteDto> UpdateByIdAsync(int id, NoteInputDto noteInput)
    {
        var note = await _dbContext.Notes
            .FirstOrDefaultAsync(note => note.Id == id && note.UserId == _currentUserInfo.Id);
        
        note.ThrowIfNotFound(id);

        _mapper.Map(noteInput, note);
        
        _dbContext.Notes.Update(note);
        await _dbContext.SaveChangesAsync();
        
        var noteDto = _mapper.Map<NoteDto>(note);
        
        return noteDto;
    }

    public async Task DeleteByIdAsync(int id)
    {
        var note = await _dbContext.Notes
            .FirstOrDefaultAsync(note => note.Id == id && note.UserId == _currentUserInfo.Id);
        
        note.ThrowIfNotFound(id);

        _dbContext.Notes.Remove(note);
        await _dbContext.SaveChangesAsync();
    }
}