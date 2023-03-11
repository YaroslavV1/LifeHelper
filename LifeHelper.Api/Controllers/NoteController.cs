using System.Net.Mime;
using LifeHelper.Services.Areas.Note;
using LifeHelper.Services.Areas.Note.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeHelper.Api.Controllers;

[ApiController]
[Route("api/notes")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize(Roles = "User,Admin")]
public class NoteController : ControllerBase
{
    private readonly INoteService _noteService;

    public NoteController(INoteService noteService)
    {
        _noteService = noteService;
    }

    /// <summary>
    /// Get the List of Notes
    /// </summary>
    /// <param name="isDescending">Select sorting: true - Descending, false - Ascending</param>
    /// <returns>List of Notes</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IList<NoteDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetListAsync([FromQuery] bool isDescending = true)
    {
        var notes = await _noteService.GetListAsync(isDescending);

        return Ok(notes);
    }

    /// <summary>
    /// Get the Note by Id
    /// </summary>
    /// <param name="id">Enter the Note Id</param>
    /// <returns>Note</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(NoteDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var note = await _noteService.GetByIdAsync(id);

        return Ok(note);
    }

    /// <summary>
    /// Create Note
    /// </summary>
    /// <param name="noteInput"></param>
    /// <returns>Created Note</returns>
    [HttpPost]
    [ProducesResponseType(typeof(NoteDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAsync([FromBody] NoteInputDto noteInput)
    {
        var note = await _noteService.CreateAsync(noteInput);

        return CreatedAtAction("GetById", new { id = note.Id }, note);
    }

    /// <summary>
    /// Update an existing Note
    /// </summary>
    /// <param name="id">Enter the Note Id</param>
    /// <param name="noteInput"></param>
    /// <returns>Updated Note</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(NoteDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateByIdAsync([FromRoute] int id, [FromBody] NoteInputDto noteInput)
    {
        var note = await _noteService.UpdateByIdAsync(id, noteInput);

        return Ok(note);
    }

    /// <summary>
    /// Delete an existing Note
    /// </summary>
    /// <param name="id">Enter the Note Id</param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteByIdAsync([FromRoute] int id)
    {
        await _noteService.DeleteByIdAsync(id);

        return Ok();
    }
}