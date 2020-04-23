using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Models.Repositories
{
    public class AuthorRepository: IbookstoreRepository<Author>
    {
        List<Author> authors;
        public AuthorRepository()
        {
            authors = new List<Author>()
            {
                new Author { Id = 1, FullName = "Khalid Essaadani"},
                new Author { Id = 2, FullName = "Hamid Makboul"},
                new Author { Id = 3, FullName = "Said Houwasse"}
            };
        }

        public void Add(Author entity)
        {
            entity.Id = authors.Max(a => a.Id) + 1;
            authors.Add(entity);
        }

        public void Delete(int id)
        {
            var author = find(id);
            authors.Remove(author);
        }

        public Author find(int id)
        {
            var author = authors.SingleOrDefault(a => a.Id == id);
            return author;
        }

        public IList<Author> List()
        {
            return authors;
        }

        public void Update(int id, Author entity)
        {
            var author = find(id);
            author.FullName = entity.FullName;
        }

        public List<Author> Search(string term)
        {
            return  authors.Where(a => a.FullName.Contains(term)).ToList();
        }
    }
}
