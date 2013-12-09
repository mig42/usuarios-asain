using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsuariosAsain
{
    public abstract class Notificador : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChange(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }

    public class ViewModel : Notificador
    {
        private ObservableCollection<Usuario> _usuarios = new UsuarioObservable();
        private Usuario _usuarioSeleccionado = new Usuario();

        internal ObservableCollection<Usuario> Usuarios
        {
            get
            {
                return _usuarios;
            }
            set
            {
                _usuarios.Clear();
                _usuarios.Concat(value);
                NotifyPropertyChange("Usuarios");
            }
        }
        
        internal Usuario UsuarioSeleccionado
        {
            get
            {
                return _usuarioSeleccionado;
            }
            set
            {
                _usuarioSeleccionado = value;
                NotifyPropertyChange("UsuarioSeleccionado");
            }
        }
    }
}
