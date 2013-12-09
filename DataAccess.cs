using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UsuariosAsain
{
    public class DataAccess
    {
        private DatosUsuariosDataContext dataContext;

        internal DataAccess()
        {

        }

        internal DataAccess(DatosUsuariosDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        internal DatosUsuariosDataContext DataContext
        {
            get
            {
                return dataContext;
            }
            set
            {
                dataContext = value;
            }
        }

        internal IQueryable<Usuario> GetAllUsuarios(Expression<Func<Usuario, string>> expresion, bool descending)
        {
            IQueryable<Usuario> query = from u in dataContext.Usuario select u;
            if (descending)
                return query.OrderByDescending(expresion);
            return query.OrderBy(expresion);
        }

        internal IQueryable<Usuario> GetAllUsuariosByDni(bool descending)
        {
            return GetAllUsuarios(u => u.NúmFax, descending);
        }

        internal IQueryable<Usuario> GetAllUsuariosByName(bool descending)
        {
            return GetAllUsuarios(u => u.Nombre, descending);
        }

        internal Usuario GetUsuarioByDni(string dni)
        {
            IEnumerable<Usuario> usuario = from u in dataContext.Usuario where u.NúmFax == dni select u;
            if (usuario.Count() == 0)
                return null;
            return usuario.ToList<Usuario>()[0];
        }
    }
}
