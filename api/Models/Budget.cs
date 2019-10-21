using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class Budget
    {
        public int Id { get; set; }

        [Required]
        public int CalendarMonth { get; set; }

        [Required]
        public int CalendarYear { get; set; }

        [Required]
        public Department Department { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal AmountBudgeted { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal AmountSpent { get; set; }
    }
}
