using Library.Healthcare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Library.Healthcare.Services;
public class PatientServiceProxy
{
    private List<Patient?> patients;
    private PatientServiceProxy()
    {
        patients = new List<Patient?>();
    }

    private static PatientServiceProxy? instance;

    public static PatientServiceProxy Current
    {
        get
        {
            if(instance == null)
            {
                instance = new PatientServiceProxy();
            }

            return instance;
        }
    }

    public List<Patient?> Patients
    {
        get
        {
            return patients;
        }
    }

    public Patient Create(Patient? patient)
    {
        if (patient != null)
        { 
            var maxId = -1;
            if (patients.Any())
            {
                maxId = patients.Select(p => p?.Id ?? -1).Max();
            }
            else
            {
                maxId = 0;
            }
            patient.Id = ++maxId;
            patients.Add(patient);
        }
        return patient;
    }

    public Patient? Delete(int id)
    {
        var patientToDelete = patients
                              .Where(p => p != null)
                              .FirstOrDefault(p => p.Id == id);
        patients.Remove(patientToDelete);

        return patientToDelete;
    }

}
