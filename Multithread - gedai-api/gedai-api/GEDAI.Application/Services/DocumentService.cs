using GEDAI.Application.DTOs;
using GEDAI.Application.Interfaces;
using GEDAI.Domain.Interfaces;
using GEDAI.Domain.Entities;
namespace GEDAI.Application.Services;

public class DocumentService : IDocumentService
{
    private readonly IDocumentRepository _documentRepository;

    public DocumentService(IDocumentRepository documentRepository)
    {
        _documentRepository = documentRepository;
    }

    public async Task<IEnumerable<DocumentDto>> GetAllDocumentsAsync()

    {
        var documents = await _documentRepository.GetAll();
        return documents.Select(d => new DocumentDto
        {
            Id = d.Id,
            Title = d.Title,
            Content = d.Content,
            CreatedDate = d.CreatedDate
        });
    }

    public async Task<DocumentDto> GetDocumentByIdAsync(int id)
    {
        var document = await _documentRepository.GetById(id);
        if (document == null) return null;

        return new DocumentDto
        {
            Id = document.Id,
            Title = document.Title,
            Content = document.Content,
            CreatedDate = document.CreatedDate
        };
    }

    public async Task<DocumentDto> AddDocumentAsync(DocumentDto documentDto)
    {
        var document = new Document
        {
            Title = documentDto.Title,
            Content = documentDto.Content,
            CreatedDate = DateTime.UtcNow
        };

        await _documentRepository.Add(document);
        documentDto.Id = document.Id; 
        return documentDto;
    }

    public Task<DocumentDto> AddDocumentWithThread(DocumentDto documentDto)
    {
        var document = new Document
        {
            Title = documentDto.Title,
            Content = documentDto.Content,
            CreatedDate = DateTime.UtcNow
        };

        // Thread initialization to use a lambda expression
        Console.WriteLine($"Iniciando a Thread para gravação do documento {document.Title} início em: {DateTime.Now.ToString()}");
        var threadDocument = new Thread(
        () =>
        {
            var result = _documentRepository.Add(document, true);
        });

        threadDocument.Start();
        threadDocument.Join();

        documentDto.Id = document.Id;

        Console.WriteLine($"Finalizando a Thread para gravação do documento {document.Title} termino em: {DateTime.Now.ToString()}");

        return Task.FromResult(documentDto);
    }

    public async Task<DocumentDto> UpdateDocumentAsync(int id, DocumentDto documentDto)
    {
        var document = await _documentRepository.GetById(documentDto.Id);
        if (document == null) return null;

        document.Title = documentDto.Title;
        document.Content = documentDto.Content;

        await _documentRepository.Update(document);
        return documentDto;
    }
}