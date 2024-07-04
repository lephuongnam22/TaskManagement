using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Domain.Entities
{
    public class TaskDb
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(255)]
        public string Title { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        [MaxLength(100)]
        public TaskStatusDb TaskStatus { get; set; }
    }
}
