using Library.Healthcare.Models;
using Library.Healthcare.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Maui.Healthcare.ViewModels
{
    public partial class MainViewModel : INotifyPropertyChanged
    {
         public MainViewModel()
        {

        }
        
        public ObservableCollection<PatientViewModel?> Patients
         {
            get
            {
                return new ObservableCollection<PatientViewModel?>
                    (PatientServiceProxy.Current.Patients.Select(p => new PatientViewModel (p)));
            }
         }

        public ObservableCollection<PhysicianViewModel?> Physicians
        {
            get
            {
                return new ObservableCollection<PhysicianViewModel?>
                    (PhysicianServiceProxy.Current.Physicians.Select(p => new PhysicianViewModel(p)));
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
            NotifyPropertyChanged(nameof(Physicians));
        }
        public PatientViewModel? SelectedPatient { get; set; }
        public PhysicianViewModel? SelectedPhysician { get; set; }
        public String? Query { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void DeletePatient()
        {
            if (SelectedPatient == null) return;
            PatientServiceProxy.Current.Delete(SelectedPatient?.Model?.Id ?? 0);
            NotifyPropertyChanged(nameof(Patients));
        }

        public void DeletePhysician()
        {
            if (SelectedPhysician == null) return;
            PhysicianServiceProxy.Current.Delete(SelectedPhysician?.Model?.Id ?? 0);
            NotifyPropertyChanged(nameof(Physicians));
        }

        public void ExpandCard()
        {
            InlineCardVisibility = 
            InlineCardVisibility == Visibility.Visible ? 
            Visibility.Collapsed : Visibility.Visible;
        }

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
