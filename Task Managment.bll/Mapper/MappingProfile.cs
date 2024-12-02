using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Managment.bll.DTOs;
using Task_Managment.DAL.Models;
namespace Task_Managment.bll.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUserDTO, ApplicationUser>()
           .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));

            CreateMap<ApplicationUser, ApplicationUserDTO>()
           .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash));

            CreateMap<ApplicationUser, ApplicationUserDisplayDTO>()
                .ForMember(dest => dest.Tasks, opt => opt
                .MapFrom(src => src.Tasks));
            CreateMap<ApplicationUser, EditUserDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Category, CategoryDisplayDTO>()
                .ForMember(dest => dest.Tasks, opt => opt
                .MapFrom(src => src.Tasks));
            CreateMap<TaskModel, TaskDTO>().ReverseMap();

            CreateMap<EditTaskDTO, TaskModel>()
         .ForMember(dest => dest.Category, opt => opt.Ignore())  // Avoid modifying related entities
         .ForMember(dest => dest.User, opt => opt.Ignore())      // Prevent changes to User
         .ForAllMembers(options =>
             options.Condition((source, destination, sourceMember) => sourceMember != null));
            CreateMap<TaskModel, EditTaskDTO>().ReverseMap();

        }
    }
}
