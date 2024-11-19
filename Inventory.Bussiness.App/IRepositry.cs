namespace Inventory.Business.App
{
 
       public interface IRepositry<TEntity> where TEntity : class
        {
            ICollection<TEntity> GetALL();
            public TEntity GetById(int id);
            public void Add(TEntity entity);
            public void Update(TEntity entity);
            public void delete(int id);
            public int save();
        bool Exists(int productID);
    }
}

