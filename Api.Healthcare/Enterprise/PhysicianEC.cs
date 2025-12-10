using Api.Healthcare.Database;
using Library.Healthcare.DTO;
using Library.Healthcare.Models;

namespace Api.Healthcare.Enterprise
{
    public class PhysicianEC
    {
        public IEnumerable<PhysicianDTO> GetPhysicians()
        {
            return Filebase.Current.Physicians
                .Select(p => new PhysicianDTO(p))
                .OrderByDescending(p => p.Id)
                .Take(100);
        }

        public PhysicianDTO? GetById(int id)
        {
            var phy = Filebase.Current.Physicians.FirstOrDefault(p => p.Id == id);
            if (phy == null)
            {
                return null;
            }
            return new PhysicianDTO(phy);
        }

        public PhysicianDTO? Delete(int id)
        {
            var toRemove = Filebase.Current.Physicians.FirstOrDefault(p => p.Id == id);

            if (toRemove != null)
            {
                Filebase.Current.DeletePhysician(id);
            }

            return toRemove != null ? new PhysicianDTO(toRemove) : null;
        }

        public PhysicianDTO? AddOrUpdate(PhysicianDTO? physicianDTO)
        {
            if (physicianDTO == null)
            {
                return null;
            }

            var phy = new Physician
            {
                Id = physicianDTO.Id,
                Name = physicianDTO.Name,
                License = physicianDTO.License,
                GraduationDate = physicianDTO.GraduationDate,
                Specialization = physicianDTO.Specialization
            };

            physicianDTO = new PhysicianDTO(Filebase.Current.AddOrUpdate(phy));
            return physicianDTO;
        }

        public IEnumerable<PhysicianDTO?> Search(string query)
        {
            return Filebase.Current.Physicians.Where(
                p => (p?.Name?.ToUpper()?.Contains(query?.ToUpper() ?? string.Empty) ?? false)
            ).Select(p => new PhysicianDTO(p));
        }
    }
}