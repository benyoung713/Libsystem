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
    public class BookTablesController : Controller
    {
        private LibraryDBEntities1 db = new LibraryDBEntities1();

        // GET: BookTables
        public ActionResult Index(string searchString)
        {
            var bookTable = db.BookTable.Include(b => b.MemberTable);
            

            if (!String.IsNullOrEmpty(searchString))
            {
                bookTable = bookTable.Where(s => s.Title.Contains(searchString));
            }


            return View(bookTable.ToList());
        }

        // GET: BookTables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookTable bookTable = db.BookTable.Find(id);
            if (bookTable == null)
            {
                return HttpNotFound();
            }
            return View(bookTable);
        }

        // GET: BookTables/Create
        public ActionResult Create()
        {
            ViewBag.MemberID = new SelectList(db.MemberTable, "MemberID", "Name");
            return View();
        }

        // POST: BookTables/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookID,Title,Author,Genre,OnLoan,MemberID,Image")] BookTable bookTable)
        {   //default image in case none is added so code doesnt break
            if(bookTable.Image == null)
            {
                bookTable.Image = "http://zombieresearchsociety.com/wp-content/uploads/2011/09/Unknown-Book.jpg";
            }

            if (ModelState.IsValid)
            {
                db.BookTable.Add(bookTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MemberID = new SelectList(db.MemberTable, "MemberID", "Name", bookTable.MemberID);
            return View(bookTable);
        }

        // GET: BookTables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookTable bookTable = db.BookTable.Find(id);
            if (bookTable == null)
            {
                return HttpNotFound();
            }
            ViewBag.MemberID = new SelectList(db.MemberTable, "MemberID", "Name", bookTable.MemberID);
            return View(bookTable);
        }

        // POST: BookTables/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookID,Title,Author,Genre,OnLoan,MemberID,Image")] BookTable bookTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MemberID = new SelectList(db.MemberTable, "MemberID", "Name", bookTable.MemberID);
            return View(bookTable);
        }

        // GET: BookTables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookTable bookTable = db.BookTable.Find(id);
            if (bookTable == null)
            {
                return HttpNotFound();
            }
            return View(bookTable);
        }

        // POST: BookTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BookTable bookTable = db.BookTable.Find(id);
            db.BookTable.Remove(bookTable);
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

        // GET: BookTables/Loan/5
        public ActionResult Loan(int? id, string Name)
        {

            //get member's names for drop down list
            var MemberList = new List<string>();
            var MemberQuery = from item in db.MemberTable
                              select item;

            //populate view model to allow data from different tables to be used in same view
            var model = new MemberBookVM
            {
                MemberTable = MemberQuery.Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Name
                })
            };

                  

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BookTable bookTable = db.BookTable.Find(id);
            if (bookTable == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: BookTables/Loan/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Loan(int id, MemberBookVM model)
        {
            //get member's names for drop down list
            var MemberList = new List<string>();
            var MemberQuery = from item in db.MemberTable
                              select item;

            //populate view model
            var model1 = new MemberBookVM
            {
                MemberTable = MemberQuery.Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.MemberID.ToString()
                })
            };
            

            if (ModelState.IsValid)
            {
                //used to get the instance of the member where the name is the same as the one saved to the vm
                var result = db.MemberTable.FirstOrDefault(s => s.Name == model.memberName);
               
                //grabs the book from the database that is currently being looked at
                var book = db.BookTable.FirstOrDefault(s => s.BookID == id);

                if (book != null)
                {   //changes the onloan status to 1 which makes it 'on loan'
                    book.OnLoan = 1;
                    //puts the member id of the selected member into the memberid section in the book table
                    book.MemberID = result.MemberID;
                    db.Entry(book).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            //ViewBag.MemberID = new SelectList(db.MemberTable, "MemberID", "Name", bookTable.MemberID);
            return View("Index");
        }

        //Get Booktables/return
        public ActionResult Return(int? id)
        {
            var loanQuery = from item in db.BookTable
                            where item.BookID == id
                            select item;

            //var loanModel = new MemberBookVM
            //{
            //    loanedTo = loanQuery.Select(a => a.MemberID)
            //};

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookTable bookTable = db.BookTable.Find(id);
            if (bookTable == null)
            {
                return HttpNotFound();
            }
            return View(bookTable);
        }

        // POST: BookTables/return/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Return(int id)
        {
            var loanQuery = from item in db.BookTable
                            where item.BookID == id
                            select item;

            if (ModelState.IsValid)
            {
                //grabs the current book and clears the memberid row as the book is returned and changes the onloan back to 0               
                var book = db.BookTable.FirstOrDefault(s => s.BookID == id);
                if (book != null)
                {
                    book.OnLoan = 0;
                    book.MemberID = null;
                    db.Entry(book).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            
            return View("Index");
        }
    }
}
