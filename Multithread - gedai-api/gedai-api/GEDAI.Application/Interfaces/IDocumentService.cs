using GEDAI.Application.DTOs;

namespace GEDAI.Application.Interfaces;

public interface IDocumentService
{
    Task<IEnumerable<DocumentDto>> GetAllDocumentsAsync();
    Task<DocumentDto> GetDocumentByIdAsync(int id);
    Task<DocumentDto> AddDocumentAsync(DocumentDto documentDto);
    Task<DocumentDto> AddDocumentWithThread(DocumentDto documentDto);
    Task<DocumentDto> UpdateDocumentAsync(int id, DocumentDto documentDto);
}