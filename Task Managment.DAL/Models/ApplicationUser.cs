using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Managment.DAL.Enums;

namespace Task_Managment.DAL.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public string Country { get; set; }
        public UserTypes? UserType { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public ICollection<Task> Tasks { get; set; }
    }
}
