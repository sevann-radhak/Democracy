using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Democracy.ModelsView
{
    public class AddCandidateView
    {
        public int VotingId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        public int UserId { get; set; }
    }
}