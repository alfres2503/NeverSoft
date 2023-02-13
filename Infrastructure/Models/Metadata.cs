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
        public long IDUser { get; set; }
        public int Habitants { get; set; }
        public int StartYear { get; set; }
        public bool InConstruction { get; set; }
        public virtual ICollection<PlanAssignment> PlanAssignment { get; set; }
        public virtual User User { get; set; }
    }
}
