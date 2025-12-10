using Api.Healthcare.Database;
using Api.Healthcare.Enterprise;
using Library.Healthcare.Models;
using Library.Healthcare.Data;
using Microsoft.AspNetCore.Mvc;
using Library.Healthcare.DTO;

namespace Api.Healthcare.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PhysicianController : ControllerBase
    {
        private readonly ILogger<PhysicianController> _logger;

        public PhysicianController(ILogger<PhysicianController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<PhysicianDTO> Get()
        {
            return new PhysicianEC().GetPhysicians();
        }

        [HttpGet("{id}")]
        public PhysicianDTO? GetById(int id)
        {
            return new PhysicianEC().GetById(id);
        }

        [HttpDelete("{id}")]
        public PhysicianDTO? Delete(int id)
        {
            return new PhysicianEC().Delete(id);
        }

        [HttpPost]
        public PhysicianDTO? Post([FromBody] PhysicianDTO physician)
        {
            return new PhysicianEC().AddOrUpdate(physician);
        }

        [HttpPost("Search")]
        public IEnumerable<PhysicianDTO?> Search([FromBody] QueryRequest query)
        {
            return new PhysicianEC().Search(query.Content);
        }
    }
}
