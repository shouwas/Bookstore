using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Models.Repositories
{
    public class BookRepository : IbookstoreRepository<Book>
    {
        List<Book> books;
        //Author author_1 = new Author { Id = 1, FullName = "Moi" };

        public BookRepository()
        {
            

            books = new List<Book>()
            {
                new Book {
                    Id = 1,
                    Title = "C# programming",
                    Description = "No description",
                    ImageUrl = "said.jpg",
                    Author = new Author { Id = 1, FullName = "Khalid Essaadani" }
                },
                new Book { Id = 2,
                    Title = "Java programming",
                    Description = "Nothing",
                    ImageUrl = "said.jpg",
                    Author = new Author { Id = 3, FullName = "Said Houwasse" }},
                new Book { Id = 3,
                    Title = "Phyton programming",
                    Description = "No data",
                    ImageUrl = "said.jpg",
                    Author = new Author()}
            };
        }


        public void Add(Book entity)
        {
            entity.Id = books.Max(b => b.Id) + 1;
            books.Add(entity);
        }

        public void Delete(int id)
        {
            var book = find(id);
            books.Remove(book);
        }

        public Book find(int id)
        {
            var book = books.SingleOrDefault(b => b.Id == id);
            return book;
        }

        public IList<Book> List()
        {
            return books;
        }

        public void Update(int id, Book newBook)
        {
            var book = find(id);
            book.Title = newBook.Title;
            book.Description = newBook.Description;
            book.ImageUrl = newBook.ImageUrl;
            book.Author = newBook.Author;
        }

        public List<Book> Search(string term)
        {
            return books.Where(b => b.Title.Contains(term)).ToList();
        }
    }
}
