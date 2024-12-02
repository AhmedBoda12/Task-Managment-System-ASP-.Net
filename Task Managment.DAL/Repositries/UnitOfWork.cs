using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Managment.DAL.Data;
using Task_Managment.DAL.Interfaces;
using Task_Managment.DAL.Models;
using Task = Task_Managment.DAL.Models.Task;

namespace Task_Managment.DAL.Repositries
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context, IBaseRepository<Task> tasks, IAuthRepository users, IBaseRepository<Category> categories)
        {
            _context = context;
            Tasks = tasks;
            Users = users;
            Categories = categories;
        }

        public IAuthRepository Users { get; private set; }

        public IBaseRepository<Models.Task> Tasks { get; private set; }

        public IBaseRepository<Category> Categories { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
