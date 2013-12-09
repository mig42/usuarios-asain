using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsuariosAsain
{
    class UsuarioObservable : ObservableCollection<Usuario>
    {
        public UsuarioObservable()
        {

        }

        public UsuarioObservable(IQueryable<Usuario> usuarios)
        {
            foreach (Usuario u in usuarios)
            {
                this.Add(u);
            }
        }
    }
}
