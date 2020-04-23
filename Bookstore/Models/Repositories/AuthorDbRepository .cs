using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Models.Repositories
{
    public class AuthorDbRepository: IbookstoreRepository<Author>
    {
        private readonly BookstoreDbContext db;
        public AuthorDbRepository( BookstoreDbContext _db)
        {
            db = _db;
        }

        public void Add(Author entity)
        {
            db.Authors.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var author = find(id);
            db.Authors.Remove(author);
            db.SaveChanges();
        }

        public Author find(int id)
        {
            var author = db.Authors.SingleOrDefault(a => a.Id == id);
            return author;
        }

        public IList<Author> List()
        {
            return db.Authors.ToList();
        }

        public void Update(int id, Author entity)
        {
            db.Authors.Update(entity);
            db.SaveChanges();
        }

        public List<Author> Search(string term)
        {
            return db.Authors.Where(a => a.FullName.Contains(term)).ToList();
        }
    }
}
