using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Managment.bll.DTOs;

namespace Task_Managment.bll.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDisplayDTO> GetCategoryByIdAsync(string id);
        Task<IEnumerable<CategoryDisplayDTO>> GetAllCategoriesAsync();
        Task<CategoryDTO> AddCategoryAsync(CategoryDTO categoryDto);
        Task<CategoryDTO> UpdateCategoryAsync(string id, CategoryDTO categoryDto);
        Task DeleteCategoryAsync(string id);

    }
}
