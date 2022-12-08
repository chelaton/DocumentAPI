using DocumentAPI.Dtos;
using DocumentAPI.Models;
using DocumentAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DocumentAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentRepository _repository;
        private readonly ILogger<DocumentsController> _logger;

        public DocumentsController(IDocumentRepository repository, ILogger<DocumentsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }



        // GET api/<DocumentsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var documment = await _repository.GetDocumentByIdAsync(id);
                if (documment!=null)
                {
                    return Ok(new DocumentDto
                    {
                        Id = documment.ExternalDocumentId,
                        Tags = documment.Tags?.Select(p => p.Name).ToList(),
                        Data = documment.Data
                    });
                }
                return NotFound();

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal error");
            }

        }

        /// <summary>
        /// Create document with tags
        /// </summary>
        /// <param name="documentDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DocumentDto documentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var entity = await _repository.GetDocumentByIdAsync(documentDto.Id);
            if (entity.ExternalDocumentId.Equals(documentDto.Id))
            {
                return BadRequest($"Document with id {documentDto.Id} already exist!");
            }
            try
            {
                var document = new Document()
                {
                    ExternalDocumentId = documentDto.Id,
                    Tags = documentDto.Tags?.Select(tag => new Tag { Name = tag }).ToList(),
                    Data = documentDto.Data 
                };
                await _repository.AddDocumentAsync(document);
                _repository.SaveChanges();
                return Accepted();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error" + ex.Message);
                return StatusCode(500, "Internal error");
            }
        }

        /// <summary>
        /// /UPDATE document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="documentDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] string id, [FromBody] DocumentDto documentDto)
        {
            if (!ModelState.IsValid || !id.Equals(documentDto.Id))
            {
                return BadRequest();
            }

            var documentEntity = await _repository.GetDocumentByIdAsync(id);
            if (documentEntity is null)
            {
                _logger.LogError($"Update document: {id}, not found in DB");
                return NotFound();
            }
            try
            {
                documentEntity.Data = documentDto.Data;
                documentEntity.Tags = documentDto.Tags?.Select(tag => new Tag { Name = tag }).ToList();
                _repository.UpdateDocument(documentEntity);
                _repository.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Update document: {id}, error {ex.Message}");
                return StatusCode(500, "Internal error");
            }

        }
    }
}
