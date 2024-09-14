using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuthorAuthentication.Data;
using AuthorAuthentication.Models;

namespace AuthorAuthentication.Controllers
{
    public class BookController : Controller
    {
        // GET: Book
        public ActionResult Index()
        {
            var authorId = (Guid?)Session["AuthorId"];

            if (authorId == null)
            {
                return RedirectToAction("Login", "Author");
            }

            using (var session = NHibernateHelper.CreateSession())
            {
                var books = session.Query<Book>().Where(b => b.Author.Id == authorId).ToList();
                return View(books);
            }
        }

        public ActionResult Details()
        {
            var authorId = (Guid?)Session["AuthorId"];

            if (authorId == null)
            {
                return RedirectToAction("Login", "Author");
            }

            using (var session = NHibernateHelper.CreateSession())
            {
                var books = session.Query<Book>().Where(b => b.Author.Id == authorId).ToList();
                return View(books);
            }
        }



        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Book book)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var authId = Session["AuthorId"];
                using (var txn = session.BeginTransaction())
                {
                    var author = session.Query<Author>().FirstOrDefault(e => e.Id == (Guid)authId);
                    book.Author = author;
                    session.Save(book);
                    txn.Commit();
                    return RedirectToAction("Index");

                }
            }
        }

        public ActionResult Edit(int id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var targetBook = session.Get<Book>(id);
                return View(targetBook);
            }

        }


        [HttpPost]
        public ActionResult Edit(Book book)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    var existingBook = session.Get<Book>(book.Id);
                    existingBook.Title = book.Title;
                    existingBook.Genre = book.Genre;
                    existingBook.Description = book.Description;
                    session.Update(existingBook);
                    txn.Commit();
                    return RedirectToAction("Index");
                }

            }

        }



        public ActionResult Delete(int id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var authId = Session["AuthorId"];
                var author = session.Query<Author>().FirstOrDefault(a => a.Id == (Guid)authId);

                if (author == null)
                {
                    return HttpNotFound("Author not found");
                }

                var targetBook = author.Books.FirstOrDefault(o => o .Id== id);

                if (targetBook == null)
                {
                    return HttpNotFound("Book not found");
                }

                return View(targetBook);
            }
        }


        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteBook(int id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var txn = session.BeginTransaction())
                {

                    var targetBook = session.Get<Book>(id);

                    session.Delete(targetBook);

                    txn.Commit();

                    return RedirectToAction("Index");
                }
            }

        }
    }
}