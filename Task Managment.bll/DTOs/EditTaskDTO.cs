using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Managment.bll.DTOs
{
    public class EditTaskDTO
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? Deadline { get; set; }

        public bool? IsCompleted { get; set; }

        public string? UserId { get; set; }

        public string? CategoryId { get; set; }
    }
}
