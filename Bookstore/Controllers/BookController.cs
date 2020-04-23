using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bookstore.Models;
using Bookstore.Models.Repositories;
using Bookstore.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Controllers
{
    public class BookController : Controller
    {
        private readonly IbookstoreRepository<Book> bookRepository;
        private readonly IbookstoreRepository<Author> authorRepository;
        private readonly IHostingEnvironment hosting;

        public BookController(IbookstoreRepository<Book> bookRepository,
            IbookstoreRepository<Author> authorRepository,
            IHostingEnvironment hosting)
        {
            this.bookRepository = bookRepository;
            this.authorRepository = authorRepository;
            this.hosting = hosting;
        }
        // GET: Book
        public ActionResult Index()
        {
            var books = bookRepository.List();
            return View(books);
        }

        // GET: Book/Details/5
        public ActionResult Details(int id)
        {
            var book = bookRepository.find(id);
            return View(book);
        }

        // GET: Book/Create
        public ActionResult Create()
        {
            var model = new BookAuthorViewModel
            {
                Authors = FillSelecList()
            };
            return View(model);
        }

        // POST: Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookAuthorViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string fileName = Uploadfile(model.File) ?? string.Empty;
                    if (model.Author == -1)
                    {
                        ViewBag.Message = "Please select an author from the list!";
                        var ViewModel = getAllAuthors();
                        return View(ViewModel);
                    }
                    var author = authorRepository.find(model.Author);
                    var book = new Book
                    {
                        Id = model.BookId,
                        Title = model.Title,
                        Description = model.Description,
                        Author = author,
                        ImageUrl = fileName
                    };

                    bookRepository.Add(book);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            ModelState.AddModelError("", "You have to fill all the required fields!");

            return View(getAllAuthors());

        }

        // GET: Book/Edit/5
        public ActionResult Edit(int id)
        {
            var authors = authorRepository.List().ToList();
            var book = bookRepository.find(id);
            var author = book.Author == null ? book.Author.Id = 0 : book.Author.Id;

            var viewModel = new BookAuthorViewModel
            {
                BookId = book.Id,
                Title = book.Title,
                Description = book.Description,
                Author = book.Author.Id,
                Authors = authors,
                ImageUrl = book.ImageUrl
            };
            return View(viewModel);
        }

        // POST: Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, BookAuthorViewModel viewModel)
        {
            try
            {
                string fileName = Uploadfile(viewModel.File, viewModel.ImageUrl);
                //if (viewModel.File != null)
                //{
                //    var uploads = Path.Combine(hosting.WebRootPath, "uploads");
                //    fileName = Path.GetFileName(viewModel.File.FileName);
                //    string fullPath = Path.Combine(uploads, fileName);

                //    string oldFileName = viewModel.ImageUrl != null ? viewModel.ImageUrl : string.Empty;
                //    string fullOldPath = Path.Combine(uploads, oldFileName);

                //    if (fullOldPath != fullPath)
                //    {

                //        if (!System.IO.File.Exists(fullPath))
                //        {
                //            viewModel.File.CopyTo(new FileStream(fullPath, FileMode.Create));
                //        }
                //    }

                //}

                var author = authorRepository.find(viewModel.Author);
                var book = new Book
                {
                    Id = viewModel.BookId,
                    Title = viewModel.Title,
                    Description = viewModel.Description,
                    ImageUrl = fileName,
                    Author = author
                };

                bookRepository.Update(viewModel.BookId, book);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Book/Delete/5
        public ActionResult Delete(int id)
        {
            var book = bookRepository.find(id);
            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            try
            {
                bookRepository.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public List<Author> FillSelecList()
        {
            var authors = authorRepository.List().ToList();
            authors.Insert(0, new Author { Id = -1, FullName = "---Please select an author ---" });
            return authors;
        }

        public BookAuthorViewModel getAllAuthors()
        {
            var ViewModel = new BookAuthorViewModel
            {
                Authors = FillSelecList()
            };

            return ViewModel;
        }

        string Uploadfile(IFormFile file)
        {
            if (file != null)
            {
                var uploads = Path.Combine(hosting.WebRootPath, "uploads");
                string fullPath = Path.Combine(uploads, Path.GetFileName(file.FileName));
                file.CopyTo(new FileStream(fullPath, FileMode.Create));

                return Path.GetFileName(file.FileName);
            }

            return null;
        }

        string Uploadfile(IFormFile file, string ImageUrl)
        {
            if (file != null)
            {
                var uploads = Path.Combine(hosting.WebRootPath, "uploads");
                string newPath = Path.Combine(uploads, Path.GetFileName(file.FileName));

                string oldPath = Path.Combine(uploads, ImageUrl);

                if (oldPath != newPath)
                {

                    if (!System.IO.File.Exists(newPath))
                    {
                        file.CopyTo(new FileStream(newPath, FileMode.Create));
                    }
                }

                return Path.GetFileName(file.FileName);

            }
            return ImageUrl;
        }

        public ActionResult Search(string term)
        {
            var result = bookRepository.Search(term);

            return View("Index", result);
        }

    }
}