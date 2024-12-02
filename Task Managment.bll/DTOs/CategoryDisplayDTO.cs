using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Managment.bll.DTOs
{
    public class CategoryDisplayDTO
    {
        public string Name { get; set; }
        public List<TaskModel> Tasks { get; set; }
    }
}
