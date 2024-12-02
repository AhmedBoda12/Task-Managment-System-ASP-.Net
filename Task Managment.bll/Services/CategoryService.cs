using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Managment.DAL.Interfaces;
using Task_Managment.bll.Interfaces;
using Task_Managment.bll.DTOs;

namespace Task_Managment.bll.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CategoryDisplayDTO> GetCategoryByIdAsync(string id)
        {

            try
            {
                var category = await _unitOfWork.Categories.FindWithIncludesAsync(
                    c => c.Id == id,
                    c => c.Tasks
                );

                if (category == null)
                    throw new Exception("Category not found");

                return _mapper.Map<CategoryDisplayDTO>(category);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the category: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<CategoryDisplayDTO>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _unitOfWork.Categories.FindAllAsync(
                    c => true,
                    includes: new[] { "Tasks" }
                );

                return _mapper.Map<IEnumerable<CategoryDisplayDTO>>(categories);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving categories: {ex.Message}", ex);
            }
        }

        public async Task<CategoryDTO> AddCategoryAsync(CategoryDTO categoryDto)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryDto);
                await _unitOfWork.Categories.AddAsync(category);
                _unitOfWork.Complete();
                return _mapper.Map<CategoryDTO>(category);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while adding the category: {ex.Message}", ex);
            }
        }

        public async Task<CategoryDTO> UpdateCategoryAsync(string id, CategoryDTO categoryDto)
        {
            try
            {
                var category = await _unitOfWork.Categories.GetByIdAsync(id);
                if (category == null)
                    throw new Exception("Category not found");

                _mapper.Map(categoryDto, category);
                _unitOfWork.Categories.Update(category);
                _unitOfWork.Complete();
                return _mapper.Map<CategoryDTO>(category);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the category: {ex.Message}", ex);
            }
        }

        public async Task DeleteCategoryAsync(string id)
        {
            try
            {
                var category = await _unitOfWork.Categories.GetByIdAsync(id);
                if (category == null)
                    throw new Exception("Category not found");

                _unitOfWork.Categories.Delete(category);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the category: {ex.Message}", ex);
            }
        }
    }
}
