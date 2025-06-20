using GEDAI.Domain.Entities;
using GEDAI.Domain.Interfaces;
using GEDAI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGedai.Infrastructure.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly ApplicationDbContext _context;

        public DocumentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Document>> GetAll()
        {
            return await _context.Documents.ToListAsync();
        }

        public async Task<Document> GetById(int id)
        {
            return await _context.Documents.FindAsync(id);
        }

        public async Task Add(Document document, bool thread)
        {
            if (thread)
                Console.WriteLine($"Executando a Thread para gravação do documento {document.Title} inicio em: {DateTime.Now.ToString()}");

            await _context.Documents.AddAsync(document);
            _context.SaveChanges();
        }

        public async Task Update(Document document)
        {
            _context.Documents.Update(document);
            _context.SaveChanges();
        }

        public async Task Delete(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document != null)
            {
                _context.Documents.Remove(document);
                await _context.SaveChangesAsync();
            }
        }
    }
}