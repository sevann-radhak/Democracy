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
    [Authorize]
    public class VotingsController : Controller
    {
        private DemocracyContext db = new DemocracyContext();

        /// <summary>
        /// GET: Votings
        /// </summary>
        /// <returns>View(votings.ToList())</returns>
        public ActionResult Index()
        {
            var votings = db.Votings.Include(v => v.State);

            return View(votings.ToList());
        }

        /// <summary>
        /// GET: Votings/Details/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View(voting)</returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Voting voting = db.Votings.Find(id);

            if (voting == null)
            {
                return HttpNotFound();
            }

            return View(voting);
        }

        /// <summary>
        /// GET: Votings/Create
        /// </summary>
        /// <returns>View(votingView)</returns>
        public ActionResult Create()
        {
            ViewBag.StateId = new SelectList(db.States, "StateId", "Description");

            VotingView votingView = new VotingView
            {
                DateStart = DateTime.Now,
                DateEnd = DateTime.Now,
                TimeStart = DateTime.Now
            };

            return View(votingView);
        }

        /// <summary>
        /// POST: Create a new Voting record
        /// </summary>
        /// <param name="votingView"></param>
        /// <returns>RedirectToAction("Index")</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VotingView votingView)
        {
            if (!ModelState.IsValid)
            {

                ViewBag.StateId = new SelectList(db.States, "StateId", "Description", votingView.StateId);

                return View(votingView);
            }

            // Create the Voting object
            Voting voting = new Voting
            {
                DateTimeEnd = votingView.DateEnd
                    .AddHours(votingView.TimeEnd.Hour)
                    .AddMinutes(votingView.TimeEnd.Minute),
                DateTimeStart = votingView.DateStart
                    .AddHours(votingView.TimeStart.Hour)
                    .AddMinutes(votingView.TimeStart.Minute),
                Description = votingView.Description,
                IsEnabledBlankVote = votingView.IsEnabledBlankVote,
                IsForAllUsers = votingView.IsForAllUsers,
                Remarks = votingView.Remarks,
                StateId = votingView.StateId
            };

            // Add record
            db.Votings.Add(voting);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// GET: Votings/Edit/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View(votingView)</returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Voting voting = db.Votings.Find(id);

            if (voting == null)
            {
                return HttpNotFound();
            }

            // Create the VotingView object
            VotingView votingView = new VotingView
            {
                DateEnd = voting.DateTimeEnd,
                DateStart = voting.DateTimeStart,
                Description = voting.Description,
                IsEnabledBlankVote = voting.IsEnabledBlankVote,
                IsForAllUsers = voting.IsForAllUsers,
                Remarks = voting.Remarks,
                StateId = voting.StateId,
                TimeEnd = voting.DateTimeEnd,
                TimeStart = voting.DateTimeStart,
                VotingId = voting.VotingId
            };

            ViewBag.StateId = new SelectList(db.States, "StateId", "Description", voting.StateId);

            return View(votingView);
        }

        /// <summary>
        /// POST: Votings/Edit/{id}
        /// </summary>
        /// <param name="votingView"></param>
        /// <returns>RedirectToAction("Index")</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VotingView votingView)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.StateId = new SelectList(db.States, "StateId", "Description", votingView.StateId);

                return View(votingView);
            }

            // Create the Voting object
            Voting voting = new Voting
            {
                DateTimeEnd = votingView.DateEnd
                    .AddHours(votingView.TimeEnd.Hour)
                    .AddMinutes(votingView.TimeEnd.Minute),
                DateTimeStart = votingView.DateStart
                    .AddHours(votingView.TimeStart.Hour)
                    .AddMinutes(votingView.TimeStart.Minute),
                Description = votingView.Description,
                IsEnabledBlankVote = votingView.IsEnabledBlankVote,
                IsForAllUsers = votingView.IsForAllUsers,
                Remarks = votingView.Remarks,
                StateId = votingView.StateId,
                VotingId = votingView.VotingId
            };

            db.Entry(voting).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// GET: Votings/Delete/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View(voting)</returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Voting voting = db.Votings.Find(id);

            if (voting == null)
            {
                return HttpNotFound();
            }

            return View(voting);
        }

        /// <summary>
        /// POST: Votings/Delete/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns>RedirectToAction("Index")</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Voting voting = db.Votings.Find(id);
            db.Votings.Remove(voting);

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

                return View(voting);
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Add a group to a voting
        /// </summary>
        /// <param name="votingId"></param>
        /// <returns>View(addGroupView)</returns>
        public ActionResult AddGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.GroupId = new SelectList(db.Groups.OrderBy(g => g.Description), "GroupId", "Description");

            AddGroupView addGroupView = new AddGroupView
            {
                VotingId = Convert.ToInt32(id)
            };

            return View(addGroupView);
        }

        /// <summary>
        /// Add a group to a voting
        /// </summary>
        /// <param name="voutingId"></param>
        /// <returns>View(addGroupView)</returns>
        [HttpPost]
        public ActionResult AddGroup(AddGroupView addGroupView)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.GroupId = new SelectList(db.Groups.OrderBy(g => g.Description), "GroupId", "Description");

                return View(addGroupView);
            }

            // Validate if group already belongs to voting
            var votingGroup = db.VotingGroup
                .Where(vg => vg.VotingId == addGroupView.VotingId
                && vg.GroupId == addGroupView.GroupId)
                .FirstOrDefault();

            if(votingGroup != null)
            {
                ViewBag.Error = "The group already belongs to voting";
                ViewBag.GroupId = new SelectList(db.Groups.OrderBy(g => g.Description), "GroupId", "Description");

                return View(addGroupView);
            }

            // Create the VotingGroup object
            votingGroup = new VotingGroup
            {
                GroupId = addGroupView.GroupId,
                VotingId = addGroupView.VotingId
            };

            // Save record
            db.VotingGroup.Add(votingGroup);
            db.SaveChanges();

            return RedirectToAction(string.Format("Details/{0}", addGroupView.VotingId));
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
