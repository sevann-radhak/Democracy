using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Democracy.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Index("UserNameIndex", IsUnique = true)]
        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(60, ErrorMessage = "The field {0} can contain maximun {1} and minimun {2} characteres", MinimumLength = 7)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-Mail")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(60, ErrorMessage = "The field {0} can contain maximun {1} and minimun {2} characteres", MinimumLength = 3)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(60, ErrorMessage = "The field {0} can contain maximun {1} and minimun {2} characteres", MinimumLength = 3)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(20, ErrorMessage = "The field {0} can contain maximun {1} and minimun {2} characteres", MinimumLength = 7)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(100, ErrorMessage = "The field {0} can contain maximun {1} and minimun {2} characteres", MinimumLength = 3)]
        public string Address { get; set; }

        public string Grade { get; set; }

        public string Group { get; set; }

        [StringLength(200, ErrorMessage = "The field {0} can contain maximun {1} and minimun {2} characteres", MinimumLength = 3)]
        [DataType(DataType.ImageUrl)]
        public string Photo { get; set; }

        /// <summary>
        /// Returns FirstName + FullName
        /// </summary>
        [Display(Name = "User")]
        public string FullName { get { return string.Format("{0} {1}", this.FirstName, this.LastName); } }

        /// <summary>
        /// Foreign key Foreing key to GroupMember
        /// </summary>
        public virtual ICollection<GroupMember> GroupMembers { get; set; }

        /// <summary>
        /// Foreign key Foreing key to Candidate
        /// </summary>
        public virtual ICollection<Candidate> Candidates { get; set; }
    }
}