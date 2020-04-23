using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Models.Repositories
{
    public class BookDbRepository : IbookstoreRepository<Book>
    {
        private readonly BookstoreDbContext db;

        public BookDbRepository(BookstoreDbContext _db)
        {
            db = _db;
        }


        public void Add(Book entity)
        {
            db.Books.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var book = find(id);
            db.Books.Remove(book);
            db.SaveChanges();
        }

        public Book find(int id)
        {
            var book = db.Books.Include(a => a.Author).SingleOrDefault(b => b.Id == id);
            return book;
        }

        public IList<Book> List()
        {
            return db.Books.Include(a => a.Author).ToList();
        }

        public void Update(int id, Book newBook)
        {
            db.Books.Update(newBook);
            db.SaveChanges();
        }

        public List<Book> Search(string term)
        {
            var result = db.Books.Include(a => a.Author)
                .Where(b => b.Title.Contains(term)
                    || b.Description.Contains(term)
                    || b.Author.FullName.Contains(term)).ToList();
            return result;
        }
    }
}
