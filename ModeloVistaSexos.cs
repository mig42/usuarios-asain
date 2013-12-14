using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsuariosAsain
{
    public class ModeloVistaSexos : Notificador
    {
        private ObservableCollection<Tipos_de_usuario> _sexos =
            new ObservableCollection<Tipos_de_usuario>();
        private Usuario _usuarioSeleccionado = new Usuario();

        public ObservableCollection<Tipos_de_usuario> Sexos
        {
            get
            {
                return _sexos;
            }
            set
            {
                if (value == _sexos)
                    return;
                _sexos = value;
                NotifyPropertyChange("Sexos");
            }
        }
    }
}
