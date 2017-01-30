using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibSystem.Models;

namespace LibSystem.Controllers
{
    public class MemberTablesController : Controller
    {
        private LibraryDBEntities1 db = new LibraryDBEntities1();

        // GET: MemberTables
        public ActionResult Index()
        {
            return View(db.MemberTable.ToList());
        }

        // GET: MemberTables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberTable memberTable = db.MemberTable.Find(id);
            if (memberTable == null)
            {
                return HttpNotFound();
            }
            return View(memberTable);
        }

        // GET: MemberTables/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MemberTables/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MemberID,Name,Address,LoanHistory")] MemberTable memberTable)
        {
            if (ModelState.IsValid)
            {
                db.MemberTable.Add(memberTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(memberTable);
        }

        // GET: MemberTables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberTable memberTable = db.MemberTable.Find(id);
            if (memberTable == null)
            {
                return HttpNotFound();
            }
            return View(memberTable);
        }

        // POST: MemberTables/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MemberID,Name,Address,LoanHistory")] MemberTable memberTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(memberTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(memberTable);
        }

        // GET: MemberTables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberTable memberTable = db.MemberTable.Find(id);
            if (memberTable == null)
            {
                return HttpNotFound();
            }
            return View(memberTable);
        }

        // POST: MemberTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var book = db.BookTable.FirstOrDefault(s => s.MemberID == id);
            book.MemberID = null;
            book.OnLoan = 0;
            MemberTable memberTable = db.MemberTable.Find(id);
            db.MemberTable.Remove(memberTable);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

       
    }
}
