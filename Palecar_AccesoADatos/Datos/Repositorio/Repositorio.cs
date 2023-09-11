using Microsoft.EntityFrameworkCore;
using Palecar_AccesoADatos.Datos.Repositorio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Palecar_AccesoADatos.Datos.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        private readonly AplicationDBContext _db;
        internal DbSet<T> dbSet;

        public Repositorio(AplicationDBContext db)
        {
            _db = db;   
            this.dbSet = _db.Set<T>();
        }
        void IRepositorio<T>.Agregar(T entidad)
        {
            dbSet.Add(entidad);
        }

        void IRepositorio<T>.Grabar()
        {
            _db.SaveChanges();
        }

        T IRepositorio<T>.Obtener(int id)
        {
            return dbSet.Find(id);
        }

        T IRepositorio<T>.ObtenerPrimero(Expression<Func<T, bool>> filtro, string incluirPropiedades, bool isTracking)
        {
            IQueryable<T> query = dbSet;

            if (filtro != null)
            {
                query = query.Where(filtro);
            }

            if (incluirPropiedades != null)
            {
                foreach(var incluirProp in incluirPropiedades.Split(new char[] { ',' }))
                {
                    query = query.Include(incluirProp); // ejemplo "Categoría, tipoAplicacion"
                }
            }
            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            return query.FirstOrDefault();
        }

        IEnumerable<T> IRepositorio<T>.ObtenerTodos(Expression<Func<T, bool>> filtro, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, string incluirPropiedades, bool isTracking)
        {
            IQueryable<T> query = dbSet;

            if (filtro != null)
            {
                query = query.Where(filtro);
            }

            if (incluirPropiedades != null)
            {
                foreach (var incluirProp in incluirPropiedades.Split(new char[] { ',' }))
                {
                    query = query.Include(incluirProp); // ejemplo "Categoría, tipoAplicacion"
                }
            }
            if(orderBy != null)
            {
                query = orderBy(query);
            }
            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            return query.ToList();
        }

        void IRepositorio<T>.Remover(T entidad)
        {
            dbSet.Remove(entidad);
        }
    }
}
