using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuthorAuthentication.Data;
using AuthorAuthentication.Models;
using System.Web.Security;
using System.Web.UI.WebControls;
using AuthorAuthentication.ViewModel;

namespace AuthorAuthentication.Controllers
{
    [AllowAnonymous]
    public class AuthorController : Controller
    {
        // GET: Author
        public ActionResult Index()
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                return View();
            }
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginVM loginVM)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    var author = session.Query<Author>().SingleOrDefault(u => u.Name == loginVM.UserName);
                    if (author != null)
                    {
                        if (BCrypt.Net.BCrypt.Verify(loginVM.Password, author.Password))
                        {
                            // Set auth cookie for username
                            FormsAuthentication.SetAuthCookie(loginVM.UserName, true);

                            // Store Author.Id in session
                            Session["AuthorId"] = author.Id;

                            return RedirectToAction("Index", "Book");
                        }
                    }
                    ModelState.AddModelError("", "UserName/Password doesn't match");
                    return View();
                }
            }
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Author author)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    author.AuthorDetail.Author=author;
                    author.Password = BCrypt.Net.BCrypt.HashPassword(author.Password);

                    session.Save(author);
                    txn.Commit();
                    return RedirectToAction("Login");
                }
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();

            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult GetAllBooks()
        {
            using(var session = NHibernateHelper.CreateSession())
            {
                var books=session.Query<Book>().ToList();
                return View(books);
            }
        }

        public ActionResult AuthorBooks()
        {
            var viewModelList = new List<AuthorBookVM>();

            using (var session = NHibernateHelper.CreateSession())
            {
                var authors = session.Query<Author>().ToList(); 

                foreach (var author in authors)
                {
                    foreach (var book in author.Books)
                    {
                        var viewModel = new AuthorBookVM
                        {
                            AuthorName = author.Name,
                            BookTitle = book.Title,
                            Genre = book.Genre
                        };
                        viewModelList.Add(viewModel);
                    }
                }
                return View(viewModelList);
            }
        }

    }
}