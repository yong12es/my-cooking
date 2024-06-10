using mycooking.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace mycooking.Services
{
    public class TallerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static TallerViewModel _instance;
        public static TallerViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TallerViewModel();
                }
                return _instance;
            }
        }

        private ObservableCollection<Taller> _talleres = new ObservableCollection<Taller>();
        public ObservableCollection<Taller> Talleres
        {
            get { return _talleres; }
            set { _talleres = value; }
        }

        private ObservableCollection<Taller> _talleresParticipados = new ObservableCollection<Taller>();
        public ObservableCollection<Taller> TalleresParticipados
        {
            get { return _talleresParticipados; }
            set { _talleresParticipados = value; }
        }

        public void AgregarTaller(Taller taller)
        {
            _talleres.Add(taller);
            OnPropertyChanged(nameof(Talleres));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void MoverTallerParticipado(Taller taller)
        {
            _talleres.Remove(taller);
            OnPropertyChanged(nameof(Talleres));
            _talleresParticipados.Add(taller);
            OnPropertyChanged(nameof(TalleresParticipados));
        }
    }
}
