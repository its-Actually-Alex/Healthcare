using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Healthcare.Models
{
    public class Diagnosis
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string? Symptoms { get; set; }
        public string? Diagnosis_Given { get; set; }
        public Physician? Physician_Diagnosed_By { get; set; }

        public override string ToString()
        {
            return $"Symptoms: {Symptoms}\nDiagnosis Given: {Diagnosis_Given}\n" +
                $"Diagnosed By: {Physician_Diagnosed_By}\n";
        }
    }
}
