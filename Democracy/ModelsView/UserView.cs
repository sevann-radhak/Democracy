using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Democracy.ModelsView
{
    public class UserView
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(60, ErrorMessage = "The field {0} can contain maximun {1} and minimun {2} characteres", MinimumLength = 7)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
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

        public HttpPostedFileBase Photo { get; set; }
    }
}