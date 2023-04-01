using System.ComponentModel.DataAnnotations;

namespace ConcertDB.DAL.Entities
{
    public class Ticket : Entity
    {
        [Display(Name = "Ticket use date.")]
        public DateTime UseDate { get; set; }
        [Required(ErrorMessage = "The field {0} is required.")]
        [Display(Name = "Ticket use")]
        public Boolean IsUsed { get; set; }
        [Display(Name = "Entrace Gate")]
        [MaxLength(10, ErrorMessage = "The field {0} must be shorter.")]
        public String EntranceGate { get; set; }
    }
}
