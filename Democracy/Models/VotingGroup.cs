﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Democracy.Models
{
    public class VotingGroup
    {
        [Key]
        public int VotingGroupId { get; set; }

        public int VotingId { get; set; }

        public int GroupId { get; set; }
        
        /// <summary>
        /// Foreign key Voting
        /// </summary>
        public virtual Voting Voting { get; set; }

        /// <summary>
        /// Foreign key Group
        /// </summary>
        public virtual Group Group { get; set; }
    }
}