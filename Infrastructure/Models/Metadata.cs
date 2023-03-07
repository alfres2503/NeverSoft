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
        [Display(Name = "Residence ID")]
        public int IDResidence { get; set; }
        [Display(Name = "User ID")]
        public long IDUser { get; set; }
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
        [Display(Name = "Owner ID")]
        public long IDUser { get; set; }
        [Display(Name = "Role ID")]
        public int IDRole { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public bool Active { get; set; }
        
    }

    internal class PaymentPlanMetadata
    {
        [Display(Name = "Plan ID")]
        public int IDPlan { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        [RegularExpression("^[0-9]+(\\.[0-9]{1,2})?$", ErrorMessage = "Only accepts numbers, with two decimal places")]
        public decimal Total { get; set; }
    }

    internal partial class PaymentItemMetadata
    {
        [Display(Name = "Item ID")]
        [Required(ErrorMessage = "{0} is a required data")]
        public int IDItem { get; set; }

        [Required(ErrorMessage = "{0} is a required data")]
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        [RegularExpression(@"^[0-9]+(\.,[0-9]{1,2})?$", ErrorMessage = "Only accepts numbers, with two decimal places")]
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
}
