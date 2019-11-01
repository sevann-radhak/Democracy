using Democracy.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Democracy.ModelsView
{
    [NotMapped]
    public class VotingVoteView : Voting
    {
        public List<Candidate> MyCandidates { get; set; }
    }
}