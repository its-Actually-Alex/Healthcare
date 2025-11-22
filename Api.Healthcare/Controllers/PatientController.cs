using Api.Healthcare.Database;
using Api.Healthcare.Enterprise;
using Library.Healthcare.Models;
using Library.Healthcare.Data;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using Library.Healthcare.DTO;

namespace Api.Healthcare.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly ILogger<PatientController> _logger;

        public PatientController(ILogger<PatientController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<PatientDTO> Get()
        {
            return new PatientEC().GetPatients();
        }

        [HttpGet("{id}")]
        public PatientDTO? GetById(int id)
        {
            return new PatientEC().GetById(id);
        }

        [HttpDelete("{id}")]
        public PatientDTO? Delete(int id)
        {
            return new PatientEC().Delete(id);
        }

        [HttpPost("Search")]
        public IEnumerable<PatientDTO?> Search([FromBody] QueryRequest query)
        {
            return new PatientEC().Search(query.Content);
        }
    }
}
