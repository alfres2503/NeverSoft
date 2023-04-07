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
        [StringLength(20, ErrorMessage = "Names with more than 20 letters are not accepted")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ]+$", ErrorMessage = "Only letters are accepted")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "{0} is a required data")]
        [StringLength(20, ErrorMessage = "Last Names with more than 20 letters are not accepted")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ]+$", ErrorMessage = "Only letters are accepted")]
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
        [UIHint("Amount")]
        public string NameAndPrice { get; set; }
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
        [Display(Name = "Assignment ID")]
        public int IDAssignment { get; set; }
        [Display(Name = "Plan")]
        public int IDPlan { get; set; }
        [Display(Name = "Residence")]
        public int IDResidence { get; set; }

        [Display(Name = "Payment Date")]
        [UIHint("AssignmentDay")]
        public System.DateTime AssignmentDate { get; set; }
        [Display(Name = "Payed Status")]
        public bool PayedStatus { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Required(ErrorMessage = "{0} is a required data, please select a Plan")]
        
        public decimal Amount { get; set; }
    }

    internal partial class IncidenceMetadata
    {
        [Display(Name = "Incidence ID")]
        public int IDIncidence { get; set; }
        [Required(ErrorMessage = "{0} is a required data")]
        public long IDUser { get; set; }
        [Required(ErrorMessage = "{0} is a required data")]
        [StringLength(40, ErrorMessage = "Titles with more than 40 letters are not accepted")]
        public string Title { get; set; }
        [Required(ErrorMessage = "{0} is a required data")]
        [StringLength(150, ErrorMessage = "Description with more than 150 letters are not accepted")]
        public string Description { get; set; }
        [Display(Name = "Attended")]
        [UIHint("Attended")]
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
        [Display(Name = "Category")]
        public int IDCategory { get; set; }
        [Required(ErrorMessage = "{0} is a required data")]
        [StringLength(500, ErrorMessage = "Descriptions with more than 500 letters are not accepted")]
        public string Description { get; set; }
        
        public byte[] Archive { get; set; }
    }

    internal partial class NewsCategoryMetadata {
        [Display(Name = "Category")]
        public int IDCategory { get; set; }
        [StringLength(50, ErrorMessage = "Descriptions with more than 50 letters are not accepted")]
        public string Description { get; set; }
    }

    internal partial class ReservationMetadata
    {
        [Display(Name = "Reservation ID")]
        public int IDReservation { get; set; }
        [Display(Name = "User")]
        public long IDUser { get; set; }
        [Display(Name = "Area")]
        public int IDArea { get; set; }
        [Required(ErrorMessage = "{0} is a required data")]
        public System.DateTime Start { get; set; }
        [Required(ErrorMessage = "{0} is a required data")]
        public System.DateTime Finish { get; set; }
        [Display(Name = "State")]
        public Nullable<bool> Approved { get; set; }

        public virtual Area Area { get; set; }
        public virtual User User { get; set; }
    }

    internal partial class AreaMetadata
    {
        [Display(Name = "Area ID")]
        public int IDArea { get; set; }
        [Display(Name = "Area")]
        public string Name { get; set; }
    }
}
