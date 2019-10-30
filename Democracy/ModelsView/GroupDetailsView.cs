using Democracy.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Democracy.ModelsView
{
    public class GroupDetailsView
    {
        /// <summary>
        /// Gets or sets the group id
        /// </summary>
        [Key]
        public int GroupId { get; set; }

        /// <summary>
        /// Gets or sets the group description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Group's members list
        /// </summary>

        public List<GroupMember> Members { get; set; }
    }
}