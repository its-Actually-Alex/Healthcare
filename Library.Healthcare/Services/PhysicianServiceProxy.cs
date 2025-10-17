using Library.Healthcare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Library.Healthcare.Services;
public class PhysicianServiceProxy
{
    private List<Physician?> physicians;

    private PhysicianServiceProxy()
    {
        physicians = new List<Physician?>();
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

    public List<Physician?> Physicians
    {
        get {  return physicians; }
    }

    public Physician? AddOrUpdate(Physician? phys)
    {
        if (phys == null)
        {
            return null;
        }

        if (phys.Id <= 0)
        {
            var maxId = -1;
            if (physicians.Any())
            {
                maxId = physicians.Select(b => b?.Id ?? -1).Max();
            }
            else
            {
                maxId = 0;
            }
            phys.Id = ++maxId;
            physicians.Add(phys);
        }
        else
        {
            var physicianToEdit = Physicians.FirstOrDefault(b => (b?.Id ?? 0) == phys.Id);
            if (physicianToEdit != null)
            {
                var index = Physicians.IndexOf(physicianToEdit);
                Physicians.RemoveAt(index);
                physicians.Insert(index, phys);
            }
        }
        return phys;
    }

    public Physician? Delete(int id)
    {
        var physicianToDelete = physicians
                              .Where(p => p != null)
                              .FirstOrDefault(p => (p?.Id ?? -1) == id);
        physicians.Remove(physicianToDelete);

        return physicianToDelete;
    }
}
