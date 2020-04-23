using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Models.Repositories
{
    public interface IbookstoreRepository<TEntity>
    {
        IList<TEntity> List();

        TEntity find(int id);

        void Add(TEntity entity);

        void Update(int id, TEntity entity);

        void Delete(int id);

        List<TEntity> Search(string term);
    }
}
