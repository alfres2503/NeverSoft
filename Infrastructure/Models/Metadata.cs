using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    internal partial class ResidenceMetadata
    {
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Only integers numbers greater than 0 are accepted")]
        [Display(Name = "Residence ID")]
        public int IDResidence { get; set; }
        [Display(Name = "User ID")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Only integers numbers greater than 0 are accepted")]
        public long IDUser { get; set; }
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Only integers numbers greater than 0 are accepted")]
        public int Habitants { get; set; }
        [Display(Name = "Start Year")]
        public int StartYear { get; set; }
        [Display(Name = "In Construction")]
        public bool InConstruction { get; set; }
        //public virtual ICollection<PlanAssignment> PlanAssignment { get; set; }
        //public virtual User User { get; set; }
    }

    internal partial class UserMetadata
    {
        [Required(ErrorMessage = "{0} is a required data")]
        [Display(Name = "User ID")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Only integers numbers greater than 0 are accepted")]
        public long IDUser { get; set; }
        [Required(ErrorMessage = "{0} is a required data")]
        [Display(Name = "Role ID")]
        public int IDRole { get; set; }
        [Required(ErrorMessage = "{0} is a required data")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "{0} is a required data")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "{0} is a required data")]
        [RegularExpression(@"[_A-Za-z0-9-]+(\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\.[A-Za-z0-9-]+)*(\.[A-Za-z]+)$", ErrorMessage = "Invalid email, enter a correct email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "{0} is a required data")]
        public string Password { get; set; }
        [Required(ErrorMessage = "{0} is a required data")]
        public bool Active { get; set; }
        [Display(Name = "Name")]
        public string FullName { get; set; }


    }

    internal class PaymentPlanMetadata
    {
        //[RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Only integers numbers greater than 0 are accepted")]
        [Display(Name = "Plan ID")]
        //[Required(ErrorMessage = "{0} is a required data")]
        public int IDPlan { get; set; }
        [Required(ErrorMessage = "{0} is a required data")]
        [StringLength(50, ErrorMessage = "Names with more than 50 letters are not accepted")]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0} is a required data")]
        [StringLength(280, ErrorMessage = "Names with more than 280 letters are not accepted")]
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        [Required(ErrorMessage = "{0} is a required data")]
        [RegularExpression("^[0-9]+(\\.[0-9]{1,2})?$", ErrorMessage = "Only accepts numbers, with two decimal places")]
        public decimal Total { get; set; }
    }

    internal partial class PaymentItemMetadata
    {
        //[RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Only integers numbers greater than 0 are accepted")]
        [Display(Name = "Item ID")]
        //[Required(ErrorMessage = "{0} is a required data")]
        public int IDItem { get; set; }

        [Required(ErrorMessage = "{0} is a required data")]
        [StringLength(50, ErrorMessage = "Descriptions with more than 50 letters are not accepted")]
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        [RegularExpression("^[0-9]+(\\.[0-9]{1,2})?$", ErrorMessage = "Only accepts numbers, with two decimal places")]
        [Required(ErrorMessage = "{0} is a required data")]
        public decimal Price { get; set; }
    }

    internal partial class PlanAssignmentMetadata
    {

        public int IDAssignment { get; set; }
        public int IDPlan { get; set; }
        public int IDResidence { get; set; }

        [Display(Name = "Payment Date")]
        public System.DateTime AssignmentDate { get; set; }
        [Display(Name = "Payed Status")]
        public bool PayedStatus { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        [RegularExpression(@"^[0-9]+(\.,[0-9]{1,2})?$", ErrorMessage = "solo acepta números, con dos decimales")]
        public decimal Amount { get; set; }
    }

    internal partial class IncidenceMetadata
    {
        [Display(Name = "Incidence ID")]
        public int IDIncidence { get; set; }
        [Required(ErrorMessage = "{0} is a required data")]
        public long IDUser { get; set; }
        [Required(ErrorMessage = "{0} is a required data")]
        public string Title { get; set; }
        [Required(ErrorMessage = "{0} is a required data")]
        public string Description { get; set; }
        [Display(Name = "Attended")]
        public bool Finished { get; set; }

        public virtual User User { get; set; }
    }

    internal partial class NewsMetadata
    {
        //[RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Only integers numbers greater than 0 are accepted")]
        //[Required(ErrorMessage = "{0} is a required data")]
        [Display(Name = "News ID")]
        public int IDNews { get; set; }
        [Required(ErrorMessage = "{0} is a required data")]
        [Display(Name = "ID Category")]
        public int IDCategory { get; set; }
        [Required(ErrorMessage = "{0} is a required data")]
        [StringLength(500, ErrorMessage = "Descriptions with more than 500 letters are not accepted")]
        public string Description { get; set; }
        
        public byte[] Archive { get; set; }
    }

    internal partial class NewsCategoryMetadata {
        [Display(Name = "ID Category")]
        public int IDCategory { get; set; }
        [StringLength(50, ErrorMessage = "Descriptions with more than 50 letters are not accepted")]
        public string Description { get; set; }
    }
}
