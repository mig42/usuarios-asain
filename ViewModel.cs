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

        public ObservableCollection<Usuario> Usuarios
        {
            get
            {
                return _usuarios;
            }
            set
            {
                if (value == _usuarios)
                    return;
                _usuarios = value;
                NotifyPropertyChange("Usuarios");
            }
        }
        
        public Usuario UsuarioSeleccionado
        {
            get
            {
                return _usuarioSeleccionado;
            }
            set
            {
                if (value == _usuarioSeleccionado)
                    return;
                _usuarioSeleccionado = value;
                NotifyPropertyChange("UsuarioSeleccionado");
            }
        }

        public void LimpiarUsuarios()
        {
            _usuarios.Clear();
        }

        public void AnyadirUsuario(Usuario u)
        {
            _usuarios.Add(u);
        }

        public void EliminarUsuario(Usuario u)
        {
            _usuarios.Remove(u);
        }
    }
}
