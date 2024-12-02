using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Managment.bll.DTOs;
using Task_Managment.bll.Interfaces;
using Task_Managment.DAL.Interfaces;
namespace Task_Managment.bll.Services
{

    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TaskService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TaskDTO> GetTaskByIdAsync(string id)
        {
            try
            {
                var task = await _unitOfWork.Tasks.GetByIdAsync(id);
                if (task == null)
                    throw new Exception("Task not found");
                return _mapper.Map<TaskDTO>(task);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the task: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<TaskDTO>> GetAllTasksAsync()
        {
            try
            {
                var tasks = await _unitOfWork.Tasks.GetAllAsync();
                return _mapper.Map<IEnumerable<TaskDTO>>(tasks);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving tasks: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<TaskDTO>> SearchTasksAsync(string keyword, bool? isCompleted = null)
        {
            try
            {
                var tasks = await _unitOfWork.Tasks.FindAllAsync(t =>
                    t.Title.Contains(keyword) && (isCompleted == null || t.IsCompleted == isCompleted));
                return _mapper.Map<IEnumerable<TaskDTO>>(tasks);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while searching tasks: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<TaskDTO>> GetTasksByCategoryIdAsync(string categoryId)
        {
            try
            {
                var tasks = await _unitOfWork.Tasks.FindAllAsync(t => t.CategoryId == categoryId);
                return _mapper.Map<IEnumerable<TaskDTO>>(tasks);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving tasks for the category: {ex.Message}", ex);
            }
        }

        public async Task<TaskDTO> AddTaskAsync(TaskDTO taskDto)
        {
            try
            {
                var task = _mapper.Map<TaskModel>(taskDto);
                await _unitOfWork.Tasks.AddAsync(task);
                _unitOfWork.Complete();
                return _mapper.Map<TaskDTO>(task);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while adding the task: {ex.Message}", ex);
            }
        }

        public async Task<EditTaskDTO> UpdateTaskAsync(string id, EditTaskDTO taskDto)
        {
            try
            {
                var task = await _unitOfWork.Tasks.GetByIdAsync(id);
                if (task == null)
                    throw new Exception("Task not found");

                task.Title = taskDto.Title ?? task.Title;
                task.Description = taskDto.Description ?? task.Description;
                task.CreatedAt = taskDto.CreatedAt ?? task.CreatedAt;
                task.Deadline = taskDto.Deadline ?? task.Deadline;
                task.CategoryId = taskDto.CategoryId ?? task.CategoryId;
                task.UserId = taskDto.UserId ?? task.UserId;
                _unitOfWork.Tasks.Update(task);
                _unitOfWork.Complete();
                return _mapper.Map<EditTaskDTO>(task);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the task: {ex.Message}", ex);
            }
        }

        public async Task DeleteTaskAsync(string id)
        {
            try
            {
                var task = await _unitOfWork.Tasks.GetByIdAsync(id);
                if (task == null)
                    throw new Exception("Task not found");

                _unitOfWork.Tasks.Delete(task);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the task: {ex.Message}", ex);
            }
        }

        public async Task<int> CountAsync()
        {
            return await _unitOfWork.Tasks.CountAsync();
        }
    }
}
