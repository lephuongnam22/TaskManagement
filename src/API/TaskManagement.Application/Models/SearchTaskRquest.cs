using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Models
{
    public class SearchTaskRquest
    {
        public TaskStatusDb? TaskStatus { get; set; }
        public Priority? Priority { get; set; }
    }
}
