using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using Democracy.Context;
using Democracy.Models;
using Democracy.ModelsView;

namespace Democracy.Controllers
{
    public class VotingsController : Controller
    {
        private DemocracyContext db = new DemocracyContext();

        /// <summary>
        /// GET: Votings
        /// </summary>
        /// <returns>View(votings.ToList())</returns>
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var votings = db.Votings.Include(v => v.State);

            List<VotingIndexView> views = new List<VotingIndexView>();
            var db2 = new DemocracyContext();

            foreach (var voting in votings)
            {
                User user = null;
                if(voting.CandidateWinId != 0)
                {
                    user = db2.Users.Find(voting.CandidateWinId);
                }

                views.Add(new VotingIndexView
                {
                    CandidateWinId = voting.CandidateWinId,
                    DateTimeEnd = voting.DateTimeEnd,
                    DateTimeStart = voting.DateTimeStart,
                    Description = voting.Description,
                    IsEnabledBlankVote = voting.IsEnabledBlankVote,
                    IsForAllUsers = voting.IsForAllUsers,
                    QuantityBlankVotes = voting.QuantityBlankVotes,
                    Remarks = voting.Remarks,
                    StateId = voting.StateId,
                    State = voting.State,
                    VotingId = voting.VotingId,
                    Winner = user
                });
            }

            return View(views);
        }

        /// <summary>
        /// GET: Votings/Details/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View(voting)</returns>
        [Authorize(Roles = "Admin")]
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

            // Create DetailsVotingView
            DetailsVotingView detailsVotingView = new DetailsVotingView
            {
                Candidates = voting.Candidates.ToList(),
                CandidateWinId = voting.CandidateWinId,
                DateTimeEnd = voting.DateTimeEnd,
                DateTimeStart = voting.DateTimeStart,
                Description = voting.Description,
                IsEnabledBlankVote = voting.IsEnabledBlankVote,
                IsForAllUsers = voting.IsForAllUsers,
                QuantityBlankVotes = voting.QuantityBlankVotes,
                QuantityVotes = voting.QuantityVotes,
                Remarks = voting.Remarks,
                StateId = voting.StateId,
                VotingGroups = voting.VotingGroups.ToList(),
                VotingId = voting.VotingId
            };

