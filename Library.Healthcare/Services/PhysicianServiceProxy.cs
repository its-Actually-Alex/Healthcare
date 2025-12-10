using Library.Healthcare.Data;
using Library.Healthcare.DTO;
using Library.Healthcare.Models;
using Library.Healthcare.Utilities;
using Newtonsoft.Json;

namespace Library.Healthcare.Services;

public class PhysicianServiceProxy
{
    private List<PhysicianDTO?> physicians;

    private PhysicianServiceProxy()
    {
        physicians = new List<PhysicianDTO?>();
        var physiciansResponse = new WebRequestHandler().Get("/Physician").Result;
        if (physiciansResponse != null)
        {
            physicians = JsonConvert.DeserializeObject<List<PhysicianDTO?>>(physiciansResponse) ?? new List<PhysicianDTO?>();
        }
    }

    private static PhysicianServiceProxy? instance;
    private static object instanceLock = new object();

    public static PhysicianServiceProxy Current
    {
        get
        {
            lock (instanceLock)
            {
                if (instance == null)
                {
                    instance = new PhysicianServiceProxy();
                }
            }
            return instance;
        }
    }

    public List<PhysicianDTO?> Physicians
    {
        get
        {
            return physicians;
        }
    }

    public async Task<PhysicianDTO?> AddOrUpdate(PhysicianDTO? phy)
    {
        if (phy == null)
        {
            return null;
        }

        var phyPayload = await new WebRequestHandler().Post("/Physician", phy);
        var phyFromServer = JsonConvert.DeserializeObject<PhysicianDTO>(phyPayload);

        if (phy.Id <= 0)
        {
            physicians.Add(phyFromServer);
        }
        else
        {
            var physicianToEdit = Physicians.FirstOrDefault(p => (p?.Id ?? 0) == phy.Id);
            if (physicianToEdit != null)
            {
                var index = Physicians.IndexOf(physicianToEdit);
                Physicians.RemoveAt(index);
                physicians.Insert(index, phy);
            }
        }
        return phy;
    }

    public PhysicianDTO? Delete(int id)
    {
        var response = new WebRequestHandler().Delete($"/Physician/{id}").Result;
        var physicianToDelete = physicians
            .Where(p => p != null)
            .FirstOrDefault(p => (p?.Id ?? -1) == id);
        physicians.Remove(physicianToDelete);
        return physicianToDelete;
    }

    public async Task<List<PhysicianDTO>> Search(QueryRequest query)
    {
        var phyPayload = await new WebRequestHandler().Post("/Physician/Search", query);
        var phyFromServer = JsonConvert.DeserializeObject<List<PhysicianDTO?>>(phyPayload);
        physicians = phyFromServer;
        return physicians;
    }

    public async Task RefreshPhysicians()
    {
        var physiciansResponse = await new WebRequestHandler().Get("/Physician");
        if (physiciansResponse != null)
        {
            physicians = JsonConvert.DeserializeObject<List<PhysicianDTO?>>(physiciansResponse) ?? new List<PhysicianDTO?>();
        }
    }
}