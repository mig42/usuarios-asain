using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsuariosAsain
{
    class DataAccess
    {
        private DatosUsuariosDataContext DataContext;

        internal DataAccess(DatosUsuariosDataContext dataContext)
        {
            DataContext = dataContext;
        }

        List<Usuario> GetAllUsuarios()
        {
            IQueryable<Usuario> usuarios = from u in DataContext.Usuario select u;
            return usuarios.ToList<Usuario>();
        }

        internal Usuario GetUsuarioByDni(string dni)
        {
            IEnumerable<Usuario> usuario = from u in DataContext.Usuario where u.NúmFax == dni select u;
            if (usuario.Count() == 0)
                return null;
            return usuario.ToList<Usuario>()[0];
        }
    }
}
