using mycooking.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace mycooking.Services
{
    public class RecetaPorPaisViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static RecetaPorPaisViewModel _instance;
        public static RecetaPorPaisViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RecetaPorPaisViewModel();
                }
                return _instance;
            }
        }

        private ObservableCollection<Pais> _paises = new ObservableCollection<Pais>();
        public ObservableCollection<Pais> Paises
        {
            get { return _paises; }
            set
            {
                _paises = value;
                OnPropertyChanged(nameof(Paises));
            }
        }

        private ObservableCollection<Receta> _recetas = new ObservableCollection<Receta>();
        public ObservableCollection<Receta> Recetas
        {
            get { return _recetas; }
            set
            {
                _recetas = value;
                OnPropertyChanged(nameof(Recetas));
            }
        }

        public void AgregarPais(Pais pais)
        {
            Paises.Add(pais);
            OnPropertyChanged(nameof(Paises));
        }

        public void AgregarReceta(Receta receta)
        {
            _recetas.Add(receta);
            OnPropertyChanged(nameof(Recetas));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private bool _isRecipeSelected;
        public bool IsRecipeSelected
        {
            get { return _isRecipeSelected; }
            set
            {
                _isRecipeSelected = value;
                OnPropertyChanged(nameof(IsRecipeSelected));
            }
        }
    }
}