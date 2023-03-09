using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LifeHelper.Infrastructure;
using LifeHelper.Infrastructure.Enums;
using LifeHelper.Infrastructure.Exceptions;
using LifeHelper.Services.Areas.Helpers.Jwt;
using LifeHelper.Services.Areas.Helpers.Jwt.DTOs;
using LifeHelper.Services.Areas.Note.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LifeHelper.Services.Areas.Note;

using Infrastructure.Entities;

public class NoteService : INoteService
{
    private readonly LifeHelperDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly TokenInfoDto _currentUserInfo;
    public NoteService(LifeHelperDbContext dbContext,
        IMapper mapper,
        IHttpContextAccessor context,
        IClaimParserService parserService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _currentUserInfo = parserService.ParseInfoFromClaims(context.HttpContext);
    }
    
    public async Task<IList<NoteDto>> GetListAsync(bool isDescending)
    {
        var notes = _dbContext.Notes.Where(note => note.UserId == _currentUserInfo.Id);
        
        notes = isDescending 
            ? notes.OrderByDescending(note => note.CreatedDate) 
            : notes.OrderBy(note => note.CreatedDate);

        return await notes.ProjectTo<NoteDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<NoteDto> GetByIdAsync(int id)
    {
        return await _dbContext.Notes
                   .Where(note => note.UserId == _currentUserInfo.Id)
                   .ProjectTo<NoteDto>(_mapper.ConfigurationProvider)
                   .FirstOrDefaultAsync(note => note.Id == id) 
               ?? throw new NotFoundException($"Note with Id: {id} not found");
    }

    public async Task<NoteDto> CreateAsync(NoteInputDto noteInput)
    {
        var note = _mapper.Map<Note>(noteInput);

        note.CreatedDate = DateTime.UtcNow;
        note.UpdatedDate = DateTime.UtcNow;
        note.UserId = _currentUserInfo.Id;

        await _dbContext.Notes.AddAsync(note);
        await _dbContext.SaveChangesAsync();

        var noteDto = _mapper.Map<NoteDto>(note);
        return noteDto;
    }

    public async Task<NoteDto> UpdateByIdAsync(int id, NoteInputDto noteInput)
    {
        var note = await _dbContext.Notes
                       .FirstOrDefaultAsync(note => note.Id == id && note.UserId == _currentUserInfo.Id) 
                   ?? throw new NotFoundException($"Note with Id: {id} not found");

        _mapper.Map(noteInput, note);
        
        note.UpdatedDate = DateTime.UtcNow;

        _dbContext.Notes.Update(note);
        await _dbContext.SaveChangesAsync();
        
        var noteDto = _mapper.Map<NoteDto>(note);
        
        return noteDto;
    }

    public async Task DeleteByIdAsync(int id)
    {
        var note = await _dbContext.Notes
                       .FirstOrDefaultAsync(note => note.Id == id && note.UserId == _currentUserInfo.Id) 
                   ?? throw new NotFoundException($"Note with Id: {id} not found");

        _dbContext.Notes.Remove(note);
        await _dbContext.SaveChangesAsync();
    }
}