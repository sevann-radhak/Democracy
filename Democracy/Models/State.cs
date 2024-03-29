﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Democracy.Models
{
    public class State
    {
        /// <summary>
        /// Gets or sets the state id
        /// </summary>
        [Key]
        public int StateId { get; set; }

        /// <summary>
        /// Gets or sets the state description
        /// </summary>
        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(60, ErrorMessage = "The field {0} can contain maximun {1} and minimun {2} characteres", MinimumLength = 3)]
        [Display(Name = "State description")]
        public string Description { get; set; }

        /// <summary>
        /// Foreing key to Voting model
        /// </summary>
        public virtual ICollection<Voting> Votings { get; set; }
    }
}