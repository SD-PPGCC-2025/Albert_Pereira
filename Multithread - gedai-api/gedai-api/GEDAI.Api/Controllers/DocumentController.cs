using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GEDAI.Application.Interfaces;
using GEDAI.Application.DTOs;

namespace GEDAI.Api.Controllers;

[Route("v1/api/[controller]")]
[ApiController]
[Authorize]
public class DocumentController : Controller
{
    private readonly IDocumentService _documentService;
    private readonly HttpClient _httpClient;

    public DocumentController(IDocumentService documentService,
                              HttpClient httpClient)
    {
        _httpClient = httpClient;
        _documentService = documentService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DocumentDto>>> GetDocuments()
    {
        var documents = await _documentService.GetAllDocumentsAsync();
        return Ok(documents);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DocumentDto>> GetDocumentById(int id)
    {
        var document = await _documentService.GetDocumentByIdAsync(id);
        if (document == null)
        {
            return NotFound();
        }
        return Ok(document);
    }

    [HttpPost]
    [Route("createAsync")]
    public async Task<ActionResult<DocumentDto>> CreateDocumentAsync(DocumentDto documentDto)
    {
        var createdDocument = await _documentService.AddDocumentAsync(documentDto);
        return CreatedAtAction(nameof(GetDocumentById), new { id = createdDocument.Id }, createdDocument);
    }

    [HttpPost]
    [Route("createThread")]
    public Task<ActionResult<DocumentDto>> CreateDocumentWithThread(DocumentDto documentDto)
    {
        var createdDocument = _documentService.AddDocumentWithThread(documentDto);
        return Task.FromResult<ActionResult<DocumentDto>>(
            CreatedAtAction(nameof(GetDocumentById), new { id = createdDocument.Result.Id }, createdDocument.Result));
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDocument(int id, DocumentDto documentDto)
    {
        if (id != documentDto.Id)
        {
            return BadRequest();
        }

        await _documentService.UpdateDocumentAsync(documentDto.Id, documentDto);
        return NoContent();
    }
}