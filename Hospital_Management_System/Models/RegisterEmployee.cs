using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Hospital_Library.Enum;

namespace Hospital_Management_System.Models
{
    public class RegisterEmployee
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name: ")]
        public string Name { get; set; }

        [Required]
        public string Designation { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        [Display(Name = "Email Address: ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "You must provide a phone number,Phone Number at least 10 digit")]
        [Display(Name = "ContactNo")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string ContactNo { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(150, MinimumLength = 6)]
        [Display(Name = "Password: ")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Is Admin: ")]
        public bool Isadmin { get; set; }
        [Required]
        public int CreatedBy { get; set; }
        [Required]
        public string CreatedDate { get; set; }
        public string CreatedName { get; set; }
        public string ModifiedName { get; set; }
        [Required]
        public int ModifiedBy { get; set; }
        [Required]
        public string ModifiedDate { get; set; }
        public Operations ActionType { get; set; }
    }
}