            return View(detailsVotingView);
        }

        /// <summary>
        /// GET: Votings/Create
        /// </summary>
        /// <returns>View(votingView)</returns>
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
                    ModelState.AddModelError(string.Empty, "Cant not delete the record because it has related records");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

                return View(voting);
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Add a group to a voting
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View(addGroupView)</returns>
        [Authorize(Roles = "Admin")]
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

            if (votingGroup != null)
            {
                ModelState.AddModelError(string.Empty, "The group already belongs to voting");

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

        /// <summary>
        /// Add a candidate to a voting
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View(id)</returns>
        [Authorize(Roles = "Admin")]
        public ActionResult AddCandidate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AddCandidateView addCandidateView = new AddCandidateView
            {
                VotingId = Convert.ToInt32(id)
            };

            ViewBag.UserID = new SelectList(
                db.Users.OrderBy(u => u.LastName).ThenBy(u => u.FirstName),
                "UserId",
                "FullName");

            return View(addCandidateView);
        }

        /// <summary>
        /// Add a candidate to a voting
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View(id)</returns>
        [HttpPost]
        public ActionResult AddCandidate(AddCandidateView addCandidateView)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.UserID = new SelectList(
                db.Users.OrderBy(u => u.LastName).ThenBy(u => u.FirstName),
                "UserId",
                "FullName");

                return View(addCandidateView);
            }

            // Validate if group already belongs to voting
            var candidate = db.Candidates
                .Where(c => c.VotingId == addCandidateView.VotingId
                && c.UserId == addCandidateView.UserId)
                .FirstOrDefault();

            if (candidate != null)
            {
                ModelState.AddModelError(string.Empty, "The candidate already belongs to voting");

                ViewBag.UserID = new SelectList(
                db.Users.OrderBy(u => u.LastName).ThenBy(u => u.FirstName),
                "UserId",
                "FullName");

                return View(addCandidateView);
            }

            // Create the VotingGroup object
            candidate = new Candidate
            {
                UserId = addCandidateView.UserId,
                VotingId = addCandidateView.VotingId
            };

            // Save record
            db.Candidates.Add(candidate);
            db.SaveChanges();

            return RedirectToAction(string.Format("Details/{0}", addCandidateView.VotingId));
        }

        /// <summary>
        /// GET: Delete a Group of voting
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var votingGroup = db.VotingGroup.Find(id);

            if (votingGroup != null)
            {
                db.VotingGroup.Remove(votingGroup);
                db.SaveChanges();
            }

            return RedirectToAction(string.Format("Details/{0}", votingGroup.VotingId));
        }

        /// <summary>
        /// GET: Delete a Candidate of voting
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View(id)</returns>
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteCandidate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var candidate = db.Candidates.Find(id);

            if (candidate != null)
            {
                db.Candidates.Remove(candidate);
                db.SaveChanges();
            }

            return RedirectToAction(string.Format("Details/{0}", candidate.VotingId));
        }

        /// <summary>
        /// MyVotings optrion for users
        /// </summary>
        /// <returns>View(votings)</returns>
        [Authorize(Roles = "User")]
        public ActionResult MyVotings()
        {
            var user = db.Users.Where(u => u.UserName == this.User.Identity.Name).FirstOrDefault();

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "There and Error with the current User , call the support");

                return View();
            }

            // Verify Open state exists
            var state = this.getState("Open");

            // Get the event votings for the correct time
            var votings = db.Votings
                .Where(v => v.StateId == state.StateId
                    && v.DateTimeStart <= DateTime.Now
                    && v.DateTimeEnd >= DateTime.Now)
                .Include(v => v.State)
                .Include(v => v.Candidates)
                .Include(v => v.VotingGroups)
                .ToList();

            // Discart voting events in the wich the user already voted
            foreach (var voting in votings.ToList())
            {
                var votingDetail = db.VotingDetails
                    .Where(vd => vd.UserId == user.UserId && vd.VotingId == voting.VotingId)
                    .FirstOrDefault();

                if (votingDetail != null)
                {
                    votings.Remove(voting);
                }
            }

            // Discart voting events by groups in wich the user is not included
            foreach (var voting in votings.ToList())
            {
                if (!voting.IsForAllUsers)
                {
                    bool userBelongsToGroup = false;

                    foreach (var votingGruop in voting.VotingGroups)
                    {
                        var userGroup = votingGruop.Group.GroupMembers
                            .Where(gr => gr.UserId == user.UserId)
                            .FirstOrDefault();

                        if (userGroup != null)
                        {
                            userBelongsToGroup = true;

                            break;
                        }
                    }

                    if (!userBelongsToGroup)
                    {
                        votings.Remove(voting);
                    }
                }
            }

            return View(votings);
        }

        /// <summary>
        /// Vote for an user get
        /// </summary>
        /// <param name="id">voteId</param>
        /// <returns>View(id)</returns>
        [Authorize(Roles = "User")]
        public ActionResult Vote(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var voting = db.Votings.Find(id);

            // Create the object to view
            VotingVoteView votingVoteView = new VotingVoteView
            {
                MyCandidates = voting.Candidates.ToList(),
                DateTimeEnd = voting.DateTimeEnd,
                DateTimeStart = voting.DateTimeStart,
                Description = voting.Description,
                IsEnabledBlankVote = voting.IsEnabledBlankVote,
                IsForAllUsers = voting.IsForAllUsers,
                Remarks = voting.Remarks,
                VotingId = voting.VotingId
            };

            return View(votingVoteView);
        }

        /// <summary>
        /// Vote for an user post
        /// </summary>
        /// <param name="candidateId"></param>
        /// <param name="votingId"></param>
        /// <returns></returns>
        [Authorize(Roles = "User")]
        public ActionResult VoteCandidate(int? candidateId, int? votingId)
        {
            if (candidateId == null || votingId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = db.Users.Where(u => u.UserName == this.User.Identity.Name).FirstOrDefault();

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "There is a problem with the User. Call the support");
                return RedirectToAction("Index", "Home");
            }

            var candidate = db.Candidates.Find(candidateId);
            if (candidate == null)
            {
                ModelState.AddModelError(string.Empty, "There is a problem with the Candidate. Call the support");
                return RedirectToAction("Index", "Home");
            }

            var voting = db.Votings.Find(votingId);
            if (voting == null)
            {
                ModelState.AddModelError(string.Empty, "There is a problem with the Voting event. Call the support");
                return RedirectToAction("Index", "Home");
            }

            if (this.VoteCandidate(user, candidate, voting))
            {
                return RedirectToAction("MyVotings");
            }

            ModelState.AddModelError(string.Empty, "There is a problem with the Voting redord. Call the support");
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Results of votings
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [Authorize(Roles = "User")]
        public ActionResult Results()
        {
            var state = this.getState("Closed");
            var votings = db.Votings
                .Where(v => v.StateId == state.StateId)
                .Include(v => v.State);

            List<VotingIndexView> views = new List<VotingIndexView>();
            var db2 = new DemocracyContext();

            foreach (var voting in votings)
            {
                User user = null;
                if (voting.CandidateWinId != 0)
                {
                    user = db2.Users.Find(voting.CandidateWinId);
                }

                views.Add(new VotingIndexView
                {
                    CandidateWinId = voting.CandidateWinId,
                    DateTimeEnd = voting.DateTimeEnd,
                    DateTimeStart = voting.DateTimeStart,
                    Description = voting.Description,
                    IsEnabledBlankVote = voting.IsEnabledBlankVote,
                    IsForAllUsers = voting.IsForAllUsers,
                    QuantityBlankVotes = voting.QuantityBlankVotes,
                    Remarks = voting.Remarks,
                    StateId = voting.StateId,
                    State = voting.State,
                    VotingId = voting.VotingId,
                    Winner = user
                });
            }

            return View(views);
        }

        /// <summary>
        /// Transaction with the voting record
        /// </summary>
        /// <param name="user"></param>
        /// <param name="candidate"></param>
        /// <param name="voting"></param>
        /// <returns></returns>
        private bool VoteCandidate(User user, Candidate candidate, Voting voting)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                VotingDetail votingDetail = new VotingDetail
                {
                    CandidateId = candidate.CandidateId,
                    DateTime = DateTime.Now,
                    UserId = user.UserId,
                    VotingId = voting.VotingId
                };

                db.VotingDetails.Add(votingDetail);

                candidate.QuantityVotes++;
                db.Entry(candidate).State = EntityState.Modified;

                voting.QuantityVotes++;
                db.Entry(voting).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    return false;
                }
            }
        }


        /// <summary>
        /// ShowResults
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ShowResults(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var report = this.GenerateResultReport(id);
            var stream = report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            return File(stream, "application/pdf");
        }

        /// <summary>
        /// Generate a Results Report by VotingId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private ReportClass GenerateResultReport(int? id)
        {
            // Variables and imports for ADO connection to DataBase
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var connection = new SqlConnection(connectionString);
            var dataTable = new DataTable();
            var sql = @"SELECT Votings.VotingId, Votings.Description AS Voting, States.Description AS State, Users.FirstName + ' ' + Users.LastName AS Candidate, Candidates.QuantityVotes
                        FROM Candidates 
                        INNER JOIN Users ON Candidates.UserId = Users.UserId 
                        INNER JOIN Votings ON Candidates.VotingId = Votings.VotingId 
                        INNER JOIN States ON Votings.StateId = States.StateId
                        WHERE Votings.VotingId = " + id;

            try
            {
                connection.Open();
                var commmand = new SqlCommand(sql, connection);
                var adapter = new SqlDataAdapter(commmand);

                adapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                ex.ToString();
                throw;
            }

            // Create the report object -> it will be the real report
            var report = new ReportClass();
            report.FileName = Server.MapPath("/Reports/ResultsReport.rpt");

            // Load the report in memory
            report.Load();

            // Load data origin
            report.SetDataSource(dataTable);

            return report;
        }

        /// <summary>
        /// Close voting
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult Close(int? id)
        {
            if (id == null)
            {
                ModelState.AddModelError(string.Empty, "There is an error with the record, call the support team");
            }

            var voting = db.Votings.Find(id);

            if (voting == null)
            {
                ModelState.AddModelError(string.Empty, "There is an error with the record, call the support team");
                return RedirectToAction("Index");
            }

            var candidate = db.Candidates
                .Where(c => c.VotingId == voting.VotingId)
                .OrderByDescending(c => c.QuantityVotes)
                .FirstOrDefault();

            if (candidate == null)
            {
                ModelState.AddModelError(string.Empty, "There is an error with winner candidate, call the support team");
                return RedirectToAction("Index");
            }

            var state = this.getState("Closed");

            voting.StateId = state.StateId;
            voting.CandidateWinId = candidate.User.UserId;

            db.Entry(voting).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Verify if a State already exists, if not create id
        /// </summary>
        /// <param name="stateName"></param>
        /// <returns></returns>
        private State getState(string stateName)
        {
            var state = db.States.Where(s => s.Description == stateName).FirstOrDefault();

            if (state == null)
            {
                db.States.Add(new State
                {
                    Description = stateName
                });

                db.SaveChanges();
            }

            return state;
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
