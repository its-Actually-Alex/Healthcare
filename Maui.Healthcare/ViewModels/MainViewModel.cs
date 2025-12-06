using Library.Healthcare.DTO;
using Library.Healthcare.Models;
using Library.Healthcare.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Maui.Healthcare.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            InlinePatient = new PatientViewModel();
            InlineCardVisibility = Visibility.Collapsed;
            ImportPath = @"C:\temp\data.json";

            patients = new ObservableCollection<PatientViewModel?>
                    (PatientServiceProxy
                    .Current
                    .Patients
                    .Select(b => new PatientViewModel(b))
                    );

            // Initialize appointments
            appointments = new ObservableCollection<AppointmentViewModel?>
                    (AppointmentServiceProxy
                    .Current
                    .Appointments
                    .Select(a => new AppointmentViewModel(a))
                    );

            ApplySort();
        }

        private ObservableCollection<PatientViewModel?> patients;
        public ObservableCollection<PatientViewModel?> Patients
        {
            get
            {
                return patients;
            }
        }

        private ObservableCollection<AppointmentViewModel?> appointments;
        public ObservableCollection<AppointmentViewModel?> Appointments
        {
            get
            {
                return appointments;
            }
        }

        private Visibility inlineCardVisibility;
        public Visibility InlineCardVisibility
        {
            get
            {
                return inlineCardVisibility;
            }

            set
            {
                if (inlineCardVisibility != value)
                {
                    inlineCardVisibility = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public void Refresh()
        {
            // Refresh patients
            patients.Clear();
            foreach (var patient in PatientServiceProxy.Current.Patients.Select(b => new PatientViewModel(b)))
            {
                patients.Add(patient);
            }
            ApplySort(); // Apply current sort after refreshing

            // Refresh appointments
            appointments.Clear();
            foreach (var appointment in AppointmentServiceProxy.Current.Appointments.Select(a => new AppointmentViewModel(a)))
            {
                appointments.Add(appointment);
            }
        }

        public void Search()
        {
            var patientDTOs = PatientServiceProxy.Current.Search(new Library.Healthcare.Data.QueryRequest { Content = Query }).Result;
            patients = new ObservableCollection<PatientViewModel?>(patientDTOs.Select(b => new PatientViewModel(b)));
            NotifyPropertyChanged(nameof(Patients));
        }

        public void Export()
        {
            var patientString = JsonConvert.SerializeObject(
                Patients
                .Where(b => b != null)
                .Select(b => b.Model));

            using (StreamWriter sw = new StreamWriter(@"C:\temp\data.json"))
            {
                sw.WriteLine(patientString);
            }
        }

        public void Import()
        {
            using (StreamReader sr = new StreamReader(ImportPath))
            {
                var patientString = sr.ReadLine();
                if (string.IsNullOrEmpty(patientString))
                {
                    return;
                }

                var patients = JsonConvert.DeserializeObject<List<PatientDTO>>(patientString);

                foreach (var pat in patients)
                {
                    pat.Id = 0;
                    PatientServiceProxy.Current.AddOrUpdate(pat);
                }
                NotifyPropertyChanged(nameof(Patients));
            }

            //var blogString = File.ReadAllText(ImportPath);

        }

        //Patient sorting mechanism
        public string SortBy
        {
            get { return sortBy; }
            set
            {
                if (sortBy != value)
                {
                    sortBy = value;
                    NotifyPropertyChanged();
                    ApplySort(); // Re-sort when option changes
                }
            }
        }

        private void ApplySort()
        {
            List<PatientViewModel?> sortedList;

            switch (SortBy)
            {
                case "Name":
                    sortedList = IsAscending
                        ? patients.OrderBy(p => p?.Model?.Name).ToList()
                        : patients.OrderByDescending(p => p?.Model?.Name).ToList();
                    break;
                case "Birthdate":
                    sortedList = IsAscending
                        ? patients.OrderBy(p => p?.Model?.Birthdate).ToList()
                        : patients.OrderByDescending(p => p?.Model?.Birthdate).ToList();
                    break;
                case "ID":
                default:
                    sortedList = IsAscending
                        ? patients.OrderBy(p => p?.Model?.Id ?? 0).ToList()
                        : patients.OrderByDescending(p => p?.Model?.Id ?? 0).ToList();
                    break;
            }

            patients.Clear();
            foreach (var patient in sortedList)
            {
                patients.Add(patient);
            }

            NotifyPropertyChanged(nameof(SortDirectionText)); // Update button text
        }

        //Sorting order (ascending vs descending)
        private bool isAscending = true; //Default to ascending
        public bool IsAscending
        {
            get { return isAscending; }
            set
            {
                if (isAscending != value)
                {
                    isAscending = value;
                    NotifyPropertyChanged();
                    ApplySort(); //Re-sort when direction changes
                }
            }
        }

        //Display text on the toggle button
        public string SortDirectionText
        {
            get { return IsAscending ? "↑ Ascending" : "↓ Descending"; }
        }

        //Sorting direction controller
        public Command ToggleSortDirectionCommand => new Command(() =>
        {
            IsAscending = !IsAscending;
        });

        public string ImportPath { get; set; }
        public PatientViewModel? SelectedPatient { get; set; }
        public AppointmentViewModel? SelectedAppointment { get; set; }
        public string? Query { get; set; }

        public PatientViewModel? InlinePatient { get; set; }

        private string sortBy = "ID"; //Default sort value

        //Sorting options
        public List<string> SortOptions { get; } = new List<string>
        {
            "ID",
            "Name",
            "Birthdate"
        };

        public event PropertyChangedEventHandler? PropertyChanged;

        public void Delete()
        {
            if (SelectedPatient == null)
            {
                return;
            }

            PatientServiceProxy.Current.Delete(SelectedPatient?.Model?.Id ?? 0);
            NotifyPropertyChanged(nameof(Patients));
        }

        public void DeleteAppointment()
        {
            if (SelectedAppointment == null)
            {
                return;
            }

            AppointmentServiceProxy.Current.Delete(SelectedAppointment?.Model?.Id ?? 0);
            appointments.Clear();
            foreach (var appointment in AppointmentServiceProxy.Current.Appointments.Select(a => new AppointmentViewModel(a)))
            {
                appointments.Add(appointment);
            }
        }

        public async Task<bool> AddInlinePatient()
        {
            try
            {
                await PatientServiceProxy.Current.AddOrUpdate(InlinePatient?.Model);
                NotifyPropertyChanged(nameof(Patients));

                InlinePatient = new PatientViewModel();
                NotifyPropertyChanged(nameof(InlinePatient));
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public void ExpandCard()
        {
            InlineCardVisibility
                = InlineCardVisibility == Visibility.Visible ?
                Visibility.Collapsed : Visibility.Visible;
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}