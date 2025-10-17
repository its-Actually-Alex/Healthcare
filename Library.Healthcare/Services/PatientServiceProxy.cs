using Library.Healthcare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
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
    private static object instanceLock = new object();

    public static PatientServiceProxy Current
    {
        get
        {
            lock(instanceLock)
            {
                if (instance == null)
                {
                    instance = new PatientServiceProxy();
                }
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

    public Patient? AddOrUpdate(Patient? pat)
    {
        if (pat == null)
        {
            return null;
        }

        if (pat.Id <= 0)
        {
            var maxId = -1;
            if (patients.Any())
            {
                maxId = patients.Select(b => b?.Id ?? -1).Max();
            }
            else
            {
                maxId = 0;
            }
            pat.Id = ++maxId;
            patients.Add(pat);
        }
        else
        {
            var blogToEdit = Patients.FirstOrDefault(b => (b?.Id ?? 0) == pat.Id);
            if (blogToEdit != null)
            {
                var index = Patients.IndexOf(blogToEdit);
                Patients.RemoveAt(index);
                patients.Insert(index, pat);
            }
        }
        return pat;
    }

    public Patient? Delete(int id)
    {
        var patientToDelete = patients
                              .Where(p => p != null)
                              .FirstOrDefault(p => (p?.Id ?? -1) == id);
        patients.Remove(patientToDelete);

        return patientToDelete;
    }

    public void AddDiagnosis(Patient pat, Diagnosis diagnosis)
    {
        if (diagnosis != null)
        {
            pat.Diagnoses.Add(diagnosis);
        }
    }

}
