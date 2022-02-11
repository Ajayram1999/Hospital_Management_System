using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Hospital_Library.Enum;
namespace Hospital_Management_System.Models
{
    public class Patient
    {
        [Display(Name = "PatientId")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is Reqired")]
        [RegularExpression("[A-Za-z]{1,30}", ErrorMessage = "Give a proper name")]
        [Display(Name = "PatientName")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Age is Reqired")]
        [Display(Name = "PatientAge")]
        [Range(0, 100, ErrorMessage = "Age must be in between 0 to 100")]
        public int? Age { get; set; }

        [Required(ErrorMessage = "Gender is Reqired")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Department is Reqired")]
        [Display(Name = "Department")]

        public string Department { get; set; }

        [Required(ErrorMessage = "CovidTestResult is Reqired")]
        [Display(Name = "CovidTestResult")]
        public string CovidTestResult { get; set; }

        [Required(ErrorMessage = "Contact No is Reqired")]
        [Display(Name = "ContactNo")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                   ErrorMessage = "Entered phone format is not valid.")]
        public string Contact { get; set; }
      

        public bool InPatient { get; set; }
        public int Deleted { get; set; }
        public Operations ActionType { get; set; }
    }
}
  