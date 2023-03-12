using System.Net.Mime;
using LifeHelper.Services.Areas.SubNote;
using LifeHelper.Services.Areas.SubNote.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeHelper.Api.Controllers;

[ApiController]
[Route("api/subnotes")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize]
public class SubNoteController : ControllerBase
{
    private readonly ISubNoteService _subNoteService;

    public SubNoteController(ISubNoteService subNoteService)
    {
        _subNoteService = subNoteService;
    }

    /// <summary>
    /// Get the List Of Subnotes by Note ID
    /// </summary>
    /// <param name="noteId">Enter the Note ID</param>
    /// <returns>List Of Subnotes</returns>
    [HttpGet("{noteId:int}")]
    [ProducesResponseType(typeof(IList<SubNoteDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetListAsync([FromRoute] int noteId)
    {
        var subNotes = await _subNoteService.GetListAsync(noteId);

        return Ok(subNotes);
    }

    /// <summary>
    /// Get the Subnote by Note ID and Subnote ID
    /// </summary>
    /// <param name="noteId">Enter the Note ID</param>
    /// <param name="id">Enter the Subnote ID</param>
    /// <returns>Subnote</returns>
    [HttpGet("{noteId:int}/{id:int}")]
    [ProducesResponseType(typeof(SubNoteDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int noteId, [FromRoute] int id)
    {
        var subNote = await _subNoteService.GetByIdAsync(noteId, id);

        return Ok(subNote);
    }

    /// <summary>
    /// Create Subnote of the Note
    /// </summary>
    /// <param name="subNoteInput"></param>
    /// <returns>Created Subnote</returns>
    [HttpPost]
    [ProducesResponseType(typeof(SubNoteDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAsync([FromBody] SubNoteInputDto subNoteInput)
    {
        var subNote = await _subNoteService.CreateAsync(subNoteInput);

        return Ok(subNote);
    }

    /// <summary>
    /// Update an existing Subnote by ID
    /// </summary>
    /// <param name="id">Enter the Subnote ID</param>
    /// <param name="subNoteInput"></param>
    /// <returns>Updated Subnote</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(SubNoteDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateByIdAsync([FromRoute] int id, [FromBody] SubNoteInputDto subNoteInput)
    {
        var subNote = await _subNoteService.UpdateByIdAsync(id, subNoteInput);

        return Ok(subNote);
    }

    /// <summary>
    /// Delete an existing the Subnote by Note ID and Subnote ID
    /// </summary>
    /// <param name="noteId">Enter the Note ID</param>
    /// <param name="id">Enter the Subnote ID</param>
    /// <returns></returns>
    [HttpDelete("{noteId:int}/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteByIdAsync([FromRoute] int noteId, [FromRoute] int id)
    {
        await _subNoteService.DeleteByIdAsync(noteId, id);

        return Ok();
    }
}