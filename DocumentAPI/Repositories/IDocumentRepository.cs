using DocumentAPI.Models;

namespace DocumentAPI.Repositories
{
    public interface IDocumentRepository
    {
        Task<Document> GetDocumentByIdAsync(string documentId);
        void SaveChanges();
        Task AddDocumentAsync(Document document);
        void UpdateDocument(Document document);
    }
}