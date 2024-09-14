using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuthorAuthentication.Data;
using AuthorAuthentication.Models;
using NHibernate.Linq;

namespace AuthorAuthentication.Controllers
{
    public class AuthorDetailController : Controller
    {
        // GET: AuthorDetail
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Detail()
        {
            // Retrieve the Author.Id from session
            var authorId = (Guid?)Session["AuthorId"];

            using (var session = NHibernateHelper.CreateSession())
            {
                var details = session.Query<AuthorDetail>().FirstOrDefault(ad => ad.Author.Id == authorId);
                return View(details);
            }
        }


        public ActionResult Edit(int id)
        {

            using (var session = NHibernateHelper.CreateSession())
            {
                var targetBook = session.Get<AuthorDetail>(id);
                return View(targetBook);
            }

        }


        [HttpPost]
        public ActionResult Edit(AuthorDetail detail)
        {
            var authId = Session["AuthorId"];

            using (var session = NHibernateHelper.CreateSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    var existingAuthorDetail = session.Get<AuthorDetail>(detail.Id);
                    existingAuthorDetail.City = detail.City;
                    existingAuthorDetail.Street = detail.Street;
                    existingAuthorDetail.State = detail.State;
                    existingAuthorDetail.Country = detail.Country;

                    session.Update(existingAuthorDetail);
                    txn.Commit();
                    return RedirectToAction("Detail");
                }

            }

        }

        public ActionResult Delete(int id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var targetBook = session.Get<AuthorDetail>(id);
                return View(targetBook);
            }
        }

        [HttpPost]
        public ActionResult Delete(AuthorDetail detail)
        {
            var authId = Session["AuthorId"];

            using (var session = NHibernateHelper.CreateSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    var existingAuthorDetail = session.Get<AuthorDetail>(detail.Id);
                    existingAuthorDetail.City = null;
                    existingAuthorDetail.Street = null;
                    existingAuthorDetail.State = null;
                    existingAuthorDetail.Country = null;

                    session.Update(existingAuthorDetail);
                    txn.Commit();
                    return RedirectToAction("Detail");
                }

            }
        }
    }
}