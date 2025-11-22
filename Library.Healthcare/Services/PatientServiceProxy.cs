using Library.Healthcare.Data;
using Library.Healthcare.DTO;
using Library.Healthcare.Models;
using Library.Healthcare.Utilities;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Reflection.Metadata;

namespace Library.Healthcare.Services;

public class PatientServiceProxy
{
    private List<PatientDTO?> patients;
    private PatientServiceProxy()
    {
        patients = new List<PatientDTO?>();
        var patientsResponse = new WebRequestHandler().Get("/Patient").Result;
        if (patientsResponse != null)
        {
            patients = JsonConvert.DeserializeObject<List<PatientDTO?>>(patientsResponse) ?? new List<PatientDTO?>();
        }
    }
    private static PatientServiceProxy? instance;
    private static object instanceLock = new object();
    public static PatientServiceProxy Current
    {
        get
        {
            lock (instanceLock)
            {
                if (instance == null)
                {
                    instance = new PatientServiceProxy();
                }
            }

            return instance;
        }
    }

    public List<PatientDTO?> Patients
    {
        get
        {
            return patients;
        }
    }

    public async Task<PatientDTO?> AddOrUpdate(PatientDTO? pat)
    {
        if (pat == null)
        {
            return null;
        }

        var patPayload = await new WebRequestHandler().Post("/Patient", pat);
        var patFromServer = JsonConvert.DeserializeObject<PatientDTO>(patPayload);

        if (pat.Id <= 0)
        {
            patients.Add(patFromServer);
        }
        else
        {
            var patientToEdit = Patients.FirstOrDefault(b => (b?.Id ?? 0) == pat.Id);
            if (patientToEdit != null)
            {
                var index = Patients.IndexOf(patientToEdit);
                Patients.RemoveAt(index);
                patients.Insert(index, pat);
            }
        }
        return pat;
    }

    public PatientDTO? Delete(int id)
    {
        var response = new WebRequestHandler().Delete($"/Patient/{id}").Result;
        //get blog object
        var patientToDelete = patients
            .Where(b => b != null)
            .FirstOrDefault(b => (b?.Id ?? -1) == id);
        //delete it!
        patients.Remove(patientToDelete);

        return patientToDelete;
    }

    public async Task<List<PatientDTO>> Search(QueryRequest query)
    {
        var patPayload = await new WebRequestHandler().Post("/Patient/Search", query);
        var patFromServer = JsonConvert.DeserializeObject<List<PatientDTO?>>(patPayload);

        patients = patFromServer;
        return patients;
    }
}