using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Healthcare.Models
{
    public class Physician
    {
        public string? Name { get; set; }
        public int Id { get; set; }
        public int License { get; set; }
        public string? GraduationDate { get; set; }
        public string? Specialization {  get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
