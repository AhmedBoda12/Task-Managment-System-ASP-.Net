using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Managment.DAL.Models;
using Task = Task_Managment.DAL.Models.Task;

namespace Task_Managment.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAuthRepository Users { get; }
        IBaseRepository<Task> Tasks { get; }
        IBaseRepository<Category> Categories { get; }
        int Complete();
    }
}
