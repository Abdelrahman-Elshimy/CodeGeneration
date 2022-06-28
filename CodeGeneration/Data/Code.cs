using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CodeGeneration.Data
{
    public class Code
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [ForeignKey(nameof(Service))]
        public long ServiceId { get; set; }
        public virtual Service Service { get; set; }

        public DateTime Date_Created { get; set; }

        [Required]
        public string Codes { get; set; }

        public int Status { get; set; }

        [ForeignKey(nameof(Approver))]
        public int ApproverId { get; set; }
        public virtual Approver Approver { get; set; }


    }
}
