using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Democracy.Models
{
    public class Group
    {
        /// <summary>
        /// Gets or sets the group id
        /// </summary>
        [Key]
        public int GroupId { get; set; }

        /// <summary>
        /// Gets or sets the group description
        /// </summary>
        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(60, ErrorMessage = "The field {0} can contain maximun {1} and minimun {2} characteres", MinimumLength = 3)]
        public string Description { get; set; }

        /// <summary>
        /// Foreing key to GroupMember
        /// </summary>
        public virtual ICollection<GroupMember> GroupMembers { get; set; }
    }
}