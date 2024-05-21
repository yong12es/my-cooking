using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using mycooking.Models;

namespace mycooking.Services
{
    public class TallerService: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static TallerService _instance;
        public static TallerService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TallerService();
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

        private ObservableCollection<Taller> _talleresParticipados = new ObservableCollection<Taller>(); // Lista de talleres en los que el usuario ha participado
        public ObservableCollection<Taller> TalleresParticipados
        {
            get { return _talleresParticipados; }
            set { _talleresParticipados = value; }
        }

        public void AgregarTaller(Taller taller)
        {
            _talleres.Add(taller);
            OnPropertyChanged(nameof(Talleres)); // Notificar que la propiedad Talleres ha cambiado
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void MoverTallerParticipado(Taller taller)
        {
            // Remover el taller de la lista principal
            _talleres.Remove(taller);
            OnPropertyChanged(nameof(Talleres));

            // Agregar el taller a la lista de talleres participados
            _talleresParticipados.Add(taller);
            OnPropertyChanged(nameof(TalleresParticipados));
        }
    }
}
