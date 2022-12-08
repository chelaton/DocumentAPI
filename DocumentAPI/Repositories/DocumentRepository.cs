using DocumentAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentAPI.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly DocumentContext _documentContext;

        public DocumentRepository(DocumentContext documentContext)
        {
            _documentContext = documentContext;
        }

        public async Task<Document> GetDocumentByIdAsync(string documentId)
        {
            var result = await _documentContext.Documents.Where(p => p.ExternalDocumentId.Equals(documentId)).Include("Tags").FirstOrDefaultAsync();
            return result;
        }

        public async Task AddDocumentAsync(Document document)
        {
            await _documentContext.AddAsync(document);
        }
        public void UpdateDocument(Document document)
        {
            _documentContext.Update(document);
        }
        public void SaveChanges()
        {
            _documentContext.SaveChanges();
        }
    }
}
