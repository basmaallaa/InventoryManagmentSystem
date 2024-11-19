using Inventory.Data.App;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Business.App
{
    public class Repository<TEntity> : IRepositry<TEntity> where TEntity : class
    {
        public InventoryContext _Context { get; set; }

        public DbSet<TEntity> entities { get; set; }
        public Repository(InventoryContext context)
        {
            _Context = context;
            entities = _Context.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            entities.Add(entity);
        }

        public void delete(int id)
        {
            TEntity entity = GetById(id);
            entities.Remove(entity);
        }

        public ICollection<TEntity> GetALL()
        {
            return entities.ToList();
        }

        public TEntity GetById(int id)
        {
            return entities.Find(id);
        }

        public int save()
        {
            return _Context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            entities.Update(entity);
        }
        public bool Exists(int productId)
        {
            return _Context.Products.Any(p => p.ProductID == productId);
        }

    }
}
