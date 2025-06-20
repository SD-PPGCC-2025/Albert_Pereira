using GEDAI.Domain.Entities;

namespace GEDAI.Domain.Interfaces;

public interface IDocumentRepository
{
    Task<IEnumerable<Document>> GetAll();
    Task<Document> GetById(int id);
    Task Add(Document document, bool thread =  false);
    Task Update(Document document);
    Task Delete(int id);
}