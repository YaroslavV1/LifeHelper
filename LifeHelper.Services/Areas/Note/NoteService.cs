using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LifeHelper.Infrastructure;
using LifeHelper.Infrastructure.Enums;
using LifeHelper.Infrastructure.Exceptions;
using LifeHelper.Services.Areas.Note.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LifeHelper.Services.Areas.Note;

using Infrastructure.Entities;

public class NoteService : INoteService
{
    private readonly LifeHelperDbContext _dbContext;
    private readonly IMapper _mapper;

    public NoteService(LifeHelperDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<IList<NoteDto>> GetListAsync(bool isDescending, int userId)
    {
        var notes = _dbContext.Notes.Where(note => note.UserId == userId);
        
        notes = isDescending 
            ? notes.OrderByDescending(note => note.CreatedDate) 
            : notes.OrderBy(note => note.CreatedDate);

        return await notes.ProjectTo<NoteDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<NoteDto> GetByIdAsync(int id, int userId)
    {
        return await _dbContext.Notes
                   .Where(note => note.UserId == userId)
                   .ProjectTo<NoteDto>(_mapper.ConfigurationProvider)
                   .FirstOrDefaultAsync(note => note.Id == id) 
               ?? throw new NotFoundException($"Note with Id: {id} not found");
    }

    public async Task<NoteDto> CreateAsync(NoteInputDto noteInput, int userId)
    {
        var note = _mapper.Map<Note>(noteInput);

        note.CreatedDate = DateTime.UtcNow;
        note.UpdatedDate = DateTime.UtcNow;
        note.UserId = userId;

        await _dbContext.Notes.AddAsync(note);
        await _dbContext.SaveChangesAsync();

        var noteDto = _mapper.Map<NoteDto>(note);
        return noteDto;
    }

    public async Task<NoteDto> UpdateByIdAsync(int id, NoteInputDto noteInput, int userId)
    {
        var note = await _dbContext.Notes
                       .Where(note => note.UserId == userId)
                       .FirstOrDefaultAsync(note => note.Id == id) 
                   ?? throw new NotFoundException($"Note with Id: {id} not found");

        _mapper.Map(noteInput, note);
        
        note.UpdatedDate = DateTime.UtcNow;

        _dbContext.Notes.Update(note);
        await _dbContext.SaveChangesAsync();
        
        var noteDto = _mapper.Map<NoteDto>(note);
        
        return noteDto;
    }

    public async Task DeleteByIdAsync(int id, int userId)
    {
        var note = await _dbContext.Notes
                       .Where(note => note.UserId == userId)
                       .FirstOrDefaultAsync(note => note.Id == id) 
                   ?? throw new NotFoundException($"Note with Id: {id} not found");

        _dbContext.Notes.Remove(note);
        await _dbContext.SaveChangesAsync();
    }
}