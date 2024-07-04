using System.ComponentModel.DataAnnotations;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
       
        public string Title { get; set; }
        
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string DueDate { get; set; }
        public Priority Priority { get; set; }
        public TaskStatusDb TaskStatus { get; set; }
    }
}
