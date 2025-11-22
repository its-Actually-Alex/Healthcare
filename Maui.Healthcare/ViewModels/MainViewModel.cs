using Library.Healthcare.DTO;
using Library.Healthcare.Models;
using Library.Healthcare.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
                    //.Where(
                    //    b => (b?.Title?.ToUpper()?.Contains(Query?.ToUpper() ?? string.Empty) ?? false)
                    //    || (b?.Content?.ToUpper()?.Contains(Query?.ToUpper() ?? string.Empty) ?? false)
                    //)
                    .Select(b => new PatientViewModel(b))
                    );
        }

        private ObservableCollection<PatientViewModel?> patients;
        public ObservableCollection<PatientViewModel?> Patients
        {
            get
            {
                return patients;
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
            NotifyPropertyChanged(nameof(Patients));
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

        public string ImportPath { get; set; }
        public PatientViewModel? SelectedPatient { get; set; }
        public string? Query { get; set; }

        public PatientViewModel? InlinePatient { get; set; }

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