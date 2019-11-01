using Democracy.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Democracy.ModelsView
{ 
    [NotMapped]
    public class UserIndexView : User
    {        
        [Display(Name = "Is Admin?")]
        public Boolean IsAdmin { get; set; }
    }
}