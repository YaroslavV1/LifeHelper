using System.Net.Mime;
using LifeHelper.Services.Areas.Archive;
using LifeHelper.Services.Areas.Archive.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeHelper.Api.Controllers;

[ApiController]
[Route("api/archiveNotes")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize]
public class ArchiveController : ControllerBase
{
    private readonly IArchiveService _archiveService;

    public ArchiveController(IArchiveService archiveService)
    {
        _archiveService = archiveService;
    }

    /// <summary>
    /// Archives an existing Note by ID
    /// </summary>
    /// <param name="noteId">Enter the Note ID</param>
    /// <returns></returns>
    [HttpPost("archive/{noteId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ArchiveByIdAsync([FromRoute] int noteId)
    {
        await _archiveService.ArchiveByIdAsync(noteId);

        return Ok();
    }

    /// <summary>
    /// Unarchives an existing archive Note by ID
    /// </summary>
    /// <param name="archiveNoteId">Enter the archive Note ID</param>
    /// <returns></returns>
    [HttpPost("unarchive/{archiveNoteId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UnArchiveByIdAsync([FromRoute] int archiveNoteId)
    {
        await _archiveService.UnArchiveByIdAsync(archiveNoteId);

        return Ok();
    }

    /// <summary>
    /// Get the List of archive Notes
    /// </summary>
    /// <param name="isDescending">Select sorting: true - Descending, false - Ascending</param>
    /// <returns>List of archive Notes</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IList<ArchiveNoteDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetListAsync([FromQuery] bool isDescending = true)
    {
        var archiveNotes = await _archiveService.GetListAsync(isDescending);

        return Ok(archiveNotes);
    }

    /// <summary>
    /// Get the archive Note by ID
    /// </summary>
    /// <param name="archiveNoteId">Enter the archive Note ID</param>
    /// <returns>Archive Note</returns>
    [HttpGet("{archiveNoteId:int}")]
    [ProducesResponseType(typeof(ArchiveNoteDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int archiveNoteId)
    {
        var archiveNote = await _archiveService.GetByIdAsync(archiveNoteId);

        return Ok(archiveNote);
    }

    /// <summary>
    /// Delete an existing archive Note by ID
    /// </summary>
    /// <param name="archiveNoteId">Enter the archive Note ID</param>
    /// <returns></returns>
    [HttpDelete("{archiveNoteId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteByIdAsync([FromRoute] int archiveNoteId)
    {
        await _archiveService.DeleteByIdAsync(archiveNoteId);

        return Ok();
    }
}