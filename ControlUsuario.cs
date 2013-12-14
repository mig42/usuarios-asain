using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UsuariosAsain
{
    public enum Criterio
    {
        Nombre,
        Dni
    }

    public enum Direccion
    {
        Ascendente,
        Descendente
    }

    public class ControlUsuario : Notificador
    {
        private DataAccess dataAccess;
        private ModeloVistaUsuarios _usuarios;
        private ModeloVistaSexos _sexos;
        private Criterio criterioActual = Criterio.Dni;
        private bool ordenacionDescendente = false;

        private readonly BackgroundWorker usuariosBackgroundWorker;
        private readonly BackgroundWorker guardarBackgroundWorker;
        private readonly BackgroundWorker cargaInicialWorker;

        public ControlUsuario()
        {
            usuariosBackgroundWorker = new BackgroundWorker();
            guardarBackgroundWorker = new BackgroundWorker();
            cargaInicialWorker = new BackgroundWorker();

            InicializaWorkerRecuperarLista();
            InicializaWorkerGuardar();
            InicializaWorkerCargaInicial();
        }

        public ModeloVistaUsuarios Usuarios
        {
            get
            {
                return _usuarios;
            }
            set
            {
                if (value == null || value == _usuarios)
                    return;
                _usuarios = value;
                RefrescarListaDeUsuarios();
                NotifyPropertyChange("Usuarios");
            }
        }

        public ModeloVistaSexos Sexos
        {
            get
            {
                return _sexos;
            }
            set
            {
                if (value == null || value == _sexos)
                    return;
                _sexos = value;
                CargarDatosIniciales();
                NotifyPropertyChange("Sexos");
            }
        }

        public DataAccess DataAccess
        {
            get
            {
                return dataAccess;
            }
            set
            {
                if (value == null)
                    return;
                dataAccess = value;
            }
        }

        public void RefrescarListaDeUsuarios()
        {
            if (dataAccess == null || _usuarios == null)
                return;
            usuariosBackgroundWorker.RunWorkerAsync();
            NotifyPropertyChange("CanExecuteThread");
        }

        public void CargarDatosIniciales()
        {
            if (dataAccess == null)
                return;

            cargaInicialWorker.RunWorkerAsync();
            NotifyPropertyChange("CanExecuteThread");
        }

        public bool CanExecuteThread
        {
            get
            {
                return !(EstaCargandoInicial() || EstaCargandoUsuarios());
            }
        }

        private bool EstaCargandoUsuarios()
        {
            return usuariosBackgroundWorker.IsBusy;
        }

        private bool EstaCargandoInicial()
        {
            return cargaInicialWorker.IsBusy;
        }

        public bool CanSave
        {
            get { return !guardarBackgroundWorker.IsBusy; }
        }

        public void AlternarDirecciónDeOrdenado()
        {
            ordenacionDescendente = !ordenacionDescendente;
            RefrescarListaDeUsuarios();
        }

        public void ReordenarListaDeUsuarios(Criterio nuevoCriterio)
        {
            if (nuevoCriterio == criterioActual)
            {
                AlternarDirecciónDeOrdenado();
                return;
            }

            criterioActual = nuevoCriterio;
            ordenacionDescendente = false;
            RefrescarListaDeUsuarios();
        }

        public void GuardarCambios()
        {
            guardarBackgroundWorker.RunWorkerAsync();
            NotifyPropertyChange("CanSave");
        }

        private void InicializaWorkerRecuperarLista()
        {
            usuariosBackgroundWorker.DoWork += delegate
            {
                _usuarios.Usuarios = RecuperarListaDeUsuarios();
            };
            usuariosBackgroundWorker.RunWorkerCompleted +=
                GetHandler("CanExecuteThread");
        }

        private void InicializaWorkerGuardar()
        {
            guardarBackgroundWorker.DoWork += delegate
            {
                dataAccess.GuardarCambios();
            };
            guardarBackgroundWorker.RunWorkerCompleted +=
                GetHandler("CanSave");
        }

        private void InicializaWorkerCargaInicial()
        {
            cargaInicialWorker.DoWork += delegate
            {
                while (EstaCargandoUsuarios())
                    Thread.Sleep(100);
                _sexos.Sexos = RecuperarListaDeSexos();
            };
            cargaInicialWorker.RunWorkerCompleted +=
                GetHandler("CanExecuteThread");
        }

        private RunWorkerCompletedEventHandler GetHandler(string property)
        {
            return delegate
            {
                NotifyPropertyChange(property);
            };
        }

        private ObservableCollection<Usuario> RecuperarListaDeUsuarios()
        {
            switch (criterioActual)
            {
                case Criterio.Nombre:
                    return new UsuarioObservable(dataAccess.GetAllUsuariosByName(ordenacionDescendente));
                case Criterio.Dni:
                    return new UsuarioObservable(dataAccess.GetAllUsuariosByDni(ordenacionDescendente));
                default:
                    throw new ArgumentException("Criterio de ordenación no reconocido.");
            }
        }

        private ObservableCollection<Tipos_de_usuario> RecuperarListaDeSexos()
        {
            ObservableCollection<Tipos_de_usuario> usuarios =
                new ObservableCollection<Tipos_de_usuario>();

            foreach(Tipos_de_usuario sexo in dataAccess.GetAllSexos())
            {
                usuarios.Add(sexo);
            }
            return usuarios;
        }
    }
}
