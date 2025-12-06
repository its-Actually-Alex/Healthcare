using Library.Healthcare.Data;
using Library.Healthcare.DTO;
using Library.Healthcare.Models;
using Library.Healthcare.Utilities;
using Newtonsoft.Json;

namespace Library.Healthcare.Services;

public class AppointmentServiceProxy
{
    private List<AppointmentDTO?> appointments;
    private AppointmentServiceProxy()
    {
        appointments = new List<AppointmentDTO?>();
        var appointmentsResponse = new WebRequestHandler().Get("/Appointment").Result;
        if (appointmentsResponse != null)
        {
            appointments = JsonConvert.DeserializeObject<List<AppointmentDTO?>>(appointmentsResponse) ?? new List<AppointmentDTO?>();
        }
    }

    private static AppointmentServiceProxy? instance;
    private static object instanceLock = new object();

    public static AppointmentServiceProxy Current
    {
        get
        {
            lock (instanceLock)
            {
                if (instance == null)
                {
                    instance = new AppointmentServiceProxy();
                }
            }

            return instance;
        }
    }

    public List<AppointmentDTO?> Appointments
    {
        get
        {
            return appointments;
        }
    }

    public async Task<AppointmentDTO?> AddOrUpdate(AppointmentDTO? apt)
    {
        if (apt == null)
        {
            return null;
        }

        var aptPayload = await new WebRequestHandler().Post("/Appointment", apt);
        var aptFromServer = JsonConvert.DeserializeObject<AppointmentDTO>(aptPayload);

        if (apt.Id <= 0)
        {
            appointments.Add(aptFromServer);
        }
        else
        {
            var appointmentToEdit = Appointments.FirstOrDefault(b => (b?.Id ?? 0) == apt.Id);
            if (appointmentToEdit != null)
            {
                var index = Appointments.IndexOf(appointmentToEdit);
                Appointments.RemoveAt(index);
                appointments.Insert(index, apt);
            }
        }
        return apt;
    }

    public AppointmentDTO? Delete(int id)
    {
        var response = new WebRequestHandler().Delete($"/Appointment/{id}").Result;
        var appointmentToDelete = appointments
            .Where(b => b != null)
            .FirstOrDefault(b => (b?.Id ?? -1) == id);
        //delete it!
        appointments.Remove(appointmentToDelete);

        return appointmentToDelete;
    }

    public async Task<List<AppointmentDTO>> Search(QueryRequest query)
    {
        var aptPayload = await new WebRequestHandler().Post("/Appointment/Search", query);
        var aptFromServer = JsonConvert.DeserializeObject<List<AppointmentDTO?>>(aptPayload);

        appointments = aptFromServer;
        return appointments;
    }
}
