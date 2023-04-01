using System.ComponentModel.DataAnnotations;

namespace ConcertDB.DAL.Entities
{
    public class Ticket
    {        //Para mostrar cierta especificación le agrego el display, con los nombres para cada variable
        [Key]
        [Required]
        [Display(Name = "Ticket use date.")]
        public Guid Id { get; set; }

        [Display(Name = "Date")]
        public DateTime? UseDate { get; set; }

        [Required(ErrorMessage = "The field {0} is required.")]
        [Display(Name = "Ticket use")]
        public bool IsUsed { get; set; }

        [Display(Name = "Entrace Gate")]
        [MaxLength(10, ErrorMessage = "The field {0} must be shorter.")]
        public String EntranceGate { get; set; }

    }
}
