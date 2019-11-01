using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Democracy.Context;
using Democracy.Models;
using Democracy.ModelsView;

namespace Democracy.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GroupsController : Controller
    {
        private DemocracyContext db = new DemocracyContext();

        // GET: Groups
        public ActionResult Index()
        {
            return View(db.Groups.ToList());
        }

        /// <summary>
        /// GET: Groups/Details/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View(GroupDetailsView)</returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Group group = db.Groups.Find(id);

            if (group == null)
            {
                return HttpNotFound();
            }

            GroupDetailsView groupDetailsView = new GroupDetailsView
            {
                GroupId = group.GroupId,
                Description = group.Description,
                Members = group.GroupMembers.ToList()
            };

            return View(groupDetailsView);
        }

        // GET: Groups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GroupId,Description")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Groups.Add(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(group);
        }

        // GET: Groups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Group group = db.Groups.Find(id);

            if (group == null)
            {
                return HttpNotFound();
            }

            return View(group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GroupId,Description")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        // GET: Groups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Group group = db.Groups.Find(id);

            if (group == null)
            {
                return HttpNotFound();
            }

            return View(group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Group group = db.Groups.Find(id);
            db.Groups.Remove(group);

            // Block cascade removing
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null
                    && ex.InnerException.InnerException != null
                    && ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                {
                    ViewBag.Error = "Cant not delete the record because has related records";
                }
                else
                {
                    ViewBag.Error = ex.Message;
                }

                return View(group);
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// GET: Add a new Membet into a Group
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns>View(AddMember)</returns>
        public ActionResult AddMember(int groupId)
        {
            var view = new AddMemberView
            {
                GroupId = groupId
            };

            ViewBag.UserID = new SelectList(
                db.Users.OrderBy(u => u.LastName).ThenBy(u => u.FirstName),
                "UserId",
                "FullName");

            return View(view);
        }

        /// <summary>
        /// POST: Add a new Membet into a Group
        /// </summary>
        /// <param name="view">AddMemverView view</param>
        /// <returns>RedirectToAction("Idex")</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMember(AddMemberView view)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.UserID = new SelectList(
                db.Users.OrderBy(u => u.LastName).ThenBy(u => u.FirstName),
                "UserId",
                "FullName");

                return View(view);
            }

            // Verify if user already belongs to group
            var member = db.GroupMembers
                .Where(gm => gm.GroupId == view.GroupId && gm.UserId == view.UserId)
                .FirstOrDefault();

            if(member != null)
            {
                ViewBag.Error = "The member already belongs to group";
                
                ViewBag.UserID = new SelectList(
                db.Users.OrderBy(u => u.LastName).ThenBy(u => u.FirstName),
                "UserId",
                "FullName");

                return View(view);
            }

            // Construct the object
            member = new GroupMember
            {
                GroupId = view.GroupId,
                UserId = view.UserId
            };

            // Add record
            db.GroupMembers.Add(member);
            db.SaveChanges();

            return RedirectToAction(string.Format("Details/{0}", view.GroupId));
        }

        /// <summary>
        /// GET: Delete a member
        /// </summary>
        /// <param name="groupMemberId"></param>
        /// <returns></returns>
        public ActionResult DeleteMember(int groupMemberId)
        {
            var member = db.GroupMembers.Find(groupMemberId);

            if(member != null)
            {
                db.GroupMembers.Remove(member);
                db.SaveChanges();
            }

            return RedirectToAction(string.Format("Details/{0}", member.GroupId));
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
