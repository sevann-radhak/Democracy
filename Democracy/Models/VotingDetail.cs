using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Democracy.Models
{
    public class VotingDetail
    {
        [Key]
        public int VotingDetailId { get; set; }

        public DateTime DateTime { get; set; }

        public int VotingId { get; set; }

        public int UserId { get; set; }

        public int CandidateId { get; set; }
        
        /// <summary>
        /// Foreign key Voting
        /// </summary>
        public virtual Voting Voting { get; set; }

        /// <summary>
        /// Foreign key User
        /// </summary>

        public virtual User User { get; set; }

        /// <summary>
        /// Foreign key Candidate
        /// </summary>
        public virtual Candidate Candidate { get; set; }
    }
}