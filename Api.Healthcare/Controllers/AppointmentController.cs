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
    public class AppointmentController : ControllerBase
    {
        private readonly ILogger<AppointmentController> _logger;

        public AppointmentController(ILogger<AppointmentController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<AppointmentDTO> Get()
        {
            return new AppointmentEC().GetAppointments();
        }

        [HttpGet("{id}")]
        public AppointmentDTO? GetById(int id)
        {
            return new AppointmentEC().GetById(id);
        }

        [HttpDelete("{id}")]
        public AppointmentDTO? Delete(int id)
        {
            return new AppointmentEC().Delete(id);
        }

        [HttpPost]
        public AppointmentDTO? Post([FromBody] AppointmentDTO appointment)
        {
            return new AppointmentEC().AddOrUpdate(appointment);
        }

        [HttpPost("Search")]
        public IEnumerable<AppointmentDTO?> Search([FromBody] QueryRequest query)
        {
            return new AppointmentEC().Search(query.Content);
        }
    }
}
