using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
        private ViewModel viewModel;
        private Criterio criterioActual = Criterio.Dni;
        private bool ordenacionDescendente = false;

        private readonly BackgroundWorker backgroundWorker;

        public ControlUsuario()
        {
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += delegate
            {
                viewModel.Usuarios = RecuperarListaDeUsuarios();
            };
            backgroundWorker.RunWorkerCompleted += delegate
            {
                NotifyPropertyChange("CanExecuteThread");
            };
        }

        public ViewModel ViewModel
        {
            get
            {
                return viewModel;
            }
            set
            {
                if (value == null)
                    return;
                viewModel = value;
                RefrescarListaDeUsuarios();
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
                RefrescarListaDeUsuarios();
            }
        }

        public void RefrescarListaDeUsuarios()
        {
            if (dataAccess == null || viewModel == null)
                return;
            backgroundWorker.RunWorkerAsync();
            NotifyPropertyChange("CanExecuteThread");
        }

        public bool CanExecuteThread
        {
            get { return !backgroundWorker.IsBusy; }
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
    }
}
